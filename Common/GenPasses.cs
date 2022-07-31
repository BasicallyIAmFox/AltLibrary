using AltLibrary.Common.Systems;
using Mono.Cecil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AltLibrary.Common
{
	internal static class GenPasses
	{
		private static MethodBase ResetInfo;
		private static MethodBase ShiniesInfo;
		private static MethodBase UnderworldInfo;
		private static MethodBase AltarsInfo;
		private static MethodBase MicroBiomesInfo;
		private static MethodBase HardmodeWallsInfo;

		internal static event ILContext.Manipulator HookGenPassReset
		{
			add => HookEndpointManager.Modify(ResetInfo, value);
			remove => HookEndpointManager.Unmodify(ResetInfo, value);
		}

		internal static event ILContext.Manipulator HookGenPassShinies
		{
			add => HookEndpointManager.Modify(ShiniesInfo, value);
			remove => HookEndpointManager.Unmodify(ShiniesInfo, value);
		}

		internal static event ILContext.Manipulator HookGenPassUnderworld
		{
			add => HookEndpointManager.Modify(UnderworldInfo, value);
			remove => HookEndpointManager.Unmodify(UnderworldInfo, value);
		}

		internal static event ILContext.Manipulator HookGenPassAltars
		{
			add => HookEndpointManager.Modify(AltarsInfo, value);
			remove => HookEndpointManager.Unmodify(AltarsInfo, value);
		}

		internal static event ILContext.Manipulator HookGenPassMicroBiomes
		{
			add => HookEndpointManager.Modify(MicroBiomesInfo, value);
			remove => HookEndpointManager.Unmodify(MicroBiomesInfo, value);
		}

		internal static event ILContext.Manipulator HookGenPassHardmodeWalls
		{
			add => HookEndpointManager.Modify(HardmodeWallsInfo, value);
			remove => HookEndpointManager.Unmodify(HardmodeWallsInfo, value);
		}

		internal static void ILGenerateWorld(ILContext il)
		{
			ResetInfo = GetGenPassInfo(il, "Reset");
			ShiniesInfo = GetGenPassInfo(il, "Shinies");
			UnderworldInfo = GetGenPassInfo(il, "Underworld");
			AltarsInfo = GetGenPassInfo(il, "Altars");
			MicroBiomesInfo = GetGenPassInfo(il, "Micro Biomes");

			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchCall(typeof(Console).GetMethod(nameof(Console.WriteLine), BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(string), typeof(object[]) }))))
			{
				AltLibrary.Instance.Logger.Info("11 $ 1");
				return;
			}

			c.Index++;
			c.EmitDelegate(() =>
			{
				Console.WriteLine("World alts: Evil - {0} {1}, Tropic - {2} {3}, Underworld - {4} {5}, Good - {6} {7}", new object[]
				{
					WorldBiomeManager.WorldEvil == "" ? (WorldBiomeManager.IsCrimson ? -666 : -333) : AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).Type,
					!WorldBiomeManager.IsAnyModdedEvil ? "NONE" : WorldBiomeManager.WorldEvil,
					WorldBiomeManager.WorldJungle == "" ? -1 : AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldJungle).Type,
					WorldBiomeManager.WorldJungle == "" ? "NONE" : WorldBiomeManager.WorldJungle,
					WorldBiomeManager.WorldHell == "" ? -1 : AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldHell).Type,
					WorldBiomeManager.WorldHell == "" ? "NONE" : WorldBiomeManager.WorldHell,
					WorldBiomeManager.WorldHallow == "" ? -1 : AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldHallow).Type,
					WorldBiomeManager.WorldHallow == "" ? "NONE" : WorldBiomeManager.WorldHallow,
				});
			});
		}

		internal static void ILSMCallBack(ILContext il) => HardmodeWallsInfo = GetGenPassInfo(il, "Hardmode Walls");

		public static void Unload()
		{
			ResetInfo = null;
			ShiniesInfo = null;
			UnderworldInfo = null;
			AltarsInfo = null;
			MicroBiomesInfo = null;
			HardmodeWallsInfo = null;
		}

		private static MethodBase GetGenPassInfo(ILContext il, string name)
		{
			try
			{
				var c = new ILCursor(il);
				MethodReference methodReference = null;
				c.GotoNext(i => i.MatchLdstr(name));
				c.GotoNext(i => i.MatchLdftn(out methodReference));
				return methodReference.ResolveReflection();
			}
			catch (KeyNotFoundException e)
			{
				AltLibrary.Instance.Logger.Error($"Could not find GenPass with name {name}", e);
				return null;
			}
		}
	}
}
