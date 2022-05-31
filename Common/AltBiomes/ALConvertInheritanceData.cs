using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    public abstract class BlockParentageData
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

        public int GetConverted(int baseTile, Func<int, int> GetAltBlock, int ConversionType) {
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

    public class TileParentageData : BlockParentageData
    {
        public override void Bake()
        {
            // Mass Parenting

            for (int x = 0; x < TileLoader.TileCount; x++)
            {
                if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass)
                    Parent.Add(x, TileID.GolfGrass);
                else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass)
                    Parent.Add(x, TileID.Grass);
                else if (Main.tileMoss[x] && x != TileID.Stone)
                {
                    ForestConversion.Add(x, x); //prevents deconversion of moss to stone
                    Parent.Add(x, TileID.Stone);
                }
                else if (TileID.Sets.Conversion.Stone[x] && x != TileID.Stone)
                    Parent.Add(x, TileID.Stone);
                else if (TileID.Sets.Conversion.Ice[x] && x != TileID.IceBlock)
                    Parent.Add(x, TileID.IceBlock);
                else if (TileID.Sets.Conversion.Sandstone[x] && x != TileID.Sandstone)
                    Parent.Add(x, TileID.Sandstone);
                else if (TileID.Sets.Conversion.HardenedSand[x] && x != TileID.HardenedSand)
                    Parent.Add(x, TileID.HardenedSand);
                else if (TileID.Sets.Conversion.Sand[x] && x != TileID.Sand)
                    Parent.Add(x, TileID.Sand);
            }

            // Forest (this entire thing ensures that deconversion is possible

            ForestConversion.Add(TileID.Stone, TileID.Stone);
            ForestConversion.Add(TileID.Grass, TileID.Grass);
            ForestConversion.Add(TileID.GolfGrass, TileID.GolfGrass);
            ForestConversion.Add(TileID.IceBlock, TileID.IceBlock);
            ForestConversion.Add(TileID.Sand, TileID.Sand);
            ForestConversion.Add(TileID.HardenedSand, TileID.HardenedSand);
            ForestConversion.Add(TileID.Sandstone, TileID.Sandstone);
            ForestConversion.Add(TileID.JungleThorns, TileID.JungleThorns);

            BreakIfConversionFail.Add(TileID.JungleThorns, new());

            Parent.Add(TileID.JungleThorns, TileID.CorruptThorns); //hacky way to do jungle => corrupt one way conversion

            // Hallowed

            HallowConversion.Add(TileID.Stone, TileID.Pearlstone);
            HallowConversion.Add(TileID.Grass, TileID.HallowedGrass);
            HallowConversion.Add(TileID.GolfGrass, TileID.GolfGrassHallowed);
            HallowConversion.Add(TileID.IceBlock, TileID.HallowedIce);
            HallowConversion.Add(TileID.Sand, TileID.Pearlsand);
            HallowConversion.Add(TileID.HardenedSand, TileID.HallowHardenedSand);
            HallowConversion.Add(TileID.Sandstone, TileID.HallowSandstone);

            // Corruption

            CorruptionConversion.Add(TileID.Stone, TileID.Ebonstone);
            CorruptionConversion.Add(TileID.Grass, TileID.CorruptGrass);
            CorruptionConversion.Add(TileID.IceBlock, TileID.CorruptIce);
            CorruptionConversion.Add(TileID.Sand, TileID.Ebonsand);
            CorruptionConversion.Add(TileID.HardenedSand, TileID.CorruptHardenedSand);
            CorruptionConversion.Add(TileID.Sandstone, TileID.CorruptSandstone);
            CorruptionConversion.Add(TileID.CorruptThorns, TileID.CorruptThorns);

            BreakIfConversionFail.Add(TileID.CorruptThorns, new());

            // Crimson

            CrimsonConversion.Add(TileID.Stone, TileID.Crimstone);
            CrimsonConversion.Add(TileID.Grass, TileID.CrimsonGrass);
            CrimsonConversion.Add(TileID.IceBlock, TileID.FleshIce);
            CrimsonConversion.Add(TileID.Sand, TileID.Crimsand);
            CrimsonConversion.Add(TileID.HardenedSand, TileID.CorruptHardenedSand);
            CrimsonConversion.Add(TileID.Sandstone, TileID.CorruptSandstone);
            CrimsonConversion.Add(TileID.CorruptThorns, TileID.CrimsonThorns);

            BreakIfConversionFail.Add(TileID.CrimsonThorns, new());

            Parent.Add(TileID.CrimsonThorns, TileID.CorruptThorns);

            // Mushroom

            MushroomConversion.Add(TileID.JungleGrass, TileID.MushroomGrass);
            Parent.Add(TileID.MushroomGrass, TileID.JungleGrass);
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
                        if(i == TileID.JungleGrass)
                            test = TileID.JungleGrass;
                        else if (!ForestConversion.TryGetValue(i, out test))
                            test = -1;
                        break;
                }
                return test;
            }, convType);
        }
    }

    public class WallParentageData : BlockParentageData
    {

        const int GRASS_UNSAFE_DIFFERENT = -3;
        public override void Bake()
        {
            for (int x = 0; x < WallLoader.WallCount; x++)
            {

                if (WallID.Sets.Conversion.Stone[x] && x != WallID.Stone)
                    Parent.Add(x, WallID.Stone);
                else if (WallID.Sets.Conversion.HardenedSand[x] && x != WallID.HardenedSand)
                    Parent.Add(x, WallID.HardenedSand);
                else if (WallID.Sets.Conversion.Sandstone[x] && x != WallID.Sandstone)
                    Parent.Add(x, WallID.Sandstone);
                else if (WallID.Sets.Conversion.NewWall1[x] && x != WallID.RocksUnsafe1)
                    Parent.Add(x, WallID.RocksUnsafe1);
                else if (WallID.Sets.Conversion.NewWall2[x] && x != WallID.RocksUnsafe2)
                    Parent.Add(x, WallID.RocksUnsafe2);
                else if (WallID.Sets.Conversion.NewWall3[x] && x != WallID.RocksUnsafe3)
                    Parent.Add(x, WallID.RocksUnsafe3);
                else if (WallID.Sets.Conversion.NewWall4[x] && x != WallID.RocksUnsafe4)
                    Parent.Add(x, WallID.RocksUnsafe4);

                if (WallID.Sets.CanBeConvertedToGlowingMushroom[x])
                    MushroomConversion.Add(x, WallID.MushroomUnsafe);
            }

            //Manual Grass conversionating to ensure safe grass walls cannot become unsafe through green solution conversion

            Parent.Add(WallID.CorruptGrassUnsafe, GRASS_UNSAFE_DIFFERENT);
            Parent.Add(WallID.CrimsonGrassUnsafe, GRASS_UNSAFE_DIFFERENT);
            Parent.Add(WallID.HallowedGrassUnsafe, GRASS_UNSAFE_DIFFERENT);

            Parent.Add(GRASS_UNSAFE_DIFFERENT, WallID.Grass);
            Parent.Add(WallID.GrassUnsafe, WallID.Grass);
            Parent.Add(WallID.CorruptGrassEcho, WallID.Grass);
            Parent.Add(WallID.CrimsonGrassEcho, WallID.Grass);
            Parent.Add(WallID.HallowedGrassEcho, WallID.Grass);

            // Forest (this entire thing ensures that deconversion is possible)

            ForestConversion.Add(WallID.GrassUnsafe, WallID.GrassUnsafe);
            ForestConversion.Add(WallID.Stone, WallID.Stone);
            ForestConversion.Add(WallID.HardenedSand, WallID.HardenedSand);
            ForestConversion.Add(WallID.Sandstone, WallID.Sandstone);
            ForestConversion.Add(WallID.RocksUnsafe1, WallID.RocksUnsafe1);
            ForestConversion.Add(WallID.RocksUnsafe2, WallID.RocksUnsafe2);
            ForestConversion.Add(WallID.RocksUnsafe3, WallID.RocksUnsafe3);
            ForestConversion.Add(WallID.RocksUnsafe4, WallID.RocksUnsafe4);

            // Hallowed

            HallowConversion.Add(WallID.Grass, WallID.HallowedGrassUnsafe);
            HallowConversion.Add(WallID.Stone, WallID.PearlstoneBrickUnsafe);
            HallowConversion.Add(WallID.HardenedSand, WallID.HallowHardenedSand);
            HallowConversion.Add(WallID.Sandstone, WallID.HallowSandstone);
            HallowConversion.Add(WallID.RocksUnsafe1, WallID.HallowUnsafe1);
            HallowConversion.Add(WallID.RocksUnsafe2, WallID.HallowUnsafe2);
            HallowConversion.Add(WallID.RocksUnsafe3, WallID.HallowUnsafe3);
            HallowConversion.Add(WallID.RocksUnsafe4, WallID.HallowUnsafe4);

            // Corruption

            CorruptionConversion.Add(WallID.Grass, WallID.CorruptGrassUnsafe);
            CorruptionConversion.Add(WallID.Stone, WallID.EbonstoneUnsafe);
            CorruptionConversion.Add(WallID.HardenedSand, WallID.CorruptHardenedSand);
            CorruptionConversion.Add(WallID.Sandstone, WallID.CorruptSandstone);
            CorruptionConversion.Add(WallID.RocksUnsafe1, WallID.CorruptionUnsafe1);
            CorruptionConversion.Add(WallID.RocksUnsafe2, WallID.CorruptionUnsafe2);
            CorruptionConversion.Add(WallID.RocksUnsafe3, WallID.CorruptionUnsafe3);
            CorruptionConversion.Add(WallID.RocksUnsafe4, WallID.CorruptionUnsafe4);

            // Crimson

            CrimsonConversion.Add(WallID.Grass, WallID.CrimsonGrassUnsafe);
            CrimsonConversion.Add(WallID.Stone, WallID.CrimstoneUnsafe);
            CrimsonConversion.Add(WallID.HardenedSand, WallID.CrimsonHardenedSand);
            CrimsonConversion.Add(WallID.Sandstone, WallID.CrimsonSandstone);
            CrimsonConversion.Add(WallID.RocksUnsafe1, WallID.CrimsonUnsafe1);
            CrimsonConversion.Add(WallID.RocksUnsafe2, WallID.CrimsonUnsafe2);
            CrimsonConversion.Add(WallID.RocksUnsafe3, WallID.CrimsonUnsafe3);
            CrimsonConversion.Add(WallID.RocksUnsafe4, WallID.CrimsonUnsafe4);

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

    public static class ALConvertInheritanceData
    {
        internal static TileParentageData tileParentageData;
        internal static WallParentageData wallParentageData;

        public class ALConvertInheritanceData_Loader : ILoadable
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
    }
}
