using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Core
{
    internal class ALReflection
    {
        private static FieldInfo WorldGen_grassSpread = null;
        internal static WorldGenScanTileColumnAndRemoveClumps WorldGen_ScanTileColumnAndRemoveClumps = null;

        internal static int WorldGen_GrassSpread
        {
            get => (int)WorldGen_grassSpread.GetValue(null);
            set => WorldGen_grassSpread.SetValue(null, value);
        }
        internal delegate void WorldGenScanTileColumnAndRemoveClumps(int x);

        internal static void Init()
        {
            WorldGen_grassSpread = typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static);
            WorldGen_ScanTileColumnAndRemoveClumps = typeof(WorldGen).GetMethod("ScanTileColumnAndRemoveClumps", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(int) }).CreateDelegate<WorldGenScanTileColumnAndRemoveClumps>();
        }

        internal static void Unload()
        {
            WorldGen_grassSpread = null;
            WorldGen_ScanTileColumnAndRemoveClumps = null;
        }
    }
}
