using AltLibrary.Common.AltBiomes;
using AltLibrary.Core.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AltLibrary.Common.Systems
{
    public class WorldBiomeGeneration : ModSystem
    {
        public static int DungeonSide { get; internal set; } = 0;
        public static int DungeonLocation { get; internal set; } = 0;
        private const int beachBordersWidth = 275;
        private const int beachSandRandomCenter = beachBordersWidth + 45;
        private const int evilBiomeBeachAvoidance = beachSandRandomCenter + 60;
        private const int evilBiomeAvoidanceMidFixer = 50;

        public static int WorldCrimson { get; internal set; }
        public static bool WorldCrimson2 { get; internal set; }
        public static AltBiome WorldCrimson3 { get; internal set; }

        public override void Unload()
        {
            DungeonSide = 0;
            DungeonLocation = 0;
            WorldCrimson = 0;
            WorldCrimson2 = false;
            WorldCrimson3 = null;
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int resetIndex = tasks.FindIndex(genpass => genpass.Name == "Reset");
            if (resetIndex != -1)
            {
                tasks.Insert(resetIndex + 1, new PassLegacy("Alt Library Setup", new WorldGenLegacyMethod(WorldSetupTask)));
            }
            int corruptionIndex = tasks.FindIndex(i => i.Name.Equals("Corruption"));
            if (WorldBiomeManager.WorldEvil != "" && corruptionIndex != -1)
            {
                tasks[corruptionIndex] = new PassLegacy("Corruption", new WorldGenLegacyMethod(EvilTaskGen));
            }
            if (WorldBiomeManager.WorldHell != "")
            {
                int underworldIndex = tasks.FindIndex(i => i.Name.Equals("Underworld"));
                if (underworldIndex != -1)
                {
                    tasks[underworldIndex] = new PassLegacy("Underworld", new WorldGenLegacyMethod(WorldHellAltTask));
                    AltBiome biome = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldHell);
                    if (biome.WorldGenPassLegacy != null)
                    {
                        tasks.Insert(underworldIndex + 1, biome.WorldGenPassLegacy);
                    }
                }
                int hellforgeIndex = tasks.FindIndex(i => i.Name.Equals("Hellforge"));
                if (hellforgeIndex != -1)
                {
                    tasks.RemoveAt(hellforgeIndex);
                }
            }
            if (WorldBiomeManager.WorldJungle != "")
            {
                int jungleIndex = tasks.FindIndex(i => i.Name.Equals("Wet Jungle"));
                if (jungleIndex != -1)
                {
                    tasks[jungleIndex] = new PassLegacy("Wet Jungle", new WorldGenLegacyMethod(JunglesWetTask)); // TODO: translatable genpass names. pass in display name of biome? 
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Mud Caves To Grass"));
                if (jungleIndex != -1)
                {
                    tasks[jungleIndex] = new PassLegacy("Mud Caves To Grass", new WorldGenLegacyMethod(JunglesGrassTask));
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Jungle Temple"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Hives"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Jungle Chests"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Jungle Chests Placement"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Temple"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Jungle Trees"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Jungle Plants"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Mud Walls In Jungle"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
                jungleIndex = tasks.FindIndex(i => i.Name.Equals("Lihzahrd Altars"));
                if (jungleIndex != -1)
                {
                    tasks.RemoveAt(jungleIndex);
                }
            }
        }

        private void EvilTaskGen(GenerationProgress progress, GameConfiguration configuration)
        {
            EvilBiomeGenerationPassHandler.GenerateAllCorruption(DungeonSide, DungeonLocation, progress);
        }

        private void JunglesWetTask(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Set(1f);
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                int i2 = i;
                for (int j = (int)WorldGen.worldSurfaceLow; j < Main.worldSurface - 1.0; j++)
                {
                    Tile tile49 = Main.tile[i2, j];
                    if (tile49.HasTile)
                    {
                        tile49 = Main.tile[i2, j];
                        bool bl = tile49.TileType == 60;
                        foreach (AltBiome biome in AltLibrary.Biomes)
                        {
                            if (biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
                            {
                                bl |= tile49.TileType == biome.BiomeGrass.Value;
                            }
                        }
                        if (bl)
                        {
                            tile49 = Main.tile[i2, j - 1];
                            tile49.LiquidAmount = 255;
                            tile49 = Main.tile[i2, j - 2];
                            tile49.LiquidAmount = 255;
                        }
                        break;
                    }
                }
            }
        }

        private void JunglesGrassTask(GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = Lang.gen[77].Value;
            typeof(WorldGen).GetMethod("NotTheBees", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, Array.Empty<object>());
            int grass = TileID.JungleGrass;
            if (!WorldGen.notTheBees)
            {
                foreach (AltBiome alt in AltLibrary.Biomes)
                {
                    if (alt.BiomeType == BiomeType.Jungle)
                    {
                        if (alt.BiomeGrass.HasValue)
                        {
                            grass = alt.BiomeGrass.Value;
                        }
                        else if (alt.BiomeJungleGrass.HasValue)
                        {
                            grass = alt.BiomeJungleGrass.Value;
                        }
                    }
                }
            }
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.tile[i, j].HasUnactuatedTile)
                    {
                        typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
                        WorldGen.SpreadGrass(i, j, TileID.Mud, grass, repeat: true, 0);
                    }
                    progress.Set(0.2f * ((i * Main.maxTilesY + j) / (float)(Main.maxTilesX * Main.maxTilesY)));
                }
            }
            WorldGen.SmallConsecutivesFound = 0;
            WorldGen.SmallConsecutivesEliminated = 0;
            float rightBorder = Main.maxTilesX - 20;
            for (int i = 10; i < Main.maxTilesX - 10; i++)
            {
                typeof(WorldGen).GetMethod("ScanTileColumnAndRemoveClumps", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(int) }).Invoke(null, new object[] { i });
                float num835 = (i - 10) / rightBorder;
                progress.Set(0.2f + num835 * 0.8f);
            }
        }

        private void WorldSetupTask(GenerationProgress progress, GameConfiguration configuration)
        {
            if (WorldBiomeManager.Copper == -1)
            {
                WorldGen.SavedOreTiers.Copper = TileID.Copper;
                WorldGen.copperBar = ItemID.CopperBar;
            }
            else if (WorldBiomeManager.Copper == -2)
            {
                WorldGen.SavedOreTiers.Copper = TileID.Tin;
                WorldGen.copperBar = ItemID.TinBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Copper = AltLibrary.Ores[WorldBiomeManager.Copper - 1].ore;
                WorldGen.copperBar = AltLibrary.Ores[WorldBiomeManager.Copper - 1].bar;
            }
            if (WorldBiomeManager.Iron == -3)
            {
                WorldGen.SavedOreTiers.Iron = TileID.Iron;
                WorldGen.ironBar = ItemID.IronBar;
            }
            else if (WorldBiomeManager.Iron == -4)
            {
                WorldGen.SavedOreTiers.Iron = TileID.Lead;
                WorldGen.ironBar = ItemID.LeadBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Iron = AltLibrary.Ores[WorldBiomeManager.Iron - 1].ore;
                WorldGen.ironBar = AltLibrary.Ores[WorldBiomeManager.Iron - 1].bar;
            }
            if (WorldBiomeManager.Silver == -5)
            {
                WorldGen.SavedOreTiers.Silver = TileID.Silver;
                WorldGen.silverBar = ItemID.SilverBar;
            }
            else if (WorldBiomeManager.Silver == -6)
            {
                WorldGen.SavedOreTiers.Silver = TileID.Tungsten;
                WorldGen.silverBar = ItemID.TungstenBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Silver = AltLibrary.Ores[WorldBiomeManager.Silver - 1].ore;
                WorldGen.silverBar = AltLibrary.Ores[WorldBiomeManager.Silver - 1].bar;
            }
            if (WorldBiomeManager.Gold == -7)
            {
                WorldGen.SavedOreTiers.Gold = TileID.Gold;
                WorldGen.goldBar = ItemID.GoldBar;
            }
            else if (WorldBiomeManager.Gold == -8)
            {
                WorldGen.SavedOreTiers.Gold = TileID.Platinum;
                WorldGen.goldBar = ItemID.PlatinumBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Gold = AltLibrary.Ores[WorldBiomeManager.Gold - 1].ore;
                WorldGen.goldBar = AltLibrary.Ores[WorldBiomeManager.Gold - 1].bar;
            }
            if (WorldBiomeManager.Cobalt == -9)
            {
                WorldGen.SavedOreTiers.Cobalt = TileID.Cobalt;
            }
            else if (WorldBiomeManager.Cobalt == -10)
            {
                WorldGen.SavedOreTiers.Cobalt = TileID.Palladium;
            }
            else
            {
                WorldGen.SavedOreTiers.Cobalt = AltLibrary.Ores[WorldBiomeManager.Cobalt - 1].ore;
            }
            if (WorldBiomeManager.Mythril == -11)
            {
                WorldGen.SavedOreTiers.Mythril = TileID.Mythril;
            }
            else if (WorldBiomeManager.Mythril == -12)
            {
                WorldGen.SavedOreTiers.Mythril = TileID.Orichalcum;
            }
            else
            {
                WorldGen.SavedOreTiers.Mythril = AltLibrary.Ores[WorldBiomeManager.Mythril - 1].ore;
            }
            if (WorldBiomeManager.Adamantite == -13)
            {
                WorldGen.SavedOreTiers.Adamantite = TileID.Adamantite;
            }
            else if (WorldBiomeManager.Adamantite == -14)
            {
                WorldGen.SavedOreTiers.Adamantite = TileID.Titanium;
            }
            else
            {
                WorldGen.SavedOreTiers.Adamantite = AltLibrary.Ores[WorldBiomeManager.Adamantite - 1].ore;
            }

            if (WorldGen.drunkWorldGen)
            {
                List<int> vs = new() { -333, -666 };
                AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable).ToList().ForEach(x => vs.Add(x.Type - 1));
                int index = WorldGen.genRand.Next(vs.Count);
                int current = !WorldGen.crimson ? (WorldBiomeManager.WorldEvil == "" ? -333 : AltLibrary.Biomes.FindIndex(x => x.Type == vs[index] + 1)) : -666;
                while (vs[index] == current)
                {
                    index = WorldGen.genRand.Next(vs.Count);
                    current = !WorldGen.crimson ? (WorldBiomeManager.WorldEvil == "" ? -333 : AltLibrary.Biomes.FindIndex(x => x.Type == vs[index] + 1)) : -666;
                }
                int worldCrimson = vs[index];
                bool worldCrimson2 = worldCrimson == -666;
                AltBiome worldCrimson3 = worldCrimson >= 0 ? AltLibrary.Biomes[worldCrimson] : null;
                WorldBiomeManager.drunkEvil = worldCrimson3 != null ? worldCrimson3.FullName : (!worldCrimson2 ? "Terraria/Corruption" : "Terraria/Crimson");
                WorldBiomeGeneration.WorldCrimson = worldCrimson;
                WorldBiomeGeneration.WorldCrimson2 = worldCrimson2;
                WorldBiomeGeneration.WorldCrimson3 = worldCrimson3;
            }
        }

        private void WorldHellAltTask(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldHell).GenPassName.GetTranslation(Language.ActiveCulture);
            progress.Set(0f);
            int num736 = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
            Tile tile60;
            for (int num737 = 0; num737 < Main.maxTilesX; num737++)
            {
                num736 += WorldGen.genRand.Next(-3, 4);
                if (num736 < Main.maxTilesY - 190)
                {
                    num736 = Main.maxTilesY - 190;
                }
                if (num736 > Main.maxTilesY - 160)
                {
                    num736 = Main.maxTilesY - 160;
                }
                for (int num738 = num736 - 20 - WorldGen.genRand.Next(3); num738 < Main.maxTilesY; num738++)
                {
                    if (num738 >= num736)
                    {
                        tile60 = Main.tile[num737, num738];
                        tile60.HasTile = false;
                        tile60 = Main.tile[num737, num738];
                        tile60.LiquidType = 0;
                        tile60 = Main.tile[num737, num738];
                        tile60.LiquidAmount = 0;
                    }
                    else
                    {
                        tile60 = Main.tile[num737, num738];
                        if (ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.HasValue)
                        {
                            tile60.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.Value;
                        }
                    }
                }
            }
            int num739 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
            for (int num740 = 10; num740 < Main.maxTilesX - 10; num740++)
            {
                num739 += WorldGen.genRand.Next(-10, 11);
                if (num739 > Main.maxTilesY - 60)
                {
                    num739 = Main.maxTilesY - 60;
                }
                if (num739 < Main.maxTilesY - 100)
                {
                    num739 = Main.maxTilesY - 120;
                }
                for (int num741 = num739; num741 < Main.maxTilesY - 10; num741++)
                {
                    tile60 = Main.tile[num740, num741];
                    if (!tile60.HasTile)
                    {
                        tile60 = Main.tile[num740, num741];
                        tile60.LiquidType = 1;
                        tile60 = Main.tile[num740, num741];
                        tile60.LiquidAmount = 255;
                    }
                }
            }
            for (int num742 = 0; num742 < Main.maxTilesX; num742++)
            {
                if (WorldGen.genRand.NextBool(50) && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.HasValue)
                {
                    int num743 = Main.maxTilesY - 65;
                    while (true)
                    {
                        tile60 = Main.tile[num742, num743];
                        if (!tile60.HasTile && num743 > Main.maxTilesY - 135)
                        {
                            num743--;
                            continue;
                        }
                        break;
                    }
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num743 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.Value, true, 0f, WorldGen.genRand.Next(1, 3), true, true, -1);
                }
            }
            Liquid.QuickWater(-2, -1, -1);
            for (int num744 = 0; num744 < Main.maxTilesX; num744++)
            {
                float num745 = num744 / (float)(Main.maxTilesX - 1);
                progress.Set(num745 / 2f + 0.5f);
                if (WorldGen.genRand.NextBool(13))
                {
                    int num746 = Main.maxTilesY - 65;
                    while (true)
                    {
                        tile60 = Main.tile[num744, num746];
                        if (tile60.LiquidAmount <= 0)
                        {
                            tile60 = Main.tile[num744, num746];
                            if (!tile60.HasTile)
                            {
                                break;
                            }
                        }
                        if (num746 > Main.maxTilesY - 140)
                        {
                            num746--;
                            continue;
                        }
                        break;
                    }
                    if ((!WorldGen.drunkWorldGen || WorldGen.genRand.NextBool(3) || !(num744 > Main.maxTilesX * 0.4) || !(num744 < Main.maxTilesX * 0.6)) && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.HasValue)
                    {
                        WorldGen.TileRunner(num744, num746 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.Value, true, 0f, WorldGen.genRand.Next(1, 3), true, true, -1);
                    }
                    float num747 = WorldGen.genRand.Next(1, 3);
                    if (WorldGen.genRand.NextBool(3))
                    {
                        num747 *= 0.5f;
                    }
                    if ((!WorldGen.drunkWorldGen || WorldGen.genRand.NextBool(3) || !(num744 > Main.maxTilesX * 0.4) || !(num744 < Main.maxTilesX * 0.6)) && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.HasValue)
                    {
                        if (WorldGen.genRand.NextBool(2))
                        {
                            WorldGen.TileRunner(num744, num746 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num747), (int)(WorldGen.genRand.Next(10, 15) * num747), ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.Value, true, 1f, 0.3f, false, true, -1);
                        }
                        if (WorldGen.genRand.NextBool(2))
                        {
                            num747 = WorldGen.genRand.Next(1, 3);
                            WorldGen.TileRunner(num744, num746 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num747), (int)(WorldGen.genRand.Next(10, 15) * num747), ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeStone.Value, true, -1f, 0.3f, false, true, -1);
                        }
                    }
                    WorldGen.TileRunner(num744 + WorldGen.genRand.Next(-10, 10), num746 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3), false, true, -1);
                    if (WorldGen.genRand.NextBool(3))
                    {
                        WorldGen.TileRunner(num744 + WorldGen.genRand.Next(-10, 10), num746 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3), false, true, -1);
                    }
                    if (WorldGen.genRand.NextBool(5))
                    {
                        WorldGen.TileRunner(num744 + WorldGen.genRand.Next(-15, 15), num746 + WorldGen.genRand.Next(-15, 10), WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3), false, true, -1);
                    }
                }
            }
            for (int num748 = 0; num748 < Main.maxTilesX; num748++)
            {
                WorldGen.TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2, false, 0f, 0f, false, true, -1);
            }
            if (WorldGen.drunkWorldGen)
            {
                for (int num749 = 0; num749 < Main.maxTilesX * 2; num749++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next((int)(Main.maxTilesX * 0.35), (int)(Main.maxTilesX * 0.65)), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(5, 20), WorldGen.genRand.Next(5, 10), -2, false, 0f, 0f, false, true, -1);
                }
            }
            for (int num750 = 0; num750 < Main.maxTilesX; num750++)
            {
                tile60 = Main.tile[num750, Main.maxTilesY - 145];
                if (!tile60.HasTile)
                {
                    tile60 = Main.tile[num750, Main.maxTilesY - 145];
                    tile60.LiquidAmount = 255;
                    tile60 = Main.tile[num750, Main.maxTilesY - 145];
                    tile60.LiquidType = 1;
                }
                tile60 = Main.tile[num750, Main.maxTilesY - 144];
                if (!tile60.HasTile)
                {
                    tile60 = Main.tile[num750, Main.maxTilesY - 144];
                    tile60.LiquidAmount = 255;
                    tile60 = Main.tile[num750, Main.maxTilesY - 144];
                    tile60.LiquidType = 1;
                }
            }
            for (int num751 = 0; num751 < (int)((Main.maxTilesX * Main.maxTilesY) * 0.0008); num751++)
            {
                if (ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeOre.HasValue)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeOre.Value, false, 0f, 0f, false, true, -1);
                }
            }
        }
    }
}
