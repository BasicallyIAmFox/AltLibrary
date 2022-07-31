using AltLibrary.Common;
using AltLibrary.Common.AltOres;
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
	internal class ALUIOreListItem : UIPanel
	{
		private readonly AltOre ore;

		private readonly UIImageFramed oreIcon;

		private readonly UIImageButton button;

		private readonly Asset<Texture2D> _innerPanelTopTexture;

		private readonly Asset<Texture2D> _innerPanelBottomTexture;

		private readonly bool _large;

		public ALUIOreListItem(AltOre ore, bool largeForOtherLanguages)
		{
			_large = largeForOtherLanguages;
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
			float num5 = 16 + _large.ToInt() * 20;
			float num7 = _large.ToInt() * 6;
			float num6 = _large.ToInt() * 12;
			this.ore = ore;
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
			Rectangle frame = new(0, 0, 30, 30);
			oreIcon = new UIImageFramed(ALTextureAssets.OreIcons, frame);
			oreIcon.Left.Set(-50f, 0f);
			oreIcon.Width.Set(-50f, 0f);
			oreIcon.Left.Set(num7, 0f);
			oreIcon.Top.Set(num6, 0f);
			oreIcon.OnUpdate += _achievementIcon_OnUpdate;
			Append(oreIcon);
			button = new(ALTextureAssets.Empty);
			oreIcon.Width.Set(0f, 1f);
			oreIcon.Height.Set(0, 1f);
			button.OnClick += _achievementIcon_OnClick;
			button.SetVisibility(0f, 0f);
			oreIcon.Append(button);
			OnClick += _achievementIcon_OnClick;
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

		private void _achievementIcon_OnUpdate(UIElement affectedElement)
		{
			if (ore.Name.StartsWith("Random"))
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.Random, new(0, 0, 30, 30));
				return;
			}
			switch (ore.Type)
			{
				case -1:
					set(0);
					break;
				case -2:
					set(1);
					break;
				case -3:
					set(2);
					break;
				case -4:
					set(3);
					break;
				case -5:
					set(4);
					break;
				case -6:
					set(5);
					break;
				case -7:
					set(6);
					break;
				case -8:
					set(7);
					break;
				case -9:
					set(8);
					break;
				case -10:
					set(9);
					break;
				case -11:
					set(10);
					break;
				case -12:
					set(11);
					break;
				case -13:
					set(12);
					break;
				case -14:
					set(13);
					break;
				case -15:
					set(14);
					break;
			}
			if (ore.Type >= 0 && ore.Mod != null)
			{
				(affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>(ore.Texture), new(0, 0, 30, 30));
			}
			void set(int i)
			{
				(affectedElement as UIImageFramed).SetImage(ALTextureAssets.OreIcons, new(i % 8 * 30, i / 8 * 30, 30, 30));
			}
		}

		private void _achievementIcon_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (ore.Name.StartsWith("Random"))
			{
				List<int> values = new();
				switch (ore.Name)
				{
					case "RandomCopper":
						values.Add(-1);
						values.Add(-2);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Copper && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Copper = Main.rand.Next(values);
						break;
					case "RandomIron":
						values.Add(-3);
						values.Add(-4);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Iron && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Iron = Main.rand.Next(values);
						break;
					case "RandomSilver":
						values.Add(-5);
						values.Add(-6);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Silver && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Silver = Main.rand.Next(values);
						break;
					case "RandomGold":
						values.Add(-7);
						values.Add(-8);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Gold && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Gold = Main.rand.Next(values);
						break;
					case "RandomCobalt":
						values.Add(-9);
						values.Add(-10);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Cobalt = Main.rand.Next(values);
						break;
					case "RandomMythril":
						values.Add(-11);
						values.Add(-12);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Mythril = Main.rand.Next(values);
						break;
					case "RandomAdamantite":
						values.Add(-13);
						values.Add(-14);
						AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
						UIWorldCreationEdits.Adamantite = Main.rand.Next(values);
						break;
				}
				return;
			}

			if (ore.Type <= -1 && ore.Type >= -2) UIWorldCreationEdits.Copper = ore.Type;
			if (ore.Type <= -3 && ore.Type >= -4) UIWorldCreationEdits.Iron = ore.Type;
			if (ore.Type <= -5 && ore.Type >= -6) UIWorldCreationEdits.Silver = ore.Type;
			if (ore.Type <= -7 && ore.Type >= -8) UIWorldCreationEdits.Gold = ore.Type;
			if (ore.Type <= -9 && ore.Type >= -10) UIWorldCreationEdits.Cobalt = ore.Type;
			if (ore.Type <= -11 && ore.Type >= -12) UIWorldCreationEdits.Mythril = ore.Type;
			if (ore.Type <= -13 && ore.Type >= -14) UIWorldCreationEdits.Adamantite = ore.Type;

			if (ore.Type < 0)
				return;
			switch (ore.OreType)
			{
				case OreType.Copper:
					if (!ore.OnClick()) UIWorldCreationEdits.Copper = ore.Type;
					break;
				case OreType.Iron:
					if (!ore.OnClick()) UIWorldCreationEdits.Iron = ore.Type;
					break;
				case OreType.Silver:
					if (!ore.OnClick()) UIWorldCreationEdits.Silver = ore.Type;
					break;
				case OreType.Gold:
					if (!ore.OnClick()) UIWorldCreationEdits.Gold = ore.Type;
					break;
				case OreType.Cobalt:
					if (!ore.OnClick()) UIWorldCreationEdits.Cobalt = ore.Type;
					break;
				case OreType.Mythril:
					if (!ore.OnClick()) UIWorldCreationEdits.Mythril = ore.Type;
					break;
				case OreType.Adamantite:
					if (!ore.OnClick()) UIWorldCreationEdits.Adamantite = ore.Type;
					break;
				case OreType.None:
					ore.OnClick();
					break;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			int num9 = this._large.ToInt() * 6;
			Vector2 value8 = new(num9, 0f);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			CalculatedStyle dimensions = oreIcon.GetDimensions();
			float num8 = dimensions.X + dimensions.Width;
			Vector2 value9 = new(num8 + 7f, innerDimensions.Y);
			float num7 = innerDimensions.Width - dimensions.Width + 1f - num9 * 2;
			Vector2 baseScale5 = new(0.85f);
			Vector2 baseScale4 = new(0.92f);
			string descValue = ore.Description != null ? ore.Description.GetTranslation(Language.ActiveCulture) : "";
			if (descValue == null && ore.Description != null)
			{
				descValue = "";
			}
			if (ore.Mod == null)
			{
				switch (ore.Name)
				{
					case "RandomCopper": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
					case "RandomIron": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
					case "RandomSilver": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
					case "RandomGold": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
					case "RandomCobalt": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
					case "RandomMythril": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
					case "RandomAdamantite": descValue = Language.GetTextValue($"Mods.AltLibrary.AltOreDescription.{ore.Name}"); break;
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
			Color value7 = ore.NameColor;
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
			string displayNameValue = ore.DisplayName != null ? ore.DisplayName.GetTranslation(Language.ActiveCulture) : ore.Name;
			if (ore.Mod == null)
			{
				switch (ore.Name)
				{
					case "Copper": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Tin": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Iron": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Lead": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Silver": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Tungsten": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Gold": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Platinum": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Cobalt": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Palladium": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Mythril": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Orichalcum": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Adamantite": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "Titanium": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomCopper": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomIron": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomSilver": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomGold": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomCobalt": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomMythril": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
					case "RandomAdamantite": displayNameValue = Language.GetTextValue($"Mods.AltLibrary.AltOreName.{ore.Name}"); break;
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
			float num10 = this._large.ToInt() * 4 + 4;
			float num11 = this._large.ToInt() * 58 - 42;
			Vector2 vector2 = new(dimensions.X + num10, dimensions.Y - num11);
			Texture2D texture = ALTextureAssets.ButtonWarn.Value;
			spriteBatch.Draw(texture, vector2, new Rectangle(0, 0, 22, 22), Color.White);
			if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(11f, 11f), Utils.Size(texture))))
			{
				string text = ore.Mod == null ? Language.GetTextValue("Mods.AltLibrary.Warn.VanillaOre") : Language.GetTextValue("Mods.AltLibrary.Warn.ModdedOre", ore.Mod.Name);
				Main.instance.MouseText(text);
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
			spriteBatch.Draw(this._innerPanelBottomTexture.Value, position, new Rectangle(0, 0, 6, this._innerPanelBottomTexture.Height()), color);
			spriteBatch.Draw(this._innerPanelBottomTexture.Value, new Vector2(position.X + 6f, position.Y), new Rectangle(6, 0, 7, this._innerPanelBottomTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 12f) / 7f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(this._innerPanelBottomTexture.Value, new Vector2(position.X + width - 6f, position.Y), new Rectangle(13, 0, 6, this._innerPanelBottomTexture.Height()), color);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			base.BackgroundColor = new Color(46, 60, 119);
			base.BorderColor = new Color(20, 30, 56);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			base.BackgroundColor = new Color(26, 40, 89) * 0.8f;
			base.BorderColor = new Color(13, 20, 44) * 0.8f;
		}
	}
}
