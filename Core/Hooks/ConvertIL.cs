using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using AltLibrary.Core.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using System;
using System.Reflection;
using Terraria;

namespace AltLibrary.Core.Hooks;

[LoadableContent(ContentOrder.EarlyContent, nameof(Load))]
internal static class ConvertIL {
	public static void Load() {
		ILHelper.On<WorldGen>(nameof(WorldGen.Convert), (Action<int, int, int, int> orig, int i, int j, int conversionType, int size) => {
			orig(i, j, ConversionHandler.ChangeVanillaConversionIdToModdedConversionId(conversionType), size);
		});
		ILHelper.IL<WorldGen>(nameof(WorldGen.Convert), (ILContext il) => {
			var c = new ILCursor(il);

			var iIndex = -1;
			var jIndex = -1;
			ILLabel endOfLoopLabel = null;

			/*
			// if (WorldGen.InWorld(l, k, 1) && Math.Abs(l - i) + Math.Abs(k - j) < 6)
			IL_0012: ldloc.0
			IL_0013: ldloc.1
			IL_0014: ldc.i4.1
			IL_0015: call bool Terraria.WorldGen::InWorld(int32, int32, int32)
			// (no C# code)
			IL_001a: brfalse IL_180d

			IL_001f: ldloc.0
			IL_0020: ldarg.0
			IL_0021: sub
			IL_0022: call int32 [System.Runtime]System.Math::Abs(int32)
			IL_0027: ldloc.1
			IL_0028: ldarg.1
			IL_0029: sub
			IL_002a: call int32 [System.Runtime]System.Math::Abs(int32)
			IL_002f: add
			IL_0030: ldc.i4.6
			IL_0031: bge IL_180d
			 */
			c.GotoNext(MoveType.After,
				i => i.MatchLdloc(out _),
				i => i.MatchLdloc(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchCall(typeof(WorldGen), nameof(WorldGen.InWorld)),
				i => i.MatchBrfalse(out endOfLoopLabel),

				i => i.MatchLdloc(out _),
				i => i.MatchLdarg(out iIndex),
				i => i.MatchSub(),
				i => i.MatchCall(typeof(Math), nameof(Math.Abs)),
				i => i.MatchLdloc(out _),
				i => i.MatchLdarg(out jIndex),
				i => i.MatchSub(),
				i => i.MatchCall(typeof(Math), nameof(Math.Abs)),
				i => i.MatchAdd(),
				i => i.MatchLdcI4(out _),
				i => i.MatchBge(endOfLoopLabel));

			c.Emit(OpCodes.Ldarg, 2); // conversionType parameter index
			c.Emit(OpCodes.Call, typeof(SolutionLoader).FindMethod(nameof(SolutionLoader.Get)));
			c.Emit(OpCodes.Ldarg, iIndex);
			c.Emit(OpCodes.Ldarg, jIndex);
			c.Emit(OpCodes.Call, typeof(ConversionHandler).GetMethod(nameof(ConversionHandler.Convert), BindingFlags.Static | BindingFlags.Public, new Type[] { typeof(ModSolution), typeof(int), typeof(int) }));
			c.Emit(OpCodes.Br, endOfLoopLabel);
		});
	}
}
