using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AltLibrary
{
	internal class AltLibraryGlobalItem : GlobalItem
	{
		public static Dictionary<int, bool> HallowedOreList;
		public override void OnSpawn(Item item, IEntitySource source)
		{
			if (HallowedOreList.Count == 0 || WorldBiomeManager.WorldHallow == "")
				return;
			EntitySource_TileBreak tile = source as EntitySource_TileBreak;
			if (tile != null && HallowedOreList.ContainsKey(Main.tile[tile.TileCoords].TileType))
			{
				AltBiome biome = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldHallow);
				if (biome.BiomeOre != null)
					item.SetDefaults(biome.MechDropItemType.Value);
			}
		}

		class AltLibraryOre_Loader : ILoadable
		{
			public void Load(Mod mod)
			{
				On.Terraria.WorldGen.OreRunner += OreRunner_ReplaceHallowedOre;
				HallowedOreList = new Dictionary<int, bool>();
			}


			public void Unload()
			{
				On.Terraria.WorldGen.OreRunner -= OreRunner_ReplaceHallowedOre;
				HallowedOreList = null;
			}
			private static void OreRunner_ReplaceHallowedOre(On.Terraria.WorldGen.orig_OreRunner orig, int i, int j, double strength, int steps, ushort type)
			{
				if (HallowedOreList.Count == 0 || WorldBiomeManager.WorldHallow == "")
				{
					orig(i, j, strength, steps, type);
					return;
				}
				if (HallowedOreList.ContainsKey(type))
				{
					AltBiome biome = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldHallow);
					if (biome.BiomeOre != null)
						type = (ushort)biome.BiomeOre.Value;
				}
				orig(i, j, strength, steps, type);
			}
		}
	}
}
