using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AltLibrary.Common.Systems
{
    public class WorldBiomeManager : ModSystem
    {
        public static string WorldEvil { get; internal set; } = "";
        public static string WorldHallow { get; internal set; } = "";
        public static string WorldHell { get; internal set; } = "";
        public static string WorldJungle { get; internal set; } = "";
        internal static string drunkEvil = "";
        public static int Copper { get; internal set; } = 0;
        public static int Iron { get; internal set; } = 0;
        public static int Silver { get; internal set; } = 0;
        public static int Gold { get; internal set; } = 0;
        public static int Cobalt { get; internal set; } = 0;
        public static int Mythril { get; internal set; } = 0;
        public static int Adamantite { get; internal set; } = 0;

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
        }
    }
}
