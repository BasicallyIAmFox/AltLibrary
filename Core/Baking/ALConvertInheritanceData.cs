using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Core.Baking
{
	internal abstract class BlockParentageData
	{
		//tiles
		public Dictionary<int, int> Parent = new();

		public Dictionary<int, int> ForestConversion = new();
		public Dictionary<int, int> HallowConversion = new();
		public Dictionary<int, int> CorruptionConversion = new();
		public Dictionary<int, int> CrimsonConversion = new();
		public Dictionary<int, int> MushroomConversion = new();

		public Dictionary<int, BitsByte> ForceDeconversion = new();
		public Dictionary<int, BitsByte> BreakIfConversionFail = new();

		public abstract int GetConverted_Vanilla(int baseTile, int ConversionType, int x, int y);

		public abstract void Bake();

		public abstract int GetConverted_Modded(int baseTile, AltBiome biome, int x, int y);

		public int GetConverted(int baseTile, Func<int, int> GetAltBlock, int ConversionType)
		{
			int ForcedConvertedTile = -1;
			while (true)
			{
				int test = GetAltBlock(baseTile);
				if (test != -1)
					return test;
				if (BreakIfConversionFail.TryGetValue(baseTile, out BitsByte bits))
				{
					if (bits[ConversionType])
						ForcedConvertedTile = -2; //change this to make use of spraytype
				}
				if (!Parent.TryGetValue(baseTile, out test))
					return ForcedConvertedTile;
				if (ForceDeconversion.TryGetValue(baseTile, out bits))
				{
					if (bits[ConversionType])
						ForcedConvertedTile = test; //change this to make use of spraytype
				}
				baseTile = test;
			}
		}
	}

	internal class TileParentageData : BlockParentageData
	{
		public override void Bake()
		{
			// Mass Parenting

			for (int x = 0; x < TileLoader.TileCount; x++)
			{
				if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass)
					Parent.TryAdd(x, TileID.GolfGrass);
				else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass)
					Parent.TryAdd(x, TileID.Grass);
				else if (Main.tileMoss[x] && x != TileID.Stone)
				{
					ForestConversion.TryAdd(x, x); //prevents deconversion of moss to stone
					Parent.TryAdd(x, TileID.Stone);
				}
				else if (TileID.Sets.Conversion.Stone[x] && x != TileID.Stone)
					Parent.TryAdd(x, TileID.Stone);
				else if (TileID.Sets.Conversion.Ice[x] && x != TileID.IceBlock)
					Parent.TryAdd(x, TileID.IceBlock);
				else if (TileID.Sets.Conversion.Sandstone[x] && x != TileID.Sandstone)
					Parent.TryAdd(x, TileID.Sandstone);
				else if (TileID.Sets.Conversion.HardenedSand[x] && x != TileID.HardenedSand)
					Parent.TryAdd(x, TileID.HardenedSand);
				else if (TileID.Sets.Conversion.Sand[x] && x != TileID.Sand)
					Parent.TryAdd(x, TileID.Sand);
			}

			// Forest (this entire thing ensures that deconversion is possible

			ForestConversion.TryAdd(TileID.Stone, TileID.Stone);
			ForestConversion.TryAdd(TileID.Grass, TileID.Grass);
			ForestConversion.TryAdd(TileID.GolfGrass, TileID.GolfGrass);
			ForestConversion.TryAdd(TileID.IceBlock, TileID.IceBlock);
			ForestConversion.TryAdd(TileID.Sand, TileID.Sand);
			ForestConversion.TryAdd(TileID.HardenedSand, TileID.HardenedSand);
			ForestConversion.TryAdd(TileID.Sandstone, TileID.Sandstone);
			ForestConversion.TryAdd(TileID.JungleThorns, TileID.JungleThorns);

			BreakIfConversionFail.TryAdd(TileID.JungleThorns, new(true, true, true, true));

			Parent.TryAdd(TileID.JungleThorns, TileID.CorruptThorns); //hacky way to do jungle => corrupt one way conversion

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
			CorruptionConversion.TryAdd(TileID.IceBlock, TileID.CorruptIce);
			CorruptionConversion.TryAdd(TileID.Sand, TileID.Ebonsand);
			CorruptionConversion.TryAdd(TileID.HardenedSand, TileID.CorruptHardenedSand);
			CorruptionConversion.TryAdd(TileID.Sandstone, TileID.CorruptSandstone);
			CorruptionConversion.TryAdd(TileID.CorruptThorns, TileID.CorruptThorns);

			BreakIfConversionFail.TryAdd(TileID.CorruptThorns, new(true, true, true, true));

			// Crimson

			CrimsonConversion.TryAdd(TileID.Stone, TileID.Crimstone);
			CrimsonConversion.TryAdd(TileID.Grass, TileID.CrimsonGrass);
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

		public override int GetConverted_Modded(int baseTile, AltBiome biome, int x, int y)
		{
			var convType = biome.BiomeType switch
			{
				BiomeType.Evil => 1,
				BiomeType.Hallow => 2,
				_ => 0,
			};
			return GetConverted(baseTile, i => biome.GetAltBlock(i, x, y), convType);
		}

		public override int GetConverted_Vanilla(int baseTile, int ConversionType, int x, int y)
		{
			var convType = ConversionType switch
			{
				1 or 4 => 1, //evil
				2 => 2, //hallow
				3 => 3, //mushroom
				_ => 0, //declentaminate
			};

			return GetConverted(baseTile, (i) =>
			{
				int test;
				switch (ConversionType)
				{
					case 1:
						if (!CorruptionConversion.TryGetValue(i, out test))
							test = -1;
						break;
					case 2:
						if (!HallowConversion.TryGetValue(i, out test))
							test = -1;
						break;
					case 3:
						if (!MushroomConversion.TryGetValue(i, out test))
							test = -1;
						break;
					case 4:
						if (!CrimsonConversion.TryGetValue(i, out test))
							test = -1;
						break;
					default:
						string worldJungle = WorldBiomeManager.WorldJungle;
						if (worldJungle != "")
							test = AltLibrary.Biomes.Find(x => x.FullName == worldJungle).GetAltBlock(i, x, y);
						else if (i == TileID.JungleGrass)
							test = TileID.JungleGrass;
						else if (!ForestConversion.TryGetValue(i, out test))
							test = -1;
						break;
				}
				return test;
			}, convType);
		}
	}

	internal class WallParentageData : BlockParentageData
	{
		const int GRASS_UNSAFE_DIFFERENT = -3;
		public override void Bake()
		{
			for (int x = 0; x < WallLoader.WallCount; x++)
			{
				if (WallID.Sets.Conversion.Grass[x] && x != WallID.Grass)
				{
					switch (x)
					{
						case WallID.CorruptGrassUnsafe:
						case WallID.CrimsonGrassUnsafe:
						case WallID.HallowedGrassUnsafe:
							Parent.TryAdd(x, GRASS_UNSAFE_DIFFERENT);
							break;
						default:
							Parent.TryAdd(x, WallID.Grass);
							break;
					}
				}
				else if (WallID.Sets.Conversion.Stone[x] && x != WallID.Stone)
					Parent.TryAdd(x, WallID.Stone);
				else if (WallID.Sets.Conversion.HardenedSand[x] && x != WallID.HardenedSand)
					Parent.TryAdd(x, WallID.HardenedSand);
				else if (WallID.Sets.Conversion.Sandstone[x] && x != WallID.Sandstone)
					Parent.TryAdd(x, WallID.Sandstone);
				else if (WallID.Sets.Conversion.NewWall1[x] && x != WallID.RocksUnsafe1)
					Parent.TryAdd(x, WallID.RocksUnsafe1);
				else if (WallID.Sets.Conversion.NewWall2[x] && x != WallID.RocksUnsafe2)
					Parent.TryAdd(x, WallID.RocksUnsafe2);
				else if (WallID.Sets.Conversion.NewWall3[x] && x != WallID.RocksUnsafe3)
					Parent.TryAdd(x, WallID.RocksUnsafe3);
				else if (WallID.Sets.Conversion.NewWall4[x] && x != WallID.RocksUnsafe4)
					Parent.TryAdd(x, WallID.RocksUnsafe4);

				if (WallID.Sets.CanBeConvertedToGlowingMushroom[x])
					MushroomConversion.TryAdd(x, WallID.MushroomUnsafe);
			}

			//Manual Grass conversionating to ensure safe grass walls cannot become unsafe through green solution conversion

			Parent.TryAdd(GRASS_UNSAFE_DIFFERENT, WallID.Grass);

			// Forest (this entire thing ensures that deconversion is possible)

			ForestConversion.TryAdd(WallID.GrassUnsafe, WallID.GrassUnsafe);
			ForestConversion.TryAdd(WallID.Stone, WallID.Stone);
			ForestConversion.TryAdd(WallID.HardenedSand, WallID.HardenedSand);
			ForestConversion.TryAdd(WallID.Sandstone, WallID.Sandstone);
			ForestConversion.TryAdd(WallID.RocksUnsafe1, WallID.RocksUnsafe1);
			ForestConversion.TryAdd(WallID.RocksUnsafe2, WallID.RocksUnsafe2);
			ForestConversion.TryAdd(WallID.RocksUnsafe3, WallID.RocksUnsafe3);
			ForestConversion.TryAdd(WallID.RocksUnsafe4, WallID.RocksUnsafe4);

			// Hallowed

			HallowConversion.TryAdd(WallID.Grass, WallID.HallowedGrassUnsafe);
			HallowConversion.TryAdd(WallID.Stone, WallID.PearlstoneBrickUnsafe);
			HallowConversion.TryAdd(WallID.HardenedSand, WallID.HallowHardenedSand);
			HallowConversion.TryAdd(WallID.Sandstone, WallID.HallowSandstone);
			HallowConversion.TryAdd(WallID.RocksUnsafe1, WallID.HallowUnsafe1);
			HallowConversion.TryAdd(WallID.RocksUnsafe2, WallID.HallowUnsafe2);
			HallowConversion.TryAdd(WallID.RocksUnsafe3, WallID.HallowUnsafe3);
			HallowConversion.TryAdd(WallID.RocksUnsafe4, WallID.HallowUnsafe4);

			// Corruption

			CorruptionConversion.TryAdd(WallID.Grass, WallID.CorruptGrassUnsafe);
			CorruptionConversion.TryAdd(WallID.Stone, WallID.EbonstoneUnsafe);
			CorruptionConversion.TryAdd(WallID.HardenedSand, WallID.CorruptHardenedSand);
			CorruptionConversion.TryAdd(WallID.Sandstone, WallID.CorruptSandstone);
			CorruptionConversion.TryAdd(WallID.RocksUnsafe1, WallID.CorruptionUnsafe1);
			CorruptionConversion.TryAdd(WallID.RocksUnsafe2, WallID.CorruptionUnsafe2);
			CorruptionConversion.TryAdd(WallID.RocksUnsafe3, WallID.CorruptionUnsafe3);
			CorruptionConversion.TryAdd(WallID.RocksUnsafe4, WallID.CorruptionUnsafe4);

			// Crimson

			CrimsonConversion.TryAdd(WallID.Grass, WallID.CrimsonGrassUnsafe);
			CrimsonConversion.TryAdd(WallID.Stone, WallID.CrimstoneUnsafe);
			CrimsonConversion.TryAdd(WallID.HardenedSand, WallID.CrimsonHardenedSand);
			CrimsonConversion.TryAdd(WallID.Sandstone, WallID.CrimsonSandstone);
			CrimsonConversion.TryAdd(WallID.RocksUnsafe1, WallID.CrimsonUnsafe1);
			CrimsonConversion.TryAdd(WallID.RocksUnsafe2, WallID.CrimsonUnsafe2);
			CrimsonConversion.TryAdd(WallID.RocksUnsafe3, WallID.CrimsonUnsafe3);
			CrimsonConversion.TryAdd(WallID.RocksUnsafe4, WallID.CrimsonUnsafe4);

			// Mushroom

		}

		public override int GetConverted_Modded(int baseTile, AltBiome biome, int x, int y)
		{
			var convType = biome.BiomeType switch
			{
				BiomeType.Evil => 1,
				BiomeType.Hallow => 2,
				_ => 0,
			};
			return GetConverted(baseTile, i =>
			{
				if (biome.WallContext.wallsReplacement.TryGetValue((ushort)i, out ushort rv))
				{
					return rv;
				}
				return -1;
			}, convType);
		}

		public override int GetConverted_Vanilla(int baseTile, int ConversionType, int x, int y)
		{
			var convType = ConversionType switch
			{
				1 or 4 => 1, //evil
				2 => 2, //hallow
				3 => 3, //mushroom
				_ => 0, //declentaminate
			};
			return GetConverted(baseTile, i =>
			{
				int test;
				switch (ConversionType)
				{
					case 1:
						if (!CorruptionConversion.TryGetValue(i, out test))
							test = -1;
						break;
					case 2:
						if (!HallowConversion.TryGetValue(i, out test))
							test = -1;
						break;
					case 3:
						if (!MushroomConversion.TryGetValue(i, out test))
							test = -1;
						break;
					case 4:
						if (!CrimsonConversion.TryGetValue(i, out test))
							test = -1;
						break;
					default:
						if (!ForestConversion.TryGetValue(i, out test))
						{
							//Hardcoded. You should be able to replicate this though in future iters
							if (i == GRASS_UNSAFE_DIFFERENT)
							{
								if (y < Main.worldSurface)
								{
									if (WorldGen.genRand.NextBool(10))
									{
										return 65;
									}
									else
									{
										return 63;
									}
								}
								else
								{
									return 64;
								}
							}
							if (i == WallID.MushroomUnsafe)
							{
								if (y < Main.worldSurface + 4.0 + WorldGen.genRand.Next(3) || y > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3))
								{
									test = 15;
								}
								else
								{
									test = 64;
								}
							}
							else
								test = -1;
						}
						break;
				}
				return test;
			}, convType);
		}
	}

	internal static class ALConvertInheritanceData
	{
		internal static TileParentageData tileParentageData;
		internal static WallParentageData wallParentageData;

		internal class ALConvertInheritanceData_Loader : ILoadable
		{
			public void Load(Mod mod)
			{
				tileParentageData = new();
				wallParentageData = new();
			}

			public void Unload()
			{
				tileParentageData = null;
				wallParentageData = null;
			}
		}

		public static void FillData()
		{
			// Mass Parenting
			tileParentageData.Bake();
			wallParentageData.Bake();
		}

		public static int GetConvertedTile_Vanilla(int baseTile, int ConversionType, int x, int y)
		{
			return tileParentageData.GetConverted_Vanilla(baseTile, ConversionType, x, y);
		}

		public static int GetConvertedTile_Modded(int baseTile, AltBiome biome, int x, int y)
		{
			return tileParentageData.GetConverted_Modded(baseTile, biome, x, y);
		}

		public static int GetConvertedWall_Vanilla(int baseWall, int ConversionType, int x, int y)
		{
			return wallParentageData.GetConverted_Vanilla(baseWall, ConversionType, x, y);
		}

		public static int GetConvertedWall_Modded(int baseWall, AltBiome biome, int x, int y)
		{
			return wallParentageData.GetConverted_Modded(baseWall, biome, x, y);
		}

		public static int GetUltimateParent(int baseTile)
		{
			while (true)
			{
				if (!tileParentageData.Parent.TryGetValue(baseTile, out int test))
					return baseTile;
				baseTile = test;
			}
		}

		public static Dictionary<int, int> GetTileParentDict()
		{
			return tileParentageData.Parent;
		}
		public static Dictionary<int, int> GetWallParentDict()
		{
			return wallParentageData.Parent;
		}

		public static void AddChildTile(int Block, int ParentBlock, BitsByte? ForceDeconvert = null, BitsByte? BreakIfConversionFail = null)
		{
			tileParentageData.Parent.Add(Block, ParentBlock);
			if (ForceDeconvert != null)
			{
				tileParentageData.ForceDeconversion.Add(Block, ForceDeconvert.Value);
			}
			if (BreakIfConversionFail != null)
			{
				tileParentageData.BreakIfConversionFail.Add(Block, BreakIfConversionFail.Value);
			}
		}

		public static void AddChildWall(int Wall, int ParentWall, BitsByte? ForceDeconvert = null, BitsByte? BreakIfConversionFail = null)
		{
			wallParentageData.Parent.Add(Wall, ParentWall);
			if (ForceDeconvert != null)
			{
				wallParentageData.ForceDeconversion.Add(Wall, ForceDeconvert.Value);
			}
			if (BreakIfConversionFail != null)
			{
				wallParentageData.BreakIfConversionFail.Add(Wall, BreakIfConversionFail.Value);
			}
		}
	}
}
