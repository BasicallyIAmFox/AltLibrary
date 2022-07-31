using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using PieData = AltLibrary.Core.UIs.ALUIPieChart.PieData;

namespace AltLibrary.Common.Systems
{
	public class WorldBiomeManager : ModSystem
	{
		public static string WorldEvil { get; internal set; } = "";
		public static string WorldHallow { get; internal set; } = "";
		public static string WorldHell { get; internal set; } = "";
		public static string WorldJungle { get; internal set; } = "";
		internal static string drunkEvil = "";
		internal static int drunkIndex = 0;
		internal static int drunkGoodGen = -1;
		internal static int drunkEvilGen = -1;
		public static int Copper { get; internal set; } = 0;
		public static int Iron { get; internal set; } = 0;
		public static int Silver { get; internal set; } = 0;
		public static int Gold { get; internal set; } = 0;
		public static int Cobalt { get; internal set; } = 0;
		public static int Mythril { get; internal set; } = 0;
		public static int Adamantite { get; internal set; } = 0;
		internal static int hmOreIndex = 0;

		public static bool IsCorruption => WorldEvil == "" && !WorldGen.crimson;
		public static bool IsCrimson => WorldEvil == "" && WorldGen.crimson;
		public static bool IsAnyModdedEvil => WorldEvil != "" && !WorldGen.crimson;

		//do not need to sync, world seed should be constant between players
		internal static AltOre[] drunkCobaltCycle;
		internal static AltOre[] drunkMythrilCycle;
		internal static AltOre[] drunkAdamantiteCycle;

		internal static PieData[] AltBiomeData;
		internal static float[] AltBiomePercentages;
		public static float PurityBiomePercentage => AltBiomePercentages == null ? 0f : AltBiomePercentages[0];
		public static float CorruptionBiomePercentage => AltBiomePercentages == null ? 0f : AltBiomePercentages[1];
		public static float CrimsonBiomePercentage => AltBiomePercentages == null ? 0f : AltBiomePercentages[2];
		public static float HallowBiomePercentage => AltBiomePercentages == null ? 0f : AltBiomePercentages[3];
		public static float[] GetBiomePercentages
		{
			get
			{
				List<float> list = AltBiomePercentages?.ToList();
				list?.RemoveRange(0, 4);
				return list?.ToArray();
			}
		}

		public override void Load()
		{
			drunkCobaltCycle = null;
			drunkMythrilCycle = null;
			drunkAdamantiteCycle = null;
			drunkGoodGen = 0;
			drunkEvilGen = 0;
		}

		public override void OnWorldLoad()
		{
			AltBiomePercentages = new float[AltLibrary.Biomes.Count + 5];
			AnalysisTiles(false);
		}

		public override void OnWorldUnload()
		{
			AltBiomeData = null;
			AltBiomePercentages = null;
		}

		internal static void AnalysisTiles(bool includeText = true)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
				return;
			ThreadPool.QueueUserWorkItem(new WaitCallback(SmAtcb), 1);
			if (includeText)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(text), 1);
			}

			static void text(object threadContext) => Main.npcChatText = Language.GetTextValue("Mods.AltLibrary.AnalysisDone", Main.LocalPlayer.name, Main.worldName) + AnalysisDoneSpaces;
		}

		private static void SmAtcb(object threadContext)
		{
			int solid = 0;
			int purity = 0;
			int hallow = 0;
			int evil = 0;
			int crimson = 0;
			int[] mods = new int[AltLibrary.Biomes.Count];
			List<int>[] modTiles = new List<int>[AltLibrary.Biomes.Count];
			List<int> extraPureSolid = new();
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				modTiles[biome.Type - 1] = new List<int>();
				if (biome.BiomeType == BiomeType.Evil || biome.BiomeType == BiomeType.Hallow)
				{
					if (biome.BiomeGrass.HasValue) modTiles[biome.Type - 1].Add(biome.BiomeGrass.Value);
					if (biome.BiomeStone.HasValue) modTiles[biome.Type - 1].Add(biome.BiomeStone.Value);
					if (biome.BiomeSand.HasValue) modTiles[biome.Type - 1].Add(biome.BiomeSand.Value);
					if (biome.BiomeIce.HasValue) modTiles[biome.Type - 1].Add(biome.BiomeIce.Value);
					if (biome.BiomeSandstone.HasValue) modTiles[biome.Type - 1].Add(biome.BiomeSandstone.Value);
					if (biome.BiomeHardenedSand.HasValue) modTiles[biome.Type - 1].Add(biome.BiomeHardenedSand.Value);
					if (biome.SpecialConversion.Count > 0)
					{
						foreach (KeyValuePair<int, int> pair in biome.SpecialConversion)
						{
							extraPureSolid.Add(pair.Key);
							modTiles[biome.Type - 1].Add(pair.Value);
						}
					}
				}
			}
			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int y = 0; y < Main.maxTilesY; y++)
				{
					Tile tile = Framing.GetTileSafely(x, y);
					if (tile.HasTile)
					{
						int type = tile.TileType;
						if (type == TileID.HallowedGrass || type == TileID.HallowedIce || type == TileID.Pearlsand || type == TileID.Pearlstone || type == TileID.HallowSandstone || type == TileID.HallowHardenedSand || type == TileID.GolfGrassHallowed)
						{
							hallow++;
							solid++;
						}
						if (type == TileID.CorruptGrass || type == TileID.CorruptIce || type == TileID.Ebonsand || type == TileID.Ebonstone || type == TileID.CorruptSandstone || type == TileID.CorruptHardenedSand)
						{
							evil++;
							solid++;
						}
						if (type == TileID.CrimsonGrass || type == TileID.FleshIce || type == TileID.Crimsand || type == TileID.Crimstone || type == TileID.CrimsonSandstone || type == TileID.CrimsonHardenedSand)
						{
							crimson++;
							solid++;
						}
						if (type == TileID.Grass || type == TileID.GolfGrass || type == TileID.Stone || type == TileID.JungleGrass || type == TileID.Sand || type == TileID.IceBlock || type == TileID.Sandstone || type == TileID.HardenedSand || extraPureSolid.Contains(type))
						{
							purity++;
							solid++;
						}
						for (int i = 0; i < mods.Length; i++)
						{
							if (modTiles[i].Contains(type))
							{
								mods[i]++;
								solid++;
							}
						}
					}
				}
			}

			AltBiomePercentages[0] = purity * 100f / (solid * 100f);
			AltBiomePercentages[1] = evil * 100f / (solid * 100f);
			AltBiomePercentages[2] = crimson * 100f / (solid * 100f);
			AltBiomePercentages[3] = hallow * 100f / (solid * 100f);
			for (int i = 0; i < mods.Length; i++)
			{
				AltBiomePercentages[i + 4] = mods[i] * 100f / (solid * 100f);
			}
		}
		internal const string AnalysisDoneSpaces = "\n\n\n\n\n\n\n\n\n\n";

		public override void Unload()
		{
			WorldEvil = null;
			WorldHallow = null;
			WorldHell = null;
			WorldJungle = null;
			drunkEvil = null;
			Copper = 0;
			Iron = 0;
			Silver = 0;
			Gold = 0;
			Cobalt = 0;
			Mythril = 0;
			Adamantite = 0;
			drunkIndex = 0;
			hmOreIndex = 0;
			drunkCobaltCycle = null;
			drunkMythrilCycle = null;
			drunkAdamantiteCycle = null;
			AltBiomeData = null;
			AltBiomePercentages = null;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("AltLibrary:WorldEvil", WorldEvil);
			tag.Add("AltLibrary:WorldHallow", WorldHallow);
			tag.Add("AltLibrary:WorldHell", WorldHell);
			tag.Add("AltLibrary:WorldJungle", WorldJungle);
			tag.Add("AltLibrary:DrunkEvil", drunkEvil);
			tag.Add("AltLibrary:Copper", Copper);
			tag.Add("AltLibrary:Iron", Iron);
			tag.Add("AltLibrary:Silver", Silver);
			tag.Add("AltLibrary:Gold", Gold);
			tag.Add("AltLibrary:Cobalt", Cobalt);
			tag.Add("AltLibrary:Mythril", Mythril);
			tag.Add("AltLibrary:Adamantite", Adamantite);
			tag.Add("AltLibrary:DrunkIndex", drunkIndex);
			tag.Add("AltLibrary:HardmodeOreIndex", hmOreIndex);
			tag.Add("AltLibrary:DrunkGoodGen", drunkGoodGen);
			tag.Add("AltLibrary:DrunkEvilGen", drunkEvilGen);

			Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
			AltLibraryConfig.WorldDataValues worldData;

			worldData.worldEvil = WorldEvil;
			worldData.worldHallow = WorldHallow;
			worldData.worldHell = WorldHell;
			worldData.worldJungle = WorldJungle;
			worldData.drunkEvil = drunkEvil;

			string path = Path.ChangeExtension(Main.worldPathName, ".twld");
			tempDict[path] = worldData;
			AltLibraryConfig.Config.SetWorldData(tempDict);
			AltLibraryConfig.Save(AltLibraryConfig.Config);
		}

		public override void LoadWorldData(TagCompound tag)
		{
			WorldEvil = tag.GetString("AltLibrary:WorldEvil");
			WorldHallow = tag.GetString("AltLibrary:WorldHallow");
			WorldHell = tag.GetString("AltLibrary:WorldHell");
			WorldJungle = tag.GetString("AltLibrary:WorldJungle");
			drunkEvil = tag.GetString("AltLibrary:DrunkEvil");
			Copper = tag.GetInt("AltLibrary:Copper");
			Iron = tag.GetInt("AltLibrary:Iron");
			Silver = tag.GetInt("AltLibrary:Silver");
			Gold = tag.GetInt("AltLibrary:Gold");
			Cobalt = tag.GetInt("AltLibrary:Cobalt");
			Mythril = tag.GetInt("AltLibrary:Mythril");
			Adamantite = tag.GetInt("AltLibrary:Adamantite");
			drunkIndex = tag.GetInt("AltLibrary:DrunkIndex");
			hmOreIndex = tag.GetInt("AltLibrary:HardmodeOreIndex");
			drunkGoodGen = tag.GetInt("AltLibrary:DrunkGoodGen");
			drunkEvilGen = tag.GetInt("AltLibrary:DrunkEvilGen");

			//reset every unload
			drunkCobaltCycle = null;
			drunkMythrilCycle = null;
			drunkAdamantiteCycle = null;
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(WorldEvil);
			writer.Write(WorldHallow);
			writer.Write(WorldHell);
			writer.Write(WorldJungle);
			writer.Write(drunkEvil);
			writer.Write(Copper);
			writer.Write(Iron);
			writer.Write(Silver);
			writer.Write(Gold);
			writer.Write(Cobalt);
			writer.Write(Mythril);
			writer.Write(Adamantite);
			writer.Write(drunkIndex);
			writer.Write(hmOreIndex);
			writer.Write(drunkGoodGen);
			writer.Write(drunkEvilGen);
		}

		public override void NetReceive(BinaryReader reader)
		{
			WorldEvil = reader.ReadString();
			WorldHallow = reader.ReadString();
			WorldHell = reader.ReadString();
			WorldJungle = reader.ReadString();
			drunkEvil = reader.ReadString();
			Copper = reader.ReadInt32();
			Iron = reader.ReadInt32();
			Silver = reader.ReadInt32();
			Gold = reader.ReadInt32();
			Cobalt = reader.ReadInt32();
			Mythril = reader.ReadInt32();
			Adamantite = reader.ReadInt32();
			drunkIndex = reader.ReadInt32();
			hmOreIndex = reader.ReadInt32();
			drunkGoodGen = reader.ReadInt32();
			drunkEvilGen = reader.ReadInt32();
		}
	}
}
