using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;

namespace AltLibrary.Core
{
    internal class ALReflection
    {
        private static FieldInfo WorldGen_grassSpread = null;
        internal static WorldGenScanTileColumnAndRemoveClumps WorldGen_ScanTileColumnAndRemoveClumps = null;
        internal static FieldInfo UIList__innerList = null;
        internal static FieldInfo MiscShaderData__uImage0 = null;
        internal static FieldInfo MiscShaderData__uImage1 = null;

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
            UIList__innerList = typeof(UIList).GetField("_innerList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            MiscShaderData__uImage0 = typeof(MiscShaderData).GetField("_uImage0", BindingFlags.NonPublic | BindingFlags.Instance);
            MiscShaderData__uImage1 = typeof(MiscShaderData).GetField("_uImage1", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        internal static void Unload()
        {
            WorldGen_grassSpread = null;
            WorldGen_ScanTileColumnAndRemoveClumps = null;
            UIList__innerList = null;
            MiscShaderData__uImage0 = null;
            MiscShaderData__uImage1 = null;
        }
    }
}
