using AltLibrary.Common.AltOres;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

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

        public override void Load()
        {
            drunkCobaltCycle = null;
            drunkMythrilCycle = null;
            drunkAdamantiteCycle = null;
        }

        //move to utility function later
        private static void ShuffleArrayUsingSeed<T>(T[] list, UnifiedRandom seed)
        {
            int randIters = list.Length - 1; //-1 cause we don't wanna shuffle the back
            if (randIters == 1)
                return;
            while (randIters > 1)
            {
                int thisRand = seed.Next(randIters);
                randIters--;
                if (thisRand != randIters)
                {
                    (list[thisRand], list[randIters]) = (list[randIters], list[thisRand]);
                }
            }
        }

        private static void SendOriginalToToBackOfList(AltOre[] list, int original)
        {
            if (list.Length <= 1)
                return;
            for (int x = 0; x < list.Length - 1; x++)
            {
                if (list[x].Type == original)
                {
                    (list[^1], list[x]) = (list[x], list[^1]);
                    return;
                }
            }
        }

        public static void BakeDrunken()
        {
            UnifiedRandom rngSeed = new(WorldGen._genRandSeed); //bake seed later
            List<AltOre> hardmodeListing = new();
            hardmodeListing.Clear();
            hardmodeListing.Add(new VanillaOre("Cobalt", "Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar, OreType.Cobalt));
            hardmodeListing.Add(new VanillaOre("Palladium", "Palladium", -10, TileID.Palladium, ItemID.PalladiumBar, OreType.Cobalt));
            hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable));
            hardmodeListing.Add(new VanillaOre("Mythril", "Mythril", -11, TileID.Mythril, ItemID.MythrilBar, OreType.Mythril));
            hardmodeListing.Add(new VanillaOre("Orichalcum", "Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar, OreType.Mythril));
            hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril && x.Selectable));
            hardmodeListing.Add(new VanillaOre("Adamantite", "Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar, OreType.Adamantite));
            hardmodeListing.Add(new VanillaOre("Titanium", "Titanium", -14, TileID.Titanium, ItemID.TitaniumBar, OreType.Adamantite));
            hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable));

            drunkCobaltCycle = hardmodeListing.Where(x => x.OreType == OreType.Cobalt).ToArray();
            drunkMythrilCycle = hardmodeListing.Where(x => x.OreType == OreType.Mythril).ToArray();
            drunkAdamantiteCycle = hardmodeListing.Where(x => x.OreType == OreType.Adamantite).ToArray();

            SendOriginalToToBackOfList(drunkCobaltCycle, Cobalt);
            SendOriginalToToBackOfList(drunkMythrilCycle, Mythril);
            SendOriginalToToBackOfList(drunkAdamantiteCycle, Adamantite);

            ShuffleArrayUsingSeed(drunkCobaltCycle, rngSeed);
            ShuffleArrayUsingSeed(drunkMythrilCycle, rngSeed);
            ShuffleArrayUsingSeed(drunkAdamantiteCycle, rngSeed);
        }

        public static void GetDrunkenOres() {

            if (drunkCobaltCycle == null)
                BakeDrunken();

            int cobaltCycle = hmOreIndex % drunkCobaltCycle.Length;
            int mythrilCycle = hmOreIndex % drunkMythrilCycle.Length;
            int adamantiteCycle = hmOreIndex % drunkAdamantiteCycle.Length;

            WorldGen.SavedOreTiers.Cobalt = drunkCobaltCycle[cobaltCycle].ore;
            WorldGen.SavedOreTiers.Mythril = drunkMythrilCycle[mythrilCycle].ore;
            WorldGen.SavedOreTiers.Adamantite = drunkAdamantiteCycle[adamantiteCycle].ore;

            if (cobaltCycle == 0 && mythrilCycle == 0 && adamantiteCycle == 0)
                hmOreIndex = 0;
            hmOreIndex++;
        }

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
