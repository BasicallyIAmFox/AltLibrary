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

        internal static int WorldGen_GrassSpread
        {
            get => (int)WorldGen_grassSpread.GetValue(null);
            set => WorldGen_grassSpread.SetValue(null, value);
        }
        internal delegate ModTranslation LocalizationLoaderGetOrCreateTranslation(Mod mod, string key, bool defaultEmpty = false);
        internal delegate void WorldGenScanTileColumnAndRemoveClumps(int x);

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
        }

        internal static void Unload()
        {
            WorldGen_grassSpread = null;
            LocalizationLoader_GetOrCreateTranslation = null;
            WorldGen_ScanTileColumnAndRemoveClumps = null;
        }
    }
}
