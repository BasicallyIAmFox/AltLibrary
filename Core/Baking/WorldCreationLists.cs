using AltLibrary.Common.AltOres;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Core.Baking
{
    internal class ALWorldCreationLists
    {
        internal static OreCreationList prehmOreData;
        internal static HardmodeOreCreationList hmOreData;

        internal class ALWorldCreationLists_Loader : ILoadable
        {
            public void Load(Mod mod)
            {
                prehmOreData = new();
                hmOreData = new();
            }

            public void Unload()
            {
                prehmOreData = null;
                hmOreData = null;
            }
        }

        public static void FillData()
        {
            prehmOreData.Initialize();
            hmOreData.Initialize();
        }

        internal class OreCreationList : WorldCreationList
        {
            public override void Initialize()
            {
                List<AltOre> preOrder = new()
            {
                new VanillaOre("Copper", "Copper", -1, TileID.Copper, ItemID.CopperBar, OreType.Copper),
                new VanillaOre("Tin", "Tin", -2, TileID.Tin, ItemID.TinBar, OreType.Copper),
                new VanillaOre("Iron", "Iron", -3, TileID.Iron, ItemID.IronBar, OreType.Iron),
                new VanillaOre("Lead", "Lead", -4, TileID.Lead, ItemID.LeadBar, OreType.Iron),
                new VanillaOre("Silver", "Silver", -5, TileID.Silver, ItemID.SilverBar, OreType.Silver),
                new VanillaOre("Tungsten", "Tungsten", -6, TileID.Tungsten, ItemID.TungstenBar, OreType.Silver),
                new VanillaOre("Gold", "Gold", -7, TileID.Gold, ItemID.GoldBar, OreType.Gold),
                new VanillaOre("Platinum", "Platinum", -8, TileID.Platinum, ItemID.PlatinumBar, OreType.Gold)
            };
                foreach (AltOre ore in AltLibrary.Ores)
                {
                    if (ore.OreType <= OreType.Gold)
                    {
                        ore.CustomSelection(ore.OreType, preOrder);
                    }
                }
                foreach (AltOre ore in preOrder)
                {
                    AltLibrary.Instance.Logger.Info(ore.Name);
                }
                Ores = preOrder;
            }
        }

        internal class HardmodeOreCreationList : WorldCreationList
        {
            public override void Initialize()
            {
                List<AltOre> preOrder = new()
            {
                new VanillaOre("Cobalt", "Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar, OreType.Cobalt),
                new VanillaOre("Palladium", "Palladium", -10, TileID.Palladium, ItemID.PalladiumBar, OreType.Cobalt),
                new VanillaOre("Mythril", "Mythril", -11, TileID.Mythril, ItemID.MythrilBar, OreType.Mythril),
                new VanillaOre("Orichalcum", "Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar, OreType.Mythril),
                new VanillaOre("Adamantite", "Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar, OreType.Adamantite),
                new VanillaOre("Titanium", "Titanium", -14, TileID.Titanium, ItemID.TitaniumBar, OreType.Adamantite)
            };
                foreach (AltOre ore in AltLibrary.Ores)
                {
                    if (ore.OreType >= OreType.Cobalt)
                    {
                        ore.CustomSelection(ore.OreType, preOrder);
                    }
                }
                foreach (AltOre ore in preOrder)
                {
                    AltLibrary.Instance.Logger.Info(ore.Name);
                }
                Ores = preOrder;
            }
        }

        internal abstract class WorldCreationList
        {
            public List<AltOre> Ores = new();

            public abstract void Initialize();
        }
    }
}
