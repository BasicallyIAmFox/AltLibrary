using AltLibrary.Common.Cache;
using AltLibrary.Common.IL;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Linq;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Common.SelectableUIs;

public static partial class ScrollableUI {
	private class BuildUI : ILoadable {
		private static readonly FieldInfo UIWorldCreation__descriptionText = typeof(UIWorldCreation).GetField("_descriptionText", BindingFlags.Instance | BindingFlags.NonPublic);

		private static LibOptionButton<int>[] groupOptions;
		private static int chosenOption;

		public void Load(Mod mod) {
			ILHelper.IL<UIWorldCreation>("BuildPage", (ILContext il) => {
				var c = new ILCursor(il);
				
				c.GotoNext(MoveType.After,
					i => i.MatchLdcR4(out _));

				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_R4, 1000f);
			});
			ILHelper.On<UIWorldCreation>("AddWorldEvilOptions", (On_UIWorldCreation.orig_AddWorldEvilOptions orig, UIWorldCreation self, UIElement container, float accumualtedHeight, UIElement.MouseEvent clickEvent, string tagGroup, float usableWidthPercent) => {
				var oldChildrenLength = container.Children.Count();

				orig(self, container, accumualtedHeight, clickEvent, tagGroup, usableWidthPercent);

				var tempArray = container.Children.ToArray();
				for (int i = tempArray.Length - 1; i > tempArray.Length - (tempArray.Length - oldChildrenLength); i--) {
					tempArray[i].Remove();
				}

				int c = OGICallCache.orderGroupInstanceCallsCache.Length;
				groupOptions = new LibOptionButton<int>[c];
				for (int i = 0; i < c; i++) {
					var texture = OGICallCache.orderGroupInstanceCallsCache[i]();
					var color = OGICallCache.orderGroupInstanceCallsCache3[i]();
					var rectangle = OGICallCache.orderGroupInstanceCallsCache2[i]();

					var groupOptionButton = new LibOptionButton<int>(i,
						OGICallCache.sampleCache[i].Group.DisplayName,
						OGICallCache.sampleCache[i].Group.Description,
						color, texture, rectangle, 1f, 1f, 16f) {
						Width = StyleDimension.FromPixelsAndPercent(4 * (c - 3), 1f / c * usableWidthPercent),
						Left = StyleDimension.FromPercent(1f - usableWidthPercent),
						HAlign = (float)i / (c - 1)
					};
					groupOptionButton.Top.Set(accumualtedHeight, 0f);
					groupOptionButton.OnLeftMouseDown += (UIMouseEvent evt, UIElement listeningElement) => {
						chosenOption = groupOptionButton.OptionValue;
						for (int i = 0; i < groupOptions.Length; i++) {
							groupOptions[i].SetCurrentOption(chosenOption);
						}
					};
					groupOptionButton.OnMouseOver += (UIMouseEvent evt, UIElement listeningElement) => {
						var desc = groupOptionButton.Description;
						if (desc == null) {
							return;
						}
						((UIText)UIWorldCreation__descriptionText.GetValue(self)).SetText(desc);
					};
					groupOptionButton.OnMouseOut += self.ClearOptionDescription;
					groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
					container.Append(groupOptionButton);

					groupOptionButton.SetCurrentOption(-1);
					groupOptions[i] = groupOptionButton;
				}
			});
		}

		public void Unload() {
		}
	}
}
