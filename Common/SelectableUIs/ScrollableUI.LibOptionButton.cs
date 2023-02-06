using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Common.SelectableUIs;

public static partial class ScrollableUI {
	public class LibOptionButton<T> : UIElement, IGroupOptionButton {
		private readonly Asset<Texture2D> _BasePanelTexture;
		private readonly Asset<Texture2D> _selectedBorderTexture;
		private readonly Asset<Texture2D> _hoveredBorderTexture;
		private readonly Asset<Texture2D> _iconTexture;
		private readonly Rectangle? SourceRect;

		private T _currentOption;

		private float _opacity = 0.7f;
		public float FadeFromBlack = 1f;
		private readonly float _whiteLerp = 0.7f;

		private bool _UseOverrideColors;
		private Color _color;
		private Color _borderColor;
		private Color _overrideUnpickedColor = Color.White;
		private Color _overridePickedColor = Color.White;

		private bool _hovered;
		private bool _soundedHover;

		public bool ShowHighlightWhenSelected = true;

		private float _overrideOpacityPicked;
		private float _overrideOpacityUnpicked;

		public readonly LocalizedText Description;
		private UIText _title;

		public T OptionValue { get; }

		public bool IsSelected {
			get {
				if (this._currentOption != null) {
					return this._currentOption.Equals(this.OptionValue);
				}
				return false;
			}
		}

		public LibOptionButton(T option, LocalizedText title, LocalizedText description, Color textColor, string iconTexturePath, Rectangle? rectangle, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f) {
			_borderColor = Color.White;
			_currentOption = option;
			OptionValue = option;
			Description = description;
			SourceRect = rectangle;
			_color = Colors.InventoryDefaultColor;
			Width = StyleDimension.FromPixels(44f);
			Height = StyleDimension.FromPixels(34f);
			_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale");
			_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight");
			_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
			if (iconTexturePath != null) {
				_iconTexture = ModContent.Request<Texture2D>(iconTexturePath);
			}

			if (title != null) {
				UIText uIText = new(title, textSize, false) {
					HAlign = titleAlignmentX,
					VAlign = 0.5f,
					Width = StyleDimension.FromPixelsAndPercent(0f - titleWidthReduction, 1f),
					Top = StyleDimension.FromPixels(0f),
					TextColor = textColor
				};
				Append(uIText);
				_title = uIText;
			}
		}

		public void SetText(LocalizedText text, float textSize, Color color) {
			this._title?.Remove();

			UIText uIText = new(text, textSize, false) {
				HAlign = 0.5f,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Top = StyleDimension.FromPixels(0f),
				TextColor = color
			};
			Append(uIText);
			_title = uIText;
		}

		public void SetCurrentOption(T option) {
			_currentOption = option;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			if (_hovered) {
				if (!_soundedHover) {
					SoundEngine.PlaySound(SoundID.MenuTick);
				}
				_soundedHover = true;
			}
			else {
				_soundedHover = false;
			}

			CalculatedStyle dimensions = GetDimensions();
			Color color3 = _color;
			float num = _opacity;
			bool isSelected = IsSelected;

			if (_UseOverrideColors) {
				color3 = isSelected ? _overridePickedColor : _overrideUnpickedColor;
				num = isSelected ? _overrideOpacityPicked : _overrideOpacityUnpicked;
			}

			Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, color3, this.FadeFromBlack) * num);
			if (isSelected && ShowHighlightWhenSelected) {
				Utils.DrawSplicedPanel(spriteBatch, _selectedBorderTexture.Value, (int)dimensions.X + 7, (int)dimensions.Y + 7, (int)dimensions.Width - 14, (int)dimensions.Height - 14, 10, 10, 10, 10, Color.Lerp(color3, Color.White, this._whiteLerp) * num);
			}
			if (_hovered) {
				Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, this._borderColor);
			}

			if (_iconTexture != null) {
				Color color2 = Color.White;
				if (!_hovered && !isSelected) {
					color2 = Color.Lerp(color3, Color.White, _whiteLerp) * num;
				}
				spriteBatch.Draw(_iconTexture.Value, new Vector2(dimensions.X + 1f, dimensions.Y + 1f), SourceRect, color2);
			}
		}

		public override void LeftMouseDown(UIMouseEvent evt) {
			SoundEngine.PlaySound(SoundID.MenuTick);
			base.LeftMouseDown(evt);
		}

		public override void MouseOver(UIMouseEvent evt) {
			base.MouseOver(evt);
			_hovered = true;
		}

		public override void MouseOut(UIMouseEvent evt) {
			base.MouseOut(evt);
			_hovered = false;
		}

		public void SetColor(Color color, float opacity) {
			_color = color;
			_opacity = opacity;
		}

		public void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked) {
			_UseOverrideColors = true;
			_overridePickedColor = pickedColor;
			_overrideUnpickedColor = unpickedColor;
			_overrideOpacityPicked = opacityPicked;
			_overrideOpacityUnpicked = opacityNotPicked;
		}

		public void SetBorderColor(Color color) {
			_borderColor = color;
		}
	}
}
