using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Light;

namespace AltLibrary.Common.Hooks {
	internal static class UnderworldVisual {
		public static void Init() {
			EditsHelper.IL<Main>(nameof(Main.DrawUnderworldBackgroudLayer), Main_DrawUnderworldBackgroudLayer);
			EditsHelper.IL<TileLightScanner>(nameof(TileLightScanner.ApplyHellLight), TileLightScanner_ApplyHellLight);
		}

		public static void Unload() {
		}

		private static void TileLightScanner_ApplyHellLight(ILContext il) {
			var c = new ILCursor(il);

			try {
				int rIndex = -1;
				int gIndex = -1;
				int bIndex = -1;
				c.GotoNext(i => i.MatchLdloc(out rIndex));
				c.GotoNext(i => i.MatchLdloc(out gIndex));
				c.GotoNext(i => i.MatchLdloc(out bIndex));

				doIt();
				doIt();

				void doIt() {
					c.GotoNext(MoveType.After, i => i.MatchStloc(bIndex));

					c.Emit(OpCodes.Nop);

					ILLabel isNull = c.DefineLabel();
					ILLabel isNotNull = c.DefineLabel();
					c.Emit(OpCodes.Call, typeof(WorldBiomeManager).GetMethod(nameof(WorldBiomeManager.GetHellBiome), BindingFlags.Static | BindingFlags.Public, Array.Empty<Type>()));
					c.Emit(OpCodes.Dup);
					c.Emit(OpCodes.Brtrue_S, isNotNull);
					c.Emit(OpCodes.Pop);
					c.Emit(OpCodes.Br_S, isNull);
					c.MarkLabel(isNotNull);
					c.Emit(OpCodes.Ldloca_S, rIndex);
					c.Emit(OpCodes.Ldloca_S, gIndex);
					c.Emit(OpCodes.Ldloca_S, bIndex);
					c.Emit(OpCodes.Callvirt, typeof(AltBiome).GetMethod(nameof(AltBiome.ModifyHellLight), BindingFlags.Instance | BindingFlags.Public));
					c.MarkLabel(isNull);
				}
			}
			catch {
			}
		}

		private static void Main_DrawUnderworldBackgroudLayer(ILContext il) {
			var c = new ILCursor(il);

			try {
				c.GotoNext(MoveType.After, i => i.MatchStloc(2));

				c.Emit(OpCodes.Ldloc, 0);
				c.Emit(OpCodes.Ldloc, 2);
				c.EmitDelegate<Func<int, Texture2D, Texture2D>>((index, orig) => {
					if (WorldBiomeManager.WorldHell != "") {
						return WorldBiomeManager.GetHellBiome().AltUnderworldBackgrounds[index].Value;
					}
					return orig;
				});
				c.Emit(OpCodes.Stloc, 2);

				c.GotoNext(MoveType.After,
					i => i.MatchLdcI4(11),
					i => i.MatchLdcI4(3),
					i => i.MatchLdcI4(7),
					i => i.MatchNewobj<Color>());

				c.EmitDelegate<Func<Color, Color>>((orig) => {
					if (WorldBiomeManager.WorldHell != "") {
						return WorldBiomeManager.GetHellBiome().AltUnderworldColor;
					}
					return orig;
				});
			}
			catch {
			}
		}
	}
}
