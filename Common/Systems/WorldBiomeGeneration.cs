using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
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
                if (hellforgeIndex != -1 && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).AltarTile.HasValue)
                {
                    tasks[hellforgeIndex] = new PassLegacy("Hellforge", new WorldGenLegacyMethod(WorldHellForgeAlt));
                }
            }
        }

        private void WorldHellForgeAlt(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = Lang.gen[36].Value;
            for (int num371 = 0; num371 < Main.maxTilesX / 200; num371++)
            {
                float value2 = num371 / (Main.maxTilesX / 200);
                progress.Set(value2);
                bool flag23 = false;
                int num372 = 0;
                while (!flag23)
                {
                    int num373 = WorldGen.genRand.Next(1, Main.maxTilesX);
                    int num374 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 30);
                    try
                    {
                        Tile tile30 = Main.tile[num373, num374];
                        if (tile30.WallType != 13)
                        {
                            tile30 = Main.tile[num373, num374];
                            if (tile30.WallType == 14)
                            {
                                goto IL_00aa;
                            }
                            goto end_IL_006a;
                        }
                        goto IL_00aa;
                    IL_00aa:
                        while (true)
                        {
                            tile30 = Main.tile[num373, num374];
                            if (!tile30.HasTile && num374 < Main.maxTilesY - 20)
                            {
                                num374++;
                                continue;
                            }
                            break;
                        }
                        num374--;
                        WorldGen.PlaceTile(num373, num374, ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).AltarTile.Value, false, false, -1, 0);
                        tile30 = Main.tile[num373, num374];
                        if (tile30.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).AltarTile.Value)
                        {
                            flag23 = true;
                        }
                        else
                        {
                            num372++;
                            if (num372 >= 10000)
                            {
                                flag23 = true;
                            }
                        }
                    end_IL_006a:;
                    }
                    catch
                    {
                        num372++;
                        if (num372 >= 10000)
                        {
                            flag23 = true;
                        }
                    }
                }
            }
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
            AddHellHouses();
        }

        // Remove obsolete mark once added compatibility with custom Hell biomes
        [Obsolete]
        private void AddHellHouses()
        {
            int num68 = (int)(Main.maxTilesX * 0.25);
            Tile tile2;
            for (int i2 = 100; i2 < Main.maxTilesX - 100; i2++)
            {
                if ((!WorldGen.drunkWorldGen || i2 <= num68 || i2 >= Main.maxTilesX - num68) && (WorldGen.drunkWorldGen || (i2 >= num68 && i2 <= Main.maxTilesX - num68)))
                {
                    int num4 = Main.maxTilesY - 40;
                    while (true)
                    {
                        tile2 = Main.tile[i2, num4];
                        if (!tile2.HasTile)
                        {
                            tile2 = Main.tile[i2, num4];
                            if (tile2.LiquidAmount <= 0)
                            {
                                break;
                            }
                        }
                        num4--;
                    }
                    tile2 = Main.tile[i2, num4 + 1];
                    if (tile2.HasTile)
                    {
                        ushort num3 = (ushort)WorldGen.genRand.Next(75, 77);
                        byte wallType = 13;
                        if (WorldGen.genRand.Next(5) > 0)
                        {
                            num3 = 75;
                        }
                        if (num3 == 75)
                        {
                            wallType = 14;
                        }
                        if (WorldGen.getGoodWorldGen)
                        {
                            num3 = 76;
                        }
                        WorldGen.HellFort(i2, num4, num3, wallType);
                        i2 += WorldGen.genRand.Next(30, 130);
                        if (WorldGen.genRand.NextBool(10))
                        {
                            i2 += WorldGen.genRand.Next(0, 200);
                        }
                    }
                }
            }
            float num67 = Main.maxTilesX / 4200;
            for (int n = 0; n < 200f * num67; n++)
            {
                int num8 = 0;
                bool flag3 = false;
                while (!flag3)
                {
                    num8++;
                    int num7 = WorldGen.genRand.Next((int)(Main.maxTilesX * 0.2), (int)(Main.maxTilesX * 0.8));
                    int num6 = WorldGen.genRand.Next(Main.maxTilesY - 300, Main.maxTilesY - 20);
                    tile2 = Main.tile[num7, num6];
                    if (tile2.HasTile)
                    {
                        tile2 = Main.tile[num7, num6];
                        if (tile2.TileType != 75)
                        {
                            tile2 = Main.tile[num7, num6];
                            if (tile2.TileType == 76)
                            {
                                goto IL_0217;
                            }
                            goto IL_0322;
                        }
                        goto IL_0217;
                    }
                    goto IL_0322;
                IL_0322:
                    if (num8 > 1000)
                    {
                        flag3 = true;
                    }
                    continue;
                IL_0217:
                    int num5 = 0;
                    tile2 = Main.tile[num7 - 1, num6];
                    if (tile2.WallType > 0)
                    {
                        num5 = -1;
                    }
                    else
                    {
                        tile2 = Main.tile[num7 + 1, num6];
                        if (tile2.WallType > 0)
                        {
                            num5 = 1;
                        }
                    }
                    tile2 = Main.tile[num7 + num5, num6];
                    if (!tile2.HasTile)
                    {
                        tile2 = Main.tile[num7 + num5, num6 + 1];
                        if (!tile2.HasTile)
                        {
                            bool flag2 = false;
                            for (int j = num7 - 8; j < num7 + 8; j++)
                            {
                                for (int i = num6 - 8; i < num6 + 8; i++)
                                {
                                    tile2 = Main.tile[j, i];
                                    if (tile2.HasTile)
                                    {
                                        bool[] torch = TileID.Sets.Torch;
                                        tile2 = Main.tile[j, i];
                                        if (torch[tile2.TileType])
                                        {
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!flag2)
                            {
                                WorldGen.PlaceTile(num7 + num5, num6, 4, true, true, -1, 7);
                                flag3 = true;
                            }
                        }
                    }
                    goto IL_0322;
                }
            }
            float num66 = 4200000f / Main.maxTilesX;
            for (int m = 0; m < num66; m++)
            {
                int num23 = 0;
                int num22 = WorldGen.genRand.Next(num68, Main.maxTilesX - num68);
                int l = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
                do
                {
                    tile2 = Main.tile[num22, l];
                    if (tile2.WallType != 13)
                    {
                        tile2 = Main.tile[num22, l];
                        if (tile2.WallType != 14)
                        {
                            goto IL_039a;
                        }
                    }
                    tile2 = Main.tile[num22, l];
                    if (!tile2.HasTile)
                    {
                        break;
                    }
                    goto IL_039a;
                IL_039a:
                    num22 = WorldGen.genRand.Next(num68, Main.maxTilesX - num68);
                    l = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
                    if (WorldGen.drunkWorldGen)
                    {
                        num22 = (!WorldGen.genRand.NextBool(2)) ? WorldGen.genRand.Next(Main.maxTilesX - num68, Main.maxTilesX - 50) : WorldGen.genRand.Next(50, num68);
                    }
                    num23++;
                }
                while (num23 <= 100000);
                if (num23 <= 100000)
                {
                    tile2 = Main.tile[num22, l];
                    if (tile2.WallType != 13)
                    {
                        tile2 = Main.tile[num22, l];
                        if (tile2.WallType == 14)
                        {
                            goto IL_04b8;
                        }
                        continue;
                    }
                    goto IL_04b8;
                }
                continue;
            IL_05d3:
                int num16;
                int num17;
                if (WorldGen.SolidTile(num16, l + 1, false))
                {
                    int style27 = 16;
                    int style26 = 13;
                    int style25 = 14;
                    int style24 = 49;
                    int style23 = 4;
                    int style22 = 8;
                    int style21 = 15;
                    int style20 = 9;
                    int style19 = 10;
                    int style18 = 17;
                    int style17 = 25;
                    int style16 = 25;
                    int style15 = 23;
                    int style14 = 25;
                    int num15 = WorldGen.genRand.Next(13);
                    int num14 = 0;
                    int num13 = 0;
                    if (num15 == 0)
                    {
                        num14 = 5;
                        num13 = 4;
                    }
                    if (num15 == 1)
                    {
                        num14 = 4;
                        num13 = 3;
                    }
                    if (num15 == 2)
                    {
                        num14 = 3;
                        num13 = 5;
                    }
                    if (num15 == 3)
                    {
                        num14 = 4;
                        num13 = 6;
                    }
                    if (num15 == 4)
                    {
                        num14 = 3;
                        num13 = 3;
                    }
                    if (num15 == 5)
                    {
                        num14 = 5;
                        num13 = 3;
                    }
                    if (num15 == 6)
                    {
                        num14 = 5;
                        num13 = 4;
                    }
                    if (num15 == 7)
                    {
                        num14 = 5;
                        num13 = 4;
                    }
                    if (num15 == 8)
                    {
                        num14 = 5;
                        num13 = 4;
                    }
                    if (num15 == 9)
                    {
                        num14 = 3;
                        num13 = 5;
                    }
                    if (num15 == 10)
                    {
                        num14 = 5;
                        num13 = 3;
                    }
                    if (num15 == 11)
                    {
                        num14 = 2;
                        num13 = 4;
                    }
                    if (num15 == 12)
                    {
                        num14 = 3;
                        num13 = 3;
                    }
                    for (int num12 = num16 - num14; num12 <= num16 + num14; num12++)
                    {
                        for (int num9 = l - num13; num9 <= l; num9++)
                        {
                            tile2 = Main.tile[num12, num9];
                            if (tile2.HasTile)
                            {
                                num15 = -1;
                                break;
                            }
                        }
                    }
                    if (num17 < num14 * 1.75)
                    {
                        num15 = -1;
                    }
                    switch (num15)
                    {
                        case 0:
                            {
                                WorldGen.PlaceTile(num16, l, 14, true, false, -1, style26);
                                int num10 = WorldGen.genRand.Next(6);
                                if (num10 < 3)
                                {
                                    WorldGen.PlaceTile(num16 + num10, l - 2, 33, true, false, -1, style16);
                                }
                                tile2 = Main.tile[num16, l];
                                if (tile2.HasTile)
                                {
                                    tile2 = Main.tile[num16 - 2, l];
                                    if (!tile2.HasTile)
                                    {
                                        WorldGen.PlaceTile(num16 - 2, l, 15, true, false, -1, style27);
                                        tile2 = Main.tile[num16 - 2, l];
                                        if (tile2.HasTile)
                                        {
                                            tile2 = Main.tile[num16 - 2, l];
                                            tile2.TileFrameX += 18;
                                            tile2 = Main.tile[num16 - 2, l - 1];
                                            tile2.TileFrameY += 18;
                                        }
                                    }
                                    tile2 = Main.tile[num16 + 2, l];
                                    if (!tile2.HasTile)
                                    {
                                        WorldGen.PlaceTile(num16 + 2, l, 15, true, false, -1, style27);
                                    }
                                }
                                break;
                            }
                        case 1:
                            {
                                WorldGen.PlaceTile(num16, l, 18, true, false, -1, style25);
                                int num11 = WorldGen.genRand.Next(4);
                                if (num11 < 2)
                                {
                                    WorldGen.PlaceTile(num16 + num11, l - 1, 33, true, false, -1, style16);
                                }
                                tile2 = Main.tile[num16, l];
                                if (tile2.HasTile)
                                {
                                    if (WorldGen.genRand.NextBool(2))
                                    {
                                        tile2 = Main.tile[num16 - 1, l];
                                        if (!tile2.HasTile)
                                        {
                                            WorldGen.PlaceTile(num16 - 1, l, 15, true, false, -1, style27);
                                            tile2 = Main.tile[num16 - 1, l];
                                            if (tile2.HasTile)
                                            {
                                                tile2 = Main.tile[num16 - 1, l];
                                                tile2.TileFrameX += 18;
                                                tile2 = Main.tile[num16 - 1, l - 1];
                                                tile2.TileFrameX += 18;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tile2 = Main.tile[num16 + 2, l];
                                        if (!tile2.HasTile)
                                        {
                                            WorldGen.PlaceTile(num16 + 2, l, 15, true, false, -1, style27);
                                        }
                                    }
                                }
                                break;
                            }
                        case 2:
                            WorldGen.PlaceTile(num16, l, 105, true, false, -1, style24);
                            break;
                        case 3:
                            WorldGen.PlaceTile(num16, l, 101, true, false, -1, style23);
                            break;
                        case 4:
                            if (WorldGen.genRand.NextBool(2))
                            {
                                WorldGen.PlaceTile(num16, l, 15, true, false, -1, style27);
                                tile2 = Main.tile[num16, l];
                                tile2.TileFrameX += 18;
                                tile2 = Main.tile[num16, l - 1];
                                tile2.TileFrameX += 18;
                            }
                            else
                            {
                                WorldGen.PlaceTile(num16, l, 15, true, false, -1, style27);
                            }
                            break;
                        case 5:
                            if (WorldGen.genRand.NextBool(2))
                            {
                                WorldGen.Place4x2(num16, l, 79, 1, style22);
                            }
                            else
                            {
                                WorldGen.Place4x2(num16, l, 79, -1, style22);
                            }
                            break;
                        case 6:
                            WorldGen.PlaceTile(num16, l, 87, true, false, -1, style21);
                            break;
                        case 7:
                            WorldGen.PlaceTile(num16, l, 88, true, false, -1, style20);
                            break;
                        case 8:
                            WorldGen.PlaceTile(num16, l, 89, true, false, -1, style19);
                            break;
                        case 9:
                            WorldGen.PlaceTile(num16, l, 104, true, false, -1, style18);
                            break;
                        case 10:
                            if (WorldGen.genRand.NextBool(2))
                            {
                                WorldGen.Place4x2(num16, l, 90, 1, style14);
                            }
                            else
                            {
                                WorldGen.Place4x2(num16, l, 90, -1, style14);
                            }
                            break;
                        case 11:
                            WorldGen.PlaceTile(num16, l, 93, true, false, -1, style15);
                            break;
                        case 12:
                            WorldGen.PlaceTile(num16, l, 100, true, false, -1, style17);
                            break;
                    }
                }
                continue;
            IL_04b8:
                tile2 = Main.tile[num22, l];
                if (!tile2.HasTile)
                {
                    for (; !WorldGen.SolidTile(num22, l, false) && l < Main.maxTilesY - 20; l++)
                    {
                    }
                    l--;
                    int num21 = num22;
                    int num20 = num22;
                    while (true)
                    {
                        tile2 = Main.tile[num21, l];
                        if (!tile2.HasTile && WorldGen.SolidTile(num21, l + 1, false))
                        {
                            num21--;
                            continue;
                        }
                        break;
                    }
                    num21++;
                    while (true)
                    {
                        tile2 = Main.tile[num20, l];
                        if (!tile2.HasTile && WorldGen.SolidTile(num20, l + 1, false))
                        {
                            num20++;
                            continue;
                        }
                        break;
                    }
                    num20--;
                    num17 = num20 - num21;
                    num16 = (num20 + num21) / 2;
                    tile2 = Main.tile[num16, l];
                    if (!tile2.HasTile)
                    {
                        tile2 = Main.tile[num16, l];
                        if (tile2.WallType != 13)
                        {
                            tile2 = Main.tile[num16, l];
                            if (tile2.WallType == 14)
                            {
                                goto IL_05d3;
                            }
                            continue;
                        }
                        goto IL_05d3;
                    }
                }
            }
            num66 = 420000f / Main.maxTilesX;
            for (int num64 = 0; num64 < num66; num64++)
            {
                int num52 = 0;
                int num51 = WorldGen.genRand.Next(num68, Main.maxTilesX - num68);
                int num50 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
                do
                {
                    tile2 = Main.tile[num51, num50];
                    if (tile2.WallType != 13)
                    {
                        tile2 = Main.tile[num51, num50];
                        if (tile2.WallType != 14)
                        {
                            goto IL_0b7c;
                        }
                    }
                    tile2 = Main.tile[num51, num50];
                    if (!tile2.HasTile)
                    {
                        break;
                    }
                    goto IL_0b7c;
                IL_0b7c:
                    num51 = WorldGen.genRand.Next(num68, Main.maxTilesX - num68);
                    num50 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
                    if (WorldGen.drunkWorldGen)
                    {
                        num51 = (!WorldGen.genRand.NextBool(2)) ? WorldGen.genRand.Next(Main.maxTilesX - num68, Main.maxTilesX - 50) : WorldGen.genRand.Next(50, num68);
                    }
                    num52++;
                }
                while (num52 <= 100000);
                if (num52 <= 100000)
                {
                    int num49 = num51;
                    int num48 = num51;
                    int num47 = num50;
                    int num46 = num50;
                    int num45 = 0;
                    for (int num44 = 0; num44 < 2; num44++)
                    {
                        num49 = num51;
                        num48 = num51;
                        while (true)
                        {
                            tile2 = Main.tile[num49, num50];
                            if (tile2.HasTile)
                            {
                                break;
                            }
                            tile2 = Main.tile[num49, num50];
                            if (tile2.WallType != 13)
                            {
                                tile2 = Main.tile[num49, num50];
                                if (tile2.WallType != 14)
                                {
                                    break;
                                }
                            }
                            num49--;
                        }
                        num49++;
                        while (true)
                        {
                            tile2 = Main.tile[num48, num50];
                            if (tile2.HasTile)
                            {
                                break;
                            }
                            tile2 = Main.tile[num48, num50];
                            if (tile2.WallType != 13)
                            {
                                tile2 = Main.tile[num48, num50];
                                if (tile2.WallType != 14)
                                {
                                    break;
                                }
                            }
                            num48++;
                        }
                        num48--;
                        num51 = (num49 + num48) / 2;
                        num47 = num50;
                        num46 = num50;
                        while (true)
                        {
                            tile2 = Main.tile[num51, num47];
                            if (tile2.HasTile)
                            {
                                break;
                            }
                            tile2 = Main.tile[num51, num47];
                            if (tile2.WallType != 13)
                            {
                                tile2 = Main.tile[num51, num47];
                                if (tile2.WallType != 14)
                                {
                                    break;
                                }
                            }
                            num47--;
                        }
                        num47++;
                        while (true)
                        {
                            tile2 = Main.tile[num51, num46];
                            if (tile2.HasTile)
                            {
                                break;
                            }
                            tile2 = Main.tile[num51, num46];
                            if (tile2.WallType != 13)
                            {
                                tile2 = Main.tile[num51, num46];
                                if (tile2.WallType != 14)
                                {
                                    break;
                                }
                            }
                            num46++;
                        }
                        num46--;
                        num50 = (num47 + num46) / 2;
                    }
                    num49 = num51;
                    num48 = num51;
                    while (true)
                    {
                        tile2 = Main.tile[num49, num50];
                        if (!tile2.HasTile)
                        {
                            tile2 = Main.tile[num49, num50 - 1];
                            if (!tile2.HasTile)
                            {
                                tile2 = Main.tile[num49, num50 + 1];
                                if (!tile2.HasTile)
                                {
                                    num49--;
                                    continue;
                                }
                            }
                        }
                        break;
                    }
                    num49++;
                    while (true)
                    {
                        tile2 = Main.tile[num48, num50];
                        if (!tile2.HasTile)
                        {
                            tile2 = Main.tile[num48, num50 - 1];
                            if (!tile2.HasTile)
                            {
                                tile2 = Main.tile[num48, num50 + 1];
                                if (!tile2.HasTile)
                                {
                                    num48++;
                                    continue;
                                }
                            }
                        }
                        break;
                    }
                    num48--;
                    num47 = num50;
                    num46 = num50;
                    while (true)
                    {
                        tile2 = Main.tile[num51, num47];
                        if (!tile2.HasTile)
                        {
                            tile2 = Main.tile[num51 - 1, num47];
                            if (!tile2.HasTile)
                            {
                                tile2 = Main.tile[num51 + 1, num47];
                                if (!tile2.HasTile)
                                {
                                    num47--;
                                    continue;
                                }
                            }
                        }
                        break;
                    }
                    num47++;
                    while (true)
                    {
                        tile2 = Main.tile[num51, num46];
                        if (!tile2.HasTile)
                        {
                            tile2 = Main.tile[num51 - 1, num46];
                            if (!tile2.HasTile)
                            {
                                tile2 = Main.tile[num51 + 1, num46];
                                if (!tile2.HasTile)
                                {
                                    num46++;
                                    continue;
                                }
                            }
                        }
                        break;
                    }
                    num46--;
                    num51 = (num49 + num48) / 2;
                    num50 = (num47 + num46) / 2;
                    int num69 = num48 - num49;
                    num45 = num46 - num47;
                    if (num69 > 7 && num45 > 5)
                    {
                        int num32 = 0;
                        if (WorldGen.nearPicture2(num51, num50))
                        {
                            num32 = -1;
                        }
                        if (num32 == 0)
                        {
                            Vector2 vector = WorldGen.randHellPicture();
                            int type = (int)vector.X;
                            int style28 = (int)vector.Y;
                            if (!WorldGen.nearPicture(num51, num50))
                            {
                                WorldGen.PlaceTile(num51, num50, type, true, false, -1, style28);
                            }
                        }
                    }
                }
            }
            int[] array = new int[3]
            {
        WorldGen.genRand.Next(16, 22),
        WorldGen.genRand.Next(16, 22),
        WorldGen.genRand.Next(16, 22)
            };
            while (array[1] == array[0])
            {
                array[1] = WorldGen.genRand.Next(16, 22);
            }
            while (true)
            {
                if (array[2] != array[0] && array[2] != array[1])
                {
                    break;
                }
                array[2] = WorldGen.genRand.Next(16, 22);
            }
            num66 = 420000f / Main.maxTilesX;
            for (int num62 = 0; num62 < num66; num62++)
            {
                int num61 = 0;
                int num60;
                int num59;
                do
                {
                    num60 = WorldGen.genRand.Next(num68, Main.maxTilesX - num68);
                    num59 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 20);
                    if (WorldGen.drunkWorldGen)
                    {
                        num60 = (!WorldGen.genRand.NextBool(2)) ? WorldGen.genRand.Next(Main.maxTilesX - num68, Main.maxTilesX - 50) : WorldGen.genRand.Next(50, num68);
                    }
                    num61++;
                    if (num61 > 100000)
                    {
                        break;
                    }
                    tile2 = Main.tile[num60, num59];
                    if (tile2.WallType != 13)
                    {
                        tile2 = Main.tile[num60, num59];
                        if (tile2.WallType != 14)
                        {
                            //goto IL_10b6;
                        }
                    }
                    tile2 = Main.tile[num60, num59];
                }
                while (tile2.HasTile);
                if (num61 <= 100000)
                {
                    while (!WorldGen.SolidTile(num60, num59, false) && num59 > 10)
                    {
                        num59--;
                    }
                    num59++;
                    tile2 = Main.tile[num60, num59];
                    if (tile2.WallType != 13)
                    {
                        tile2 = Main.tile[num60, num59];
                        if (tile2.WallType == 14)
                        {
                            goto IL_11f4;
                        }
                        continue;
                    }
                    goto IL_11f4;
                }
                continue;
            IL_11f4:
                int num57 = WorldGen.genRand.Next(3);
                int style30 = 32;
                int style29 = 32;
                int num56;
                int num55;
                switch (num57)
                {
                    default:
                        num56 = 1;
                        num55 = 3;
                        break;
                    case 1:
                        num56 = 3;
                        num55 = 3;
                        break;
                    case 2:
                        num56 = 1;
                        num55 = 2;
                        break;
                }
                for (int num54 = num60 - 1; num54 <= num60 + num56; num54++)
                {
                    for (int num53 = num59; num53 <= num59 + num55; num53++)
                    {
                        Tile tile = Main.tile[num60, num59];
                        if (num54 < num60 || num54 == num60 + num56)
                        {
                            if (tile.HasTile)
                            {
                                ushort type2 = tile.TileType;
                                if (type2 <= 34u)
                                {
                                    if ((uint)(type2 - 10) > 1u && type2 != 34)
                                    {
                                        continue;
                                    }
                                }
                                else if (type2 != 42 && type2 != 91)
                                {
                                    continue;
                                }
                                num57 = -1;
                            }
                        }
                        else if (tile.HasTile)
                        {
                            num57 = -1;
                        }
                    }
                }
                switch (num57)
                {
                    case 0:
                        WorldGen.PlaceTile(num60, num59, 91, true, false, -1, array[WorldGen.genRand.Next(3)]);
                        break;
                    case 1:
                        WorldGen.PlaceTile(num60, num59, 34, true, false, -1, style30);
                        break;
                    case 2:
                        WorldGen.PlaceTile(num60, num59, 42, true, false, -1, style29);
                        break;
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
                    for (int num728 = num715; num728 < num716; num728++)
                    {
                        for (int num729 = 0; num729 < Main.maxTilesY - 50; num729++)
                        {
                            tile59 = Main.tile[num728, num729];
                            if (tile59.HasTile)
                            {
                                tile59 = Main.tile[num728, num729];
                                if (ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OrbTile.HasValue && tile59.TileType == ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OrbTile.Value)
                                {
                                    int num730 = num728 - 13;
                                    int num731 = num728 + 13;
                                    int num732 = num729 - 13;
                                    int num733 = num729 + 13;
                                    for (int num734 = num730; num734 < num731; num734++)
                                    {
                                        if (num734 > 10 && num734 < Main.maxTilesX - 10)
                                        {
                                            for (int num735 = num732; num735 < num733; num735++)
                                            {
                                                if (Math.Abs(num734 - num728) + Math.Abs(num735 - num729) < 9 + WorldGen.genRand.Next(11) && !WorldGen.genRand.NextBool(3))
                                                {
                                                    tile59 = Main.tile[num734, num735];
                                                    if (tile59.TileType != ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OrbTile.Value && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                                                    {
                                                        tile59 = Main.tile[num734, num735];
                                                        tile59.HasTile = true;
                                                        tile59 = Main.tile[num734, num735];
                                                        tile59.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                                                        if (Math.Abs(num734 - num728) <= 1 && Math.Abs(num735 - num729) <= 1)
                                                        {
                                                            tile59 = Main.tile[num734, num735];
                                                            tile59.HasTile = false;
                                                        }
                                                    }
                                                }
                                                tile59 = Main.tile[num734, num735];
                                                if (tile59.TileType != ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OrbTile.Value && Math.Abs(num734 - num728) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num735 - num729) <= 2 + WorldGen.genRand.Next(3))
                                                {
                                                    tile59 = Main.tile[num734, num735];
                                                    tile59.HasTile = false;
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
    }
}
