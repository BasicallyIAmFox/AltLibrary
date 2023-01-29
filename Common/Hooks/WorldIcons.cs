using AltLibrary.Common.AltBiomes;
using AltLibrary.Core;
using AltLibrary.Core.UIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Common.Hooks
{
	internal static class WorldIcons
	{
		internal static int WarnUpdate = 0;

		public static void Init()
		{
			IL_UIWorldListItem.ctor += UIWorldListItem_ctor;
			IL_UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf1;
			On_UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
			EditsHelper.On<AWorldListItem>(nameof(AWorldListItem.GetIconElement), AWorldListItem_GetIconElement);
			WarnUpdate = 0;
		}

		public static void Unload()
		{
			IL_UIWorldListItem.ctor -= UIWorldListItem_ctor;
			IL_UIWorldListItem.DrawSelf -= UIWorldListItem_DrawSelf1;
			On_UIWorldListItem.DrawSelf -= UIWorldListItem_DrawSelf;
			WarnUpdate = 0;
		}

		private static UIElement AWorldListItem_GetIconElement(Action<AWorldListItem> orig, AWorldListItem self)
		{
			return new UIImage(ALTextureAssets.UIWorldSeedIcon[0])
			{
				Left = new StyleDimension(4f, 0f)
			};
		}

		private static void UIWorldListItem_DrawSelf1(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchCall<Color>("get_White")))
			{
				AltLibrary.Instance.Logger.Info("s $ 1");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldarg, 0);
			c.EmitDelegate<Func<Color, UIWorldListItem, Color>>((value, self) =>
			{
				return ALUtils.IsWorldValid(self) ? value : Color.MediumPurple;
			});
		}

		private static void UIWorldListItem_ctor(ILContext il)
		{
			ILCursor c = new(il);
			FieldReference canBePlayed = null;
			if (!c.TryGotoNext(i => i.MatchLdarg(3),
				i => i.MatchStfld(out canBePlayed)))
			{
				AltLibrary.Instance.Logger.Info("t $ 1");
				return;
			}

			c.Index += 2;
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldarg, 0);
			c.Emit(OpCodes.Ldarg, 3);
			c.EmitDelegate<Func<UIWorldListItem, bool, bool>>((self, canPlay) =>
			{
				return ALUtils.IsWorldValid(self) && canPlay;
			});
			c.Emit(OpCodes.Stfld, canBePlayed);

			if (!c.TryGotoNext(i => i.MatchLdftn(out _)))
			{
				AltLibrary.Instance.Logger.Info("t $ 2");
				return;
			}
			FieldReference fieldReference = null;
			if (!c.TryGotoNext(i => i.MatchLdfld(out fieldReference)))
			{
				AltLibrary.Instance.Logger.Info("t $ 3");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchCall(out _)))
			{
				AltLibrary.Instance.Logger.Info("t $ 4");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Ldfld, fieldReference);
			c.EmitDelegate<Action<UIWorldListItem, UIImage>>((uiWorldListItem, _worldIcon) =>
			{
				WorldFileData data = uiWorldListItem._data;
				ALUtils.GetWorldData(data, out Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, out string path2);

				LayeredWorldIcon worldIcon = new(data, tempDict.TryGetValue(path2, out AltLibraryConfig.WorldDataValues worldData) ? worldData : new());
				worldIcon.Height.Set(0, 1);
				worldIcon.Width.Set(0, 1);
				_worldIcon.Append(worldIcon);
			});
		}

		private static void UIWorldListItem_DrawSelf(On_UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
		{
			orig(self, spriteBatch);
			if (++WarnUpdate >= 120)
			{
				WarnUpdate = 0;
			}
			WorldFileData data = self._data;
			if (data == null)
				return;
			ALUtils.GetWorldData(data, out Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, out string path2);
			WorldFileData _data = data;
			CalculatedStyle innerDimensions = self.GetInnerDimensions();
			CalculatedStyle dimensions = self._worldIcon.GetDimensions();
			float num7 = innerDimensions.X + innerDimensions.Width;
			bool flag = tempDict.ContainsKey(path2);
			for (int i = 0; i < 4; i++)
			{
				Asset<Texture2D> asset = ALTextureAssets.BestiaryIcons;
				Rectangle? rectangle = null;
				if (i == 0)
				{
					if (flag && tempDict[path2].worldHallow.IsNotEmptyAndNull())
					{
						asset = ModContent.TryFind(tempDict[path2].worldHallow, out AltBiome hallow) ? ModContent.Request<Texture2D>(hallow.IconSmall ?? "AltLibrary/Assets/Menu/ButtonHallow") : ALTextureAssets.ButtonHallow;
					}
					else
					{
						rectangle = new(30, 30, 30, 30);
					}
				}
				else if (i == 1)
				{
					if (flag && tempDict[path2].worldEvil.IsNotEmptyAndNull())
					{
						asset = ModContent.TryFind(tempDict[path2].worldEvil, out AltBiome hallow) ? ModContent.Request<Texture2D>(hallow.IconSmall ?? "AltLibrary/Assets/Menu/ButtonEvil") : ALTextureAssets.ButtonCorrupt;
					}
					else
					{
						rectangle = new(_data.HasCorruption ? 210 : 360, 0, 30, 30);
					}
				}
				else if (i == 2)
				{
					if (flag && tempDict[path2].worldHell.IsNotEmptyAndNull())
					{
						asset = ModContent.TryFind(tempDict[path2].worldHell, out AltBiome hallow) ? ModContent.Request<Texture2D>(hallow.IconSmall ?? "AltLibrary/Assets/Menu/ButtonHell") : ALTextureAssets.ButtonHell;
					}
					else
					{
						rectangle = new(30, 60, 30, 30);
					}
				}
				else if (i == 3)
				{
					if (flag && tempDict[path2].worldJungle.IsNotEmptyAndNull())
					{
						asset = ModContent.TryFind(tempDict[path2].worldJungle, out AltBiome hallow) ? ModContent.Request<Texture2D>(hallow.IconSmall ?? "AltLibrary/Assets/Menu/ButtonJungle") : ALTextureAssets.ButtonJungle;
					}
					else
					{
						rectangle = new(180, 30, 30, 30);
					}
				}

				spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(num7 - 26f * (i + 1), dimensions.Y - 2f), Color.White);
				spriteBatch.Draw(asset.Value, new Vector2(num7 - 26f * (i + 1) + 3f, dimensions.Y + 1f), rectangle, Color.White, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
			}

			if (!ALUtils.IsWorldValid(self))
			{
				Rectangle mouseRectangle = Utils.CenteredRectangle(Main.MouseScreen, Vector2.One * 2f);
				Asset<Texture2D> asset = ALTextureAssets.ButtonWarn;
				int num = WarnUpdate % 120;
				int num2 = num < 60 ? 0 : 1;
				Rectangle rectangle = new(num2 * 22, 0, 22, 22);
				spriteBatch.Draw(asset.Value, new Vector2(num7 - 26f * 5, dimensions.Y - 2f), rectangle, Color.White);
				Vector2 vector2 = new(num7 - 26f * 5, dimensions.Y - 2f);
				if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(11f, 11f), Utils.Size(new Rectangle(0, 0, 22, 22)))))
				{
					string line = Language.GetTextValue("Mods.AltLibrary.WorldBreak");
					Main.instance.MouseText(line);
				}
			}
		}
	}
}
