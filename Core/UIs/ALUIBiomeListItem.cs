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
        private readonly AltBiome _achievement;

        private readonly UIImageFramed _achievementIcon;

        private readonly UIImageButton button;

        private readonly Asset<Texture2D> _innerPanelTopTexture;

        private readonly Asset<Texture2D> _innerPanelBottomTexture;

        private readonly bool _large;

        public ALUIBiomeListItem(AltBiome achievement, bool largeForOtherLanguages)
        {
            this._large = !largeForOtherLanguages;
            base.BackgroundColor = new Color(26, 40, 89) * 0.8f;
            base.BorderColor = new Color(13, 20, 44) * 0.8f;
            float num5 = 16 + this._large.ToInt() * 20;
            float num7 = this._large.ToInt() * 6;
            float num6 = this._large.ToInt() * 12;
            this._achievement = achievement;
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
            string ifUnknown = "AltLibrary/Assets/Menu/ButtonCorrupt";
            if (achievement.BiomeType == BiomeType.Hallow) ifUnknown = "AltLibrary/Assets/Menu/ButtonHallow";
            if (achievement.BiomeType == BiomeType.Hell) ifUnknown = "AltLibrary/Assets/Menu/ButtonHell";
            if (achievement.BiomeType == BiomeType.Jungle) ifUnknown = "AltLibrary/Assets/Menu/ButtonJungle";
            Rectangle frame = new(0, 0, 30, 30);
            this._achievementIcon = new UIImageFramed(achievement.IconSmall == null ? ModContent.Request<Texture2D>(ifUnknown) : ModContent.Request<Texture2D>(achievement.IconSmall), frame);
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

            if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff == 0)
            {
                string hallow = "AltLibrary/Assets/Menu/ButtonHallow";
                string hell = "AltLibrary/Assets/Menu/ButtonHell";
                string jungle = "AltLibrary/Assets/Menu/ButtonJungle";
                string corrupt = "AltLibrary/Assets/Menu/ButtonCorrupt";
                string path = "";
                if (_achievement.BiomeType == BiomeType.Hallow) path = hallow;
                if (_achievement.BiomeType == BiomeType.Hell) path = hell;
                if (_achievement.BiomeType == BiomeType.Jungle) path = jungle;
                if (_achievement.BiomeType == BiomeType.Evil) path = corrupt;
                (affectedElement as UIImageFramed).SetImage(_achievement.IconSmall != null ? ModContent.Request<Texture2D>(_achievement.IconSmall) : ModContent.Request<Texture2D>(path), new(0, 0, 30, 30));
                return;
            }

            if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -3)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"), new(30, 30, 30, 30));
            }
            if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -1)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"), new(210, 0, 30, 30));
            }
            if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -2)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"), new(360, 0, 30, 30));
            }
            if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -5)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"), new(30, 60, 30, 30));
            }
            if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff == -4)
            {
                (affectedElement as UIImageFramed).SetImage(ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"), new(180, 30, 30, 30));
            }
        }

        private void _achievementIcon_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_achievement.Name.StartsWith("Random"))
            {
                if (_achievement.Name.StartsWith("RandomEvil"))
                {
                    List<int> values = new()
                    {
                        -666,
                        -333
                    };
                    AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
                    UIWorldCreationEdits.AltEvilBiomeChosenType = values[Main.rand.Next(values.Count)];
                    UIWorldCreationEdits.isCrimson = UIWorldCreationEdits.AltEvilBiomeChosenType == -666;
                }
                if (_achievement.Name.StartsWith("RandomHallow"))
                {
                    List<int> values = new()
                    {
                        -3
                    };
                    AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
                    UIWorldCreationEdits.AltHallowBiomeChosenType = values[Main.rand.Next(values.Count)];
                }
                if (_achievement.Name.StartsWith("RandomJungle"))
                {
                    List<int> values = new()
                    {
                        -4
                    };
                    AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
                    UIWorldCreationEdits.AltJungleBiomeChosenType = values[Main.rand.Next(values.Count)];
                }
                if (_achievement.Name.StartsWith("RandomUnderworld"))
                {
                    List<int> values = new()
                    {
                        -5
                    };
                    AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).ToList().ForEach((x) => values.Add(x.Type - 1));
                    UIWorldCreationEdits.AltHellBiomeChosenType = values[Main.rand.Next(values.Count)];
                }
                return;
            }

            if (_achievement.BiomeType == BiomeType.Evil)
            {
                if (_achievement.isForCrimsonOrCorruptWorldUIFix.HasValue && _achievement.isForCrimsonOrCorruptWorldUIFix.Value.Equals(false))
                {
                    UIWorldCreationEdits.isCrimson = false;
                    UIWorldCreationEdits.AltEvilBiomeChosenType = -333;
                    _achievement.Type = -333;
                    return;
                }
                if (_achievement.isForCrimsonOrCorruptWorldUIFix.HasValue && _achievement.isForCrimsonOrCorruptWorldUIFix.Value.Equals(true))
                {
                    UIWorldCreationEdits.isCrimson = true;
                    UIWorldCreationEdits.AltEvilBiomeChosenType = -666;
                    _achievement.Type = -666;
                    return;
                }
                UIWorldCreationEdits.AltEvilBiomeChosenType = _achievement.Type > 0 ? _achievement.Type - 1 : _achievement.Type;
                UIWorldCreationEdits.isCrimson = false;
            }
            if (_achievement.BiomeType == BiomeType.Hallow)
            {
                if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff < 0) _achievement.Type = -3;
                UIWorldCreationEdits.AltHallowBiomeChosenType = _achievement.Type > 0 ? _achievement.Type - 1 : _achievement.Type;
            }
            if (_achievement.BiomeType == BiomeType.Hell)
            {
                if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff < 0) _achievement.Type = -5;
                UIWorldCreationEdits.AltHellBiomeChosenType = _achievement.Type > 0 ? _achievement.Type - 1 : _achievement.Type;
            }
            if (_achievement.BiomeType == BiomeType.Jungle)
            {
                if (_achievement.specialValueForWorldUIDoNotTouchElseYouCanBreakStuff < 0) _achievement.Type = -4;
                UIWorldCreationEdits.AltJungleBiomeChosenType = _achievement.Type > 0 ? _achievement.Type - 1 : _achievement.Type;
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
            string descValue3 = $"Mods.{(_achievement.Mod != null ? _achievement.Mod.Name : "AltLibrary")}.Biomes.{_achievement.Name}Desc";
            string descValue2 = LanguageManager.Instance.Exists(descValue3) ? Language.GetTextValue(descValue3) : "";
            string descValue = _achievement.Description != null ? _achievement.Description.Value : descValue2;
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
            string displayNameValue3 = $"Mods.{(_achievement.Mod != null ? _achievement.Mod.Name : "AltLibrary")}.Biomes.{_achievement.Name}Name";
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
            float num10 = this._large.ToInt() * 4;
            float num11 = this._large.ToInt() * 58 - 102;
            Vector2 vector2 = new(dimensions.X + num10, dimensions.Y - num11);
            Texture2D texture = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonWarn", AssetRequestMode.ImmediateLoad).Value;
            spriteBatch.Draw(texture, vector2, new Rectangle(0, 0, 22, 22), Color.White);
            if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(11f, 11f), Utils.Size(texture))))
            {
                Main.instance.MouseText(_achievement.Mod == null ? Language.GetTextValue("Mods.AltLibrary.Warn.VanillaBiome") : Language.GetTextValue("Mods.AltLibrary.Warn.ModdedBiome", _achievement.Mod.Name));
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
