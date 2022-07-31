using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace AltLibrary.Common.Hooks
{
	internal class DrunkCrimsonFix
	{
		public static void Load()
		{
			IL.Terraria.Main.UpdateTime_StartDay += Main_UpdateTime_StartDay;
		}

		public static void Unload()
		{
			IL.Terraria.Main.UpdateTime_StartDay -= Main_UpdateTime_StartDay;
		}

		private static void Main_UpdateTime_StartDay(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
			{
				AltLibrary.Instance.Logger.Info("7 $ 1");
				return;
			}

			ILLabel skipVanilla = c.DefineLabel();

			if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.crimson))))
			{
				AltLibrary.Instance.Logger.Info("7 $ 2");
				return;
			}

			c.Emit(OpCodes.Br, skipVanilla);

			if (!c.TryGotoNext(i => i.MatchStsfld<WorldGen>(nameof(WorldGen.crimson))))
			{
				AltLibrary.Instance.Logger.Info("7 $ 2");
				return;
			}

			c.Index++;
			c.MarkLabel(skipVanilla);
			c.EmitDelegate(() =>
			{
				List<int> AllBiomes = new() { -333, -666 };
				AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().ForEach(x => AllBiomes.Add(x.Type));
				int gotIndex = AllBiomes[WorldBiomeManager.drunkIndex % AllBiomes.Count];
				if (gotIndex < 0)
				{
					WorldBiomeManager.WorldEvil = "";
				}
				else
				{
					WorldBiomeManager.WorldEvil = AltLibrary.Biomes.Find(x => x.Type == gotIndex).FullName;
				}
				WorldGen.crimson = gotIndex == -666;
				gotIndex = AllBiomes[(WorldBiomeManager.drunkIndex + 1) % AllBiomes.Count];
				if (gotIndex < 0)
				{
					WorldBiomeManager.drunkEvil = gotIndex == -666 ? "Terraria/Crimson" : "Terraria/Corruption";
				}
				else
				{
					WorldBiomeManager.drunkEvil = AltLibrary.Biomes.Find(x => x.Type == gotIndex).FullName;
				}
				WorldBiomeManager.drunkIndex++;
				if (WorldBiomeManager.drunkIndex >= AllBiomes.Count)
				{
					WorldBiomeManager.drunkIndex = 0;
				}
			});
		}
	}
}
