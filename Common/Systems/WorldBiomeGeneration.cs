using AltLibrary.Common.AltBiomes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

// TODO:
// Make in Drunk Gen generate 1 additional random evil biome (1 chosen and second is random)

namespace AltLibrary.Common.Systems
{
    internal class WorldBiomeGeneration : ModSystem
    {
        public static int dungeonLocation = 0;
        private static readonly int beachBordersWidth = 275;
        private static readonly int beachSandRandomCenter = beachBordersWidth + 45;
        private static readonly int evilBiomeBeachAvoidance = beachSandRandomCenter + 60;
        private static readonly int evilBiomeAvoidanceMidFixer = 50;

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int resetIndex = tasks.FindIndex(genpass => genpass.Name == "Reset");
            if (resetIndex != -1)
            {
                tasks.Insert(resetIndex + 1, new PassLegacy("Alt Library Setup", new WorldGenLegacyMethod(WorldSetupTask)));
            }
            int corruptionIndex = tasks.FindIndex(i => i.Name.Equals("Corruption"));
            if (WorldBiomeManager.worldEvil != "" && corruptionIndex != -1)
            {
                tasks[corruptionIndex] = new PassLegacy("Corruption", new WorldGenLegacyMethod(WorldEvilAltTask));
            }
            if (WorldBiomeManager.worldHell != "")
            {
                int underworldIndex = tasks.FindIndex(i => i.Name.Equals("Underworld"));
                if (underworldIndex != -1)
                {
                    tasks[underworldIndex] = new PassLegacy("Underworld", new WorldGenLegacyMethod(WorldHellAltTask));
                }
                int hellforgeIndex = tasks.FindIndex(i => i.Name.Equals("Hellforge"));
                if (hellforgeIndex != -1)
                {
                    tasks.RemoveAt(hellforgeIndex);
                }
            }
        }

        private void WorldSetupTask(GenerationProgress progress, GameConfiguration configuration)
        {
            if (WorldBiomeManager.Copper == 0)
            {
                WorldGen.SavedOreTiers.Copper = TileID.Copper;
                WorldGen.copperBar = ItemID.CopperBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Copper = TileID.Tin;
                WorldGen.copperBar = ItemID.TinBar;
            }
            if (WorldBiomeManager.Iron == 0)
            {
                WorldGen.SavedOreTiers.Iron = TileID.Iron;
                WorldGen.ironBar = ItemID.IronBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Iron = TileID.Lead;
                WorldGen.ironBar = ItemID.LeadBar;
            }
            if (WorldBiomeManager.Silver == 0)
            {
                WorldGen.SavedOreTiers.Silver = TileID.Silver;
                WorldGen.silverBar = ItemID.SilverBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Silver = TileID.Tungsten;
                WorldGen.silverBar = ItemID.TungstenBar;
            }
            if (WorldBiomeManager.Gold == 0)
            {
                WorldGen.SavedOreTiers.Gold = TileID.Gold;
                WorldGen.goldBar = ItemID.GoldBar;
            }
            else
            {
                WorldGen.SavedOreTiers.Gold = TileID.Platinum;
                WorldGen.goldBar = ItemID.PlatinumBar;
            }
            WorldGen.SavedOreTiers.Cobalt = WorldBiomeManager.Cobalt == 0 ? TileID.Cobalt : TileID.Palladium;
            WorldGen.SavedOreTiers.Mythril = WorldBiomeManager.Mythril == 0 ? TileID.Mythril : TileID.Orichalcum;
            WorldGen.SavedOreTiers.Adamantite = WorldBiomeManager.Adamantite == 0 ? TileID.Adamantite : TileID.Titanium;
        }

        private void WorldHellAltTask(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = Lang.gen[18].Value;
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
                        if (ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.HasValue)
                        {
                            tile60.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.Value;
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
                if (WorldGen.genRand.NextBool(50) && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.HasValue)
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
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num743 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.Value, true, 0f, WorldGen.genRand.Next(1, 3), true, true, -1);
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
                    if ((!WorldGen.drunkWorldGen || WorldGen.genRand.NextBool(3) || !(num744 > Main.maxTilesX * 0.4) || !(num744 < Main.maxTilesX * 0.6)) && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.HasValue)
                    {
                        WorldGen.TileRunner(num744, num746 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.Value, true, 0f, WorldGen.genRand.Next(1, 3), true, true, -1);
                    }
                    float num747 = WorldGen.genRand.Next(1, 3);
                    if (WorldGen.genRand.NextBool(3))
                    {
                        num747 *= 0.5f;
                    }
                    if ((!WorldGen.drunkWorldGen || WorldGen.genRand.NextBool(3) || !(num744 > Main.maxTilesX * 0.4) || !(num744 < Main.maxTilesX * 0.6)) && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.HasValue)
                    {
                        if (WorldGen.genRand.NextBool(2))
                        {
                            WorldGen.TileRunner(num744, num746 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num747), (int)(WorldGen.genRand.Next(10, 15) * num747), ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.Value, true, 1f, 0.3f, false, true, -1);
                        }
                        if (WorldGen.genRand.NextBool(2))
                        {
                            num747 = WorldGen.genRand.Next(1, 3);
                            WorldGen.TileRunner(num744, num746 - WorldGen.genRand.Next(2, 5), (int)(WorldGen.genRand.Next(5, 15) * num747), (int)(WorldGen.genRand.Next(10, 15) * num747), ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeStone.Value, true, -1f, 0.3f, false, true, -1);
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
                if (ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeOre.HasValue)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).BiomeOre.Value, false, 0f, 0f, false, true, -1);
                }
            }
        }

        // Remove obsolete mark once added drunk worldgen
        [Obsolete]
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
                        foreach (AltBiome biome in AltLibrary.biomes)
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
                //flag46 = true;
                num686 /= 2.0;
                if (WorldGen.genRand.NextBool(2))
                {
                    flag47 = false;
                }
            }
            if (!flag46)
            {
                progress.Message = Lang.gen[20].Value;
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
                        if (num715 < dungeonLocation + num685 && num716 > dungeonLocation - num685)
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
                                        foreach (AltBiome biome in AltLibrary.biomes)
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
                                if (tile59.TileType == TileID.Sand && i2 >= num715 + WorldGen.genRand.Next(5) && i2 <= num716 - WorldGen.genRand.Next(5) && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSand.Value;
                                }
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Dirt && num727 < Main.worldSurface - 1.0 && !flag52 && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                                {
                                    typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
                                    WorldGen.SpreadGrass(i2, num727, 0, ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value, true, 0);
                                }
                                flag52 = true;
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Stone && i2 >= num715 + WorldGen.genRand.Next(5) && i2 <= num716 - WorldGen.genRand.Next(5) && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                                }
                                tile59 = Main.tile[i2, num727];
                                if (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(216) && tile59.WallType == 216)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(216, out ushort value);
                                    tile59.WallType = value;
                                }
                                else
                                {
                                    tile59 = Main.tile[i2, num727];
                                    if (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.ContainsKey(187) && tile59.WallType == 187)
                                    {
                                        tile59 = Main.tile[i2, num727];
                                        ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).WallContext.wallsReplacement.TryGetValue(187, out ushort value);
                                        tile59.WallType = value;
                                    }
                                }
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.Grass && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeGrass.Value;
                                }
                                tile59 = Main.tile[i2, num727];
                                if (tile59.TileType == TileID.IceBlock && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.HasValue)
                                {
                                    tile59 = Main.tile[i2, num727];
                                    tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeIce.Value;
                                }
                                else
                                {
                                    tile59 = Main.tile[i2, num727];
                                    if (tile59.TileType == TileID.Sandstone && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.HasValue)
                                    {
                                        tile59 = Main.tile[i2, num727];
                                        tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeSandstone.Value;
                                    }
                                    else
                                    {
                                        tile59 = Main.tile[i2, num727];
                                        if (tile59.TileType == TileID.HardenedSand && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.HasValue)
                                        {
                                            tile59 = Main.tile[i2, num727];
                                            tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeHardenedSand.Value;
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
