using AltLibrary.Common.Hooks;
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
		void AddIn(string fullName, Func<Player, UnifiedRandom, bool> whenTrue, Func<int> getBg, Action<Player, UnifiedRandom> randomizeBg, Action onEncounter) => BackgroundsAlternating._cacheIndexes.Add(fullName, (whenTrue, getBg, randomizeBg, onEncounter));

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

			internal static Dictionary<string, (Func<Player, UnifiedRandom, bool>, Func<int>, Action<Player, UnifiedRandom>, Action)> _cacheIndexes = new();
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

			internal static void Inject()
			{
				On.Terraria.GameContent.BackgroundChangeFlashInfo.UpdateCache += FlashUpdateCache;
				On.Terraria.WorldGen.RandomizeBackgroundBasedOnPlayer += FlashRandomizeOnPlayer;

				On.Terraria.Main.DrawSurfaceBG_BackMountainsStep1 += GetMagicNums;

				var UIMods = ReflectionDictionary.GetClass("Terraria.ModLoader.SurfaceBackgroundStylesLoader");
				SBSL_DCB = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawCloseBackground));
				SBSL_DMT = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawMiddleTexture));
				SBSL_DFT = UIMods.GetMethod(nameof(SurfaceBackgroundStylesLoader.DrawFarTexture));

				ModifySBSL_DCB += MSBSL_DCB;
				ModifySBSL_DMT += MSBSL_DMT;
				ModifySBSL_DFT += MSBSL_DFT;
			}

			internal static void Init()
			{
				float[] _flashPower = (float[])ReflectionDictionary.GetField("Terraria.GameContent.BackgroundChangeFlashInfo", "_flashPower").Value.GetValue(WorldGen.BackgroundsCache);
				int[] _variations = (int[])ReflectionDictionary.GetField("Terraria.GameContent.BackgroundChangeFlashInfo", "_variations").Value.GetValue(WorldGen.BackgroundsCache);
				
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

				ReflectionDictionary.GetField("Terraria.GameContent.BackgroundChangeFlashInfo", "_flashPower").Value.SetValue(WorldGen.BackgroundsCache, newFlashPower);
				ReflectionDictionary.GetField("Terraria.GameContent.BackgroundChangeFlashInfo", "_variations").Value.SetValue(WorldGen.BackgroundsCache, newVariations);
			}

			public static void Uninit()
			{
				ReflectionDictionary.GetField("Terraria.GameContent.BackgroundChangeFlashInfo", "_flashPower").Value.SetValue(WorldGen.BackgroundsCache, _oldFlashPower);
				ReflectionDictionary.GetField("Terraria.GameContent.BackgroundChangeFlashInfo", "_variations").Value.SetValue(WorldGen.BackgroundsCache, _oldVariations);
				
				_oldFlashPower = null;
				_oldVariations = null;

				_cacheIndexes = null;
				_cacheIndexByName = null;

				On.Terraria.GameContent.BackgroundChangeFlashInfo.UpdateCache -= FlashUpdateCache;
				On.Terraria.WorldGen.RandomizeBackgroundBasedOnPlayer -= FlashRandomizeOnPlayer;

				On.Terraria.Main.DrawSurfaceBG_BackMountainsStep1 -= GetMagicNums;

				if (SBSL_DCB != null)
					ModifySBSL_DCB -= MSBSL_DCB;
				if (SBSL_DMT != null)
					ModifySBSL_DMT -= MSBSL_DMT;
				if (SBSL_DFT != null)
					ModifySBSL_DFT -= MSBSL_DFT;

				SBSL_DCB = null;
				SBSL_DMT = null;
				SBSL_DFT = null;
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
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetCloseTexture(_cacheIndexes[style.FullName].Item2()).Value;
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
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetCloseTexture(_cacheIndexes[style.FullName].Item2()).Value.Width;
						}
						return v;
					});

					c.GotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
						i => i.MatchLdloc(3),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 0);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();

							Texture2D texture2 = TextureAssets.MagicPixel.Value;
							Color color = Color.Black * WorldGen.BackgroundsCache.GetFlashPower(_cacheIndexByName[style.FullName]);
							Main.spriteBatch.Draw(texture2, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);

							return (style as IAlternatingSurfaceBackground).GetCloseTexture(_cacheIndexes[style.FullName].Item2()).Value.Height;
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

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetMidTexture(_cacheIndexes[style.FullName].Item2()).Value;
						}
						return value;
					});

					c.TryGotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
						i => i.MatchLdloc(4),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetMidTexture(_cacheIndexes[style.FullName].Item2()).Value.Width;
						}
						return v;
					});

					c.TryGotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
						i => i.MatchLdloc(4),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetMidTexture(_cacheIndexes[style.FullName].Item2()).Value.Height;
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
					c.GotoNext(i => i.MatchLdloc(3), i => i.MatchLdcR4(0));

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Action<ModSurfaceBackgroundStyle>>((style) =>
					{
						if (style == null || style is not IAlternatingSurfaceBackground)
							return;

						int slot = style.Slot;
						float alpha = Main.bgAlphaFarBackLayer[slot];
						if (alpha > 0f)
						{
							for (int i = 0; i < (int)ReflectionDictionary.GetField("Terraria.Main", "bgLoops").Value.GetValue(Main.instance); i++)
							{
								_cacheIndexes[style.FullName].Item4();

								Color ColorOfSurfaceBackgroundsModified = (Color)ReflectionDictionary.GetField("Terraria.Main", "ColorOfSurfaceBackgroundsModified").Value.GetValue(null);
								float scAdj = (float)ReflectionDictionary.GetField("Terraria.Main", "scAdj").Value.GetValue(Main.instance);
								int bgWidthScaled = (int)ReflectionDictionary.GetField("Terraria.Main", "bgWidthScaled").Value.GetValue(null);

								float bgParallax = 0.1f;
								int bgTopY = !Main.gameMenu ? (int)(_backgroundTopMagicNumberCache * 1300 + 1005 + (int)scAdj + _pushBGTopHackCache + 40) : 75 + _pushBGTopHackCache;
								float bgScale = (float)ReflectionDictionary.GetField("Terraria.Main", "bgScale").Value.GetValue(null);
								int bgStartX = (int)(0 - Math.IEEERemainder(Main.screenPosition.X * bgParallax, bgWidthScaled) - (bgWidthScaled / 2));

								Texture2D texture = (style as IAlternatingSurfaceBackground).GetUltraFarTexture(_cacheIndexes[style.FullName].Item2(), ColorOfSurfaceBackgroundsModified, scAdj, bgWidthScaled, ref bgTopY, ref bgScale, ref bgStartX).Value;
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

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Func<Texture2D, ModSurfaceBackgroundStyle, Texture2D>>((value, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetFarTexture(_cacheIndexes[style.FullName].Item2()).Value;
						}
						return value;
					});

					c.GotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundWidth)),
						i => i.MatchLdloc(4),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetFarTexture(_cacheIndexes[style.FullName].Item2()).Value.Width;
						}
						return v;
					});

					c.GotoNext(MoveType.After,
						i => i.MatchLdsfld<Main>(nameof(Main.backgroundHeight)),
						i => i.MatchLdloc(4),
						i => i.MatchLdelemI4());

					c.Emit(OpCodes.Ldloc, 1);
					c.EmitDelegate<Func<int, ModSurfaceBackgroundStyle, int>>((v, style) =>
					{
						if (style is IAlternatingSurfaceBackground)
						{
							_cacheIndexes[style.FullName].Item4();
							return (style as IAlternatingSurfaceBackground).GetFarTexture(_cacheIndexes[style.FullName].Item2()).Value.Height;
						}
						return v;
					});
				}
				catch
				{
				}
			}

			private static void GetMagicNums(On.Terraria.Main.orig_DrawSurfaceBG_BackMountainsStep1 orig, Main self, double backgroundTopMagicNumber, float bgGlobalScaleMultiplier, int pushBGTopHack)
			{
				_backgroundTopMagicNumberCache = backgroundTopMagicNumber;
				_pushBGTopHackCache = pushBGTopHack;
				orig(self, backgroundTopMagicNumber, bgGlobalScaleMultiplier, pushBGTopHack);
			}

			private static void FlashUpdateCache(On.Terraria.GameContent.BackgroundChangeFlashInfo.orig_UpdateCache orig, BackgroundChangeFlashInfo self)
			{
				orig(self);

				int _count = 0;
				foreach (var v in _cacheIndexes)
				{
					ReflectionDictionary.GetMethod("Terraria.GameContent.BackgroundChangeFlashInfo", "UpdateVariation").Value.Invoke(self, new object[] { _latestFlash + _count, v.Value.Item2() });
					_cacheIndexByName.TryAdd(v.Key, _latestFlash + _count);
					_count++;
				}
			}

			private static void FlashRandomizeOnPlayer(On.Terraria.WorldGen.orig_RandomizeBackgroundBasedOnPlayer orig, UnifiedRandom random, Player player)
			{
				orig(random, player);

				int _count = 0;
				foreach (var v in _cacheIndexes)
				{
					if (v.Value.Item1(player, random))
					{
						v.Value.Item3(player, random);

						ReflectionDictionary.GetMethod("Terraria.GameContent.BackgroundChangeFlashInfo", "UpdateVariation").Value.Invoke(WorldGen.BackgroundsCache, new object[] { _latestFlash + _count, v.Value.Item2() });

						if (Main.netMode == NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.WorldData);
					}
					_count++;
				}
			}
		}
	}
}
