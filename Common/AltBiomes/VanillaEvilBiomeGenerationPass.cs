using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace AltLibrary.Common.AltBiomes
{
	internal class CorruptionEvilBiomeGenerationPass : EvilBiomeGenerationPass
	{
		public override string ProgressMessage => Lang.gen[20].Value;
		public override bool CanGenerateNearDungeonOcean => false;

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
					while ((double)num39 < Main.worldSurface - 1.0)
					{
						if (Main.tile[n, num39].HasTile || Main.tile[n, num39].WallType > 0)
						{
							if (n == num33)
							{
								num38 = 20;
								WorldGen.ChasmRunner(n, num39, WorldGen.genRand.Next(150) + 150, true);
								break;
							}
							if (WorldGen.genRand.Next(35) == 0 && num38 == 0)
							{
								num38 = 30;
								bool makeOrb = true;
								WorldGen.ChasmRunner(n, num39, WorldGen.genRand.Next(50) + 50, makeOrb);
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
				while ((double)num40 < Main.worldSurface - 1.0)
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
				num43 += (double)WorldGen.genRand.Next(-2, 3);
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
				while ((double)num45 < num43)
				{
					if (Main.tile[i2, num45].HasTile)
					{
						if (Main.tile[i2, num45].TileType == 53 && i2 >= num34 + WorldGen.genRand.Next(5) && i2 <= num35 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num45].TileType = 112;
						}
						if (Main.tile[i2, num45].TileType == 0 && (double)num45 < Main.worldSurface - 1.0 && !flag7)
						{
							//WorldGen.grassSpread = 0;
							WorldGen.SpreadGrass(i2, num45, 0, 23, true, 0);
						}
						flag7 = true;
						if (Main.tile[i2, num45].TileType == 1 && i2 >= num34 + WorldGen.genRand.Next(5) && i2 <= num35 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num45].TileType = 25;
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
							Main.tile[i2, num45].TileType = 23;
						}
						if (Main.tile[i2, num45].TileType == 161)
						{
							Main.tile[i2, num45].TileType = 163;
						}
						else if (Main.tile[i2, num45].TileType == 396)
						{
							Main.tile[i2, num45].TileType = 400;
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
									if (Math.Abs(num52 - num46) + Math.Abs(num53 - num47) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && Main.tile[num52, num53].TileType != 31)
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
	}
    internal class CrimsonEvilBiomeGenerationPass : EvilBiomeGenerationPass
	{
		public override string ProgressMessage => Lang.gen[72].Value;
		public override int DrunkRNGMapCenterGive => 100;
        public override void GenerateEvil(int num15, int num16, int num17)
        {
			WorldGen.CrimStart(num15, (int)WorldGen.worldSurfaceLow - 10);
			for (int j = num16; j < num17; j++)
			{
				int num20 = (int)WorldGen.worldSurfaceLow;
				while ((double)num20 < Main.worldSurface - 1.0)
				{
					if (Main.tile[j, num20].HasTile)
					{
						int num21 = num20 + WorldGen.genRand.Next(10, 14);
						for (int k = num20; k < num21; k++)
						{
							if ((Main.tile[j, k].TileType == 59 || Main.tile[j, k].TileType == 60) && j >= num16 + WorldGen.genRand.Next(5) && j < num17 - WorldGen.genRand.Next(5))
							{
								Main.tile[j, k].TileType = 0;
							}
						}
						break;
					}
					num20++;
				}
			}
			double num22 = Main.worldSurface + 40.0;
			for (int l = num16; l < num17; l++)
			{
				num22 += (double)WorldGen.genRand.Next(-2, 3);
				if (num22 < Main.worldSurface + 30.0)
				{
					num22 = Main.worldSurface + 30.0;
				}
				if (num22 > Main.worldSurface + 50.0)
				{
					num22 = Main.worldSurface + 50.0;
				}
				int i2 = l;
				bool flag4 = false;
				int num23 = (int)WorldGen.worldSurfaceLow;
				while ((double)num23 < num22)
				{
					if (Main.tile[i2, num23].HasTile)
					{
						if (Main.tile[i2, num23].TileType == 53 && i2 >= num16 + WorldGen.genRand.Next(5) && i2 <= num17 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num23].TileType =234;
						}
						if (Main.tile[i2, num23].TileType == 0 && (double)num23 < Main.worldSurface - 1.0 && !flag4)
						{
							//WorldGen.grassSpread = 0;
							WorldGen.SpreadGrass(i2, num23, 0, 199, true, 0);
						}
						flag4 = true;
						if (Main.tile[i2, num23].WallType == 216)
						{
							Main.tile[i2, num23].WallType = 218;
						}
						else if (Main.tile[i2, num23].WallType == 187)
						{
							Main.tile[i2, num23].WallType = 221;
						}
						if (Main.tile[i2, num23].TileType == 1)
						{
							if (i2 >= num16 + WorldGen.genRand.Next(5) && i2 <= num17 - WorldGen.genRand.Next(5))
							{
								Main.tile[i2, num23].TileType =203;
							}
						}
						else if (Main.tile[i2, num23].TileType == 2)
						{
							Main.tile[i2, num23].TileType =199;
						}
						else if (Main.tile[i2, num23].TileType == 161)
						{
							Main.tile[i2, num23].TileType =200;
						}
						else if (Main.tile[i2, num23].TileType == 396)
						{
							Main.tile[i2, num23].TileType =401;
						}
						else if (Main.tile[i2, num23].TileType == 397)
						{
							Main.tile[i2, num23].TileType =399;
						}
					}
					num23++;
				}
			}
			int num24 = WorldGen.genRand.Next(10, 15);
			for (int m = 0; m < num24; m++)
			{
				int num25 = 0;
				bool flag5 = false;
				int num26 = 0;
				while (!flag5)
				{
					num25++;
					int x = WorldGen.genRand.Next(num16 - num26, num17 + num26);
					int num27 = WorldGen.genRand.Next((int)(Main.worldSurface - (double)(num26 / 2)), (int)(Main.worldSurface + 100.0 + (double)num26));
					while (WorldGen.oceanDepths(x, num27))
					{
						x = WorldGen.genRand.Next(num16 - num26, num17 + num26);
						num27 = WorldGen.genRand.Next((int)(Main.worldSurface - (double)(num26 / 2)), (int)(Main.worldSurface + 100.0 + (double)num26));
					}
					if (num25 > 100)
					{
						num26++;
						num25 = 0;
					}
					if (!Main.tile[x, num27].HasTile)
					{
						while (!Main.tile[x, num27].HasTile)
						{
							num27++;
						}
						num27--;
					}
					else
					{
						while (Main.tile[x, num27].HasTile && (double)num27 > Main.worldSurface)
						{
							num27--;
						}
					}
					if ((num26 > 10 || (Main.tile[x, num27 + 1].HasTile && Main.tile[x, num27 + 1].TileType == 203)) && !WorldGen.IsTileNearby(x, num27, 26, 3))
					{
						WorldGen.Place3x2(x, num27, 26, 1);
						if (Main.tile[x, num27].TileType == 26)
						{
							flag5 = true;
						}
					}
					if (num26 > 100)
					{
						flag5 = true;
					}
				}
			}
        }

        public override void PostGenerateEvil()
		{
			bool worldCrimson = WorldGen.crimson;
			WorldGen.crimson = false;
			WorldGen.CrimPlaceHearts();
			WorldGen.crimson = worldCrimson;
		}
    }
}
