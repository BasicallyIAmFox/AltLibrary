using System;
using Terraria;
using Terraria.WorldBuilding;

namespace AltLibrary.Core.Generation
{
	internal class CorruptionEvilBiomeGenerationPass : EvilBiomeGenerationPass
	{
		public override string ProgressMessage => Lang.gen[20].Value;
		public override bool CanGenerateNearDungeonOcean => false;

		public override void GenerateEvil(int num40, int num41, int num42)
		{
			bool worldCrimson = WorldGen.crimson;
			WorldGen.crimson = false;

			int num45 = 0;
			for (int num46 = num41; num46 < num42; num46++)
			{
				if (num45 > 0)
				{
					num45--;
				}
				if (num46 == num40 || num45 == 0)
				{
					int num47 = (int)GenVars.worldSurfaceLow;
					while ((double)num47 < Main.worldSurface - 1.0)
					{
						if (Main.tile[num46, num47].HasTile || Main.tile[num46, num47].WallType > 0)
						{
							if (num46 == num40)
							{
								num45 = 20;
								WorldGen.ChasmRunner(num46, num47, WorldGen.genRand.Next(150) + 150, true);
								break;
							}
							if (WorldGen.genRand.Next(35) == 0 && num45 == 0)
							{
								num45 = 30;
								bool makeOrb = true;
								WorldGen.ChasmRunner(num46, num47, WorldGen.genRand.Next(50) + 50, makeOrb);
								break;
							}
							break;
						}
						else
						{
							num47++;
						}
					}
				}
				int num48 = (int)GenVars.worldSurfaceLow;
				while ((double)num48 < Main.worldSurface - 1.0)
				{
					if (Main.tile[num46, num48].HasTile)
					{
						int num49 = num48 + WorldGen.genRand.Next(10, 14);
						for (int num50 = num48; num50 < num49; num50++)
						{
							if ((Main.tile[num46, num50].TileType == 59 || Main.tile[num46, num50].TileType == 60) && num46 >= num41 + WorldGen.genRand.Next(5) && num46 < num42 - WorldGen.genRand.Next(5))
							{
								Main.tile[num46, num50].TileType = 0;
							}
						}
						break;
					}
					num48++;
				}
			}
			double num51 = Main.worldSurface + 40.0;
			for (int num52 = num41; num52 < num42; num52++)
			{
				num51 += (double)WorldGen.genRand.Next(-2, 3);
				if (num51 < Main.worldSurface + 30.0)
				{
					num51 = Main.worldSurface + 30.0;
				}
				if (num51 > Main.worldSurface + 50.0)
				{
					num51 = Main.worldSurface + 50.0;
				}
				int i2 = num52;
				bool flag52 = false;
				int num53 = (int)GenVars.worldSurfaceLow;
				while ((double)num53 < num51)
				{
					if (Main.tile[i2, num53].HasTile)
					{
						if (Main.tile[i2, num53].TileType == 53 && i2 >= num41 + WorldGen.genRand.Next(5) && i2 <= num42 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num53].TileType = 112;
						}
						if (Main.tile[i2, num53].TileType == 0 && (double)num53 < Main.worldSurface - 1.0 && !flag52)
						{
							WorldGen.grassSpread = 0;
							WorldGen.SpreadGrass(i2, num53, 0, 23, true);
						}
						flag52 = true;
						if (Main.tile[i2, num53].TileType == 1 && i2 >= num41 + WorldGen.genRand.Next(5) && i2 <= num42 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num53].TileType = 25;
						}
						if (Main.tile[i2, num53].WallType == 216)
						{
							Main.tile[i2, num53].WallType = 217;
						}
						else if (Main.tile[i2, num53].WallType == 187)
						{
							Main.tile[i2, num53].WallType = 220;
						}
						if (Main.tile[i2, num53].TileType == 2)
						{
							Main.tile[i2, num53].TileType = 23;
						}
						if (Main.tile[i2, num53].TileType == 161)
						{
							Main.tile[i2, num53].TileType = 163;
						}
						else if (Main.tile[i2, num53].TileType == 396)
						{
							Main.tile[i2, num53].TileType = 400;
						}
						else if (Main.tile[i2, num53].TileType == 397)
						{
							Main.tile[i2, num53].TileType = 398;
						}
					}
					num53++;
				}
			}
			for (int num54 = num41; num54 < num42; num54++)
			{
				for (int num55 = 0; num55 < Main.maxTilesY - 50; num55++)
				{
					if (Main.tile[num54, num55].HasTile && Main.tile[num54, num55].TileType == 31)
					{
						int num61 = num54 - 13;
						int num56 = num54 + 13;
						int num57 = num55 - 13;
						int num58 = num55 + 13;
						for (int num59 = num61; num59 < num56; num59++)
						{
							if (num59 > 10 && num59 < Main.maxTilesX - 10)
							{
								for (int num60 = num57; num60 < num58; num60++)
								{
									Tile tile = Main.tile[num59, num60];
									if (Math.Abs(num59 - num54) + Math.Abs(num60 - num55) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && Main.tile[num59, num60].TileType != 31)
									{
										tile.HasTile = true;
										Main.tile[num59, num60].TileType = 25;
										if (Math.Abs(num59 - num54) <= 1 && Math.Abs(num60 - num55) <= 1)
										{
											tile.HasTile = false;
										}
									}
									if (Main.tile[num59, num60].TileType != 31 && Math.Abs(num59 - num54) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num60 - num55) <= 2 + WorldGen.genRand.Next(3))
									{
										tile.HasTile = true;
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
	}
}
