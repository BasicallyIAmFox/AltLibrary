using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using AltLibrary.Common.Hooks;
using AltLibrary.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary
{
    public class AltLibrary : Mod
    {
        private static AltLibrary instance;

        public static AltLibrary Instance { get => instance; internal set => instance = value; }

        public static List<AltBiome> Biomes = new();

        public static List<AltOre> Ores = new();

        public static Dictionary<string, float> HellAltTrans = new();
        internal static List<CustomPreviews> PreviewWorldIcons = new();

        // Spreading related lists.
        public static List<int> planteraBulbs = new() { TileID.PlanteraBulb };
        public static List<int> jungleGrass = new() { TileID.JungleGrass };
        public static List<int> jungleThorns = new() { TileID.JungleThorns };
        public static List<int> evilStoppingOres = new() { TileID.Chlorophyte, TileID.ChlorophyteBrick };

        internal static int TimeHoveringOnIcon = 0;
        internal static bool HallowBunnyUnlocked = false;
        internal static int ModIconVariation = 0;

        public AltLibrary()
        {
            Instance = this;
        }

        public override void Load()
        {
            ILHooks.OnInitialize();
            AnimatedModIcon.Init();
            ALTextureAssets.Load();
            ModIconVariation = Main.rand.Next(ALTextureAssets.AnimatedModIcon.Length);
            TimeHoveringOnIcon = 0;
            HallowBunnyUnlocked = false;
        }

        public override object Call(params object[] args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args), "Arguments cannot be null!");
            if (args.Length == 0)
                throw new ArgumentException("Arguments cannot be empty!");
            if (args[0] is string content)
            {
                switch (content.ToLower())
                {
                    case "addcustomseedpreviews":
                        if (args.Length != 5)
                            throw new ArgumentException("Arguments cannot be less or more than 5 in length for AddCustomSeedPreviews");
                        if (args[1] is not string seed)
                            throw new ArgumentException("Second argument (seed) is not string!");
                        if (args[2] is not string small)
                            throw new ArgumentException("Third argument (small) is not string!");
                        if (args[3] is not string medium)
                            throw new ArgumentException("Fourth argument (medium) is not string!");
                        if (args[4] is not string large)
                            throw new ArgumentException("Fifth argument (large) is not string!");
                        PreviewWorldIcons.Add(new CustomPreviews(seed, small, medium, large));
                        break;
                }
            }
            return null;
        }

        public override void Unload()
        {
            AnimatedModIcon.Unload();
            ALTextureAssets.Unload();
            AltLibraryConfig.Config = null;
            TimeHoveringOnIcon = 0;
            HallowBunnyUnlocked = false;
        }

        internal struct CustomPreviews
        {
            internal string seed;
            internal string pathSmall;
            internal string pathMedium;
            internal string pathLarge;

            public CustomPreviews(string seed, string pathSmall, string pathMedium, string pathLarge)
            {
                this.seed = seed;
                this.pathSmall = pathSmall;
                this.pathMedium = pathMedium;
                this.pathLarge = pathLarge;
            }
        }
    }
}