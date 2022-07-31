using AltLibrary.Common;
using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace AltLibrary.Core.UIs
{
	internal class ALUIBiomeListItem : UIPanel
	{
		private readonly AltBiome biome;

		private readonly UIImageFramed biomeIcon;

		private readonly UIImageButton button;

		private readonly Asset<Texture2D> _innerPanelTopTexture;

		private readonly Asset<Texture2D> _innerPanelBottomTexture;

		private readonly bool _large;

		public ALUIBiomeListItem(AltBiome biome, bool largeForOtherLanguages)
		{
			_large = !largeForOtherLanguages;
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
			float num5 = 16 + _large.ToInt() * 20;
			float num7 = _large.ToInt() * 6;
			float num6 = _large.ToInt() * 12;
			this.biome = biome;
			Height.Set(66f + num5, 0f);
			Width.Set(0f, 1f);
			PaddingTop = 8f;
			PaddingLeft = 9f;
			UIImage image = new(ALTextureAssets.Button2);
			image.Left.Set(-50f, 0f);
			image.Width.Set(-50f, 0f);
			image.Left.Set(num7 - 1f, 0f);
			image.Top.Set(num6 - 1f, 0f);
			Append(image);
			Asset<Texture2D> ifUnknown = ALTextureAssets.ButtonCorrupt;
			if (biome.BiomeType == BiomeType.Hallow) ifUnknown = ALTextureAssets.ButtonHallow;
			if (biome.BiomeType == BiomeType.Hell) ifUnknown = ALTextureAssets.ButtonHell;
			if (biome.BiomeType == BiomeType.Jungle) ifUnknown = ALTextureAssets.ButtonJungle;
			Rectangle frame = new(0, 0, 30, 30);
			biomeIcon = new UIImageFramed(biome.IconSmall == null ? ifUnknown : ModContent.Request<Texture2D>(biome.IconSmall, AssetRequestMode.ImmediateLoad), frame);
			biomeIcon.Left.Set(-50f, 0f);
			biomeIcon.Width.Set(-50f, 0f);
			biomeIcon.Left.Set(num7, 0f);
			biomeIcon.Top.Set(num6, 0f);
			biomeIcon.OnUpdate += AchievementIcon_OnUpdate;
			Append(biomeIcon);
			button = new(ALTextureAssets.Empty);
			biomeIcon.Width.Set(0f, 1f);
			biomeIcon.Height.Set(0, 1f);
			button.OnClick += AchievementIcon_OnClick;
			button.SetVisibility(0f, 0f);
			OnClick += AchievementIcon_OnClick;
			biomeIcon.Append(button);
			_innerPanelTopTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelTop");
			if (_large)
			{
				_innerPanelBottomTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelBottom_Large");
			}
			else
			{
				_innerPanelBottomTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelBottom");
			}
		}

		private void AchievementIcon_OnUpdate(UIElement affectedElement)
		{
			if (biome.Name.StartsWith("Random"))
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.Random, new(0, 0, 30, 30));
				return;
			}

			if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff == 0)
			{
				Asset<Texture2D> hallow = ALTextureAssets.ButtonHallow;
				Asset<Texture2D> hell = ALTextureAssets.ButtonHell;
				Asset<Texture2D> jungle = ALTextureAssets.ButtonJungle;
				Asset<Texture2D> corrupt = ALTextureAssets.ButtonCorrupt;
				Asset<Texture2D> path = corrupt;
				if (biome.BiomeType == BiomeType.Hallow) path = hallow;
				if (biome.BiomeType == BiomeType.Hell) path = hell;
				if (biome.BiomeType == BiomeType.Jungle) path = jungle;
				(affectedElement as UIImageFramed).SetImage(biome.IconSmall != null ? ALTextureAssets.BiomeIconSmall[biome.Type - 1] : path, new(0, 0, 30, 30));
				return;
			}

			if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -3)
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.BestiaryIcons, new(30, 30, 30, 30));
			}
			if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -1)
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.BestiaryIcons, new(210, 0, 30, 30));
			}
			if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -2)
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.BestiaryIcons, new(360, 0, 30, 30));
			}
			if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -5)
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.BestiaryIcons, new(30, 60, 30, 30));
			}
			if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -4)
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.BestiaryIcons, new(180, 30, 30, 30));
			}
		}

		private void AchievementIcon_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (biome.Name.StartsWith("Random"))
			{
				if (biome.Name.StartsWith("RandomEvil"))
				{
					List<int> values = new()
					{
						-666,
						-333
					};
					AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
					UIWorldCreationEdits.AltEvilBiomeChosenType = Main.rand.Next(values);
					UIWorldCreationEdits.isCrimson = UIWorldCreationEdits.AltEvilBiomeChosenType == -666;
				}
				if (biome.Name.StartsWith("RandomHallow"))
				{
					List<int> values = new()
					{
						-3
					};
					AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
					UIWorldCreationEdits.AltHallowBiomeChosenType = Main.rand.Next(values);
				}
				if (biome.Name.StartsWith("RandomJungle"))
				{
					List<int> values = new()
					{
						-4
					};
					AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
					UIWorldCreationEdits.AltJungleBiomeChosenType = Main.rand.Next(values);
				}
				if (biome.Name.StartsWith("RandomUnderworld"))
				{
					List<int> values = new()
					{
						-5
					};
					AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
					UIWorldCreationEdits.AltHellBiomeChosenType = Main.rand.Next(values);
				}
				return;
			}

			if (biome.BiomeType == BiomeType.Evil)
			{
				if (biome.IsForCrimsonOrCorruptWorldUIFix.HasValue && biome.IsForCrimsonOrCorruptWorldUIFix.Value.Equals(false))
				{
					UIWorldCreationEdits.isCrimson = false;
					UIWorldCreationEdits.AltEvilBiomeChosenType = -333;
					biome.Type = -333;
					return;
				}
				if (biome.IsForCrimsonOrCorruptWorldUIFix.HasValue && biome.IsForCrimsonOrCorruptWorldUIFix.Value.Equals(true))
				{
					UIWorldCreationEdits.isCrimson = true;
					UIWorldCreationEdits.AltEvilBiomeChosenType = -666;
					biome.Type = -666;
					return;
				}
				UIWorldCreationEdits.AltEvilBiomeChosenType = biome.Type > 0 ? biome.Type - 1 : biome.Type;
				UIWorldCreationEdits.isCrimson = false;
			}
			else if (biome.BiomeType == BiomeType.Hallow)
			{
				if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff < 0) biome.Type = -3;
				UIWorldCreationEdits.AltHallowBiomeChosenType = biome.Type > 0 ? biome.Type - 1 : biome.Type;
			}
			else if (biome.BiomeType == BiomeType.Hell)
			{
				if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff < 0) biome.Type = -5;
				UIWorldCreationEdits.AltHellBiomeChosenType = biome.Type > 0 ? biome.Type - 1 : biome.Type;
			}
			else if (biome.BiomeType == BiomeType.Jungle)
			{
				if (biome.SpecialValueForWorldUIDoNotTouchElseYouCanBreakStuff < 0) biome.Type = -4;
				UIWorldCreationEdits.AltJungleBiomeChosenType = biome.Type > 0 ? biome.Type - 1 : biome.Type;
			}
			else
				biome.OnClick();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			int num9 = this._large.ToInt() * 6;
			Vector2 value8 = new(num9, 0f);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			CalculatedStyle dimensions = biomeIcon.GetDimensions();
			float num8 = dimensions.X + dimensions.Width;
			Vector2 value9 = new(num8 + 7f, innerDimensions.Y);
			float num7 = innerDimensions.Width - dimensions.Width + 1f - num9 * 2;
			Vector2 baseScale5 = new(0.85f);
			Vector2 baseScale4 = new(0.92f);
			string descValue = biome.Description != null ? biome.Description.GetTranslation(Language.ActiveCulture) : "";
			if (descValue == null && biome.Description != null)
			{
				descValue = "";
			}
			if (biome.Mod == null)
			{
				switch (biome.Name)
				{
					case "CorruptBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "CrimsonBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "HallowBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "JungleBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "UnderworldBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "RandomEvilBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "RandomHallowBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "RandomJungleBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					case "RandomUnderworldBiome": descValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeDescription.{biome.Name}"); break;
					default: break;
				}
			}
			string text3 = FontAssets.ItemStack.Value.CreateWrappedText(descValue, (num7 - 20f) * (1f / baseScale4.X), Language.ActiveCulture.CultureInfo);
			Vector2 stringSize3 = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text3, baseScale4, num7);
			if (!this._large)
			{
				stringSize3 = ChatManager.GetStringSize(FontAssets.ItemStack.Value, descValue, baseScale4, num7);
			}
			float num6 = 38f + (this._large ? 20 : 0);
			if (stringSize3.Y > num6)
			{
				baseScale4.Y *= num6 / stringSize3.Y;
			}
			Color value7 = biome.NameColor;
			value7 = Color.Lerp(value7, Color.White, base.IsMouseHovering ? 0.25f : 0f);
			Color value5 = Color.White;
			value5 = Color.Lerp(value5, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color5 = base.IsMouseHovering ? Color.White : Color.Gray;
			Vector2 vector = value9 - Vector2.UnitY * 4f + value8;
			this.DrawPanelTop(spriteBatch, vector, num7, color5);
			vector.Y += 2f;
			vector.X += 4f;
			vector.X += 4f;
			vector.X += 17f;
			string displayNameValue = biome.DisplayName != null ? biome.DisplayName.GetTranslation(Language.ActiveCulture) : biome.Name;
			if (biome.Mod == null)
			{
				switch (biome.Name)
				{
					case "CorruptBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "CrimsonBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "HallowBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "JungleBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "UnderworldBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "RandomEvilBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "RandomHallowBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "RandomJungleBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					case "RandomUnderworldBiome": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltBiomeName.{biome.Name}"); break;
					default: break;
				}
			}
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, displayNameValue, vector, value7, 0f, Vector2.Zero, baseScale5, num7, 2f);
			vector.X -= 17f;
			Vector2 position = value9 + Vector2.UnitY * 25f + value8;
			this.DrawPanelBottom(spriteBatch, position, num7, color5);
			position.X += 8f;
			position.Y += 4f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text3, position, value5, 0f, Vector2.Zero, baseScale4, -1f, 2f);
			Rectangle mouseRectangle = Utils.CenteredRectangle(Main.MouseScreen, Vector2.One * 2f);
			float num10 = this._large.ToInt() * 4;
			float num11 = this._large.ToInt() * 58 - 102;
			Vector2 vector2 = new(dimensions.X + num10, dimensions.Y - num11);
			Texture2D texture = ALTextureAssets.ButtonWarn.Value;
			spriteBatch.Draw(texture, vector2, new Rectangle(0, 0, 22, 22), Color.White);
			if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(11f, 11f), Utils.Size(texture))))
			{
				Main.instance.MouseText(biome.Mod == null ? Language.GetTextValue("Mods.AltLibrary.Warn.VanillaBiome") : Language.GetTextValue("Mods.AltLibrary.Warn.ModdedBiome", biome.Mod.Name));
			}
		}

		private void DrawPanelTop(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(this._innerPanelTopTexture.Value, position, new Rectangle(0, 0, 2, this._innerPanelTopTexture.Height()), color);
			spriteBatch.Draw(this._innerPanelTopTexture.Value, new Vector2(position.X + 2f, position.Y), new Rectangle(2, 0, 2, this._innerPanelTopTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 4f) / 2f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this._innerPanelTopTexture.Value, new Vector2(position.X + width - 2f, position.Y), new Rectangle(4, 0, 2, this._innerPanelTopTexture.Height()), color);
		}

		private void DrawPanelBottom(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(_innerPanelBottomTexture.Value, position, new Rectangle(0, 0, 6, this._innerPanelBottomTexture.Height()), color);
			spriteBatch.Draw(_innerPanelBottomTexture.Value, new Vector2(position.X + 6f, position.Y), new Rectangle(6, 0, 7, this._innerPanelBottomTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 12f) / 7f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelBottomTexture.Value, new Vector2(position.X + width - 6f, position.Y), new Rectangle(13, 0, 6, this._innerPanelBottomTexture.Height()), color);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = new Color(46, 60, 119);
			BorderColor = new Color(20, 30, 56);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
		}
	}
}
