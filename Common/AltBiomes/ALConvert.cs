using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    public static class ALConvert
    {
        internal static void Load()
        {
            On.Terraria.WorldGen.Convert += WorldGen_Convert;
        }

        internal static void Unload()
        {
            On.Terraria.WorldGen.Convert -= WorldGen_Convert;
        }

        private static void WorldGen_Convert(On.Terraria.WorldGen.orig_Convert orig, int i, int j, int conversionType, int size)
		{
			int[,] allTiles = new int[AltLibrary.Biomes.Count, TileLoader.TileCount];
			int[,] repTiles = new int[AltLibrary.Biomes.Count, TileLoader.TileCount];
			int[,] allWalls = new int[AltLibrary.Biomes.Count, WallLoader.WallCount];
			int[,] repWalls = new int[AltLibrary.Biomes.Count, WallLoader.WallCount];
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				for (int m = 0; m < repTiles.Length; m++)
				{
					if (biome.SpecialConversion.ContainsKey(m))
					{
						biome.SpecialConversion.TryGetValue(m, out int value);
						repTiles[biome.Type - 1, m] = value;
					}
					else
					{
						repTiles[biome.Type - 1, m] = INVALID_TYPE;
					}
				}
				for (int m = 0; m < allTiles.Length; m++)
				{
					if (repTiles[biome.Type - 1, m] != INVALID_TYPE)
					{
						allTiles[biome.Type - 1, m] = m;
					}
					else
					{
						allTiles[biome.Type - 1, m] = INVALID_TYPE;
					}
				}
				for (int m = 0; m < repWalls.Length; m++)
				{
					if (biome.WallContext.wallsReplacement.ContainsKey((ushort)m))
					{
						biome.WallContext.wallsReplacement.TryGetValue((ushort)m, out ushort value);
						repWalls[biome.Type - 1, m] = value;
					}
					else
					{
						repWalls[biome.Type - 1, m] = INVALID_TYPE;
					}
				}
				for (int m = 0; m < allWalls.Length; m++)
				{
					if (repWalls[biome.Type - 1, m] != INVALID_TYPE)
					{
						allWalls[biome.Type - 1, m] = m;
					}
					else
					{
						allWalls[biome.Type - 1, m] = INVALID_TYPE;
					}
				}
			}

			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6)
					{
						int type = Main.tile[k, l].TileType;
						int wall = Main.tile[k, l].WallType;
						
						bool isGrass = TileID.Sets.Conversion.Grass[type];
						bool isTropicGrass = type == TileID.JungleGrass;
						bool isStone = TileID.Sets.Conversion.Stone[type];
						bool isGolfGrass = TileID.Sets.Conversion.GolfGrass[type];
						bool isIce = TileID.Sets.Conversion.Ice[type];
						bool isSand = TileID.Sets.Conversion.Sand[type];
						bool isHardSand = TileID.Sets.Conversion.HardenedSand[type];
						bool isSandstone = TileID.Sets.Conversion.Sandstone[type];
						foreach (AltBiome biome in AltLibrary.Biomes)
						{
							if (biome.BiomeGrass.HasValue)
							{
								isGrass |= type == biome.BiomeGrass.Value;
							}
							if (biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
							{
								isTropicGrass |= type == biome.BiomeGrass.Value;
							}
							if (biome.BiomeStone.HasValue)
							{
								isStone |= type == biome.BiomeStone.Value;
							}
							if (biome.BiomeMowedGrass.HasValue)
							{
								isGolfGrass |= type == biome.BiomeMowedGrass.Value;
							}
							if (biome.BiomeIce.HasValue)
							{
								isIce |= type == biome.BiomeIce.Value;
							}
							if (biome.BiomeSand.HasValue)
							{
								isSand |= type == biome.BiomeSand.Value;
							}
							if (biome.BiomeHardenedSand.HasValue)
							{
								isHardSand |= type == biome.BiomeHardenedSand.Value;
							}
							if (biome.BiomeSandstone.HasValue)
							{
								isSandstone |= type == biome.BiomeSandstone.Value;
							}
						}

						switch (conversionType)
						{
                            #region Corruption
                            case 1:
								if (WallID.Sets.Conversion.Grass[wall] && wall != 69)
								{
									Main.tile[k, l].WallType = 69;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Stone[wall] && wall != 3)
								{
									Main.tile[k, l].WallType = 3;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 217)
								{
									Main.tile[k, l].WallType = 217;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 220)
								{
									Main.tile[k, l].WallType = 220;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall1[wall] && wall != 188)
								{
									Main.tile[k, l].WallType = 188;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall2[wall] && wall != 189)
								{
									Main.tile[k, l].WallType = 189;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall3[wall] && wall != 190)
								{
									Main.tile[k, l].WallType = 190;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall4[wall] && wall != 191)
								{
									Main.tile[k, l].WallType = 191;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if ((Main.tileMoss[type] || isStone) && type != 25)
								{
									Main.tile[k, l].TileType = 25;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isGrass && type != 23)
								{
									Main.tile[k, l].TileType = 23;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isIce && type != 163)
								{
									Main.tile[k, l].TileType = 163;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isSand && type != 112)
								{
									Main.tile[k, l].TileType = 112;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isHardSand && type != 398)
								{
									Main.tile[k, l].TileType = 398;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isSand && type != 400)
								{
									Main.tile[k, l].TileType = 400;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Thorn[type] && type != 32)
								{
									Main.tile[k, l].TileType = 32;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if (type == 59 && (Main.tile[k - 1, l].TileType == 23 || Main.tile[k + 1, l].TileType == 23 || Main.tile[k, l - 1].TileType == 23 || Main.tile[k, l + 1].TileType == 23))
								{
									Main.tile[k, l].TileType = 0;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								break;
                            #endregion
                            #region Hallow
                            case 2:
								if (WallID.Sets.Conversion.Grass[wall] && wall != 70)
								{
									Main.tile[k, l].WallType = 70;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Stone[wall] && wall != 28)
								{
									Main.tile[k, l].WallType = 28;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 219)
								{
									Main.tile[k, l].WallType = 219;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 222)
								{
									Main.tile[k, l].WallType = 222;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall1[wall] && wall != 200)
								{
									Main.tile[k, l].WallType = 200;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall2[wall] && wall != 201)
								{
									Main.tile[k, l].WallType = 201;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall3[wall] && wall != 202)
								{
									Main.tile[k, l].WallType = 202;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall4[wall] && wall != 203)
								{
									Main.tile[k, l].WallType = 203;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if ((Main.tileMoss[type] || isStone) && type != 117)
								{
									Main.tile[k, l].TileType = 117;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isGolfGrass && type != 492)
								{
									Main.tile[k, l].TileType = 492;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isGrass && type != 109 && type != 492)
								{
									Main.tile[k, l].TileType = 109;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isIce && type != 164)
								{
									Main.tile[k, l].TileType = 164;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isSand && type != 116)
								{
									Main.tile[k, l].TileType = 116;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isHardSand && type != 402)
								{
									Main.tile[k, l].TileType = 402;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isSand && type != 403)
								{
									Main.tile[k, l].TileType = 403;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Thorn[type])
								{
									WorldGen.KillTile(k, l, false, false, false);
									if (Main.netMode == NetmodeID.MultiplayerClient)
									{
										NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l, 0f, 0, 0, 0);
									}
								}
								if (type == 59 && (Main.tile[k - 1, l].TileType == 109 || Main.tile[k + 1, l].TileType == 109 || Main.tile[k, l - 1].TileType == 109 || Main.tile[k, l + 1].TileType == 109))
								{
									Main.tile[k, l].TileType = 0;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								break;
                            #endregion
                            #region Glowing Mushroom
                            case 3:
								if (WallID.Sets.CanBeConvertedToGlowingMushroom[wall])
								{
									Main.tile[k, l].WallType = 80;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if (isTropicGrass)
								{
									Main.tile[k, l].TileType = 70;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Thorn[type])
								{
									WorldGen.KillTile(k, l, false, false, false);
									if (Main.netMode == NetmodeID.MultiplayerClient)
									{
										NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l, 0f, 0, 0, 0);
									}
								}
								break;
                            #endregion
                            #region Crimson
                            case 4:
								if (WallID.Sets.Conversion.Grass[wall] && wall != 81)
								{
									Main.tile[k, l].WallType = 81;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Stone[wall] && wall != 83)
								{
									Main.tile[k, l].WallType = 83;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 218)
								{
									Main.tile[k, l].WallType = 218;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 221)
								{
									Main.tile[k, l].WallType = 221;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall1[wall] && wall != 192)
								{
									Main.tile[k, l].WallType = 192;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall2[wall] && wall != 193)
								{
									Main.tile[k, l].WallType = 193;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall3[wall] && wall != 194)
								{
									Main.tile[k, l].WallType = 194;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall4[wall] && wall != 195)
								{
									Main.tile[k, l].WallType = 195;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if ((Main.tileMoss[type] || isStone) && type != 203)
								{
									Main.tile[k, l].TileType = 203;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isGrass && type != 199)
								{
									Main.tile[k, l].TileType = 199;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isIce && type != 200)
								{
									Main.tile[k, l].TileType = 200;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isSand && type != 234)
								{
									Main.tile[k, l].TileType = 234;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isHardSand && type != 399)
								{
									Main.tile[k, l].TileType = 399;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (isSandstone && type != 401)
								{
									Main.tile[k, l].TileType = 401;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Thorn[type] && type != 352)
								{
									Main.tile[k, l].TileType = 352;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if (type == 59 && (Main.tile[k - 1, l].TileType == 199 || Main.tile[k + 1, l].TileType == 199 || Main.tile[k, l - 1].TileType == 199 || Main.tile[k, l + 1].TileType == 199))
								{
									Main.tile[k, l].TileType = 0;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                                }
                                break;
                            #endregion
                            #region Normal?
                            default:
								for (int m = 0; m < AltLibrary.Biomes.Count; m++)
								{
									if (repTiles[m,type] != INVALID_TYPE && type == repTiles[m,wall] && allTiles[m,type] != INVALID_TYPE)
									{
										Main.tile[k, l].TileType = (ushort)allTiles[m, wall];
										WorldGen.SquareTileFrame(k, l, true);
										NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
									}
								}
								for (int m = 0; m < AltLibrary.Biomes.Count; m++)
                                {
									if (repWalls[m,wall] != INVALID_TYPE && wall == repWalls[m,wall] && allWalls[m,wall] != INVALID_TYPE)
									{
										Main.tile[k, l].WallType = (ushort)allWalls[m,wall];
										WorldGen.SquareWallFrame(k, l, true);
										NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
									}
									if (repWalls[m, wall] != INVALID_TYPE)
                                    {
										AltLibrary.Instance.Logger.Info(m + " " + wall + " " + repWalls[m,wall] + " " + allWalls[m,wall]);
                                    }
								}

								if (Main.tile[k, l].WallType == 69 || Main.tile[k, l].WallType == 70 || Main.tile[k, l].WallType == 81)
								{
									if (l < Main.worldSurface)
									{
										if (WorldGen.genRand.NextBool(10))
										{
											Main.tile[k, l].WallType = 65;
										}
										else
										{
											Main.tile[k, l].WallType = 63;
										}
									}
									else
									{
										Main.tile[k, l].WallType = 64;
									}
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Stone[wall] && wall != 1 && wall != 262 && wall != 274 && wall != 61 && wall != 185)
								{
									Main.tile[k, l].WallType = 1;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Stone[wall] && wall == 262)
								{
									Main.tile[k, l].WallType = 61;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Stone[wall] && wall == 274)
								{
									Main.tile[k, l].WallType = 185;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if (WallID.Sets.Conversion.NewWall1[wall] && wall != 212)
								{
									Main.tile[k, l].WallType = 212;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall2[wall] && wall != 213)
								{
									Main.tile[k, l].WallType = 213;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall3[wall] && wall != 214)
								{
									Main.tile[k, l].WallType = 214;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.NewWall4[wall] && wall != 215)
								{
									Main.tile[k, l].WallType = 215;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (Main.tile[k, l].WallType == 80)
								{
									if (l < Main.worldSurface + 4.0 + WorldGen.genRand.Next(3) || l > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3))
									{
										Main.tile[k, l].WallType = 15;
										WorldGen.SquareWallFrame(k, l, true);
										NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
									}
									else
									{
										Main.tile[k, l].WallType = 64;
										WorldGen.SquareWallFrame(k, l, true);
										NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
									}
								}
								else if (WallID.Sets.Conversion.HardenedSand[wall] && wall != 216)
								{
									Main.tile[k, l].WallType = 216;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (WallID.Sets.Conversion.Sandstone[wall] && wall != 187)
								{
									Main.tile[k, l].WallType = 187;
									WorldGen.SquareWallFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								if (Main.tile[k, l].TileType == 492)
								{
									Main.tile[k, l].TileType = 477;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (type != 60 && TileID.Sets.Conversion.Grass[type] && type != 2 && type != 477)
								{
									Main.tile[k, l].TileType = 2;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Stone[type] && type != 1)
								{
									Main.tile[k, l].TileType = 1;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Sand[type] && type != 53)
								{
									Main.tile[k, l].TileType = 53;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.HardenedSand[type] && type != 397)
								{
									Main.tile[k, l].TileType = 397;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Sandstone[type] && type != 396)
								{
									Main.tile[k, l].TileType = 396;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (TileID.Sets.Conversion.Ice[type] && type != 161)
								{
									Main.tile[k, l].TileType = 161;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (Main.tile[k, l].TileType == 70)
								{
									Main.tile[k, l].TileType = 60;
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
								}
								else if (Main.tile[k, l].TileType == 32 || Main.tile[k, l].TileType == 352)
								{
									WorldGen.KillTile(k, l, false, false, false);
									if (Main.netMode == NetmodeID.MultiplayerClient)
									{
										NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)k, (float)l, 0f, 0, 0, 0);
                                    }
                                }
                                break;
                                #endregion
                        }
                    }
				}
			}
		}

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="fullName"></param>
        public static void SimulateThrownWater(Projectile projectile, string fullName)
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert(fullName, i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="mod"></param>
        /// <param name="name"></param>
        public static void SimulateThrownWater(Projectile projectile, Mod mod, string name)
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert(mod, name, i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projectile"></param>
        public static void SimulateThrownWater<T>(Projectile projectile) where T : AltBiome
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert<T>(i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="biome"></param>
        public static void SimulateThrownWater(Projectile projectile, AltBiome biome)
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert(biome, i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <typeparam name="TProj"></typeparam>
        /// <typeparam name="TBiome"></typeparam>
        public static void SimulateThrownWater<TProj, TBiome>() where TBiome : AltBiome where TProj : Projectile
        {
            Projectile projectile = ContentInstance<TProj>.Instance;
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert<TBiome>(i, j, 4);
        }

        /// <summary>
        /// Makes solution converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="mod"></param>
        /// <param name="name"></param>
        /// <param name="dustType"></param>
        public static void SimulateSolution(Projectile projectile, Mod mod, string name) => SimulateSolution(projectile, AltLibrary.Biomes.Find(x => x.Mod == mod && x.Name == name));

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="fullname"></param>
		/// <param name="dustType"></param>
		public static void SimulateSolution(Projectile projectile, string fullname) => SimulateSolution(projectile, AltLibrary.Biomes.Find(x => x.FullName == fullname));

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="projectile"></param>
		/// <param name="dustType"></param>
		public static void SimulateSolution<T>(Projectile projectile) where T : AltBiome => SimulateSolution(projectile, ContentInstance<T>.Instance);

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="biome"></param>
		/// <param name="dustType"></param>
		public static void SimulateSolution(Projectile projectile, AltBiome biome)
        {
            Convert(biome, (int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
        }

        public static void Convert(string fullName, int i, int j, int size = 4) => Convert(AltLibrary.Biomes.Find(x => x.FullName == fullName), i, j, size);

        public static void Convert(Mod mod, string name, int i, int j, int size = 4) => Convert(AltLibrary.Biomes.Find(x => x.Mod == mod && x.Name == name), i, j, size);

        public static void Convert<T>(int i, int j, int size = 4) where T : AltBiome => Convert(ContentInstance<T>.Instance, i, j, size);

		private const int INVALID_TYPE = -208070612;
        public static void Convert(AltBiome biome, int i, int j, int size = 4)
        {
            if (biome is null)
                throw new ArgumentNullException(nameof(biome), "Can't be null!");

			int[] allTiles = new int[TileLoader.TileCount];
			int[] repTiles = new int[TileLoader.TileCount];
			int[] allWalls = new int[WallLoader.WallCount];
			int[] repWalls = new int[WallLoader.WallCount];
			for (int m = 0; m < repTiles.Length; m++)
			{
				if (biome.SpecialConversion.ContainsKey(m))
				{
					biome.SpecialConversion.TryGetValue(m, out int value);
					repTiles[m] = value;
				}
				else
				{
					repTiles[m] = INVALID_TYPE;
				}
			}
			for (int m = 0; m < allTiles.Length; m++)
			{
				if (repTiles[m] != INVALID_TYPE)
				{
					allTiles[m] = m;
				}
				else
				{
					allTiles[m] = INVALID_TYPE;
				}
			}
			for (int m = 0; m < repWalls.Length; m++)
			{
				if (biome.WallContext.wallsReplacement.ContainsKey((ushort)m))
				{
					biome.WallContext.wallsReplacement.TryGetValue((ushort)m, out ushort value);
					repWalls[m] = value;
				}
				else
				{
					repWalls[m] = INVALID_TYPE;
				}
			}
			for (int m = 0; m < allWalls.Length; m++)
			{
				if (repWalls[m] != INVALID_TYPE)
				{
					allWalls[m] = m;
				}
				else
				{
					allWalls[m] = INVALID_TYPE;
                }
            }

            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6)
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        if (allTiles[type] != INVALID_TYPE && type == allTiles[type] && repTiles[type] != INVALID_TYPE)
                        {
                            Main.tile[k, l].TileType = (ushort)repTiles[type];
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }

                        if (allWalls[wall] != INVALID_TYPE && wall == allWalls[wall] && repWalls[wall] != INVALID_TYPE)
                        {
                            Main.tile[k, l].WallType = (ushort)repWalls[wall];
                            WorldGen.SquareWallFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }

                        bool isGrass = TileID.Sets.Conversion.Grass[type];
                        bool isTropicGrass = type == TileID.JungleGrass;
                        bool isStone = TileID.Sets.Conversion.Stone[type];
                        bool isGolfGrass = TileID.Sets.Conversion.GolfGrass[type];
                        bool isIce = TileID.Sets.Conversion.Ice[type];
                        bool isSand = TileID.Sets.Conversion.Sand[type];
                        bool isHardSand = TileID.Sets.Conversion.HardenedSand[type];
                        bool isSandstone = TileID.Sets.Conversion.Sandstone[type];
                        if (biome.BiomeGrass.HasValue)
                        {
                            isGrass |= type == biome.BiomeGrass.Value;
                        }
                        if (biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
                        {
                            isTropicGrass |= type == biome.BiomeGrass.Value;
                        }
                        if (biome.BiomeStone.HasValue)
                        {
                            isStone |= type == biome.BiomeStone.Value;
                        }
                        if (biome.BiomeMowedGrass.HasValue)
                        {
                            isGolfGrass |= type == biome.BiomeMowedGrass.Value;
                        }
                        if (biome.BiomeIce.HasValue)
                        {
                            isIce |= type == biome.BiomeIce.Value;
                        }
                        if (biome.BiomeSand.HasValue)
                        {
                            isSand |= type == biome.BiomeSand.Value;
                        }
                        if (biome.BiomeHardenedSand.HasValue)
                        {
                            isHardSand |= type == biome.BiomeHardenedSand.Value;
                        }
                        if (biome.BiomeSandstone.HasValue)
                        {
                            isSandstone |= type == biome.BiomeSandstone.Value;
                        }

                        if ((Main.tileMoss[type] || isStone) && type != biome.BiomeStone.Value && biome.BiomeStone.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeStone.Value;
                            WorldGen.SquareWallFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l);
                        }
                        else if (isGrass && type != biome.BiomeGrass.Value && biome.BiomeGrass.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeGrass.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}
						else if (isTropicGrass && type != biome.BiomeJungleGrass.Value && biome.BiomeJungleGrass.HasValue)
						{
							Main.tile[k, l].TileType = (ushort)biome.BiomeJungleGrass.Value;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}
						else if (isGolfGrass && type != biome.BiomeMowedGrass.Value && biome.BiomeMowedGrass.HasValue)
						{
							Main.tile[k, l].TileType = (ushort)biome.BiomeMowedGrass.Value;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}
						else if (isIce && type != biome.BiomeIce.Value && biome.BiomeIce.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeIce.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (isSand && type != biome.BiomeSand.Value && biome.BiomeSand.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeSand.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (isHardSand && type != biome.BiomeHardenedSand.Value && biome.BiomeHardenedSand.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeHardenedSand.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (isSandstone && type != biome.BiomeSandstone.Value && biome.BiomeSandstone.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeSandstone.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (TileID.Sets.Conversion.Thorn[type] && type != biome.BiomeThornBush.Value && biome.BiomeThornBush.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeThornBush.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        if (biome.MudToDirt && type == TileID.Mud && (Main.tile[k - 1, l].TileType == biome.BiomeGrass || Main.tile[k + 1, l].TileType == biome.BiomeGrass || Main.tile[k, l - 1].TileType == biome.BiomeGrass || Main.tile[k, l + 1].TileType == biome.BiomeGrass))
                        {
                            Main.tile[k, l].TileType = TileID.Dirt;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }

                        foreach (KeyValuePair<int, int> pair in biome.SpecialConversion)
                        {
                            if (type == pair.Key)
                            {
                                Main.tile[k, l].TileType = (ushort)pair.Value;
                            }
                        }
                        foreach (KeyValuePair<ushort, ushort> pair in biome.WallContext.wallsReplacement)
                        {
                            if (wall == pair.Key)
                            {
                                Main.tile[k, l].WallType = pair.Value;
                            }
                        }
                    }
                }
            }
        }
    }
}
