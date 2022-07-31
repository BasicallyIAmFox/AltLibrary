using System;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;

namespace AltLibrary.Core
{
	internal class ALReflection
	{
		private static FieldInfo WorldGen_grassSpread = null;
		internal static WorldGenScanTileColumnAndRemoveClumps WorldGen_ScanTileColumnAndRemoveClumps = null;
		internal static FieldInfo UIList__innerList = null;
		private static FieldInfo WorldGen_jChestX = null;
		private static FieldInfo WorldGen_jChestY = null;
		private static FieldInfo WorldGen_NumJChests = null;

		internal static int WorldGen_GrassSpread
		{
			get => (int)WorldGen_grassSpread.GetValue(null);
			set => WorldGen_grassSpread.SetValue(null, value);
		}
		internal static int WorldGen_numJChests
		{
			get => (int)WorldGen_NumJChests.GetValue(null);
			set => WorldGen_NumJChests.SetValue(null, value);
		}
		internal static int[] WorldGen_JChestX
		{
			get => (int[])WorldGen_jChestX.GetValue(null);
			set => WorldGen_jChestX.SetValue(null, value);
		}
		internal static int[] WorldGen_JChestY
		{
			get => (int[])WorldGen_jChestY.GetValue(null);
			set => WorldGen_jChestY.SetValue(null, value);
		}
		internal delegate void WorldGenScanTileColumnAndRemoveClumps(int x);

		internal static void Init()
		{
			WorldGen_grassSpread = typeof(WorldGen).GetField("grassSpread", BindingFlags.NonPublic | BindingFlags.Static);
			WorldGen_ScanTileColumnAndRemoveClumps = typeof(WorldGen).GetMethod("ScanTileColumnAndRemoveClumps", BindingFlags.NonPublic | BindingFlags.Static, new Type[] { typeof(int) }).CreateDelegate<WorldGenScanTileColumnAndRemoveClumps>();
			UIList__innerList = typeof(UIList).GetField("_innerList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			WorldGen_jChestX = typeof(WorldGen).GetField("JChestX", BindingFlags.NonPublic | BindingFlags.Static);
			WorldGen_jChestY = typeof(WorldGen).GetField("JChestY", BindingFlags.NonPublic | BindingFlags.Static);
			WorldGen_NumJChests = typeof(WorldGen).GetField("numJChests", BindingFlags.NonPublic | BindingFlags.Static);
		}

		internal static void Unload()
		{
			WorldGen_grassSpread = null;
			WorldGen_ScanTileColumnAndRemoveClumps = null;
			UIList__innerList = null;
			WorldGen_jChestX = null;
			WorldGen_jChestY = null;
			WorldGen_NumJChests = null;
		}
	}
}
