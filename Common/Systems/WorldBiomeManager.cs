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
        internal static string drunkEvil = "";
        public static int Copper;
        public static int Iron;
        public static int Silver;
        public static int Gold;
        public static int Cobalt;
        public static int Mythril;
        public static int Adamantite;

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("AltLibrary:WorldEvil", worldEvil);
            tag.Add("AltLibrary:WorldHallow", worldHallow);
            tag.Add("AltLibrary:WorldHell", worldHell);
            tag.Add("AltLibrary:WorldJungle", worldJungle);
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

            worldData.worldEvil = worldEvil;
            worldData.worldHallow = worldHallow;
            worldData.worldHell = worldHell;
            worldData.worldJungle = worldJungle;
            worldData.drunkEvil = drunkEvil;

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
            writer.Write(worldEvil);
            writer.Write(worldHallow);
            writer.Write(worldHell);
            writer.Write(worldJungle);
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
            worldEvil = reader.ReadString();
            worldHallow = reader.ReadString();
            worldHell = reader.ReadString();
            worldJungle = reader.ReadString();
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
