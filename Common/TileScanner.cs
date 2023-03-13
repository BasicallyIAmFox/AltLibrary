using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SysVector2 = System.Numerics.Vector2;

namespace AltLibrary.Core;

public static class TileScanner {
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static bool ScanSquareInternal<T>(int centerX, int centerY, int radiusLeft, int radiusRight, int radiusTop, int radiusBottom, ushort type) where T : unmanaged, ITileData {
		var height = Main.tile.Height;

		var minX = Math.Max(centerX - radiusLeft, 0);
		var minY = Math.Max(centerY - radiusTop, 0);
		var maxX = Math.Min(centerX + radiusRight, Main.tile.Width);
		var maxY = Math.Min(centerY + radiusBottom, height);

		ref var arrayData = ref Unsafe.As<T, ushort>(ref MemoryMarshal.GetArrayDataReference(Main.tile.GetData<T>()));
		if (!Avx2.IsSupported) {
			int tempMaxX = maxX - (maxX % 4);
			for (int j = minY; j < maxY; j++) {
				int i = minX;
				for (; i < tempMaxX; i += 4) {
					if (maxX > (uint)i && Unsafe.Add(ref arrayData, j + i * height) == type) {
						return true;
					}
					if (maxX > (uint)(i + 1) && Unsafe.Add(ref arrayData, j + (i + 1) * height) == type) {
						return true;
					}
					if (maxX > (uint)(i + 2) && Unsafe.Add(ref arrayData, j + (i + 2) * height) == type) {
						return true;
					}
					if (maxX > (uint)(i + 3) && Unsafe.Add(ref arrayData, j + (i + 3) * height) == type) {
						return true;
					}
				}
				for (; i < maxX; i++) {
					if (Unsafe.Add(ref arrayData, j + i * height) == type) {
						return true;
					}
				}
			}
			return false;
		}

		var typeVector = Vector256.Create(type);
		var remCount = maxY - (maxY % Vector256<ushort>.Count);
		for (int i = minX; i < maxX; i++) {
			int j = minY;
			for (; j < remCount; j += Vector256<ushort>.Count) {
				if (LibUtils.EqualsAny(in typeVector, Unsafe.As<ushort, Vector256<ushort>>(ref Unsafe.Add(ref arrayData, j + (i * height))))) {
					return true;
				}
			}
			for (; j < maxY; j++) {
				if (Unsafe.Add(ref arrayData, j + i * height) == type) {
					return true;
				}
			}
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static bool ScanCircleInternal<T>(int centerX, int centerY, int radius, ushort type) where T : unmanaged, ITileData {
		var width = Main.tile.Width;
		var height = Main.tile.Height;

		var minX = Math.Max(centerX - radius, 0);
		var minY = Math.Max(centerY - radius, 0);
		var maxX = Math.Min(centerX + radius, width);
		var maxY = Math.Min(centerY + radius, height);

		ref var arrayData = ref Unsafe.As<T, ushort>(ref MemoryMarshal.GetArrayDataReference(Main.tile.GetData<T>()));
		var span = MemoryMarshal.CreateSpan(ref arrayData, width * height);
		var distVec = new SysVector2(centerX, centerY);
		var circleSquared = radius * radius;

		if (!Vector.IsHardwareAccelerated) {
			int tempMaxY = maxY - (maxY % 4);

			for (int i = minX; i < maxX; i++) {
				int j = minY;
				for (; j <= tempMaxY; j += 4) {
					if (span.Slice(j + i * height, 4).Contains(type)) {
						continue;
					}

					if (Unsafe.Add(ref arrayData, j + i * height) == type && SysVector2.DistanceSquared(new(i, j), distVec) <= circleSquared) {
						return true;
					}
					if (Unsafe.Add(ref arrayData, j + i * height + 1) == type && SysVector2.DistanceSquared(new(i, j + 1), distVec) <= circleSquared) {
						return true;
					}
					if (Unsafe.Add(ref arrayData, j + i * height + 2) == type && SysVector2.DistanceSquared(new(i, j + 2), distVec) <= circleSquared) {
						return true;
					}
					if (Unsafe.Add(ref arrayData, j + i * height + 3) == type && SysVector2.DistanceSquared(new(i, j + 3), distVec) <= circleSquared) {
						return true;
					}
				}
				for (; j <= maxY; j++) {
					if (Unsafe.Add(ref arrayData, j + i * height) == type && SysVector2.DistanceSquared(new(i, j), distVec) <= circleSquared) {
						return true;
					}
				}
			}
		}

		var typeVector = new Vector<ushort>(type);

		for (int i = minX; i < maxX; i++) {
			int j = minY;
			for (; j <= maxY - Vector<ushort>.Count; j += Vector<ushort>.Count) {
				if (!Vector.EqualsAny(typeVector, new Vector<ushort>(span.Slice(j + i * height, Vector<ushort>.Count)))) {
					continue;
				}
				for (int k = 0; k < Vector<ushort>.Count; k++) {
					if (Unsafe.Add(ref arrayData, j + k + i * height) == type && SysVector2.DistanceSquared(new(i, j + k), distVec) <= circleSquared) {
						return true;
					}
				}
			}
			for (; j < maxY; j++) {
				if (Unsafe.Add(ref arrayData, j + i * height) == type && SysVector2.DistanceSquared(new(i, j), distVec) <= circleSquared) {
					return true;
				}
			}
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanSquareTile<T>(Point16 center, Rectangle radius) where T : ModTile => ScanSquareTile(center.X, center.Y, radius.X, radius.Y, radius.Width, radius.Height, (ushort)ModContent.TileType<T>());
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanSquareTile(Point16 center, Rectangle radius, ushort tileType) => ScanSquareTile(center.X, center.Y, radius.X, radius.Y, radius.Width, radius.Height, tileType);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanSquareTile(int centerX, int centerY, int radiusLeft, int radiusRight, int radiusTop, int radiusBottom, ushort tileType) => ScanSquareInternal<TileTypeData>(centerX, centerY, radiusLeft, radiusRight, radiusBottom, radiusTop, tileType);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanSquareWall<T>(Point16 center, Rectangle radius) where T : ModWall => ScanSquareTile(center.X, center.Y, radius.X, radius.Y, radius.Width, radius.Height, (ushort)ModContent.WallType<T>());
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanSquareWall(Point16 center, Rectangle radius, ushort tileType) => ScanSquareTile(center.X, center.Y, radius.X, radius.Y, radius.Width, radius.Height, tileType);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanSquareWall(int centerX, int centerY, int radiusLeft, int radiusRight, int radiusTop, int radiusBottom, ushort wallType) => ScanSquareInternal<WallTypeData>(centerX, centerY, radiusLeft, radiusRight, radiusBottom, radiusTop, wallType);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanCircleTile<T>(Point16 center, int radius) where T : ModTile => ScanCircleTile(center.X, center.Y, radius, (ushort)ModContent.TileType<T>());
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanCircleTile(Point16 center, int radius, ushort tileType) => ScanCircleTile(center.X, center.Y, radius, tileType);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanCircleTile(int centerX, int centerY, int radius, ushort tileType) => ScanCircleInternal<TileTypeData>(centerX, centerY, radius, tileType);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanCircleWall<T>(Point16 center, int radius) where T : ModWall => ScanCircleWall(center.X, center.Y, radius, (ushort)ModContent.WallType<T>());
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanCircleWall(Point16 center, int radius, ushort tileType) => ScanCircleWall(center.X, center.Y, radius, tileType);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool ScanCircleWall(int centerX, int centerY, int radius, ushort wallType) => ScanCircleInternal<WallTypeData>(centerX, centerY, radius, wallType);
}
