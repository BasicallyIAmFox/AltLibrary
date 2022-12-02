using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace AltLibrary.Core.Generation
{
	internal class CrimsonEvilBiomeGenerationPass : EvilBiomeGenerationPass
	{
		public override string ProgressMessage => Lang.gen[72].Value;
		public override int DrunkRNGMapCenterGive => 100;
		public override void GenerateEvil(int num17, int num18, int num19)
		{
			WorldGen.CrimStart(num17, (int)GenVars.worldSurfaceLow - 10);
			for (int num22 = num18; num22 < num19; num22++)
			{
				int num23 = (int)GenVars.worldSurfaceLow;
				while ((double)num23 < Main.worldSurface - 1.0)
				{
					if (Main.tile[num22, num23].HasTile)
					{
						int num24 = num23 + WorldGen.genRand.Next(10, 14);
						for (int num25 = num23; num25 < num24; num25++)
						{
							if ((Main.tile[num22, num25].TileType == 59 || Main.tile[num22, num25].TileType == 60) && num22 >= num18 + WorldGen.genRand.Next(5) && num22 < num19 - WorldGen.genRand.Next(5))
							{
								Main.tile[num22, num25].TileType = 0;
							}
						}
						break;
					}
					num23++;
				}
			}
			double num26 = Main.worldSurface + 40.0;
			for (int num27 = num18; num27 < num19; num27++)
			{
				num26 += (double)WorldGen.genRand.Next(-2, 3);
				if (num26 < Main.worldSurface + 30.0)
				{
					num26 = Main.worldSurface + 30.0;
				}
				if (num26 > Main.worldSurface + 50.0)
				{
					num26 = Main.worldSurface + 50.0;
				}
				int i2 = num27;
				bool flag49 = false;
				int num28 = (int)GenVars.worldSurfaceLow;
				while ((double)num28 < num26)
				{
					if (Main.tile[i2, num28].HasTile)
					{
						if (Main.tile[i2, num28].TileType == 53 && i2 >= num18 + WorldGen.genRand.Next(5) && i2 <= num19 - WorldGen.genRand.Next(5))
						{
							Main.tile[i2, num28].TileType = 234;
						}
						if (Main.tile[i2, num28].TileType == 0 && (double)num28 < Main.worldSurface - 1.0 && !flag49)
						{
							WorldGen.grassSpread = 0;
							WorldGen.SpreadGrass(i2, num28, 0, 199, true);
						}
						flag49 = true;
						if (Main.tile[i2, num28].WallType == 216)
						{
							Main.tile[i2, num28].WallType = 218;
						}
						else if (Main.tile[i2, num28].WallType == 187)
						{
							Main.tile[i2, num28].WallType = 221;
						}
						if (Main.tile[i2, num28].TileType == 1)
						{
							if (i2 >= num18 + WorldGen.genRand.Next(5) && i2 <= num19 - WorldGen.genRand.Next(5))
							{
								Main.tile[i2, num28].TileType = 203;
							}
						}
						else if (Main.tile[i2, num28].TileType == 2)
						{
							Main.tile[i2, num28].TileType = 199;
						}
						else if (Main.tile[i2, num28].TileType == 161)
						{
							Main.tile[i2, num28].TileType = 200;
						}
						else if (Main.tile[i2, num28].TileType == 396)
						{
							Main.tile[i2, num28].TileType = 401;
						}
						else if (Main.tile[i2, num28].TileType == 397)
						{
							Main.tile[i2, num28].TileType = 399;
						}
					}
					num28++;
				}
			}
			int num29 = WorldGen.genRand.Next(10, 15);
			for (int num30 = 0; num30 < num29; num30++)
			{
				int num31 = 0;
				bool flag50 = false;
				int num32 = 0;
				while (!flag50)
				{
					num31++;
					int num33 = WorldGen.genRand.Next(num18 - num32, num19 + num32);
					int num34 = WorldGen.genRand.Next((int)(Main.worldSurface - (double)(num32 / 2)), (int)(Main.worldSurface + 100.0 + (double)num32));
					while (WorldGen.oceanDepths(num33, num34))
					{
						num33 = WorldGen.genRand.Next(num18 - num32, num19 + num32);
						num34 = WorldGen.genRand.Next((int)(Main.worldSurface - (double)(num32 / 2)), (int)(Main.worldSurface + 100.0 + (double)num32));
					}
					if (num31 > 100)
					{
						num32++;
						num31 = 0;
					}
					if (!Main.tile[num33, num34].HasTile)
					{
						while (!Main.tile[num33, num34].HasTile)
						{
							num34++;
						}
						num34--;
					}
					else
					{
						while (Main.tile[num33, num34].HasTile && (double)num34 > Main.worldSurface)
						{
							num34--;
						}
					}
					if ((num32 > 10 || (Main.tile[num33, num34 + 1].HasTile && Main.tile[num33, num34 + 1].TileType == 203)) && !WorldGen.IsTileNearby(num33, num34, 26, 3))
					{
						WorldGen.Place3x2(num33, num34, 26, 1);
						if (Main.tile[num33, num34].TileType == 26)
						{
							flag50 = true;
						}
					}
					if (num32 > 100)
					{
						flag50 = true;
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
