﻿using AltLibrary.Common.Hooks;
using AltLibrary.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AltLibrary.Common
{
	public interface IAlternatingSurfaceBackground
	{
		void AddIn(string fullName, Func<Player, UnifiedRandom, bool> whenTrue, Func<int, int> getBg, Action<Player, UnifiedRandom> randomizeBg, Action<int> onEncounter) => BackgroundsAlternating._cacheIndexes.Add(fullName, (whenTrue, getBg, randomizeBg, onEncounter));

		abstract Asset<Texture2D> GetFarTexture(int i);
		abstract Asset<Texture2D> GetCloseTexture(int i);
		abstract Asset<Texture2D> GetMidTexture(int i);
		virtual Asset<Texture2D> GetUltraFarTexture(int i, Color ColorOfSurfaceBackgroundsModified, float scAdj, int bgWidthScaled, ref int bgTopY, ref float bgScale, ref int bgStartX) { return null; }
	}

	namespace Hooks
	{
		public static class BackgroundsAlternating
		{
			public static int rand = -1;

			internal static Dictionary<string, (Func<Player, UnifiedRandom, bool>, Func<int, int>, Action<Player, UnifiedRandom>, Action<int>)> _cacheIndexes = new();
			internal static Dictionary<string, int> _cacheIndexByName = new();
			private static float[] _oldFlashPower;
			private static int[] _oldVariations;
			private static int _latestFlash = 0;

			private static double _backgroundTopMagicNumberCache;
			private static int _pushBGTopHackCache;

			private static MethodInfo SBSL_DMT = null;
			private static MethodInfo SBSL_DFT = null;
			private static MethodInfo SBSL_DCB = null;

			private static event ILContext.Manipulator ModifySBSL_DMT
			{
				add => HookEndpointManager.Modify(SBSL_DMT, value);
				remove => HookEndpointManager.Unmodify(SBSL_DMT, value);
			}

			private static event ILContext.Manipulator ModifySBSL_DFT
			{
				add => HookEndpointManager.Modify(SBSL_DFT, value);
				remove => HookEndpointManager.Unmodify(SBSL_DFT, value);
			}

			private static event ILContext.Manipulator ModifySBSL_DCB
			{
				add => HookEndpointManager.Modify(SBSL_DCB, value);
				remove => HookEndpointManager.Unmodify(SBSL_DCB, value);
			}

			private static dynamic[] Fields;
			internal static void Inject()
			{
				On_BackgroundChangeFlashInfo.UpdateCache += FlashUpdateCache;
				On_WorldGen.RandomizeBackgroundBasedOnPlayer += FlashRandomizeOnPlayer;

				On_Main.DrawSurfaceBG_BackMountainsStep1 += GetMagicNums;

				var UIMods = typeof(SurfaceBackgroundStylesLoader);
				SBSL_DCB = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawCloseBackground));
				SBSL_DMT = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawMiddleTexture));
				SBSL_DFT = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawFarTexture));

				ModifySBSL_DCB += MSBSL_DCB;
				ModifySBSL_DMT += MSBSL_DMT;
				ModifySBSL_DFT += MSBSL_DFT;
			}

			internal static void Init()
			{
				float[] _flashPower = WorldGen.BackgroundsCache._flashPower;
				int[] _variations = WorldGen.BackgroundsCache._variations;
				
				_oldFlashPower = _flashPower;
				_oldVariations = _variations;

				float[] newFlashPower = new float[_flashPower.Length + _cacheIndexes.Count];
				int[] newVariations = new int[_variations.Length + _cacheIndexes.Count];

				int _count = 0;

				for (int i = 0; i < newFlashPower.Length; i++)
				{
					if (i >= _flashPower.Length)
					{
						newFlashPower[i] = 0f;
						newVariations[i] = 0;
						continue;
					}

					newFlashPower[i] = _flashPower[i];
					newVariations[i] = _variations[i];
					_latestFlash = i;
				}
				foreach (var v in _cacheIndexes)
				{
					newFlashPower[_latestFlash + _count] = 0f;
					newVariations[_latestFlash + _count] = 0;
					_cacheIndexByName.TryAdd(v.Key, _latestFlash + _count);
					_count++;
				}

				WorldGen.BackgroundsCache._flashPower = newFlashPower;
				WorldGen.BackgroundsCache._variations = newVariations;
			}

			public static void Uninit()
			{
				On_BackgroundChangeFlashInfo.UpdateCache -= FlashUpdateCache;
				On_WorldGen.RandomizeBackgroundBasedOnPlayer -= FlashRandomizeOnPlayer;

				On_Main.DrawSurfaceBG_BackMountainsStep1 -= GetMagicNums;

				if (SBSL_DCB != null)
					ModifySBSL_DCB -= MSBSL_DCB;
				if (SBSL_DMT != null)
					ModifySBSL_DMT -= MSBSL_DMT;
				if (SBSL_DFT != null)
					ModifySBSL_DFT -= MSBSL_DFT;

				WorldGen.BackgroundsCache._flashPower = _oldFlashPower;
				WorldGen.BackgroundsCache._variations = _oldVariations;

				SBSL_DCB = null;
				SBSL_DMT = null;
				SBSL_DFT = null;
				_oldFlashPower = null;
				_oldVariations = null;
				_cacheIndexes = null;
				_cacheIndexByName = null;
				Fields = null;
			}

			private static void MSBSL_DCB(ILContext il)
			{
				ILCursor c = new(il);
				try
				{
					c.GotoNext(MoveType.After, i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value")));

					c.Emit(OpCodes.Ldloc, 0);
					c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(0);
							return (style as IAlternatingSurfaceBackground).GetCloseTexture(_cacheIndexes[style.FullName].Item2(0)).Value;
						}
						return value;
					});

					c.GotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
						i => i.MatchLdloc(3),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 0);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(0);
							return (style as IAlternatingSurfaceBackground).GetCloseTexture(_cacheIndexes[style.FullName].Item2(0)).Value.Width;
						}
						return v;
					});

					c.Index += 3;

					c.Emit(OpCodes.Ldloc, 0);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(0);

							Texture2D texture2 = TextureAssets.MagicPixel.Value;
							Color color = Color.Black * WorldGen.BackgroundsCache.GetFlashPower(_cacheIndexByName[style.FullName]);
							Main.spriteBatch.Draw(texture2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);

							return (style as IAlternatingSurfaceBackground).GetCloseTexture(_cacheIndexes[style.FullName].Item2(0)).Value.Height;
						}
						return v;
					});
				}
				catch (Exception e)
				{
					AltLibrary.Instance.Logger.Error($"[BG Close Alt]\n{e.Message}\n{e.StackTrace}");
				}
			}

			private static void MSBSL_DMT(ILContext il)
			{
				ILCursor c = new(il);
				try
				{
					c.TryGotoNext(MoveType.After, i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value")));

					c.Emit(OpCodes.Ldloc, 2);
					c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(1);
							return (style as IAlternatingSurfaceBackground).GetMidTexture(_cacheIndexes[style.FullName].Item2(1)).Value;
						}
						return value;
					});

					c.TryGotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
						i => i.MatchLdloc(5),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 2);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(1);
							return (style as IAlternatingSurfaceBackground).GetMidTexture(_cacheIndexes[style.FullName].Item2(1)).Value.Width;
						}
						return v;
					});

					c.Index += 3;

					c.Emit(OpCodes.Ldloc, 2);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(1);
							return (style as IAlternatingSurfaceBackground).GetMidTexture(_cacheIndexes[style.FullName].Item2(1)).Value.Height;
						}
						return v;
					});
				}
				catch (Exception e)
				{
					AltLibrary.Instance.Logger.Error($"[BG Middle Alt]\n{e.Message}\n{e.StackTrace}");
				}
			}

			private static void MSBSL_DFT(ILContext il)
			{
				ILCursor c = new(il);
				try
				{
					c.GotoNext(i => i.MatchLdloc(5), i => i.MatchLdcR4(0));

					c.Emit(OpCodes.Ldloc, 3);
					c.EmitDelegate<Action<ModSurfaceBackgroundStyle>>((style) =>
					{
						if (style == null || style is not IAlternatingSurfaceBackground)
							return;

						int slot = style.Slot;
						float alpha = Main.bgAlphaFarBackLayer[slot];
						if (alpha > 0f)
						{
							for (int i = 0; i < (int)Fields[0].GetValue(Main.instance); i++)
							{
								_cacheIndexes[style.FullName].Item4(3);

								Color ColorOfSurfaceBackgroundsModified = (Color)Fields[1].GetValue(null);
								float scAdj = (float)Fields[3].GetValue(Main.instance);
								int bgWidthScaled = (int)Fields[2].GetValue(null);
								int bgTopY = !Main.gameMenu ? (int)(_backgroundTopMagicNumberCache * 1300.0 + 1005.0 + (int)scAdj + _pushBGTopHackCache + 40) : 75 + _pushBGTopHackCache;

								float bgScale = (float)Fields[4].GetValue(null);
								int bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * 0.1f, bgWidthScaled) - (bgWidthScaled / 2));

								Texture2D texture = (style as IAlternatingSurfaceBackground).GetUltraFarTexture(_cacheIndexes[style.FullName].Item2(3), ColorOfSurfaceBackgroundsModified, scAdj, bgWidthScaled, ref bgTopY, ref bgScale, ref bgStartX).Value;
								if (texture is null)
									return;

								Main.spriteBatch.Draw(texture,
									new Vector2(bgStartX + bgWidthScaled * i, bgTopY - 20),
									new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)),
									ColorOfSurfaceBackgroundsModified, 0f, default,
									bgScale, 0, 0f);
							}
						}
					});

					c.GotoNext(MoveType.After, i => i.MatchCallvirt(typeof(Asset<Texture2D>).GetMethod("get_Value")));

					c.Emit(OpCodes.Ldloc, 3);
					c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(2);
							return (style as IAlternatingSurfaceBackground).GetFarTexture(_cacheIndexes[style.FullName].Item2(2)).Value;
						}
						return value;
					});

					c.GotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
						i => i.MatchLdloc(6),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 3);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(2);
							return (style as IAlternatingSurfaceBackground).GetFarTexture(_cacheIndexes[style.FullName].Item2(2)).Value.Width;
						}
						return v;
					});

					c.Index += 3;

					c.Emit(OpCodes.Ldloc, 3);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4(2);
							return (style as IAlternatingSurfaceBackground).GetFarTexture(_cacheIndexes[style.FullName].Item2(2)).Value.Height;
						}
						return v;
					});
				}
				catch
				{
				}
			}

			private static void GetMagicNums(On_Main.orig_DrawSurfaceBG_BackMountainsStep1 orig, Main self, double backgroundTopMagicNumber, float bgGlobalScaleMultiplier, int pushBGTopHack)
			{
				_backgroundTopMagicNumberCache = backgroundTopMagicNumber;
				_pushBGTopHackCache = pushBGTopHack;
				orig(self, backgroundTopMagicNumber, bgGlobalScaleMultiplier, pushBGTopHack);
			}

			private static void FlashUpdateCache(On_BackgroundChangeFlashInfo.orig_UpdateCache orig, BackgroundChangeFlashInfo self)
			{
				orig(self);

				/*int _count = 0;
				foreach (var v in _cacheIndexes)
				{
					self.UpdateVariation(_latestFlash + _count, v.Value.Item2(0));
					
					ReflectionDictionary.GetMethod("Terraria.GameContent.BackgroundChangeFlashInfo", "UpdateVariation").Value
						.Invoke(self, new object[] { _latestFlash + _count, v.Value.Item2() });

					_cacheIndexByName.TryAdd(v.Key, _latestFlash + _count);
					_count++;
				}*/
			}

			private static void FlashRandomizeOnPlayer(On_WorldGen.orig_RandomizeBackgroundBasedOnPlayer orig, UnifiedRandom random, Player player)
			{
				orig(random, player);

				/*int _count = 0;
				foreach (var v in _cacheIndexes)
				{
					if (v.Value.Item1(player, random))
					{
						v.Value.Item3(player, random);
						
						ReflectionDictionary.GetMethod("Terraria.GameContent.BackgroundChangeFlashInfo", "UpdateVariation").Value
							.Invoke(WorldGen.BackgroundsCache, new object[] { _latestFlash + _count, v.Value.Item2() });

						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}
					_count++;
				}*/
			}
		}
	}
}
