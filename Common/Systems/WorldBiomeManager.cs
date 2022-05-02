using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AltLibrary.Common.Systems
{
    public class WorldBiomeManager : ModSystem
    {
        public static string worldEvil = "";
        public static string worldHallow = "";
        public static string worldHell = "";
        public static string worldJungle = "";

        public override void PreWorldGen()
        {
            List<string> HallowList = new() { "" };
            List<string> HellList = new() { "" };
            List<string> JungleList = new() { "" };
            List<string> EvilList = new() { "", "" };
            foreach (AltBiome biome in AltLibrary.biomes)
            {
                if (biome.BiomeType == BiomeType.Hallow)
                {
                    HallowList.Add(biome.FullName);
                }
                if (biome.BiomeType == BiomeType.Hell)
                {
                    HellList.Add(biome.FullName);
                }
                if (biome.BiomeType == BiomeType.Jungle)
                {
                    JungleList.Add(biome.FullName);
                }
                if (biome.BiomeType == BiomeType.Evil)
                {
                    EvilList.Add(biome.FullName);
                }
            }
            worldHallow = HallowList[WorldGen.genRand.Next(HallowList.Count)];
            worldHell = HellList[WorldGen.genRand.Next(HellList.Count)];
            worldJungle = JungleList[WorldGen.genRand.Next(JungleList.Count)];

            WorldGen.crimson = WorldGen.genRand.NextBool(2);
            if (WorldGen.WorldGenParam_Evil == 0)
            {
                WorldGen.crimson = false;
            }
            if (WorldGen.WorldGenParam_Evil == 1)
            {
                WorldGen.crimson = true;
            }
            if (WorldGen.WorldGenParam_Evil == -1)
            {
                worldEvil = EvilList[WorldGen.genRand.Next(EvilList.Count)];
                if (worldEvil != "")
                {
                    WorldGen.crimson = false;
                }
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("AltLibrary:WorldEvil", worldEvil);
            tag.Add("AltLibrary:WorldHallow", worldHallow);
            tag.Add("AltLibrary:WorldHell", worldHell);
            tag.Add("AltLibrary:WorldJungle", worldJungle);

            Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
            AltLibraryConfig.WorldDataValues worldData;

            worldData.worldEvil = worldEvil;
            worldData.worldHallow = worldHallow;
            worldData.worldHell = worldHell;
            worldData.worldJungle = worldJungle;

            string path = Path.ChangeExtension(Main.worldPathName, ".twld");
            tempDict[path] = worldData;
            AltLibraryConfig.Config.SetWorldData(tempDict);
            AltLibraryConfig.Save(AltLibraryConfig.Config);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            worldEvil = tag.GetString("AltLibrary:WorldEvil");
            worldHallow = tag.GetString("AltLibrary:WorldHallow");
            worldHell = tag.GetString("AltLibrary:WorldHell");
            worldJungle = tag.GetString("AltLibrary:WorldJungle");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(worldEvil);
            writer.Write(worldHallow);
            writer.Write(worldHell);
            writer.Write(worldJungle);
        }

        public override void NetReceive(BinaryReader reader)
        {
            worldEvil = reader.ReadString();
            worldHallow = reader.ReadString();
            worldHell = reader.ReadString();
            worldJungle = reader.ReadString();
        }
    }
}
