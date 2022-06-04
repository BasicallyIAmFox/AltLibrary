using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltLiquidStyles;
using AltLibrary.Common.AltOres;
using AltLibrary.Common.Hooks;
using AltLibrary.Core;
using AltLibrary.Core.Baking;
using AltLibrary.Core.States;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

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

        /// <summary>
        ///     Gets or sets a value indicating whether the mouse should be checked in an interface or not.
        /// </summary>
        public bool CheckPointer { get; set; }

        internal static UserInterface userInterface;
        internal ALPieChartState pieChartState;

        public AltLibrary()
        {
            Instance = this;
        }

        public override void Load()
        {
            ALReflection.Init();
            ILHooks.OnInitialize();
            AnimatedModIcon.Init();
            ALTextureAssets.Load();
            ALConvert.Load();
            GuideHelpText.Load();
            UIChanges.Apply();
            FishingCrateLoot.Load();
            AnalystShopLoader.Load();
            ModIconVariation = Main.rand.Next(ALTextureAssets.AnimatedModIcon.Length);
            TimeHoveringOnIcon = 0;
            HallowBunnyUnlocked = false;
            if (!Main.dedServ)
            {
                pieChartState = new ALPieChartState();
                userInterface = new UserInterface();
                userInterface.SetState(pieChartState);
            }
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
                        {
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
                            Logger.Debug($"Registered custom preview! Seed: {seed} Path: {small} {medium} {large}");
                            return "Success";
                        }
                    case "conversionaddchildtile":
                        {
                            if (args.Length != 5)
                                throw new ArgumentException("Arguments cannot be less or more than 4 in length for AddChildTile");
                            if (args[1] is not int block)
                                throw new ArgumentException("Second argument (block) is not int!");
                            if (args[2] is not int parentBlock)
                                throw new ArgumentException("Third argument (parentBlock) is not int!");
                            if (args[3] is not BitsByte ForceDeconvert)
                                throw new ArgumentException("Fourth argument (ForceDeconvert) is not BitsByte!");
                            if (args[4] is not BitsByte BreakIfConversionFail)
                                throw new ArgumentException("Fifth argument (BreakIfConversionFail) is not BitsByte!");
                            ALConvertInheritanceData.AddChildTile(block, parentBlock, ForceDeconvert, BreakIfConversionFail);
                            return "Success";
                        }
                    case "conversionaddchildwall":
                        {
                            if (args.Length != 5)
                                throw new ArgumentException("Arguments cannot be less or more than 4 in length for AddChildWall");
                            if (args[1] is not int wall)
                                throw new ArgumentException("Second argument (wall) is not int!");
                            if (args[2] is not int parentWall)
                                throw new ArgumentException("Third argument (parentWall) is not int!");
                            if (args[3] is not BitsByte ForceDeconvert)
                                throw new ArgumentException("Fourth argument (ForceDeconvert) is not BitsByte!");
                            if (args[4] is not BitsByte BreakIfConversionFail)
                                throw new ArgumentException("Fifth argument (BreakIfConversionFail) is not BitsByte!");
                            ALConvertInheritanceData.AddChildWall(wall, parentWall, ForceDeconvert, BreakIfConversionFail);
                            return "Success";
                        }
                    case "conversiongettileparentdictionary":
                        if (args.Length != 1)
                            throw new ArgumentException("Arguments cannot be less or more than 0 in length for GetTileParentDictionary");
                        return ALConvertInheritanceData.GetTileParentDict();
                    case "conversiongetwallparentdictionary":
                        if (args.Length != 1)
                            throw new ArgumentException("Arguments cannot be less or more than 0 in length for GetWallParentDictionary");
                        return ALConvertInheritanceData.GetWallParentDict();
                    case "conversiongetultimateparent":
                        if (args.Length != 2)
                            throw new ArgumentException("Arguments cannot be less or more than 0 in length for GetUltimateParent");
                        if (args[1] is not int tile)
                            throw new ArgumentException("Second argument (tile) is not int!");
                        return ALConvertInheritanceData.GetUltimateParent(tile);
                    case "convert":
                        if (args.Length == 6)
                        {
                            if (args[1] is not Mod mod)
                                throw new ArgumentException("Second argument (mod) is not Mod!");
                            if (args[2] is not string name)
                                throw new ArgumentException("Third argument (name) is not string!");
                            if (args[3] is not int i)
                                throw new ArgumentException("Fourth argument (i) is not int!");
                            if (args[4] is not int j)
                                throw new ArgumentException("Fifth argument (j) is not int!");
                            if (args[5] is not int size)
                                throw new ArgumentException("Sixth argument (size) is not int!");
                            ALConvert.Convert(mod, name, i, j, size);
                        }
                        else if (args.Length == 5)
                        {
                            if (args[1] is not string fullname)
                                throw new ArgumentException("Second argument (fullname) is not string!");
                            if (args[2] is not int i)
                                throw new ArgumentException("Third argument (i) is not int!");
                            if (args[3] is not int j)
                                throw new ArgumentException("Fourth argument (j) is not int!");
                            if (args[4] is not int size)
                                throw new ArgumentException("Fifth argument (size) is not int!");
                            ALConvert.Convert(fullname, i, j, size);
                        }
                        else
                        {
                            throw new ArgumentException("Arguments cannot be less or more than 5 or 6 in length for Convert");
                        }
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
            GuideHelpText.Unload();
            UIChanges.Unapply();
            FishingCrateLoot.Unload();
            ALReflection.Unload();
            AnalystShopLoader.Unload();
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
            pieChartState = null;
            userInterface = null;
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
