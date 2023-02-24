using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.IL;
using AltLibrary.Common.IO;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using System;
using Terraria.Graphics.Light;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.EarlyContent, nameof(Load))]
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
			c.Emit(OpCodes.Stloc, shouldAffectLightingIndex);

			/*
	// float num11 = 0f;
	IL_0000: ldc.r4 0.0
	IL_0005: stloc.0
	// float num10 = 0f;
	IL_0006: ldc.r4 0.0
	IL_000b: stloc.1
	// float num9 = 0f;
	IL_000c: ldc.r4 0.0
	IL_0011: stloc.2
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
	IL_0012: ldc.r4 0.55
	IL_0017: ldsfld float32 Terraria.Main::GlobalTimeWrappedHourly
	IL_001c: ldc.r4 2
	IL_0021: mul
	IL_0022: conv.r8
	IL_0023: call float64 [System.Runtime]System.Math::Sin(float64)
	IL_0028: conv.r4
	IL_0029: ldc.r4 0.08
	IL_002e: mul
	IL_002f: add
	IL_0030: stloc.3
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
		IL_0159: ldloc.3
		IL_015a: stloc.0
		// num10 = num8 * 0.6f;
		IL_015b: ldloc.3
		IL_015c: ldc.r4 0.6
		IL_0161: mul
		IL_0162: stloc.1
		// num9 = num8 * 0.2f;
		IL_0163: ldloc.3
		IL_0164: ldc.r4 0.2
		IL_0169: mul
		IL_016a: stloc.2
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
				c.Emit(OpCodes.Ldloca, rIndex);
				c.Emit(OpCodes.Ldloca, gIndex);
				c.Emit(OpCodes.Ldloca, bIndex);
				c.Emit(OpCodes.Ldloca, shouldAffectLightingIndex);
				c.Emit(OpCodes.Callvirt, typeof(IAltBiome).FindMethod(nameof(IAltBiome.ModifyUnderworldLighting)));
			};

			gotoNext();
			gotoNext();

			var skipTileLightingModificationLabel = c.DefineLabel();

			c.Emit(OpCodes.Ldloc, shouldAffectLightingIndex);
			c.Emit(OpCodes.Brtrue, skipTileLightingModificationLabel);

			/*
	// if (lightColor.X < num11)
	IL_02de: ldarg.s lightColor
	IL_02e0: ldfld float32 [FNA]Microsoft.Xna.Framework.Vector3::X
	IL_02e5: ldloc.0
	IL_02e6: bge.un.s IL_02f0

	// lightColor.X = num11;
	IL_02e8: ldarg.s lightColor
	IL_02ea: ldloc.0
	IL_02eb: stfld float32 [FNA]Microsoft.Xna.Framework.Vector3::X
			 */
			var tempLightColorIndex = 0;
			c.GotoNext(
				i => i.MatchLdarg(out tempLightColorIndex),
				i => i.MatchLdfld<Vector3>("X"),
				i => i.MatchLdloc(rIndex),
				i => i.MatchBgeUn(out _),

				i => i.MatchLdarg(tempLightColorIndex),
				i => i.MatchLdloc(rIndex),
				i => i.MatchStfld<Vector3>("X"));

			c.MarkLabel(skipTileLightingModificationLabel);
		});
	}
}
