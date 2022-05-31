using AltLibrary.Common;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltLiquidStyles;
using AltLibrary.Common.AltOres;
using AltLibrary.Common.Hooks;
using AltLibrary.Common.Systems;
using AltLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary
{
    public class AltLibrary : Mod
    {
        private static AltLibrary instance;

        public static AltLibrary Instance { get => instance; internal set => instance = value; }

        internal static List<AltBiome> Biomes = new();

        internal static List<AltOre> Ores = new();

        internal static List<AltLiquidStyle> LiquidStyles = new();

        internal static List<CustomPreviews> PreviewWorldIcons = new();

        // Spreading related lists.
        internal static List<int> planteraBulbs = new() { TileID.PlanteraBulb };
        internal static List<int> jungleGrass = new() { TileID.JungleGrass };
        internal static List<int> jungleThorns = new() { TileID.JungleThorns };
        internal static List<int> evilStoppingOres = new() { TileID.Chlorophyte, TileID.ChlorophyteBrick };

        internal static int HallowBunnyCageRecipeIndex;
        internal static int TimeHoveringOnIcon;
        internal static bool HallowBunnyUnlocked;
        internal static int ModIconVariation;

        public AltLibrary()
        {
            Instance = this;
        }

        public override void Load()
        {
            ILHooks.OnInitialize();
            AnimatedModIcon.Init();
            ALTextureAssets.Load();
            ALConvert.Load();
            ModIconVariation = Main.rand.Next(ALTextureAssets.AnimatedModIcon.Length);
            TimeHoveringOnIcon = 0;
            HallowBunnyUnlocked = false;
        }

        public override void PostSetupContent()
        {
            ALTextureAssets.PostContentLoad();
            ALConvertInheritanceData.FillData();
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
                        Logger.Info($"Registered custom preview! Seed: {seed} Path: {small} {medium} {large}");
                        return "Success";
                }
            }
            return null;
        }

        public override void Unload()
        {
            AnimatedModIcon.Unload();
            ALTextureAssets.Unload();
            ALConvert.Unload();
            AltLibraryConfig.Config = null;
            TimeHoveringOnIcon = 0;
            HallowBunnyUnlocked = false;
            PreviewWorldIcons = null;
            if (!Main.dedServ)
            {
                Instance = null;
            }
            Biomes = null;
            Ores = null;
            LiquidStyles = null;
            planteraBulbs = null;
            jungleGrass = null;
            jungleThorns = null;
            evilStoppingOres = null;
            ILHooks.Unload();
            AltLibraryConfig.Config = null;
            HallowBunnyCageRecipeIndex = 0;
        }

        public static AltBiome GetAltBiome(int type) => Biomes.Find(x => x.Type == type);

        public static int AltBiomeType<T>() where T : AltBiome => ModContent.GetInstance<T>()?.Type ?? 0;

        internal struct CustomPreviews
        {
            internal string seed;
            internal string pathSmall;
            internal string pathMedium;
            internal string pathLarge;

            internal CustomPreviews(string seed, string pathSmall, string pathMedium, string pathLarge)
            {
                if (seed is null)
                {
                    throw new ArgumentNullException(nameof(seed), "Cannot be null!");
                }
                if (pathSmall is null)
                {
                    throw new ArgumentNullException(nameof(pathSmall), "Cannot be null!");
                }
                if (pathMedium is null)
                {
                    throw new ArgumentNullException(nameof(pathMedium), "Cannot be null!");
                }
                if (pathLarge is null)
                {
                    throw new ArgumentNullException(nameof(pathLarge), "Cannot be null!");
                }

                this.seed = seed;
                this.pathSmall = pathSmall;
                this.pathMedium = pathMedium;
                this.pathLarge = pathLarge;
            }
        }
    }
}
