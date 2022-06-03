using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AltLibrary.Common.AltBiomes
{

	internal static class EvilBiomeGenerationPassHandler {
		internal static bool GenerateAllCorruption(
			object encapsulated,
			GenerationProgress progress)
		{
			Type encapsuType = encapsulated.GetType();
			;
			int dungeonSide = (int)encapsuType.GetField("dungeonSide").GetValue(encapsulated);
			int dungeonLocation = (int)encapsuType.GetField("dungeonLocation").GetValue(encapsulated);

			int JungleBoundMinX = Main.maxTilesX;
			int JungleBoundMaxX = 0;
			int SnowBoundMinX = Main.maxTilesX;
			int SnowBoundMaxX = 0;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				int snowJungleIter = 0;
				while ((double)snowJungleIter < Main.worldSurface)
				{
					if (Main.tile[i, snowJungleIter].HasTile)
					{
						if (Main.tile[i, snowJungleIter].TileType == 60)
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
					EvilBiomeGenerationPass value = EvilBiomes[k];
					EvilBiomes[k] = EvilBiomes[n];
					EvilBiomes[n] = value;
				}
			}
			else
			{
				if (UIWorldCreationEdits.AltEvilBiomeChosenType == -333)
					EvilBiomes.Add(VanillaBiome.corruptPass);
				else if (UIWorldCreationEdits.AltEvilBiomeChosenType == -666)
					EvilBiomes.Add(VanillaBiome.crimsonPass);
				else
					EvilBiomes.Add(AltLibrary.Biomes[UIWorldCreationEdits.AltEvilBiomeChosenType].GetEvilBiomeGenerationPass());
			}
			

			double numberPasses = (double)Main.maxTilesX * 0.00045;
			numberPasses /= EvilBiomes.Count;

			int drunkIter = 0;
			int drunkMax = EvilBiomes.Count;

			EvilBiomes.ForEach(i =>
			{
				progress.Message = i.ProgressMessage;

				int passesDone = 0;
				while ((double)passesDone < numberPasses)
				{
					int evilMid;
					int evilLeft;
					int evilRight;

					i.GetEvilSpawnLocation(dungeonSide, dungeonLocation, SnowBoundMinX, SnowBoundMaxX, JungleBoundMinX, JungleBoundMaxX, drunkIter, drunkMax, out evilMid, out evilLeft, out evilRight);
					i.GenerateEvil(evilMid, evilLeft, evilRight);
					passesDone++;
				}
				i.PostGenerateEvil();
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
		private const int evilBiomeAvoidanceMidFixer = 50;
		private const int nonDrunkBorderDist = 500;
		private const int dungeonGive = 100;

		public virtual int DrunkRNGMapCenterGive => 200; //100 if crimson

		public virtual string ProgressMessage => "";

		/* This is the code which allows you to spawn the evil */
		public abstract void GenerateEvil(int evilBiomePosition, int evilBiomePositionWestBound, int evilBiomePositionEastBound);

		//use this if you need to do stuff after spawning all chasms
		public abstract void PostGenerateEvil();

		/*public sealed override void SetupContent()
		{
			SetStaticDefaults();
		}

        protected sealed override void Register()
        {
			if (EvilBiomeGenerationPassHandler.EvilBiomes == null)
				EvilBiomeGenerationPassHandler.EvilBiomes = new();
			EvilBiomeGenerationPassHandler.EvilBiomes.Add(this);
        }*/

        //Call this method if you somehow need to know a valid evil spawn location. This is automatically called when generating the world.
        //This is a very long function. Please do not overwrite this unless you absolutely know what you are doing.
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

					int diff = Main.maxTilesX - nonDrunkBorderDist - nonDrunkBorderDist;

					int left = nonDrunkBorderDist + diff * currentDrunkIter / (maxDrunkBorders);
					int right = nonDrunkBorderDist + diff * (currentDrunkIter + 1) / (maxDrunkBorders);

					evilBiomePosition = WorldGen.genRand.Next(left, right);

					/*
					if (drunkRNGTilt)
						evilBiomePosition = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.5), Main.maxTilesX - nonDrunkBorderDist);
					else
						evilBiomePosition = WorldGen.genRand.Next(nonDrunkBorderDist, (int)((double)Main.maxTilesX * 0.5));*/
				}
				else
				{
					evilBiomePosition = WorldGen.genRand.Next(nonDrunkBorderDist, Main.maxTilesX - nonDrunkBorderDist);
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
				if (evilBiomePosition < evilBiomePositionWestBound + evilBiomeAvoidanceMidFixer)
				{
					evilBiomePosition = evilBiomePositionWestBound + evilBiomeAvoidanceMidFixer;
				}
				if (evilBiomePosition > evilBiomePositionEastBound - evilBiomeAvoidanceMidFixer)
				{
					evilBiomePosition = evilBiomePositionEastBound - evilBiomeAvoidanceMidFixer;
				}
				//DIFFERENCE 2 - CRIMSON ONLY
				if (dungeonSide < 0 && evilBiomePositionWestBound < 400)
				{
					evilBiomePositionWestBound = 400;
				}
				else if (dungeonSide > 0 && evilBiomePositionWestBound > Main.maxTilesX - 400)
				{
					evilBiomePositionWestBound = Main.maxTilesX - 400;
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
				if (evilBiomePositionWestBound < dungeonLocation + dungeonGive && evilBiomePositionEastBound > dungeonLocation - dungeonGive)
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
