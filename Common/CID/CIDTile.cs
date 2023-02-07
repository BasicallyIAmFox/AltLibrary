using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Biomes;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

public sealed class CIDTile : CIData {
	public Dictionary<int, BitsByte> TryKillTreesOnConversion = new();

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override void Bake() {
		for (int x = 0; x < TileLoader.TileCount; x++) {
			if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				Parent.TryAdd(x, TileID.GolfGrass);
				TryKillTreesOnConversion.TryAdd(x, new(false, true, true, true, true, true, true, true));
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass) {
				Parent.TryAdd(x, TileID.Grass);
				TryKillTreesOnConversion.TryAdd(x, new(false, true, true, true, true, true, true, true));
			}
			else if (TileID.Sets.Conversion.JungleGrass[x] && x != TileID.JungleGrass) {
				Parent.TryAdd(x, TileID.JungleGrass);
				TryKillTreesOnConversion.TryAdd(x, new(false, true, true, true, true, true, true, true));
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x]) && x != TileID.Stone) {
				PurityConversion.TryAdd(x, x);
				Parent.TryAdd(x, TileID.Stone);
			}
			else if (TileID.Sets.Conversion.Ice[x] && x != TileID.IceBlock) {
				Parent.TryAdd(x, TileID.IceBlock);
				TryKillTreesOnConversion.TryAdd(x, new(false, true, true, true, true, true, true, true));
			}
			else if (TileID.Sets.Conversion.Sandstone[x] && x != TileID.Sandstone) {
				Parent.TryAdd(x, TileID.Sandstone);
			}
			else if (TileID.Sets.Conversion.HardenedSand[x] && x != TileID.HardenedSand) {
				Parent.TryAdd(x, TileID.HardenedSand);
			}
			else if (TileID.Sets.Conversion.Sand[x] && x != TileID.Sand) {
				Parent.TryAdd(x, TileID.Sand);
				TryKillTreesOnConversion.TryAdd(x, new(false, true, true, true, true, true, true, true));
			}

			// Brown Solution
			if ((TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.Stone) {
				ForestConversion.TryAdd(x, TileID.Stone);
				TryKillTreesOnConversion.TryAdd(x, new(true, true, true, true, true, true, true, false));
			}
			else if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				ForestConversion.TryAdd(x, TileID.GolfGrass);
				TryKillTreesOnConversion.TryAdd(x, new(true, true, true, true, true, true, true, false));
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass && x != TileID.GolfGrass) {
				ForestConversion.TryAdd(x, TileID.Grass);
				TryKillTreesOnConversion.TryAdd(x, new(true, true, true, true, true, true, true, false));
			}

			// White Solution
			if ((TileID.Sets.Conversion.Grass[x] || TileID.Sets.Conversion.Sand[x] || TileID.Sets.Conversion.HardenedSand[x] || TileID.Sets.Conversion.Snow[x] || TileID.Sets.Conversion.Dirt[x]) && x != TileID.SnowBlock) {
				SnowConversion.TryAdd(x, TileID.SnowBlock);
				TryKillTreesOnConversion.TryAdd(x, new(true, true, true, true, true, true, false, true));
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.IceBlock) {
				SnowConversion.TryAdd(x, TileID.IceBlock);
				TryKillTreesOnConversion.TryAdd(x, new(true, true, true, true, true, true, false, true));
			}

			// TODO: Add Yellow Solution
		}

		// Purity
		PurityConversion.TryAdd(TileID.Stone, TileID.Stone);
		PurityConversion.TryAdd(TileID.Grass, TileID.Grass);
		PurityConversion.TryAdd(TileID.GolfGrass, TileID.GolfGrass);
		PurityConversion.TryAdd(TileID.JungleGrass, TileID.JungleGrass);
		PurityConversion.TryAdd(TileID.IceBlock, TileID.IceBlock);
		PurityConversion.TryAdd(TileID.Sand, TileID.Sand);
		PurityConversion.TryAdd(TileID.HardenedSand, TileID.HardenedSand);
		PurityConversion.TryAdd(TileID.Sandstone, TileID.Sandstone);
		PurityConversion.TryAdd(TileID.JungleThorns, TileID.JungleThorns);
		PurityConversion.TryAdd(TileID.MushroomGrass, TileID.Grass);

		TryKillTreesOnConversion.TryAdd(TileID.MushroomGrass, new(false, true, true, true, true, true, true, true));
		BreakIfConversionFail.TryAdd(TileID.JungleThorns, new(true, true, true, true, true, true, true, true));

		Parent.TryAdd(TileID.JungleThorns, TileID.CorruptThorns);
		Parent.TryAdd(TileID.JungleThorns, TileID.CrimsonThorns);

		// Hallowed
		HallowConversion.TryAdd(TileID.Stone, TileID.Pearlstone);
		HallowConversion.TryAdd(TileID.Grass, TileID.HallowedGrass);
		HallowConversion.TryAdd(TileID.GolfGrass, TileID.GolfGrassHallowed);
		HallowConversion.TryAdd(TileID.IceBlock, TileID.HallowedIce);
		HallowConversion.TryAdd(TileID.Sand, TileID.Pearlsand);
		HallowConversion.TryAdd(TileID.HardenedSand, TileID.HallowHardenedSand);
		HallowConversion.TryAdd(TileID.Sandstone, TileID.HallowSandstone);

		// Corruption
		CorruptionConversion.TryAdd(TileID.Stone, TileID.Ebonstone);
		CorruptionConversion.TryAdd(TileID.Grass, TileID.CorruptGrass);
		CorruptionConversion.TryAdd(TileID.JungleGrass, TileID.CorruptJungleGrass);
		CorruptionConversion.TryAdd(TileID.IceBlock, TileID.CorruptIce);
		CorruptionConversion.TryAdd(TileID.Sand, TileID.Ebonsand);
		CorruptionConversion.TryAdd(TileID.HardenedSand, TileID.CorruptHardenedSand);
		CorruptionConversion.TryAdd(TileID.Sandstone, TileID.CorruptSandstone);
		CorruptionConversion.TryAdd(TileID.CorruptThorns, TileID.CorruptThorns);

		BreakIfConversionFail.TryAdd(TileID.CorruptThorns, new(true, true, true, true, true, true, true, true));

		// Crimson
		CrimsonConversion.TryAdd(TileID.Stone, TileID.Crimstone);
		CrimsonConversion.TryAdd(TileID.Grass, TileID.CrimsonGrass);
		CrimsonConversion.TryAdd(TileID.JungleGrass, TileID.CrimsonGrass);
		CrimsonConversion.TryAdd(TileID.IceBlock, TileID.FleshIce);
		CrimsonConversion.TryAdd(TileID.Sand, TileID.Crimsand);
		CrimsonConversion.TryAdd(TileID.HardenedSand, TileID.CrimsonHardenedSand);
		CrimsonConversion.TryAdd(TileID.Sandstone, TileID.CrimsonSandstone);
		CrimsonConversion.TryAdd(TileID.CrimsonThorns, TileID.CrimsonThorns);

		BreakIfConversionFail.TryAdd(TileID.CrimsonThorns, new(true, true, true, true));

		Parent.TryAdd(TileID.CrimsonThorns, TileID.CorruptThorns);

		// Mushroom
		MushroomConversion.TryAdd(TileID.JungleGrass, TileID.MushroomGrass);
		Parent.TryAdd(TileID.MushroomGrass, TileID.JungleGrass);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override int GetConverted_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		return GetConverted(baseTile, in conversionType, in x, in y);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override int GetConverted_Modded(in int baseTile, in IAltBiome biome, in ushort x, in ushort y) {
		if (biome.Type == ModContent.GetInstance<CorruptBiome>().Type) {
			return GetConverted_Vanilla(in baseTile, BiomeConversionID.Corruption, in x, in y);
		}
		else if (biome.Type == ModContent.GetInstance<CrimsonBiome>().Type) {
			return GetConverted_Vanilla(in baseTile, BiomeConversionID.Crimson, in x, in y);
		}
		else if (biome.Type == ModContent.GetInstance<HallowBiome>().Type) {
			return GetConverted_Vanilla(in baseTile, BiomeConversionID.Hallow, in x, in y);
		}
		else if (biome.Type == ModContent.GetInstance<TropicsBiome>().Type) {
			return GetConverted_Vanilla(in baseTile, BiomeConversionID.Purity, in x, in y);
		}
		return biome.GetAltBlock(baseTile, in x, in y);
	}
}
