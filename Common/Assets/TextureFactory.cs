using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Assets;

public static class TextureFactory {
	public static Asset<Texture2D> CreateSingle(string path) {
		return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
	}

	public static Asset<Texture2D>[] CreateMultiple(Func<int, string> selector, int size) {
		var array = new Asset<Texture2D>[size];
		var enumerator = array.GetEnumerator();
		int i = 0;

		while (enumerator.MoveNext()) {
			array[i] = CreateSingle(selector(i));
			i++;
		}

		return array;
	}

	public static Array CreateMultidimensional(Func<int, string> selector, params int[] size) {
		var mdArray = Array.CreateInstance(typeof(Asset<Texture2D>), size);
		RecursiveFill(new int[size.Length], 0);

		int GetIndex(int[] indices, int[] size) {
			int index = 0;
			for (int i = 0; i < indices.Length; i++) {
				index = index * size[i] + indices[i];
			}
			return index;
		}
		void RecursiveFill(int[] indices, int dimension) {
			if (dimension == size.Length) {
				mdArray.SetValue(CreateSingle(selector(GetIndex(indices, size))), indices);
				return;
			}
			for (int i = 0, c = size[dimension]; i < c; i++) {
				indices[dimension] = i;
				RecursiveFill(indices, dimension + 1);
			}
		}

		return mdArray;
	}
}
