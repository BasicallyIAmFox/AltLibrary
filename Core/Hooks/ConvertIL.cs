﻿using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using AltLibrary.Core.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Core.Hooks;

[FeedEventCore(nameof(Feed))]
internal static class ConvertIL {
	private static void Feed() {
		EventSystem.EarlyContentHook += Load;
	}

	private static void Load(Mod mod) {
		ILHelper.On<WorldGen>(nameof(WorldGen.Convert), (Action<int, int, int, int> orig, int i, int j, int conversionType, int size) => {
			var convType = ConversionHandler.ChangeVanillaConversionIdToModdedConversionId(conversionType);
			if (convType == -1) {
				return;
			}
			orig(i, j, convType, size);
		});
		ILHelper.IL<WorldGen>(nameof(WorldGen.Convert), (ILContext il) => {
			var c = new ILCursor(il);

			var iIndex = -1;
			var jIndex = -1;
			ILLabel endOfLoopLabel = null;

			var arrayDataIndex = c.AddVariable<ConversionData.Data>();
			c.Emit(OpCodes.Ldsfld, typeof(ConversionHandler).GetField("data", BindingFlags.Static | BindingFlags.NonPublic));
			c.Emit(OpCodes.Call, typeof(MemoryMarshal).GetMethods().Where(x => x.Name == nameof(MemoryMarshal.GetArrayDataReference) && x.IsGenericMethod && x.ContainsGenericParameters).First());
			c.Emit(OpCodes.Stloc, arrayDataIndex);

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
			c.GotoNext(
				i => i.MatchLdloc(out _),
				i => i.MatchLdloc(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchCall(typeof(WorldGen), nameof(WorldGen.InWorld)),
				i => i.MatchBrfalse(out endOfLoopLabel));
			c.GotoNext(
				i => i.MatchLdloc(out _),
				i => i.MatchLdarg(out iIndex),
				i => i.MatchSub(),
				i => i.MatchCall(typeof(Math), nameof(Math.Abs)));
			c.GotoNext(
				i => i.MatchLdloc(out _),
				i => i.MatchLdarg(out jIndex),
				i => i.MatchSub(),
				i => i.MatchCall(typeof(Math), nameof(Math.Abs)));
			c.GotoNext(MoveType.After,
				i => i.MatchAdd(),
				i => i.MatchLdcI4(out _),
				i => i.MatchBge(endOfLoopLabel));

			c.Emit(OpCodes.Ldarg, 2); // conversionType parameter index
			c.Emit(OpCodes.Call, typeof(SolutionLoader).FindMethod(nameof(SolutionLoader.Get)));
			c.Emit(OpCodes.Ldarg, iIndex);
			c.Emit(OpCodes.Ldarg, jIndex);
			c.Emit(OpCodes.Ldloc, arrayDataIndex);
			c.Emit(OpCodes.Call, typeof(ConversionHandler).FindMethod("ConvertInternal"));
			c.Emit(OpCodes.Br, endOfLoopLabel);
		});
	}
}