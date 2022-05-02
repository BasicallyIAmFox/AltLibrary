using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.GameContent.UI.Elements;
using AltLibrary.Common.AltBiomes;
using Terraria.ModLoader;
using AltLibrary.Common;

namespace AltLibrary.Core.UIs
{
    internal class ALUIBiomeListItem : UIPanel
	{
		private AltBiome _achievement;

		private UIImageFramed _achievementIcon;

		private Asset<Texture2D> _innerPanelTopTexture;

		private Asset<Texture2D> _innerPanelBottomTexture;

		private bool _large;

        public ALUIBiomeListItem(AltBiome achievement, bool largeForOtherLanguages)
        {
            this._large = largeForOtherLanguages;
            base.BackgroundColor = new Color(26, 40, 89) * 0.8f;
            base.BorderColor = new Color(13, 20, 44) * 0.8f;
            float num5 = (float)(16 + this._large.ToInt() * 20);
            float num7 = (float)(this._large.ToInt() * 6);
            float num6 = (float)(this._large.ToInt() * 20);
            this._achievement = achievement;
            base.Height.Set(66f + num5, 0f);
            base.Width.Set(0f, 1f);
            base.PaddingTop = 8f;
            base.PaddingLeft = 9f;
			string ifUnknown = "AltLibrary/Assets/WorldIcons/ButtonCorrupt";
			if (achievement.BiomeType == BiomeType.Hallow) ifUnknown = "AltLibrary/Assets/WorldIcons/ButtonHallow";
			if (achievement.BiomeType == BiomeType.Hell) ifUnknown = "AltLibrary/Assets/WorldIcons/ButtonHell";
			if (achievement.BiomeType == BiomeType.Jungle) ifUnknown = "AltLibrary/Assets/WorldIcons/ButtonJungle";
			Rectangle frame = new(0, 0, 30, 30);
			if (achievement.BiomeType == BiomeType.Hallow && achievement.Name == "HallowBiome")
			{
				frame = new(30, 30, 30, 30);
			}
			if (achievement.BiomeType == BiomeType.Evil && achievement.Name == "CorruptBiome")
            {
				frame = new(210, 0, 30, 30);
			}
			if (achievement.BiomeType == BiomeType.Evil && achievement.Name == "CrimsonBiome")
			{
				frame = new(360, 0, 30, 30);
			}
			if (achievement.BiomeType == BiomeType.Hell && achievement.Type <= -1)
			{
				frame = new(30, 60, 30, 30);
			}
			if (achievement.BiomeType == BiomeType.Jungle && achievement.Type <= -1)
			{
				frame = new(180, 30, 30, 30);
			}
			this._achievementIcon = new UIImageFramed(achievement.IconSmall == null ? ModContent.Request<Texture2D>(ifUnknown) : ModContent.Request<Texture2D>(achievement.IconSmall), frame);
			_achievementIcon.Left.Set(-50f, 0f);
			_achievementIcon.Width.Set(-50f, 0f);
			this._achievementIcon.Left.Set(num7, 0f);
            this._achievementIcon.Top.Set(num6, 0f);
            _achievementIcon.OnClick += _achievementIcon_OnClick;
            Append(_achievementIcon);
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

        private void _achievementIcon_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
			if (_achievement.BiomeType == BiomeType.Evil)
			{
				UIWorldCreationEdits.AltEvilBiomeChosenType = _achievement.Type;
				AltLibrary.Instance.Logger.Info($"Clicked Evil! {UIWorldCreationEdits.AltEvilBiomeChosenType}, {_achievement.Type}");
			}
			if (_achievement.BiomeType == BiomeType.Hallow)
			{
				UIWorldCreationEdits.AltHallowBiomeChosenType = _achievement.Type;
				AltLibrary.Instance.Logger.Info($"Clicked Hallow! {UIWorldCreationEdits.AltHallowBiomeChosenType}, {_achievement.Type}");
			}
			if (_achievement.BiomeType == BiomeType.Hell)
			{
				UIWorldCreationEdits.AltHellBiomeChosenType = _achievement.Type;
				AltLibrary.Instance.Logger.Info($"Clicked Hell! {UIWorldCreationEdits.AltHellBiomeChosenType}, {_achievement.Type}");
			}
			if (_achievement.BiomeType == BiomeType.Jungle)
			{
				UIWorldCreationEdits.AltJungleBiomeChosenType = _achievement.Type;
				AltLibrary.Instance.Logger.Info($"Clicked Jungle! {UIWorldCreationEdits.AltJungleBiomeChosenType}, {_achievement.Type}");
			}
		}

        protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			int num9 = this._large.ToInt() * 6;
			Vector2 value8 = new((float)num9, 0f);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			CalculatedStyle dimensions = _achievementIcon.GetDimensions();
			float num8 = dimensions.X + dimensions.Width;
			Vector2 value9 = new(num8 + 7f, innerDimensions.Y);
			bool flag = false;
			float num7 = innerDimensions.Width - dimensions.Width + 1f - (float)(num9 * 2);
			Vector2 baseScale5 = new(0.85f);
			Vector2 baseScale4 = new(0.92f);
			string text3 = FontAssets.ItemStack.Value.CreateWrappedText(_achievement.Description != null ? this._achievement.Description.GetTranslation(Language.ActiveCulture) : "", (num7 - 20f) * (1f / baseScale4.X), Language.ActiveCulture.CultureInfo);
			Vector2 stringSize3 = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text3, baseScale4, num7);
			if (!this._large)
			{
				stringSize3 = ChatManager.GetStringSize(FontAssets.ItemStack.Value, _achievement.Description != null ? this._achievement.Description.GetTranslation(Language.ActiveCulture) : "", baseScale4, num7);
			}
			float num6 = 38f + (float)(this._large ? 20 : 0);
			if (stringSize3.Y > num6)
			{
				baseScale4.Y *= num6 / stringSize3.Y;
			}
			Color value7 = _achievement.OuterColor;
			value7 = Color.Lerp(value7, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color value5 = Color.White;
			value5 = Color.Lerp(value5, Color.White, base.IsMouseHovering ? 1f : 0f);
			Color color5 = base.IsMouseHovering ? Color.White : Color.Gray;
			Vector2 vector = value9 - Vector2.UnitY * 4f + value8;
			this.DrawPanelTop(spriteBatch, vector, num7, color5);
			vector.Y += 2f;
			vector.X += 4f;
			vector.X += 4f;
			vector.X += 17f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, this._achievement.Name, vector, value7, 0f, Vector2.Zero, baseScale5, num7, 2f);
			vector.X -= 17f;
			Vector2 position = value9 + Vector2.UnitY * 25f + value8;
			this.DrawPanelBottom(spriteBatch, position, num7, color5);
			position.X += 8f;
			position.Y += 4f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text3, position, value5, 0f, Vector2.Zero, baseScale4, -1f, 2f);
			/*if (flag)
			{
				Vector2 vector2 = vector + Vector2.UnitX * num7 + Vector2.UnitY;
				int num10 = (int)trackerValues.Item1;
				string str = num10.ToString();
				num10 = (int)trackerValues.Item2;
				string text2 = str + "/" + num10.ToString();
				Vector2 baseScale3 = new Vector2(0.75f);
				Vector2 stringSize2 = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text2, baseScale3, -1f);
				float progress = (float)(trackerValues.Item1 / trackerValues.Item2);
				float num5 = 80f;
				Color color4 = new Color(100, 255, 100);
				if (!base.IsMouseHovering)
				{
					color4 = Color.Lerp(color4, Color.Black, 0.25f);
				}
				Color color3 = new Color(255, 255, 255);
				if (!base.IsMouseHovering)
				{
					color3 = Color.Lerp(color3, Color.Black, 0.25f);
				}
				this.DrawProgressBar(spriteBatch, progress, vector2 - Vector2.UnitX * num5 * 0.7f, num5, color3, color4, color4.MultiplyRGBA(new Color(new Vector4(1f, 1f, 1f, 0.5f))));
				vector2.X -= num5 * 1.4f + stringSize2.X;
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text2, vector2, value7, 0f, new Vector2(0f, 0f), baseScale3, 90f, 2f);
			}*/
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

		private void DrawProgressBar(SpriteBatch spriteBatch, float progress, Vector2 spot, float Width = 169f, Color BackColor = default(Color), Color FillingColor = default(Color), Color BlipColor = default(Color))
		{
			if (BlipColor == Color.Transparent)
			{
				BlipColor = new Color(255, 165, 0, 127);
			}
			if (FillingColor == Color.Transparent)
			{
				FillingColor = new Color(255, 241, 51);
			}
			if (BackColor == Color.Transparent)
			{
				FillingColor = new Color(255, 255, 255);
			}
			Texture2D value3 = TextureAssets.ColorBar.Value;
			Texture2D value4 = TextureAssets.ColorBlip.Value;
			Texture2D value2 = TextureAssets.MagicPixel.Value;
			float num7 = MathHelper.Clamp(progress, 0f, 1f);
			float num6 = Width * 1f;
			float num5 = 8f;
			float num4 = num6 / 169f;
			Vector2 position = spot + Vector2.UnitY * num5 + Vector2.UnitX * 1f;
			spriteBatch.Draw(value3, spot, new Rectangle(5, 0, value3.Width - 9, value3.Height), BackColor, 0f, new Vector2(84.5f, 0f), new Vector2(num4, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value3, spot + new Vector2((0f - num4) * 84.5f - 5f, 0f), new Rectangle(0, 0, 5, value3.Height), BackColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(value3, spot + new Vector2(num4 * 84.5f, 0f), new Rectangle(value3.Width - 4, 0, 4, value3.Height), BackColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			position += Vector2.UnitX * (num7 - 0.5f) * num6;
			position.X -= 1f;
			spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), FillingColor, 0f, new Vector2(1f, 0.5f), new Vector2(num6 * num7, num5), SpriteEffects.None, 0f);
			if (progress != 0f)
			{
				spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), BlipColor, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num5), SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), Color.Black, 0f, new Vector2(0f, 0.5f), new Vector2(num6 * (1f - num7), num5), SpriteEffects.None, 0f);
		}
	}
}
