using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Core
{
    internal class ALReflection
    {
        private static FieldInfo WorldGen_grassSpread = null;
        internal static LocalizationLoaderGetOrCreateTranslation LocalizationLoader_GetOrCreateTranslation = null;
        internal static WorldGenScanTileColumnAndRemoveClumps WorldGen_ScanTileColumnAndRemoveClumps = null;
        private static object ExxoWorldGen_GetInstance;
        private static PropertyInfo ExxoWorldGen_rhodiumOre = null;
        private static PropertyInfo ExxoWorldGen_shmTier1Ore = null;
        private static PropertyInfo ExxoWorldGen_shmTier2Ore = null;

        internal static int WorldGen_GrassSpread
        {
            get => (int)WorldGen_grassSpread.GetValue(null);
            set => WorldGen_grassSpread.SetValue(null, value);
        }
        internal delegate ModTranslation LocalizationLoaderGetOrCreateTranslation(Mod mod, string key, bool defaultEmpty = false);
        internal delegate void WorldGenScanTileColumnAndRemoveClumps(int x);
        internal static int? ExxoWorldGen_RhodiumOre
        {
            get => (int?)ExxoWorldGen_rhodiumOre.GetValue(ExxoWorldGen_GetInstance);
            set => ExxoWorldGen_rhodiumOre.SetValue(ExxoWorldGen_GetInstance, Enum.ToObject(AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen+RhodiumVariant"), value));
        }
        internal static int? ExxoWorldGen_SHMTier1Ore
        {
            get => (int?)ExxoWorldGen_shmTier1Ore.GetValue(ExxoWorldGen_GetInstance);
            set => ExxoWorldGen_shmTier1Ore.SetValue(ExxoWorldGen_GetInstance, Enum.ToObject(AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen+SHMTier1Variant"), value));
        }
        internal static int? ExxoWorldGen_SHMTier2Ore
        {
            get => (int?)ExxoWorldGen_shmTier2Ore.GetValue(ExxoWorldGen_GetInstance);
            set => ExxoWorldGen_shmTier2Ore.SetValue(ExxoWorldGen_GetInstance, Enum.ToObject(AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen+SHMTier2Variant"), value));
        }

        internal static void Init()
        {
            WorldGen_grassSpread = typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static);
            if (BuildInfo.IsStable)
            {
                LocalizationLoader_GetOrCreateTranslation = typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, new Type[] { typeof(Mod), typeof(string), typeof(bool) }).CreateDelegate<LocalizationLoaderGetOrCreateTranslation>();
            }
            else
            {
                LocalizationLoader_GetOrCreateTranslation = typeof(LocalizationLoader).GetMethod("GetOrCreateTranslation", BindingFlags.Public | BindingFlags.Static, new Type[] { typeof(Mod), typeof(string), typeof(bool) }).CreateDelegate<LocalizationLoaderGetOrCreateTranslation>();
            }
            WorldGen_ScanTileColumnAndRemoveClumps = typeof(WorldGen).GetMethod("ScanTileColumnAndRemoveClumps", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(int) }).CreateDelegate<WorldGenScanTileColumnAndRemoveClumps>();
            if (AltLibrary.Avalon != null)
            {
                ExxoWorldGen_GetInstance = typeof(ModContent).GetMethod(nameof(ModContent.GetInstance)).MakeGenericMethod(AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen")).Invoke(null, Array.Empty<object>());
                ExxoWorldGen_rhodiumOre = AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen").GetProperty("RhodiumOre", BindingFlags.Public | BindingFlags.Instance);
                ExxoWorldGen_shmTier1Ore = AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen").GetProperty("SHMTier1Ore", BindingFlags.Public | BindingFlags.Instance);
                ExxoWorldGen_shmTier2Ore = AltLibrary.Avalon.GetType().Assembly.GetType("AvalonTesting.Systems.ExxoWorldGen").GetProperty("SHMTier2Ore", BindingFlags.Public | BindingFlags.Instance);
            }
        }

        internal static void Unload()
        {
            WorldGen_grassSpread = null;
            LocalizationLoader_GetOrCreateTranslation = null;
            WorldGen_ScanTileColumnAndRemoveClumps = null;
            ExxoWorldGen_GetInstance = null;
            ExxoWorldGen_rhodiumOre = null;
            ExxoWorldGen_shmTier1Ore = null;
            ExxoWorldGen_shmTier2Ore = null;
        }
    }
}
