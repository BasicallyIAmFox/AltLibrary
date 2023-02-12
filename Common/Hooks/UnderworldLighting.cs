using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.IL;
using AltLibrary.Common.IO;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Light;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class UnderworldLighting {
	private static void Load() {
		ILHelper.IL<TileLightScanner>("ApplyHellLight", (ILContext il) => {
			var c = new ILCursor(il);

			var shouldAffectLightingIndex = il.AddVariable<bool>();

			var rIndex = 0;
			var gIndex = 0;
			var bIndex = 0;
			var intensityIndex = 0;

			c.Emit(OpCodes.Ldc_I4, 0);
			c.Emit(OpCodes.Ldloc_S, (byte)shouldAffectLightingIndex);

			/*
	// float num11 = 0f;
	IL_0001: ldc.r4 0.0
	IL_0006: stloc.0
	// float num10 = 0f;
	IL_0007: ldc.r4 0.0
	IL_000c: stloc.1
	// float num9 = 0f;
	IL_000d: ldc.r4 0.0
	IL_0012: stloc.2
			 */
			c.GotoNext(
				i => i.MatchLdcR4(out _),
				i => i.MatchStloc(out rIndex),
				i => i.MatchLdcR4(out _),
				i => i.MatchStloc(out gIndex),
				i => i.MatchLdcR4(out _),
				i => i.MatchStloc(out bIndex));

			/*
	// float num8 = 0.55f + (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 2f)) * 0.08f;
	IL_0013: ldc.r4 0.55
	IL_0018: ldsfld float32 Terraria.Main::GlobalTimeWrappedHourly
	IL_001d: ldc.r4 2
	IL_0022: mul
	IL_0023: conv.r8
	IL_0024: call float64[System.Runtime]System.Math::Sin(float64)
	IL_0029: conv.r4
	IL_002a: ldc.r4 0.08
	IL_002f: mul
	IL_0030: add
	IL_0031: stloc.3
			*/
			c.GotoNext(
				i => i.MatchConvR4(),
				i => i.MatchLdcR4(out _),
				i => i.MatchMul(),
				i => i.MatchAdd(),
				i => i.MatchStloc(out intensityIndex));

			void gotoNext() {
				/*
		// num11 = num8;
		IL_0164: ldloc.3
		IL_0165: stloc.0
		// num10 = num8 * 0.6f;
		IL_0166: ldloc.3
		IL_0167: ldc.r4 0.6
		IL_016c: mul
		IL_016d: stloc.1
		// num9 = num8 * 0.2f;
		IL_016e: ldloc.3
		IL_016f: ldc.r4 0.2
		IL_0174: mul
		IL_0175: stloc.2
				*/
				c.GotoNext(MoveType.After,
					i => i.MatchLdloc(intensityIndex),
					i => i.MatchStloc(rIndex),

					i => i.MatchLdloc(intensityIndex),
					i => i.MatchLdcR4(out _),
					i => i.MatchMul(),
					i => i.MatchStloc(gIndex),

					i => i.MatchLdloc(intensityIndex),
					i => i.MatchLdcR4(out _),
					i => i.MatchMul(),
					i => i.MatchStloc(bIndex));

				c.Emit(OpCodes.Call, typeof(WorldDataManager).GetMethod(nameof(WorldDataManager.GetUnderworld), 0, Array.Empty<Type>()));
				c.Emit(OpCodes.Ldloca_S, (byte)rIndex);
				c.Emit(OpCodes.Ldloca_S, (byte)gIndex);
				c.Emit(OpCodes.Ldloca_S, (byte)bIndex);
				c.Emit(OpCodes.Ldloca_S, (byte)shouldAffectLightingIndex);
				c.Emit(OpCodes.Call, typeof(IAltBiome).FindMethod(nameof(IAltBiome.ModifyUnderworldLighting)));
			};

			gotoNext();
			gotoNext();

			var skipTileLightingModificationLabel = c.DefineLabel();

			c.Emit(OpCodes.Ldloc_S, (byte)shouldAffectLightingIndex);
			c.Emit(OpCodes.Brtrue_S, skipTileLightingModificationLabel);

			/*
	IL_02f2: nop

	// if (lightColor.X < num11)
	IL_02f3: ldarg.s lightColor
	IL_02f5: ldfld float32 [FNA]Microsoft.Xna.Framework.Vector3::X
	IL_02fa: ldloc.0
	IL_02fb: clt
	IL_02fd: stloc.s 11
	// (no C# code)
	IL_02ff: ldloc.s 11
	IL_0301: brfalse.s IL_030b

	// lightColor.X = num11;
	IL_0303: ldarg.s lightColor
	IL_0305: ldloc.0
	IL_0306: stfld float32 [FNA]Microsoft.Xna.Framework.Vector3::X
			 */
			c.GotoNext(
				i => i.MatchLdarg(out _),
				i => i.MatchLdfld<Vector3>("X"),
				i => i.MatchLdloc(rIndex),
				i => i.MatchBgeUn(out _));

			c.MarkLabel(skipTileLightingModificationLabel);
		});
	}
}
