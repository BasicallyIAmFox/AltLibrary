using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using System.Collections.Generic;
using System.IO;
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
        public static int Copper { get; internal set; } = 0;
        public static int Iron { get; internal set; } = 0;
        public static int Silver { get; internal set; } = 0;
        public static int Gold { get; internal set; } = 0;
        public static int Cobalt { get; internal set; } = 0;
        public static int Mythril { get; internal set; } = 0;
        public static int Adamantite { get; internal set; } = 0;
        internal static int hmOreIndex = 0;

        //do not need to sync, world seed should be constant between players
        internal static AltOre[] drunkCobaltCycle;
        internal static AltOre[] drunkMythrilCycle;
        internal static AltOre[] drunkAdamantiteCycle;

        internal static PieData[] AltBiomeData;
        internal static float[] AltBiomePercentages;

        public override void Load()
        {
            drunkCobaltCycle = null;
            drunkMythrilCycle = null;
            drunkAdamantiteCycle = null;
        }

        internal static void AnalysisTiles()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            ThreadPool.QueueUserWorkItem(new WaitCallback(SmAtcb), 1);
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
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (tile.HasTile)
                    {
                        int type = tile.TileType;
                        if (type == 164 || type == 109 || type == 117 || type == 116 || type == TileID.HallowSandstone || type == TileID.HallowHardenedSand)
                        {
                            hallow++;
                            solid++;
                        }
                        if (type == 23 || type == 163 || type == 112 || type == 25 || type == TileID.CorruptSandstone || type == TileID.CorruptHardenedSand)
                        {
                            evil++;
                            solid++;
                        }
                        if (type == 199 || type == 234 || type == 203 || type == 200 || type == TileID.CrimsonSandstone || type == TileID.CrimsonHardenedSand)
                        {
                            crimson++;
                            solid++;
                        }
                        if (type == 2 || type == 477 || type == 1 || type == 60 || type == 60 || type == 53 || type == 161 || type == TileID.Sandstone || type == TileID.HardenedSand || extraPureSolid.Contains(type))
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
            Main.npcChatText = Language.GetTextValue("Mods.AltLibrary.AnalysisDone") + AnalysisDoneSpaces;
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
        }
    }
}
