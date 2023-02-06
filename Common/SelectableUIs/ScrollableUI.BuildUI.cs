using AltLibrary.Common.Assets;
using AltLibrary.Common.Cache;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Common.SelectableUIs;

public partial class ScrollableUI {
	private class UIImageButtonWithRectangle : UIElement {
		private readonly Rectangle? rectangle;
		private readonly Asset<Texture2D> _texture;
		private readonly Asset<Texture2D> _borderTexture;

		public UIImageButtonWithRectangle(Asset<Texture2D> texture, Rectangle? rectangle) {
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
			this.rectangle = rectangle;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(_texture.Value, dimensions.Position(), rectangle, Color.White);
			if (_borderTexture != null && IsMouseHovering)
				spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), rectangle, Color.White);
		}

		public override void MouseOver(UIMouseEvent evt) {
			base.MouseOver(evt);
			SoundEngine.PlaySound(SoundID.MenuTick);
		}

		public override void MouseOut(UIMouseEvent evt) {
			base.MouseOut(evt);
		}
	}

	private class BuildUI : ILoadable {
		private static UIElement _basePanel;
		private static float _accumulatedHeight = 0f;

		public void Load(Mod mod) {
			ILHelper.On<UIWorldCreation>("BuildPage", (On_UIWorldCreation.orig_BuildPage orig, UIWorldCreation self) => {
				orig(self);
				BuildBasePage(self);
			});
			ILHelper.IL<UIWorldCreation>("MakeInfoMenu", (ILContext il) => {
				var c = new ILCursor(il);

				var heightIndex = 0;
				var label = c.DefineLabel();

				ILHelper.CompleteLog(AltLibrary.Instance, c, true);

				c.GotoNext(i => i.MatchLdstr("evil"))
					.GotoNext(
						i => i.MatchLdloc(out heightIndex),
						i => i.MatchLdcR4(out _)
					);

				c.Emit(OpCodes.Br, label)
					.GotoNext(i => i.MatchLdarg(0),
						i => i.MatchLdloc(out _),
						i => i.MatchLdloc(heightIndex),
						i => i.MatchLdstr("desc")
					);

				c.MarkLabel(label);

				ILHelper.CompleteLog(AltLibrary.Instance, c, false);
			});
			ILHelper.IL<UIWorldCreation>("AddWorldEvilOptions", (ILContext il) => {
				var c = new ILCursor(il);

				c.Emit(OpCodes.Ldarg, 2);
				c.Emit(OpCodes.Stsfld, typeof(BuildUI).GetField(nameof(_accumulatedHeight), BindingFlags.Static | BindingFlags.NonPublic));
			});
		}

		private static void BuildBasePage(UIWorldCreation self) {
			var basePanel = new UIPanel {
				Width = StyleDimension.FromPixels(500f),
				Height = StyleDimension.FromPixels(452f),
				Top = StyleDimension.FromPixels(152f),
				HAlign = 0.5f,
			};
			basePanel.BackgroundColor.A = 255;

			var closeButton = new UIImageButton(LibAssets.CloseButton);
			closeButton.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => {
				_basePanel = basePanel;
				basePanel.RemoveChild(basePanel);
			};
			closeButton.OnUpdate += (UIElement affectedElement) => {
				if (affectedElement.IsMouseHovering) {
					Main.instance.MouseTextHackZoom("Close");
				}
			};

			for (int i = 0, c = LibAssets.BaseOrderGroups.Length; i < c; i++) {
				var button = new UIImageButtonWithRectangle(
						LibAssets.BaseOrderGroups[i],
						OGICallCache.orderGroupInstanceCallsCache2[i]()
					) {
					Width = StyleDimension.FromPixelsAndPercent(-4 * (c - 1), 1f / c),
					Left = StyleDimension.FromPercent(1f),
					HAlign = i / (c - 1)
				};
				button.OnLeftClick += (UIMouseEvent evt, UIElement listeningElement) => {
					self.Append(_basePanel);
				};
				button.Top.Set(_accumulatedHeight, 0f);
				button.SetSnapPoint($"baseordergroup{i}", i);
				self.Append(button);
			}

			basePanel.Append(closeButton);
			_basePanel = basePanel;
			_basePanel.Append(basePanel);
		}

		public void Unload() {
		}
	}
}
