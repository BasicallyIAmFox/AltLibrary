using AltLibrary.Common.Assets;
using AltLibrary.Common.Cache;
using AltLibrary.Common.OrderGroups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Linq;
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
		private readonly Rectangle rectangle;
		private readonly Asset<Texture2D> _texture;
		private readonly Asset<Texture2D> _borderTexture;

		public UIImageButtonWithRectangle(Asset<Texture2D> texture, Rectangle rectangle) {
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
			ILHelper.IL<UIWorldCreation>("AddWorldEvilOptions", (ILContext il) => {
				var c = new ILCursor(il);

				c.Emit(OpCodes.Ldarg, 2);
				c.Emit(OpCodes.Stsfld, typeof(BuildUI).GetField(nameof(_accumulatedHeight), BindingFlags.Static | BindingFlags.NonPublic));
			});
			ILHelper.IL<UIWorldCreation>("SetDefaultOptions", (ILContext il) => {
				var c = new ILCursor(il);

				#region Difference
				/*
					C#:
						before:
							GroupOptionButton<WorldEvilId>[] evilButtons = this._evilButtons;
							for (int i = 0; i < evilButtons.Length; i++) {
								evilButtons[i].SetCurrentOption(WorldEvilId.Random);
							}
						after:
							goto skipEvilButtons;
							GroupOptionButton<WorldEvilId>[] evilButtons = this._evilButtons;
							for (int i = 0; i < evilButtons.Length; i++) {
								evilButtons[i].SetCurrentOption(WorldEvilId.Random);
							}
							skipEvilButtons:
					IL:
						before:
							// GroupOptionButton<WorldEvilId>[] evilButtons = this._evilButtons;
							IL_00cc: ldarg.0
							IL_00cd: ldfld class Terraria.GameContent.UI.Elements.GroupOptionButton`1<valuetype Terraria.GameContent.UI.States.UIWorldCreation/WorldEvilId>[] Terraria.GameContent.UI.States.UIWorldCreation::_evilButtons
							IL_00d2: stloc.1
							// for (int i = 0; i < evilButtons.Length; i++)
							IL_00d3: ldc.i4.0
							IL_00d4: stloc.s 11
							// (no C# code)
							IL_00d6: br.s IL_00eb
							// loop start (head: IL_00eb)
								IL_00d8: nop
								// evilButtons[i].SetCurrentOption(WorldEvilId.Random);
								IL_00d9: ldloc.1
								IL_00da: ldloc.s 11
								IL_00dc: ldelem.ref
								IL_00dd: ldc.i4.0
								IL_00de: callvirt instance void class Terraria.GameContent.UI.Elements.GroupOptionButton`1<valuetype Terraria.GameContent.UI.States.UIWorldCreation/WorldEvilId>::SetCurrentOption(!0)
								// (no C# code)
								IL_00e3: nop
								IL_00e4: nop
								// for (int i = 0; i < evilButtons.Length; i++)
								IL_00e5: ldloc.s 11
								IL_00e7: ldc.i4.1
								IL_00e8: add
								IL_00e9: stloc.s 11
								
								// for (int i = 0; i < evilButtons.Length; i++)
								IL_00eb: ldloc.s 11
								IL_00ed: ldloc.1
								IL_00ee: ldlen
								IL_00ef: conv.i4
								IL_00f0: clt
								IL_00f2: stloc.s 12
								// (no C# code)
								IL_00f4: ldloc.s 12
								IL_00f6: brtrue.s IL_00d8
							// end loop
							
							IL_00f8: ret
						after:
						[+] IL_0000: ldarg.0
						[+] IL_0000: pop
						[+] IL_0000: br IL_00f8
							// GroupOptionButton<WorldEvilId>[] evilButtons = this._evilButtons;
							IL_00cc: ldarg.0
							IL_00cd: ldfld class Terraria.GameContent.UI.Elements.GroupOptionButton`1<valuetype Terraria.GameContent.UI.States.UIWorldCreation/WorldEvilId>[] Terraria.GameContent.UI.States.UIWorldCreation::_evilButtons
							IL_00d2: stloc.1
							// for (int i = 0; i < evilButtons.Length; i++)
							IL_00d3: ldc.i4.0
							IL_00d4: stloc.s 11
							// (no C# code)
							IL_00d6: br.s IL_00eb
							// loop start (head: IL_00eb)
								IL_00d8: nop
								// evilButtons[i].SetCurrentOption(WorldEvilId.Random);
								IL_00d9: ldloc.1
								IL_00da: ldloc.s 11
								IL_00dc: ldelem.ref
								IL_00dd: ldc.i4.0
								IL_00de: callvirt instance void class Terraria.GameContent.UI.Elements.GroupOptionButton`1<valuetype Terraria.GameContent.UI.States.UIWorldCreation/WorldEvilId>::SetCurrentOption(!0)
								// (no C# code)
								IL_00e3: nop
								IL_00e4: nop
								// for (int i = 0; i < evilButtons.Length; i++)
								IL_00e5: ldloc.s 11
								IL_00e7: ldc.i4.1
								IL_00e8: add
								IL_00e9: stloc.s 11
								
								// for (int i = 0; i < evilButtons.Length; i++)
								IL_00eb: ldloc.s 11
								IL_00ed: ldloc.1
								IL_00ee: ldlen
								IL_00ef: conv.i4
								IL_00f0: clt
								IL_00f2: stloc.s 12
								// (no C# code)
								IL_00f4: ldloc.s 12
								IL_00f6: brtrue.s IL_00d8
							// end loop
							
							IL_00f8: ret
				 */
				#endregion

				ILHelper.CompleteLog(AltLibrary.Instance, c, true);

				var skipEvilButtons = c.DefineLabel();

				c.GotoNext(
					i => i.MatchLdarg(0),
					i => i.MatchLdfld<UIWorldCreation>("_evilButtons"),
					i => i.MatchStloc(out _)
				);

				c.Emit(OpCodes.Ldarg, 0);
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Br, skipEvilButtons);

				c.GotoNext(i => i.MatchRet());

				c.MarkLabel(skipEvilButtons);
				c.Emit(OpCodes.Nop);

				ILHelper.CompleteLog(AltLibrary.Instance, c, false);
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
		}

		public void Unload() {
		}
	}
}
