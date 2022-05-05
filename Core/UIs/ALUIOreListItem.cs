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
        private AltOre _achievement;

        private UIImageFramed _achievementIcon;

        private UIImageButton button;

        private Asset<Texture2D> _innerPanelTopTexture;

        private Asset<Texture2D> _innerPanelBottomTexture;

        private bool _large;

        public ALUIOreListItem(AltOre ore, bool largeForOtherLanguages)
        {
            this._large = largeForOtherLanguages;
            base.BackgroundColor = new Color(26, 40, 89) * 0.8f;
            base.BorderColor = new Color(13, 20, 44) * 0.8f;
            float num5 = 16 + this._large.ToInt() * 20;
            float num7 = this._large.ToInt() * 6;
            float num6 = this._large.ToInt() * 12;
            this._achievement = ore;
            base.Height.Set(66f + num5, 0f);
            base.Width.Set(0f, 1f);
            base.PaddingTop = 8f;
            base.PaddingLeft = 9f;
            UIImage image = new(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button2", AssetRequestMode.ImmediateLoad));
            image.Left.Set(-50f, 0f);
            image.Width.Set(-50f, 0f);
            image.Left.Set(num7 - 1f, 0f);
            image.Top.Set(num6 - 1f, 0f);
            Append(image);
            Rectangle frame = new(0, 0, 30, 30);
            this._achievementIcon = new UIImageFramed(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/OreIcons"), frame);
            _achievementIcon.Left.Set(-50f, 0f);
            _achievementIcon.Width.Set(-50f, 0f);
            this._achievementIcon.Left.Set(num7, 0f);
            this._achievementIcon.Top.Set(num6, 0f);
            _achievementIcon.OnUpdate += _achievementIcon_OnUpdate;
            Append(_achievementIcon);
            button = new(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Empty", AssetRequestMode.ImmediateLoad));
            _achievementIcon.Width.Set(0f, 1f);
            _achievementIcon.Height.Set(0, 1f);
            button.OnClick += _achievementIcon_OnClick;
            button.SetVisibility(0f, 0f);
            _achievementIcon.Append(button);
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
            if (_achievement.Name.StartsWith("Random"))
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Random"), new(0, 0, 30, 30));
                return;
            }

            if (_achievement.Type == -1)
            {
                set(0);
            }
            else if (_achievement.Type == -2)
            {
                set(1);
            }
            else if (_achievement.Type == -3)
            {
                set(2);
            }
            else if (_achievement.Type == -4)
            {
                set(3);
            }
            else if (_achievement.Type == -5)
            {
                set(4);
            }
            else if (_achievement.Type == -6)
            {
                set(5);
            }
            else if (_achievement.Type == -7)
            {
                set(6);
            }
            else if (_achievement.Type == -8)
            {
                set(7);
            }
            else if (_achievement.Type == -9)
            {
                set(8);
            }
            else if (_achievement.Type == -10)
            {
                set(9);
            }
            else if (_achievement.Type == -11)
            {
                set(10);
            }
            else if (_achievement.Type == -12)
            {
                set(11);
            }
            else if (_achievement.Type == -13)
            {
                set(12);
            }
            else if (_achievement.Type == -14)
            {
                set(13);
            }
            else if (_achievement.Type == -15)
            {
                set(14);
            }
            if (_achievement.Type >= 0 && _achievement.Mod != null)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>(_achievement.Texture), new(0, 0, 30, 30));
            }

            void set(int i)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/OreIcons"), new(i % 8 * 30, i / 8 * 30, 30, 30));
            }
        }

        private void _achievementIcon_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_achievement.Name.StartsWith("Random"))
            {
                List<int> values = new();
                switch (_achievement.Name)
                {
                    case "RandomCopper":
                        values.Add(-1);
                        values.Add(-2);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Copper && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Copper = values[Main.rand.Next(values.Count)];
                        break;
                    case "RandomIron":
                        values.Add(-3);
                        values.Add(-4);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Iron && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Iron = values[Main.rand.Next(values.Count)];
                        break;
                    case "RandomSilver":
                        values.Add(-5);
                        values.Add(-6);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Silver && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Silver = values[Main.rand.Next(values.Count)];
                        break;
                    case "RandomGold":
                        values.Add(-7);
                        values.Add(-8);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Gold && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Gold = values[Main.rand.Next(values.Count)];
                        break;
                    case "RandomCobalt":
                        values.Add(-9);
                        values.Add(-10);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Cobalt = values[Main.rand.Next(values.Count)];
                        break;
                    case "RandomMythril":
                        values.Add(-11);
                        values.Add(-12);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Mythril && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Mythril = values[Main.rand.Next(values.Count)];
                        break;
                    case "RandomAdamantite":
                        values.Add(-13);
                        values.Add(-14);
                        AltLibrary.ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable).ToList().ForEach(x => values.Add(x.Type));
                        UIWorldCreationEdits.Adamantite = values[Main.rand.Next(values.Count)];
                        break;
                }
                return;
            }

            if (_achievement.Type <= -1 && _achievement.Type >= -2) UIWorldCreationEdits.Copper = _achievement.Type;
            if (_achievement.Type <= -3 && _achievement.Type >= -4) UIWorldCreationEdits.Iron = _achievement.Type;
            if (_achievement.Type <= -5 && _achievement.Type >= -6) UIWorldCreationEdits.Silver = _achievement.Type;
            if (_achievement.Type <= -7 && _achievement.Type >= -8) UIWorldCreationEdits.Gold = _achievement.Type;
            if (_achievement.Type <= -9 && _achievement.Type >= -10) UIWorldCreationEdits.Cobalt = _achievement.Type;
            if (_achievement.Type <= -11 && _achievement.Type >= -12) UIWorldCreationEdits.Mythril = _achievement.Type;
            if (_achievement.Type <= -13 && _achievement.Type >= -14) UIWorldCreationEdits.Adamantite = _achievement.Type;

            if (_achievement.Type < 0)
                return;
            switch (_achievement.OreType)
            {
                case OreType.Copper:
                    UIWorldCreationEdits.Copper = _achievement.Type;
                    break;
                case OreType.Iron:
                    UIWorldCreationEdits.Iron = _achievement.Type;
                    break;
                case OreType.Silver:
                    UIWorldCreationEdits.Silver = _achievement.Type;
                    break;
                case OreType.Gold:
                    UIWorldCreationEdits.Gold = _achievement.Type;
                    break;
                case OreType.Cobalt:
                    UIWorldCreationEdits.Cobalt = _achievement.Type;
                    break;
                case OreType.Mythril:
                    UIWorldCreationEdits.Mythril = _achievement.Type;
                    break;
                case OreType.Adamantite:
                    UIWorldCreationEdits.Adamantite = _achievement.Type;
                    break;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            int num9 = this._large.ToInt() * 6;
            Vector2 value8 = new(num9, 0f);
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            CalculatedStyle dimensions = _achievementIcon.GetDimensions();
            float num8 = dimensions.X + dimensions.Width;
            Vector2 value9 = new(num8 + 7f, innerDimensions.Y);
            float num7 = innerDimensions.Width - dimensions.Width + 1f - num9 * 2;
            Vector2 baseScale5 = new(0.85f);
            Vector2 baseScale4 = new(0.92f);
            string descValue3 = $"Mods.{(_achievement.Mod != null ? _achievement.Mod.Name : "AltLibrary")}.Ores.{_achievement.Name}Desc";
            string descValue2 = LanguageManager.Instance.Exists(descValue3) ? Language.GetTextValue(descValue3) : "";
            string descValue = _achievement.Description != null ? descValue2 : descValue2;
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
            Color value7 = _achievement.NameColor;
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
            string displayNameValue3 = $"Mods.{(_achievement.Mod != null ? _achievement.Mod.Name : "AltLibrary")}.Ores.{_achievement.Name}Name";
            string displayNameValue2 = LanguageManager.Instance.Exists(displayNameValue3) ? Language.GetTextValue(displayNameValue3) : _achievement.Name;
            string displayNameValue = _achievement.DisplayName != null ? _achievement.DisplayName.Value : displayNameValue2;
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
            Texture2D texture = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonWarn").Value;
            spriteBatch.Draw(texture, vector2, new Rectangle(0, 0, 22, 22), Color.White);
            if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(11f, 11f), Utils.Size(texture))))
            {
                Main.instance.MouseText(_achievement.Mod == null ? Language.GetTextValue("Mods.AltLibrary.Warn.VanillaOre") : Language.GetTextValue("Mods.AltLibrary.Warn.ModdedOre", _achievement.Mod.Name));
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
