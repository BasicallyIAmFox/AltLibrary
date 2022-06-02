using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace AltLibrary.Common.AltBiomes
{
    public class EvilBiomeGenerationPass
    {
		public void GetEvilSpawnLocation(GenerationProgress progress, int dungeonSide, int dungeonLocation, out int evilBiomePositionTest, out int evilBiomePositionTestWestBound, out int evilBiomePositionTestEastBound)
		{
			int beachBordersWidth = 275;
			int beachSandRandomCenter = beachBordersWidth + 5 + 40;
			int evilBiomeBeachAvoidance = beachSandRandomCenter + 60;
			int evilBiomeAvoidanceMidFixer = 50;

			int JungleBoundMaxX = Main.maxTilesX;
			int JungleBoundMinX = 0;
			int SnowBoundMaxX = Main.maxTilesX;
			int SnowBoundMinX = 0;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				int DistFromCeil = 0;
				while ((double)DistFromCeil < Main.worldSurface)
				{
					if (Main.tile[i, DistFromCeil].HasTile)
					{
						if (Main.tile[i, DistFromCeil].TileType == TileID.JungleGrass)
						{
							if (i < JungleBoundMaxX)
							{
								JungleBoundMaxX = i;
							}
							if (i > JungleBoundMinX)
							{
								JungleBoundMinX = i;
							}
						}
						else if (Main.tile[i, DistFromCeil].TileType == TileID.SnowBlock || Main.tile[i, DistFromCeil].TileType == TileID.IceBlock)
						{
							if (i < SnowBoundMaxX)
							{
								SnowBoundMaxX = i;
							}
							if (i > SnowBoundMinX)
							{
								SnowBoundMinX = i;
							}
						}
					}
					DistFromCeil++;
				}
			}
			int BoundGive = 10;
			JungleBoundMaxX -= BoundGive;
			JungleBoundMinX += BoundGive;
			SnowBoundMaxX -= BoundGive;
			SnowBoundMinX += BoundGive;
			int nonDrunkBorderDist = 500;
			int dungeonGive = 100;

			int SnowBoundMaxX2 = SnowBoundMaxX;
			int SnowBoundMinX2 = SnowBoundMinX;
			int JungleBoundMaxX2 = JungleBoundMaxX;
			int JungleBoundMinX2 = JungleBoundMinX;

			#region MoveLater:
			int numPasses = 0;
			double maxPasses = (double)Main.maxTilesX * 0.00045;
			float value = (float)numPasses / (float)maxPasses;
			progress.Set(value);
            #endregion

            #region Baked?:
			bool drunkRNGTilt = false;
            #endregion

            bool FoundEvilLocation = false;
			evilBiomePositionTest = 0;
			evilBiomePositionTestWestBound = 0;
			evilBiomePositionTestEastBound = 0;

			while (!FoundEvilLocation)
			{
				FoundEvilLocation = true;
				int MapCenter = Main.maxTilesX / 2;
				int MapCenterGive = 200;

				if (WorldGen.drunkWorldGen)
				{
					MapCenterGive = 100; // DIFFERENCE 1 - CRIMSON ONLY

					if (drunkRNGTilt)
						evilBiomePositionTest = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.5), Main.maxTilesX - nonDrunkBorderDist);
					else
						evilBiomePositionTest = WorldGen.genRand.Next(nonDrunkBorderDist, (int)((double)Main.maxTilesX * 0.5));
				}
				else
				{
					evilBiomePositionTest = WorldGen.genRand.Next(nonDrunkBorderDist, Main.maxTilesX - nonDrunkBorderDist);
				}
				evilBiomePositionTestWestBound = evilBiomePositionTest - WorldGen.genRand.Next(200) - 100;
				evilBiomePositionTestEastBound = evilBiomePositionTest + WorldGen.genRand.Next(200) + 100;

				if (evilBiomePositionTestWestBound < evilBiomeBeachAvoidance)
				{
					evilBiomePositionTestWestBound = evilBiomeBeachAvoidance;
				}
				if (evilBiomePositionTestEastBound > Main.maxTilesX - evilBiomeBeachAvoidance)
				{
					evilBiomePositionTestEastBound = Main.maxTilesX - evilBiomeBeachAvoidance;
				}
				if (evilBiomePositionTest < evilBiomePositionTestWestBound + evilBiomeAvoidanceMidFixer)
				{
					evilBiomePositionTest = evilBiomePositionTestWestBound + evilBiomeAvoidanceMidFixer;
				}
				if (evilBiomePositionTest > evilBiomePositionTestEastBound - evilBiomeAvoidanceMidFixer)
				{
					evilBiomePositionTest = evilBiomePositionTestEastBound - evilBiomeAvoidanceMidFixer;
				}
				//DIFFERENCE 2 - CRIMSON ONLY
				if (dungeonSide < 0 && evilBiomePositionTestWestBound < 400)
				{
					evilBiomePositionTestWestBound = 400;
				}
				else if (dungeonSide > 0 && evilBiomePositionTestWestBound > Main.maxTilesX - 400)
				{
					evilBiomePositionTestWestBound = Main.maxTilesX - 400;
				}
				//DIFFERENCE 2 END
				if (evilBiomePositionTest > MapCenter - MapCenterGive && evilBiomePositionTest < MapCenter + MapCenterGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestWestBound > MapCenter - MapCenterGive && evilBiomePositionTestWestBound < MapCenter + MapCenterGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestEastBound > MapCenter - MapCenterGive && evilBiomePositionTestEastBound < MapCenter + MapCenterGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTest > WorldGen.UndergroundDesertLocation.X && evilBiomePositionTest < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestWestBound > WorldGen.UndergroundDesertLocation.X && evilBiomePositionTestWestBound < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestEastBound > WorldGen.UndergroundDesertLocation.X && evilBiomePositionTestEastBound < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestWestBound < dungeonLocation + dungeonGive && evilBiomePositionTestEastBound > dungeonLocation - dungeonGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestWestBound < SnowBoundMinX2 && evilBiomePositionTestEastBound > SnowBoundMaxX2)
				{
					SnowBoundMaxX2++;
					SnowBoundMinX2--;
					FoundEvilLocation = false;
				}
				if (evilBiomePositionTestWestBound < JungleBoundMinX2 && evilBiomePositionTestEastBound > JungleBoundMaxX2)
				{
					JungleBoundMaxX2++;
					JungleBoundMinX2--;
					FoundEvilLocation = false;
				}
			}

			//START GENERATING!
		}
    }
}
