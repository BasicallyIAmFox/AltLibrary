using AltLibrary.Common.AltBiomes;
using AltLibrary.Core;
using AltLibrary.Core.Generation;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content
{
    internal class EpicSwag : AltBiome
    {
        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Evil;
            BiomeGrass = TileID.GoldBrick;
            BiomeStone = TileID.Gold;
            BiomeSand = TileID.YellowStucco;
            BiomeIce = TileID.TeamBlockYellow;
            BiomeSandstone = TileID.PlatinumBrick;
            MechDropItemType = ItemID.GoldBar;
            BiomeChestItem = ItemID.ReflectiveGoldDye;
            BiomeChestTile = TileID.Containers2;
            BiomeChestTileStyle = 6;
            BiomeMowedGrass = TileID.Platinum;
            MimicKeyType = ItemID.GoldCrown;
            MimicType = NPCID.GoldenSlime;
            FountainTile = TileID.SeaweedPlanter;
            FountainTileStyle = 0;
            FountainActiveFrameY = 0;
        }

        public override EvilBiomeGenerationPass GetEvilBiomeGenerationPass()
        {
            return new EpicSwagGenPass();
        }

        internal class EpicSwagGenPass : EvilBiomeGenerationPass
        {
            public override void GenerateEvil(int num33, int num34, int num35)
            {
                bool worldCrimson = WorldGen.crimson;
                WorldGen.crimson = false;

                int num38 = 0;
                for (int n = num34; n < num35; n++)
                {
                    if (num38 > 0)
                    {
                        num38--;
                    }
                    if (n == num33 || num38 == 0)
                    {
                        int num39 = (int)WorldGen.worldSurfaceLow;
                        while (num39 < Main.worldSurface - 1.0)
                        {
                            if (Main.tile[n, num39].HasTile || Main.tile[n, num39].WallType > 0)
                            {
                                if (n == num33)
                                {
                                    num38 = 20;
                                    ChasmRunner(n, num39, WorldGen.genRand.Next(150) + 150, true);
                                    break;
                                }
                                if (WorldGen.genRand.NextBool(35) && num38 == 0)
                                {
                                    num38 = 30;
                                    bool makeOrb = true;
                                    ChasmRunner(n, num39, WorldGen.genRand.Next(50) + 50, makeOrb);
                                    break;
                                }
                                break;
                            }
                            else
                            {
                                num39++;
                            }
                        }
                    }
                    int num40 = (int)WorldGen.worldSurfaceLow;
                    while (num40 < Main.worldSurface - 1.0)
                    {
                        if (Main.tile[n, num40].HasTile)
                        {
                            int num41 = num40 + WorldGen.genRand.Next(10, 14);
                            for (int num42 = num40; num42 < num41; num42++)
                            {
                                if ((Main.tile[n, num42].TileType == 59 || Main.tile[n, num42].TileType == 60) && n >= num34 + WorldGen.genRand.Next(5) && n < num35 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[n, num42].TileType = 0;
                                }
                            }
                            break;
                        }
                        num40++;
                    }
                }
                double num43 = Main.worldSurface + 40.0;
                for (int num44 = num34; num44 < num35; num44++)
                {
                    num43 += WorldGen.genRand.Next(-2, 3);
                    if (num43 < Main.worldSurface + 30.0)
                    {
                        num43 = Main.worldSurface + 30.0;
                    }
                    if (num43 > Main.worldSurface + 50.0)
                    {
                        num43 = Main.worldSurface + 50.0;
                    }
                    int i2 = num44;
                    bool flag7 = false;
                    int num45 = (int)WorldGen.worldSurfaceLow;
                    while (num45 < num43)
                    {
                        if (Main.tile[i2, num45].HasTile)
                        {
                            if (Main.tile[i2, num45].TileType == 53 && i2 >= num34 + WorldGen.genRand.Next(5) && i2 <= num35 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num45].TileType = TileID.YellowStucco;
                            }
                            if (Main.tile[i2, num45].TileType == 0 && num45 < Main.worldSurface - 1.0 && !flag7)
                            {
                                ALReflection.WorldGen_GrassSpread = 0;
                                WorldGen.SpreadGrass(i2, num45, 0, TileID.GoldBrick, true, 0);
                            }
                            flag7 = true;
                            if (Main.tile[i2, num45].TileType == 1 && i2 >= num34 + WorldGen.genRand.Next(5) && i2 <= num35 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num45].TileType = TileID.Gold;
                            }
                            if (Main.tile[i2, num45].WallType == 216)
                            {
                                Main.tile[i2, num45].WallType = 217;
                            }
                            else if (Main.tile[i2, num45].WallType == 187)
                            {
                                Main.tile[i2, num45].WallType = 220;
                            }
                            if (Main.tile[i2, num45].TileType == 2)
                            {
                                Main.tile[i2, num45].TileType = TileID.GoldBrick;
                            }
                            if (Main.tile[i2, num45].TileType == 161)
                            {
                                Main.tile[i2, num45].TileType = TileID.TeamBlockYellow;
                            }
                            else if (Main.tile[i2, num45].TileType == 396)
                            {
                                Main.tile[i2, num45].TileType = TileID.PlatinumBrick;
                            }
                            else if (Main.tile[i2, num45].TileType == 397)
                            {
                                Main.tile[i2, num45].TileType = 398;
                            }
                        }
                        num45++;
                    }
                }
                for (int num46 = num34; num46 < num35; num46++)
                {
                    for (int num47 = 0; num47 < Main.maxTilesY - 50; num47++)
                    {
                        if (Main.tile[num46, num47].HasTile && Main.tile[num46, num47].TileType == 31)
                        {
                            int num48 = num46 - 13;
                            int num49 = num46 + 13;
                            int num50 = num47 - 13;
                            int num51 = num47 + 13;
                            for (int num52 = num48; num52 < num49; num52++)
                            {
                                if (num52 > 10 && num52 < Main.maxTilesX - 10)
                                {
                                    for (int num53 = num50; num53 < num51; num53++)
                                    {
                                        if (Math.Abs(num52 - num46) + Math.Abs(num53 - num47) < 9 + WorldGen.genRand.Next(11) && !WorldGen.genRand.NextBool(3) && Main.tile[num52, num53].TileType != 31)
                                        {
                                            Main.tile[num52, num53].TileType = 0;
                                            Main.tile[num52, num53].TileType = 25;
                                            if (Math.Abs(num52 - num46) <= 1 && Math.Abs(num53 - num47) <= 1)
                                            {
                                                Main.tile[num52, num53].TileType = 0;
                                            }
                                        }
                                        if (Main.tile[num52, num53].TileType != 31 && Math.Abs(num52 - num46) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num53 - num47) <= 2 + WorldGen.genRand.Next(3))
                                        {
                                            Main.tile[num52, num53].TileType = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                WorldGen.crimson = worldCrimson;
            }

            public override void PostGenerateEvil()
            {
            }

			public static void ChasmRunner(int i, int j, int steps, bool makeOrb = false)
			{
				bool flag7 = false;
				bool flag6 = false;
				bool flag5 = false;
				if (!makeOrb)
				{
					flag6 = true;
				}
				float num28 = (float)steps;
				Vector2 vector = default(Vector2);
				vector.X = (float)i;
				vector.Y = (float)j;
				Vector2 vector2 = default(Vector2);
				vector2.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
				vector2.Y = (float)WorldGen.genRand.Next(11) * 0.2f + 0.5f;
				int num27 = 5;
				double num26 = (double)(WorldGen.genRand.Next(5) + 7);
				while (num26 > 0.0)
				{
					if (num28 > 0f)
					{
						num26 += (double)WorldGen.genRand.Next(3);
						num26 -= (double)WorldGen.genRand.Next(3);
						if (num26 < 7.0)
						{
							num26 = 7.0;
						}
						if (num26 > 20.0)
						{
							num26 = 20.0;
						}
						if (num28 == 1f && num26 < 10.0)
						{
							num26 = 10.0;
						}
					}
					else if ((double)vector.Y > Main.worldSurface + 45.0)
					{
						num26 -= (double)WorldGen.genRand.Next(4);
					}
					if ((double)vector.Y > Main.rockLayer && num28 > 0f)
					{
						num28 = 0f;
					}
					num28 -= 1f;
					if (!flag7 && (double)vector.Y > Main.worldSurface + 20.0)
					{
						flag7 = true;
						ChasmRunnerSideways((int)vector.X, (int)vector.Y, -1, WorldGen.genRand.Next(20, 40));
						ChasmRunnerSideways((int)vector.X, (int)vector.Y, 1, WorldGen.genRand.Next(20, 40));
					}
					Tile tile;
					int num24;
					int num23;
					int num22;
					int num21;
					if (num28 > (float)num27)
					{
						num24 = (int)((double)vector.X - num26 * 0.5);
						num23 = (int)((double)vector.X + num26 * 0.5);
						num22 = (int)((double)vector.Y - num26 * 0.5);
						num21 = (int)((double)vector.Y + num26 * 0.5);
						if (num24 < 0)
						{
							num24 = 0;
						}
						if (num23 > Main.maxTilesX - 1)
						{
							num23 = Main.maxTilesX - 1;
						}
						if (num22 < 0)
						{
							num22 = 0;
						}
						if (num21 > Main.maxTilesY)
						{
							num21 = Main.maxTilesY;
						}
						for (int n = num24; n < num23; n++)
						{
							for (int m = num22; m < num21; m++)
							{
								if ((double)(Math.Abs((float)n - vector.X) + Math.Abs((float)m - vector.Y)) < num26 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
								{
									tile = Main.tile[n, m];
									if (tile.TileType != 31)
									{
										tile = Main.tile[n, m];
										if (tile.TileType != 22)
										{
											tile = Main.tile[n, m];
											tile.HasActuator = (false);
										}
									}
								}
							}
						}
					}
					if (num28 <= 2f && (double)vector.Y < Main.worldSurface + 45.0)
					{
						num28 = 2f;
					}
					if (num28 <= 0f)
					{
						if (!flag6)
						{
							flag6 = true;
							WorldGen.AddShadowOrb((int)vector.X, (int)vector.Y);
						}
						else if (!flag5)
						{
							flag5 = false;
							bool flag4 = false;
							int num20 = 0;
							while (!flag4)
							{
								int num19 = WorldGen.genRand.Next((int)vector.X - 25, (int)vector.X + 25);
								int num18 = WorldGen.genRand.Next((int)vector.Y - 50, (int)vector.Y);
								if (num19 < 5)
								{
									num19 = 5;
								}
								if (num19 > Main.maxTilesX - 5)
								{
									num19 = Main.maxTilesX - 5;
								}
								if (num18 < 5)
								{
									num18 = 5;
								}
								if (num18 > Main.maxTilesY - 5)
								{
									num18 = Main.maxTilesY - 5;
								}
								if ((double)num18 > Main.worldSurface)
								{
									if (!WorldGen.IsTileNearby(num19, num18, 26, 3))
									{
										WorldGen.Place3x2(num19, num18, 26, 0);
									}
									tile = Main.tile[num19, num18];
									if (tile.TileType == 26)
									{
										flag4 = true;
									}
									else
									{
										num20++;
										if (num20 >= 10000)
										{
											flag4 = true;
										}
									}
								}
								else
								{
									flag4 = true;
								}
							}
						}
					}
					vector += vector2;
					vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
					if ((double)vector2.X > 0.3)
					{
						vector2.X = 0.3f;
					}
					if ((double)vector2.X < -0.3)
					{
						vector2.X = -0.3f;
					}
					num24 = (int)((double)vector.X - num26 * 1.1);
					num23 = (int)((double)vector.X + num26 * 1.1);
					num22 = (int)((double)vector.Y - num26 * 1.1);
					num21 = (int)((double)vector.Y + num26 * 1.1);
					if (num24 < 1)
					{
						num24 = 1;
					}
					if (num23 > Main.maxTilesX - 1)
					{
						num23 = Main.maxTilesX - 1;
					}
					if (num22 < 0)
					{
						num22 = 0;
					}
					if (num21 > Main.maxTilesY)
					{
						num21 = Main.maxTilesY;
					}
					for (int l = num24; l < num23; l++)
					{
						for (int k = num22; k < num21; k++)
						{
							if ((double)(Math.Abs((float)l - vector.X) + Math.Abs((float)k - vector.Y)) < num26 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
							{
								tile = Main.tile[l, k];
								if (tile.TileType != 25 && k > j + WorldGen.genRand.Next(3, 20))
								{
									tile = Main.tile[l, k];
									tile.HasTile = (true);
								}
								if (steps <= num27)
								{
									tile = Main.tile[l, k];
									tile.HasTile = (true);
								}
								tile = Main.tile[l, k];
								if (tile.TileType != 31)
								{
									tile = Main.tile[l, k];
									tile.TileType = TileID.Gold;
								}
							}
						}
					}
					for (int num13 = num24; num13 < num23; num13++)
					{
						for (int num12 = num22; num12 < num21; num12++)
						{
							if ((double)(Math.Abs((float)num13 - vector.X) + Math.Abs((float)num12 - vector.Y)) < num26 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
							{
								tile = Main.tile[num13, num12];
								if (tile.TileType != 31)
								{
									tile = Main.tile[num13, num12];
									tile.TileType = TileID.Gold;
								}
								if (steps <= num27)
								{
									tile = Main.tile[num13, num12];
									tile.HasTile = (true);
								}
								if (num12 > j + WorldGen.genRand.Next(3, 20))
								{
									tile = Main.tile[num13, num12];
									tile.WallType = 3;
								}
							}
						}
					}
				}
			}

			public static void ChasmRunnerSideways(int i, int j, int direction, int steps)
			{
				float num22 = (float)steps;
				Vector2 vector = default(Vector2);
				vector.X = (float)i;
				vector.Y = (float)j;
				Vector2 vector2 = default(Vector2);
				vector2.X = (float)WorldGen.genRand.Next(10, 21) * 0.1f * (float)direction;
				vector2.Y = (float)WorldGen.genRand.Next(-10, 10) * 0.01f;
				double num21 = (double)(WorldGen.genRand.Next(5) + 7);
				Tile tile;
				while (num21 > 0.0)
				{
					if (num22 > 0f)
					{
						num21 += (double)WorldGen.genRand.Next(3);
						num21 -= (double)WorldGen.genRand.Next(3);
						if (num21 < 7.0)
						{
							num21 = 7.0;
						}
						if (num21 > 20.0)
						{
							num21 = 20.0;
						}
						if (num22 == 1f && num21 < 10.0)
						{
							num21 = 10.0;
						}
					}
					else
					{
						num21 -= (double)WorldGen.genRand.Next(4);
					}
					if ((double)vector.Y > Main.rockLayer && num22 > 0f)
					{
						num22 = 0f;
					}
					num22 -= 1f;
					int num17 = (int)((double)vector.X - num21 * 0.5);
					int num16 = (int)((double)vector.X + num21 * 0.5);
					int num15 = (int)((double)vector.Y - num21 * 0.5);
					int num14 = (int)((double)vector.Y + num21 * 0.5);
					if (num17 < 0)
					{
						num17 = 0;
					}
					if (num16 > Main.maxTilesX - 1)
					{
						num16 = Main.maxTilesX - 1;
					}
					if (num15 < 0)
					{
						num15 = 0;
					}
					if (num14 > Main.maxTilesY)
					{
						num14 = Main.maxTilesY;
					}
					for (int n = num17; n < num16; n++)
					{
						for (int k = num15; k < num14; k++)
						{
							if ((double)(Math.Abs((float)n - vector.X) + Math.Abs((float)k - vector.Y)) < num21 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
							{
								tile = Main.tile[n, k];
								if (tile.TileType != 31)
								{
									tile = Main.tile[n, k];
									if (tile.TileType != 22)
									{
										tile = Main.tile[n, k];
										tile.HasTile = (false);
									}
								}
							}
						}
					}
					vector += vector2;
					vector2.Y += (float)WorldGen.genRand.Next(-10, 10) * 0.1f;
					if (vector.Y < (float)(j - 20))
					{
						vector2.Y += (float)WorldGen.genRand.Next(20) * 0.01f;
					}
					if (vector.Y > (float)(j + 20))
					{
						vector2.Y -= (float)WorldGen.genRand.Next(20) * 0.01f;
					}
					if ((double)vector2.Y < -0.5)
					{
						vector2.Y = -0.5f;
					}
					if ((double)vector2.Y > 0.5)
					{
						vector2.Y = 0.5f;
					}
					vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
					switch (direction)
					{
						case -1:
							if ((double)vector2.X > -0.5)
							{
								vector2.X = -0.5f;
							}
							if (vector2.X < -2f)
							{
								vector2.X = -2f;
							}
							break;
						case 1:
							if ((double)vector2.X < 0.5)
							{
								vector2.X = 0.5f;
							}
							if (vector2.X > 2f)
							{
								vector2.X = 2f;
							}
							break;
					}
					num17 = (int)((double)vector.X - num21 * 1.1);
					num16 = (int)((double)vector.X + num21 * 1.1);
					num15 = (int)((double)vector.Y - num21 * 1.1);
					num14 = (int)((double)vector.Y + num21 * 1.1);
					if (num17 < 1)
					{
						num17 = 1;
					}
					if (num16 > Main.maxTilesX - 1)
					{
						num16 = Main.maxTilesX - 1;
					}
					if (num15 < 0)
					{
						num15 = 0;
					}
					if (num14 > Main.maxTilesY)
					{
						num14 = Main.maxTilesY;
					}
					for (int m = num17; m < num16; m++)
					{
						for (int l = num15; l < num14; l++)
						{
							if ((double)(Math.Abs((float)m - vector.X) + Math.Abs((float)l - vector.Y)) < num21 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
							{
								tile = Main.tile[m, l];
								if (tile.WallType != 3)
								{
									tile = Main.tile[m, l];
									if (tile.TileType != TileID.Gold && l > j + WorldGen.genRand.Next(3, 20))
									{
										tile = Main.tile[m, l];
										tile.HasTile = (true);
									}
									tile = Main.tile[m, l];
									tile.HasTile = (true);
									tile = Main.tile[m, l];
									if (tile.TileType != 31)
									{
										tile = Main.tile[m, l];
										if (tile.TileType != 22)
										{
											tile = Main.tile[m, l];
											tile.TileType = TileID.Gold;
										}
									}
									tile = Main.tile[m, l];
									if (tile.WallType == 2)
									{
										tile = Main.tile[m, l];
										tile.WallType = 0;
									}
								}
							}
						}
					}
					for (int num9 = num17; num9 < num16; num9++)
					{
						for (int num8 = num15; num8 < num14; num8++)
						{
							if ((double)(Math.Abs((float)num9 - vector.X) + Math.Abs((float)num8 - vector.Y)) < num21 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
							{
								tile = Main.tile[num9, num8];
								if (tile.TileType != 3)
								{
									tile = Main.tile[num9, num8];
									if (tile.TileType != 31)
									{
										tile = Main.tile[num9, num8];
										if (tile.TileType != 22)
										{
											tile = Main.tile[num9, num8];
											tile.TileType = TileID.Gold;
										}
									}
									tile = Main.tile[num9, num8];
									tile.HasTile = (true);
									WorldGen.PlaceWall(num9, num8, 3, true);
								}
							}
						}
					}
				}
				if (WorldGen.genRand.NextBool(3))
				{
					int num20 = (int)vector.X;
					int num19 = (int)vector.Y;
					while (true)
					{
						tile = Main.tile[num20, num19];
						if (!tile.HasTile)
						{
							num19++;
							continue;
						}
						break;
					}
					WorldGen.TileRunner(num20, num19, (double)WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), 22, false, 0f, 0f, false, true, -1);
				}
			}
		}
	}
}
