using Terraria;
using Terraria.ID;

namespace AltLibrary.Core.Generation
{
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
				while (num20 < Main.worldSurface - 1.0)
				{
					if (Main.tile[j, num20].HasTile)
					{
						int num21 = num20 + WorldGen.genRand.Next(10, 14);
						for (int k = num20; k < num21; k++)
						{
							if ((Main.tile[j, k].TileType == TileID.Mud || Main.tile[j, k].TileType == TileID.JungleGrass) && j >= num16 + WorldGen.genRand.Next(5) && j < num17 - WorldGen.genRand.Next(5))
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
				num22 += WorldGen.genRand.Next(-2, 3);
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
				while (num23 < num22)
				{
					if (Main.tile[i2, num23].HasTile)
					{
						if (Main.tile[i2, num23].TileType == 53 && i2 >= num16 + WorldGen.genRand.Next(5) && i2 <= num17 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num23].TileType = 234;
						}
						if (Main.tile[i2, num23].TileType == 0 && num23 < Main.worldSurface - 1.0 && !flag4)
						{
							WorldGen.grassSpread = 0;
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
								Main.tile[i2, num23].TileType = 203;
							}
						}
						else if (Main.tile[i2, num23].TileType == 2)
						{
							Main.tile[i2, num23].TileType = 199;
						}
						else if (Main.tile[i2, num23].TileType == 161)
						{
							Main.tile[i2, num23].TileType = 200;
						}
						else if (Main.tile[i2, num23].TileType == 396)
						{
							Main.tile[i2, num23].TileType = 401;
						}
						else if (Main.tile[i2, num23].TileType == 397)
						{
							Main.tile[i2, num23].TileType = 399;
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
					int num27 = WorldGen.genRand.Next((int)(Main.worldSurface - num26 / 2), (int)(Main.worldSurface + 100.0 + num26));
					while (WorldGen.oceanDepths(x, num27))
					{
						x = WorldGen.genRand.Next(num16 - num26, num17 + num26);
						num27 = WorldGen.genRand.Next((int)(Main.worldSurface - num26 / 2), (int)(Main.worldSurface + 100.0 + num26));
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
						while (Main.tile[x, num27].HasTile && num27 > Main.worldSurface)
						{
							num27--;
						}
					}
					if ((num26 > 10 || Main.tile[x, num27 + 1].HasTile && Main.tile[x, num27 + 1].TileType == 203) && !WorldGen.IsTileNearby(x, num27, 26, 3))
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
