using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AltLibrary.Core.Generation
{
	internal static class EvilBiomeGenerationPassHandler
	{
		internal static bool GenerateAllCorruption(
			int dungeonSide,
			int dungeonLocation,
			GenerationProgress progress)
		{
			int JungleBoundMinX = Main.maxTilesX;
			int JungleBoundMaxX = 0;
			int SnowBoundMinX = Main.maxTilesX;
			int SnowBoundMaxX = 0;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				int snowJungleIter = 0;
				while (snowJungleIter < Main.worldSurface)
				{
					if (Main.tile[i, snowJungleIter].HasTile)
					{
						if (Main.tile[i, snowJungleIter].TileType == (WorldBiomeManager.WorldJungle == "" ? TileID.JungleGrass : ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeGrass.Value))
						{
							if (i < JungleBoundMinX)
							{
								JungleBoundMinX = i;
							}
							if (i > JungleBoundMaxX)
							{
								JungleBoundMaxX = i;
							}
						}
						else if (Main.tile[i, snowJungleIter].TileType == 147 || Main.tile[i, snowJungleIter].TileType == 161)
						{
							if (i < SnowBoundMinX)
							{
								SnowBoundMinX = i;
							}
							if (i > SnowBoundMaxX)
							{
								SnowBoundMaxX = i;
							}
						}
					}
					snowJungleIter++;
				}
			}

			int jungleSnowGive = 10;
			JungleBoundMinX -= jungleSnowGive;
			JungleBoundMaxX += jungleSnowGive;
			SnowBoundMinX -= jungleSnowGive;
			SnowBoundMaxX += jungleSnowGive;

			List<EvilBiomeGenerationPass> EvilBiomes = new();

			if (Main.drunkWorld)
			{
				EvilBiomes.Add(VanillaBiome.crimsonPass);
				EvilBiomes.Add(VanillaBiome.corruptPass);
				AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().ForEach(i => EvilBiomes.Add(i.GetEvilBiomeGenerationPass()));
				//shuffle list

				int n = EvilBiomes.Count;
				while (n > 1)
				{
					n--;
					int k = WorldGen.genRand.Next(n + 1);
					(EvilBiomes[n], EvilBiomes[k]) = (EvilBiomes[k], EvilBiomes[n]);
				}
			}
			else
			{
				if (WorldBiomeManager.WorldEvil == "" && !WorldGen.crimson)
					EvilBiomes.Add(VanillaBiome.corruptPass);
				else if (WorldBiomeManager.WorldEvil == "" && WorldGen.crimson)
					EvilBiomes.Add(VanillaBiome.crimsonPass);
				else
					EvilBiomes.Add(AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).GetEvilBiomeGenerationPass());
			}

			double numberPasses = Main.maxTilesX * 0.00045;
			numberPasses /= EvilBiomes.Count;

			int drunkIter = 0;
			int drunkMax = EvilBiomes.Count;

			EvilBiomes.ForEach(i =>
			{
				progress.Message = (i?.ProgressMessage) ?? "No ProgressMessage! Report that to Mod Developer!";

				int passesDone = 0;
				while (passesDone < numberPasses)
				{
					if (i != null)
					{
						i.GetEvilSpawnLocation(dungeonSide, dungeonLocation, SnowBoundMinX, SnowBoundMaxX, JungleBoundMinX, JungleBoundMaxX, drunkIter, drunkMax, out int evilMid, out int evilLeft, out int evilRight);
						i.GenerateEvil(evilMid, evilLeft, evilRight);
					}
					passesDone++;
				}
				if (i != null)
				{
					i?.PostGenerateEvil();
				}
				drunkIter++;
			});

			return false;
		}
	}

	public abstract class EvilBiomeGenerationPass
	{
		private const int beachBordersWidth = 275;
		private const int beachSandRandomCenter = beachBordersWidth + 5 + 40;
		private const int evilBiomeBeachAvoidance = beachSandRandomCenter + 60;
		public virtual int EvilBiomeAvoidanceMidFixer => 50;
		public virtual int NonDrunkBorderDist => 500;
		public virtual int DungeonGive => 100;

		public virtual int DrunkRNGMapCenterGive => 200; //100 if crimson

		public virtual bool CanGenerateNearDungeonOcean => true;

		public virtual string ProgressMessage => "";

		/* This is the code which allows you to spawn the evil */
		public abstract void GenerateEvil(int evilBiomePosition, int evilBiomePositionWestBound, int evilBiomePositionEastBound);

		/// <summary>
		/// Use this if you need to do stuff after spawning all chasms.
		/// </summary>
		public abstract void PostGenerateEvil();

		/// <summary>
		/// Call this method if you somehow need to know a valid evil spawn location.
		/// <br/>This is automatically called when generating the world.
		/// <br/>This is a very long function. Please do not overwrite this unless you absolutely know what you are doing.
		/// </summary>
		/// <param name="dungeonSide"></param>
		/// <param name="dungeonLocation"></param>
		/// <param name="SnowBoundMinX"></param>
		/// <param name="SnowBoundMaxX"></param>
		/// <param name="JungleBoundMinX"></param>
		/// <param name="JungleBoundMaxX"></param>
		/// <param name="currentDrunkIter"></param>
		/// <param name="maxDrunkBorders"></param>
		/// <param name="evilBiomePosition"></param>
		/// <param name="evilBiomePositionWestBound"></param>
		/// <param name="evilBiomePositionEastBound"></param>
		public virtual void GetEvilSpawnLocation(
			int dungeonSide,
			int dungeonLocation,

			int SnowBoundMinX,
			int SnowBoundMaxX,
			int JungleBoundMinX,
			int JungleBoundMaxX,

			int currentDrunkIter,
			int maxDrunkBorders,

			out int evilBiomePosition, out int evilBiomePositionWestBound, out int evilBiomePositionEastBound)
		{
			bool FoundEvilLocation = false;
			evilBiomePosition = 0;
			evilBiomePositionWestBound = 0;
			evilBiomePositionEastBound = 0;

			while (!FoundEvilLocation)
			{
				FoundEvilLocation = true;
				int MapCenter = Main.maxTilesX / 2;
				int MapCenterGive = 200;

				if (WorldGen.drunkWorldGen)
				{
					MapCenterGive = DrunkRNGMapCenterGive;

					int diff = Main.maxTilesX - NonDrunkBorderDist - NonDrunkBorderDist;

					int left = NonDrunkBorderDist + diff * currentDrunkIter / maxDrunkBorders;
					int right = NonDrunkBorderDist + diff * (currentDrunkIter + 1) / maxDrunkBorders;

					evilBiomePosition = WorldGen.genRand.Next(left, right);

					/*
					if (drunkRNGTilt)
						evilBiomePosition = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.5), Main.maxTilesX - nonDrunkBorderDist);
					else
						evilBiomePosition = WorldGen.genRand.Next(nonDrunkBorderDist, (int)((double)Main.maxTilesX * 0.5));*/
				}
				else
				{
					evilBiomePosition = WorldGen.genRand.Next(NonDrunkBorderDist, Main.maxTilesX - NonDrunkBorderDist);
				}
				evilBiomePositionWestBound = evilBiomePosition - WorldGen.genRand.Next(200) - 100;
				evilBiomePositionEastBound = evilBiomePosition + WorldGen.genRand.Next(200) + 100;

				if (evilBiomePositionWestBound < evilBiomeBeachAvoidance)
				{
					evilBiomePositionWestBound = evilBiomeBeachAvoidance;
				}
				if (evilBiomePositionEastBound > Main.maxTilesX - evilBiomeBeachAvoidance)
				{
					evilBiomePositionEastBound = Main.maxTilesX - evilBiomeBeachAvoidance;
				}
				if (evilBiomePosition < evilBiomePositionWestBound + EvilBiomeAvoidanceMidFixer)
				{
					evilBiomePosition = evilBiomePositionWestBound + EvilBiomeAvoidanceMidFixer;
				}
				if (evilBiomePosition > evilBiomePositionEastBound - EvilBiomeAvoidanceMidFixer)
				{
					evilBiomePosition = evilBiomePositionEastBound - EvilBiomeAvoidanceMidFixer;
				}
				//DIFFERENCE 2 - CRIMSON ONLY
				if (!CanGenerateNearDungeonOcean)
				{
					if (dungeonSide < 0 && evilBiomePositionWestBound < 400)
					{
						evilBiomePositionWestBound = 400;
					}
					else if (dungeonSide > 0 && evilBiomePositionWestBound > Main.maxTilesX - 400)
					{
						evilBiomePositionWestBound = Main.maxTilesX - 400;
					}
				}
				//DIFFERENCE 2 END
				if (evilBiomePosition > MapCenter - MapCenterGive && evilBiomePosition < MapCenter + MapCenterGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionWestBound > MapCenter - MapCenterGive && evilBiomePositionWestBound < MapCenter + MapCenterGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionEastBound > MapCenter - MapCenterGive && evilBiomePositionEastBound < MapCenter + MapCenterGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePosition > WorldGen.UndergroundDesertLocation.X && evilBiomePosition < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionWestBound > WorldGen.UndergroundDesertLocation.X && evilBiomePositionWestBound < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionEastBound > WorldGen.UndergroundDesertLocation.X && evilBiomePositionEastBound < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionWestBound < dungeonLocation + DungeonGive && evilBiomePositionEastBound > dungeonLocation - DungeonGive)
				{
					FoundEvilLocation = false;
				}
				if (evilBiomePositionWestBound < SnowBoundMinX && evilBiomePositionEastBound > SnowBoundMaxX)
				{
					SnowBoundMinX++;
					SnowBoundMaxX--;
					FoundEvilLocation = false;
				}
				if (evilBiomePositionWestBound < JungleBoundMinX && evilBiomePositionEastBound > JungleBoundMaxX)
				{
					JungleBoundMinX++;
					JungleBoundMaxX--;
					FoundEvilLocation = false;
				}
			}
			//START GENERATING!
		}
	}
}
