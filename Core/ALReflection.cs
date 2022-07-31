using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace AltLibrary.Core
{
	internal class ALReflection
	{
		internal static WorldGenScanTileColumnAndRemoveClumps WorldGen_ScanTileColumnAndRemoveClumps = null;
		internal static FieldInfo UIList__innerList = null;

		internal delegate void WorldGenScanTileColumnAndRemoveClumps(int x);

		internal static void Init()
		{
			WorldGen_ScanTileColumnAndRemoveClumps = typeof(WorldGen).GetMethod("ScanTileColumnAndRemoveClumps", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(int) }).CreateDelegate<WorldGenScanTileColumnAndRemoveClumps>();
			UIList__innerList = typeof(UIList).GetField("_innerList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		}

		internal static void Unload()
		{
			WorldGen_ScanTileColumnAndRemoveClumps = null;
			UIList__innerList = null;
		}
	}
}
