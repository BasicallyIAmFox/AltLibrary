using AltLibrary.Common.AltBiomes;
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
    internal class WorldBiomeGeneration : ModSystem
    {
        public static int dungeonSide = 0;
        private const int beachBordersWidth = 275;
        private const int beachSandRandomCenter = beachBordersWidth + 45;
        private const int evilBiomeBeachAvoidance = beachSandRandomCenter + 60;
        private const int evilBiomeAvoidanceMidFixer = 50;

        internal static int worldCrimson;
        internal static bool worldCrimson2;
        internal static AltBiome worldCrimson3;

        public override void Unload()
        {
            dungeonSide = 0;
            worldCrimson = 0;
            worldCrimson2 = false;
            worldCrimson3 = null;
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
                tasks[corruptionIndex] = new PassLegacy("Corruption", new WorldGenLegacyMethod(WorldEvilAltTask));
                AltBiome biome = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil);
                if (biome.WorldGenPassLegacy != null)
                {
                    tasks.Insert(corruptionIndex + 1, biome.WorldGenPassLegacy);
                }
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
            //WorldGen.NotTheBees(); 
            // stinky re-logic and their private methods
            int grass = TileID.JungleGrass;
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
                ScanTileColumnAndRemoveClumps(i);
                float num835 = (i - 10) / rightBorder;
                progress.Set(0.2f + num835 * 0.8f);
            }
        }

        private static void ScanTileColumnAndRemoveClumps(int x)
        {
            int num = 0;
            int y = 0;
            for (int i = 10; i < Main.maxTilesY - 10; i++)
            {
                if (Main.tile[x, i].HasUnactuatedTile && Main.tileSolid[Main.tile[x, i].TileType] && TileID.Sets.CanBeClearedDuringGeneration[Main.tile[x, i].TileType])
                {
                    if (num == 0)
                    {
                        y = i;
                    }
                    num++;
                    continue;
                }
                if (num > 0 && num < 20)
                {
                    WorldGen.SmallConsecutivesFound++;
                    if (WorldGen.tileCounter(x, y) < 20)
                    {
                        WorldGen.SmallConsecutivesEliminated++;
                        WorldGen.tileCounterKill();
                    }
                }
                num = 0;
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
                bool worldCrimson2 = worldCrimson < 0;
                AltBiome worldCrimson3 = worldCrimson >= 0 ? AltLibrary.Biomes[worldCrimson] : null;
                WorldBiomeManager.drunkEvil = worldCrimson3 != null ? worldCrimson3.FullName : (worldCrimson == -333 ? "Terraria/Corruption" : "Terraria/Crimson");
                WorldBiomeGeneration.worldCrimson = worldCrimson;
                WorldBiomeGeneration.worldCrimson2 = worldCrimson2;
                WorldBiomeGeneration.worldCrimson3 = worldCrimson3;
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

        private void WorldEvilAltTask(GenerationProgress progress, GameConfiguration configuration)
        {
            int num677 = Main.maxTilesX;
            int num678 = 0;
            int num679 = Main.maxTilesX;
            int num680 = 0;
            Tile tile59;
            for (int num681 = 0; num681 < Main.maxTilesX; num681++)
            {
                for (int num682 = 0; num682 < Main.worldSurface; num682++)
                {
                    tile59 = Main.tile[num681, num682];
                    if (tile59.HasTile)
                    {
                        tile59 = Main.tile[num681, num682];
                        bool bl = tile59.TileType == TileID.JungleGrass;
                        foreach (AltBiome biome in AltLibrary.Biomes)
                        {
                            if (biome.BiomeType == BiomeType.Jungle && biome.BiomeJungleGrass.HasValue)
                                bl |= tile59.TileType == biome.BiomeJungleGrass.Value;
                        }
                        if (bl)
                        {
                            if (num681 < num677)
                            {
                                num677 = num681;
                            }
                            if (num681 > num678)
                            {
                                num678 = num681;
                            }
                            continue;
                        }
                        tile59 = Main.tile[num681, num682];
                        if (tile59.TileType != TileID.SnowBlock)
                        {
                            tile59 = Main.tile[num681, num682];
                            if (tile59.TileType == TileID.IceBlock)
                            {
                                goto IL_00a5;
                            }
                            continue;
                        }
                        goto IL_00a5;
                    }
                    continue;
                IL_00a5:
                    if (num681 < num679)
                    {
                        num679 = num681;
                    }
                    if (num681 > num680)
                    {
                        num680 = num681;
                    }
                }
            }
            int num683 = 10;
            num677 -= num683;
            num678 += num683;
            num679 -= num683;
            num680 += num683;
            int num684 = 500;
            int num685 = 100;
            bool flag46 = false;
            bool flag47 = true;
            double num686 = Main.maxTilesX * 0.00045;
            if (WorldGen.drunkWorldGen)
            {
                flag46 = true;
                num686 /= 2.0;
                if (WorldGen.genRand.NextBool(2))
                {
                    flag47 = false;
                }
            }
            if (flag46)
            {
                if (WorldBiomeManager.drunkEvil == "Terraria/Corruption")
                {
                    progress.Message = Lang.misc[20].Value;
                }
                else if (WorldBiomeManager.drunkEvil == "Terraria/Crimson")
                {
                    progress.Message = Lang.misc[71].Value;
                }
                else
                {
                    if (AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.drunkEvil).GenPassName != null)
                    {
                        progress.Message = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.drunkEvil).GenPassName.GetTranslation(Language.ActiveCulture);
                    }
                    else
                    {
                        progress.Message = "Creating " + AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.drunkEvil).DisplayName.GetTranslation(Language.ActiveCulture);
                    }
                }
                for (int num687 = 0; num687 < num686; num687++)
                {
                    int num688 = num679;
                    int num689 = num680;
                    int num690 = num677;
                    int num691 = num678;
                    float value15 = (float)(num687 / num686);
                    progress.Set(value15);
                    bool flag48 = false;
                    int num692 = 0;
                    int num693 = 0;
                    int num694 = 0;
                    while (!flag48)
                    {
                        flag48 = true;
                        int num695 = Main.maxTilesX / 2;
                        int num696 = 200;
                        if (WorldGen.drunkWorldGen)
                        {
                            num696 = 100;
                            num692 = (!flag47) ? WorldGen.genRand.Next((int)(Main.maxTilesX * 0.5), Main.maxTilesX - num684) : WorldGen.genRand.Next(num684, (int)(Main.maxTilesX * 0.5));
                        }
                        else
                        {
                            num692 = WorldGen.genRand.Next(num684, Main.maxTilesX - num684);
                        }
                        num693 = num692 - WorldGen.genRand.Next(200) - 100;
                        num694 = num692 + WorldGen.genRand.Next(200) + 100;
                        if (num693 < evilBiomeBeachAvoidance)
                        {
                            num693 = evilBiomeBeachAvoidance;
                        }
                        if (num694 > Main.maxTilesX - evilBiomeBeachAvoidance)
                        {
                            num694 = Main.maxTilesX - evilBiomeBeachAvoidance;
                        }
                        if (num692 < num693 + evilBiomeAvoidanceMidFixer)
                        {
                            num692 = num693 + evilBiomeAvoidanceMidFixer;
                        }
                        if (num692 > num694 - evilBiomeAvoidanceMidFixer)
                        {
                            num692 = num694 - evilBiomeAvoidanceMidFixer;
                        }
                        if (dungeonSide < 0 && num693 < 400)
                        {
                            num693 = 400;
                        }
                        else if (dungeonSide > 0 && num693 > Main.maxTilesX - 400)
                        {
                            num693 = Main.maxTilesX - 400;
                        }
                        if (num692 > num695 - num696 && num692 < num695 + num696)
                        {
                            flag48 = false;
                        }
                        if (num693 > num695 - num696 && num693 < num695 + num696)
                        {
                            flag48 = false;
                        }
                        if (num694 > num695 - num696 && num694 < num695 + num696)
                        {
                            flag48 = false;
                        }
                        if (num692 > WorldGen.UndergroundDesertLocation.X && num692 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                        {
                            flag48 = false;
                        }
                        if (num693 > WorldGen.UndergroundDesertLocation.X && num693 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                        {
                            flag48 = false;
                        }
                        if (num694 > WorldGen.UndergroundDesertLocation.X && num694 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                        {
                            flag48 = false;
                        }
                        if (num693 < dungeonSide + num685 && num694 > dungeonSide - num685)
                        {
                            flag48 = false;
                        }
                        if (num693 < num689 && num694 > num688)
                        {
                            num688++;
                            num689--;
                            flag48 = false;
                        }
                        if (num693 < num691 && num694 > num690)
                        {
                            num690++;
                            num691--;
                            flag48 = false;
                        }
                    }
                    for (int num697 = num693; num697 < num694; num697++)
                    {
                        for (int num698 = (int)WorldGen.worldSurfaceLow; num698 < Main.worldSurface - 1.0; num698++)
                        {
                            tile59 = Main.tile[num697, num698];
                            if (tile59.HasTile)
                            {
                                int num699 = num698 + WorldGen.genRand.Next(10, 14);
                                for (int num700 = num698; num700 < num699; num700++)
                                {
                                    tile59 = Main.tile[num697, num700];
                                    if (tile59.TileType != 59)
                                    {
                                        tile59 = Main.tile[num697, num700];
                                        bool bl = tile59.TileType == TileID.JungleGrass;
                                        foreach (AltBiome biome in AltLibrary.Biomes)
                                        {
                                            if (biome.BiomeType == BiomeType.Jungle)
                                                bl |= tile59.TileType != biome.BiomeGrass;
                                        }
                                        if (bl)
                                        {
                                            goto IL_0487;
                                        }
                                        continue;
                                    }
                                    goto IL_0487;
                                IL_0487:
                                    if (num697 >= num693 + WorldGen.genRand.Next(5) && num697 < num694 - WorldGen.genRand.Next(5))
                                    {
                                        tile59 = Main.tile[num697, num700];
                                        tile59.TileType = TileID.Dirt;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    double num701 = Main.worldSurface + 40.0;
                    for (int num702 = num693; num702 < num694; num702++)
                    {
                        num701 += WorldGen.genRand.Next(-2, 3);
                        if (num701 < Main.worldSurface + 30.0)
                        {
                            num701 = Main.worldSurface + 30.0;
                        }
                        if (num701 > Main.worldSurface + 50.0)
                        {
                            num701 = Main.worldSurface + 50.0;
                        }
                        int i2 = num702;
                        bool flag49 = false;
                        for (int num703 = (int)WorldGen.worldSurfaceLow; num703 < num701; num703++)
                        {
                            tile59 = Main.tile[i2, num703];
                            if (tile59.HasTile)
                            {
                                tile59 = Main.tile[i2, num703];
                                if (tile59.TileType == TileID.Sand && i2 >= num693 + WorldGen.genRand.Next(5) && i2 <= num694 - WorldGen.genRand.Next(5))
                                {
                                    tile59 = Main.tile[i2, num703];
                                    int value1 = TileID.Crimsand;
                                    int value2 = TileID.Ebonsand;
                                    int value3 = worldCrimson3 != null ? (worldCrimson3.BiomeSand ?? TileID.Sand) : TileID.Sand;
                                    tile59.TileType = (ushort)(worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                                }
                                tile59 = Main.tile[i2, num703];
                                if (tile59.TileType == TileID.Dirt && num703 < Main.worldSurface - 1.0 && !flag49 && worldCrimson3 != null && worldCrimson3.BiomeGrass.HasValue)
                                {
                                    typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
                                    WorldGen.SpreadGrass(i2, num703, 0, worldCrimson3.BiomeGrass.Value, true, 0);
                                }
                                flag49 = true;
                                tile59 = Main.tile[i2, num703];
                                if (tile59.WallType == 216)
                                {
                                    tile59 = Main.tile[i2, num703];
                                    tile59.WallType = 218;
                                }
                                else
                                {
                                    tile59 = Main.tile[i2, num703];
                                    if (tile59.WallType == 187)
                                    {
                                        tile59 = Main.tile[i2, num703];
                                        tile59.WallType = 221;
                                    }
                                }
                                tile59 = Main.tile[i2, num703];
                                if (tile59.TileType == TileID.Stone)
                                {
                                    if (i2 >= num693 + WorldGen.genRand.Next(5) && i2 <= num694 - WorldGen.genRand.Next(5))
                                    {
                                        tile59 = Main.tile[i2, num703];
                                        int value1 = TileID.Crimstone;
                                        int value2 = TileID.Ebonstone;
                                        int value3 = worldCrimson3 != null ? (worldCrimson3.BiomeStone ?? TileID.Stone) : TileID.Stone;
                                        tile59.TileType = (ushort)(worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                                    }
                                }
                                else
                                {
                                    tile59 = Main.tile[i2, num703];
                                    if (tile59.TileType == TileID.Grass)
                                    {
                                        tile59 = Main.tile[i2, num703];
                                        int value1 = TileID.CrimsonGrass;
                                        int value2 = TileID.CorruptGrass;
                                        int value3 = worldCrimson3 != null ? (worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                                        tile59.TileType = (ushort)(worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                                    }
                                    else
                                    {
                                        tile59 = Main.tile[i2, num703];
                                        if (tile59.TileType == TileID.IceBlock)
                                        {
                                            tile59 = Main.tile[i2, num703];
                                            int value1 = 200;
                                            int value2 = TileID.CorruptIce;
                                            int value3 = worldCrimson3 != null ? (worldCrimson3.BiomeIce ?? TileID.IceBlock) : TileID.IceBlock;
                                            tile59.TileType = (ushort)(worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                                        }
                                        else
                                        {
                                            tile59 = Main.tile[i2, num703];
                                            if (tile59.TileType == TileID.Sandstone)
                                            {
                                                tile59 = Main.tile[i2, num703];
                                                int value1 = TileID.CrimsonSandstone;
                                                int value2 = TileID.CorruptSandstone;
                                                int value3 = worldCrimson3 != null ? (worldCrimson3.BiomeSandstone ?? TileID.Sandstone) : TileID.Sandstone;
                                                tile59.TileType = (ushort)(worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                                            }
                                            else
                                            {
                                                tile59 = Main.tile[i2, num703];
                                                if (tile59.TileType == TileID.HardenedSand)
                                                {
                                                    tile59 = Main.tile[i2, num703];
                                                    int value1 = TileID.CrimsonHardenedSand;
                                                    int value2 = TileID.CorruptHardenedSand;
                                                    int value3 = worldCrimson3 != null ? (worldCrimson3.BiomeHardenedSand ?? TileID.HardenedSand) : TileID.HardenedSand;
                                                    tile59.TileType = (ushort)(worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    int num704 = WorldGen.genRand.Next(10, 15);
                    for (int num705 = 0; num705 < num704; num705++)
                    {
                        int num706 = 0;
                        bool flag50 = false;
                        int num707 = 0;
                        while (!flag50)
                        {
                            num706++;
                            int x27 = WorldGen.genRand.Next(num693 - num707, num694 + num707);
                            int num708 = WorldGen.genRand.Next((int)(Main.worldSurface - num707 / 2), (int)(Main.worldSurface + 100.0 + num707));
                            while (WorldGen.oceanDepths(x27, num708))
                            {
                                x27 = WorldGen.genRand.Next(num693 - num707, num694 + num707);
                                num708 = WorldGen.genRand.Next((int)(Main.worldSurface - num707 / 2), (int)(Main.worldSurface + 100.0 + num707));
                            }
                            if (num706 > 100)
                            {
                                num707++;
                                num706 = 0;
                            }
                            tile59 = Main.tile[x27, num708];
                            if (!tile59.HasTile)
                            {
                                while (true)
                                {
                                    tile59 = Main.tile[x27, num708];
                                    if (!tile59.HasTile)
                                    {
                                        num708++;
                                        continue;
                                    }
                                    break;
                                }
                                num708--;
                            }
                            else
                            {
                                while (true)
                                {
                                    tile59 = Main.tile[x27, num708];
                                    if (tile59.HasTile && num708 > Main.worldSurface)
                                    {
                                        num708--;
                                        continue;
                                    }
                                    break;
                                }
                            }
                            if (num707 <= 10)
                            {
                                tile59 = Main.tile[x27, num708 + 1];
                                if (tile59.HasTile)
                                {
                                    tile59 = Main.tile[x27, num708 + 1];
                                    if (worldCrimson3 != null && worldCrimson3.BiomeStone.HasValue && tile59.TileType == worldCrimson3.BiomeStone.Value)
                                    {
                                    }
                                }
                                goto IL_0a59;
                            }
                        IL_0a59:
                            if (num707 > 100)
                            {
                                flag50 = true;
                            }
                        }
                    }
                }
            }
            if (WorldGen.drunkWorldGen)
            {
                flag46 = false;
            }
            if (!flag46)
            {
                if (WorldBiomeManager.WorldEvil == "")
                {
                    progress.Message = Lang.misc[71].Value;
                }
                else
                {
                    if (AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).GenPassName != null)
                    {
                        progress.Message = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).GenPassName.GetTranslation(Language.ActiveCulture) ?? "Creating " + AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).DisplayName.GetTranslation(Language.ActiveCulture);
                    }
                    else
                    {
                        progress.Message = "Creating " + AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).DisplayName.GetTranslation(Language.ActiveCulture);
                    }
                }
                for (int num709 = 0; num709 < num686; num709++)
                {
                    int num710 = num679;
                    int num711 = num680;
                    int num712 = num677;
                    int num713 = num678;
                    float value16 = num709 / (float)num686;
                    progress.Set(value16);
                    bool flag51 = false;
                    int num715 = 0;
                    int num716 = 0;
                    while (!flag51)
                    {
                        flag51 = true;
                        int num717 = Main.maxTilesX / 2;
                        int num718 = 200;
                        int num714 = (!WorldGen.drunkWorldGen) ? WorldGen.genRand.Next(num684, Main.maxTilesX - num684) : (flag47 ? WorldGen.genRand.Next((int)(Main.maxTilesX * 0.5), Main.maxTilesX - num684) : WorldGen.genRand.Next(num684, (int)(Main.maxTilesX * 0.5)));
                        num715 = num714 - WorldGen.genRand.Next(200) - 100;
                        num716 = num714 + WorldGen.genRand.Next(200) + 100;
                        if (num715 < evilBiomeBeachAvoidance)
                        {
                            num715 = evilBiomeBeachAvoidance;
                        }
                        if (num716 > Main.maxTilesX - evilBiomeBeachAvoidance)
                        {
                            num716 = Main.maxTilesX - evilBiomeBeachAvoidance;
                        }
                        if (num714 < num715 + evilBiomeAvoidanceMidFixer)
                        {
                            num714 = num715 + evilBiomeAvoidanceMidFixer;
                        }
                        if (num714 > num716 - evilBiomeAvoidanceMidFixer)
                        {
                            num714 = num716 - evilBiomeAvoidanceMidFixer;
                        }
                        if (num714 > num717 - num718 && num714 < num717 + num718)
                        {
                            flag51 = false;
                        }
                        if (num715 > num717 - num718 && num715 < num717 + num718)
                        {
                            flag51 = false;
                        }
                        if (num716 > num717 - num718 && num716 < num717 + num718)
                        {
                            flag51 = false;
                        }
                        if (num714 > WorldGen.UndergroundDesertLocation.X && num714 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                        {
                            flag51 = false;
                        }
                        if (num715 > WorldGen.UndergroundDesertLocation.X && num715 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                        {
                            flag51 = false;
                        }
                        if (num716 > WorldGen.UndergroundDesertLocation.X && num716 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                        {
                            flag51 = false;
                        }
                        if (num715 < dungeonSide + num685 && num716 > dungeonSide - num685)
                        {
                            flag51 = false;
                        }
                        if (num715 < num711 && num716 > num710)
                        {
                            num710++;
                            num711--;
                            flag51 = false;
                        }
                        if (num715 < num713 && num716 > num712)
                        {
                            num712++;
                            num713--;
                            flag51 = false;
                        }
                    }
                    int num719 = 0;
                    for (int num720 = num715; num720 < num716; num720++)
                    {
                        if (num719 > 0)
                        {
                            num719--;
                        }
                        for (int num722 = (int)WorldGen.worldSurfaceLow; num722 < Main.worldSurface - 1.0; num722++)
                        {
                            tile59 = Main.tile[num720, num722];
                            if (tile59.HasTile)
                            {
                                int num723 = num722 + WorldGen.genRand.Next(10, 14);
                                for (int num724 = num722; num724 < num723; num724++)
                                {
                                    tile59 = Main.tile[num720, num724];
                                    if (tile59.TileType != TileID.Mud)
                                    {
                                        tile59 = Main.tile[num720, num724];
                                        bool bl = tile59.TileType == TileID.JungleGrass;
                                        foreach (AltBiome biome in AltLibrary.Biomes)
                                        {
                                            if (biome.BiomeType == BiomeType.Jungle && biome.BiomeJungleGrass.HasValue)
                                                bl |= tile59.TileType == biome.BiomeJungleGrass.Value;
                                        }
                                        if (bl)
                                        {
                                            goto IL_0e60;
                                        }
                                        continue;
                                    }
                                    goto IL_0e60;
                                IL_0e60:
                                    if (num720 >= num715 + WorldGen.genRand.Next(5) && num720 < num716 - WorldGen.genRand.Next(5))
                                    {
                                        tile59 = Main.tile[num720, num724];
                                        tile59.TileType = TileID.Dirt;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    double num725 = Main.worldSurface + 40.0;
                    for (int num726 = num715; num726 < num716; num726++)
                    {
                        num725 += WorldGen.genRand.Next(-2, 3);
                        if (num725 < Main.worldSurface + 30.0)
                        {
                            num725 = Main.worldSurface + 30.0;
                        }
                        if (num725 > Main.worldSurface + 50.0)
                        {
                            num725 = Main.worldSurface + 50.0;
                        }
                        int i2 = num726;
                        bool flag52 = false;
                        for (int num727 = (int)WorldGen.worldSurfaceLow; num727 < num725; num727++)
                        {
                            tile59 = Main.tile[i2, num727];
                            if (tile59.HasTile)
                            {
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Sand && i2 >= num715 + WorldGen.genRand.Next(5) && i2 <= num716 - WorldGen.genRand.Next(5) && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.Value;
                                }
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Dirt && num727 < Main.worldSurface - 1.0 && !flag52 && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue)
                                {
                                    typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
                                    WorldGen.SpreadGrass(i2, num727, 0, ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.Value, true, 0);
                                }
                                flag52 = true;
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Stone && i2 >= num715 + WorldGen.genRand.Next(5) && i2 <= num716 - WorldGen.genRand.Next(5) && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.Value;
                                }
                                tile59 = Main.tile[i2, num727];
                                if (ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(216) && tile59.WallType == 216)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(216, out ushort value);
                                    tile59.WallType = value;
                                }
                                else
                                {
                                    tile59 = Main.tile[i2, num727];
                                    if (ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(187) && tile59.WallType == 187)
                                    {
                                        tile59 = Main.tile[i2, num727];
                                        ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(187, out ushort value);
                                        tile59.WallType = value;
                                    }
                                }
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Grass && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.Value;
                                }
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.IceBlock && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.Value;
                                }
                                else
                                {
                                    tile59 = Main.tile[i2, num727];
                                    if (tile59.TileType == TileID.Sandstone && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue)
                                    {
                                        tile59 = Main.tile[i2, num727];
                                        tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.Value;
                                    }
                                    else
                                    {
                                        tile59 = Main.tile[i2, num727];
                                        if (tile59.TileType == TileID.HardenedSand && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue)
                                        {
                                            tile59 = Main.tile[i2, num727];
                                            tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
