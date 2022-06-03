using System.Reflection;
using Terraria;

namespace AltLibrary.Core
{
    internal class ALReflection
    {
        private static FieldInfo WorldGen_grassSpread = null;

        internal static int WorldGen_GrassSpread
        {
            get => (int)WorldGen_grassSpread.GetValue(null);
            set => WorldGen_grassSpread.SetValue(null, value);
        }

        internal static void Init()
        {
            WorldGen_grassSpread = typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static);
        }

        internal static void Unload()
        {
            WorldGen_grassSpread = null;
        }
    }
}
