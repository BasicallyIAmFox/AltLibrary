using AltLibrary.Common;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Core;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;

namespace AltLibrary
{
    public class AltLibrary : Mod
    {
        private static AltLibrary instance;

        public static AltLibrary Instance { get => instance; set => instance = value; }

        public static List<AltBiome> biomes = new();

        public static List<AltOre> ores = new();

        public static Dictionary<string, float> hellAltTrans = new();

        // Spreading related lists.
        public static List<int> planteraBulbs = new List<int> { TileID.PlanteraBulb };
        public static List<int> jungleGrass = new List<int> { TileID.JungleGrass };
        public static List<int> jungleThorns = new List<int> { TileID.JungleThorns };
        public static List<int> evilStoppingOres = new List<int> { TileID.Chlorophyte, TileID.ChlorophyteBrick };

        public AltLibrary()
        {
            Instance = this;
        }

        public override void Load()
        {
            ILHooks.OnInitialize();
        }

        public override void Unload()
        {
            AltLibraryConfig.Config = null;
        }
    }
}