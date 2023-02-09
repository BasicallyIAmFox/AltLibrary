using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary.Common.Assets;

public sealed class AssetFactory : ILoadable {
	private static Dictionary<Type, IProcessor> processes = new(4);

	public void Load(Mod mod) {
		LibUtils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IProcessor)), (current, mod) => {
			var process = Activator.CreateInstance(current) as IProcessor;
			processes.TryAdd(process.ProcessType, process);
		});
	}

	public static T CreateSingle<T>(string path) where T : class {
		if (processes.TryGetValue(typeof(T), out IProcessor process)) {
			return process.Load(path) as T;
		}
		throw new ArgumentOutOfRangeException();
	}

	public static T[] CreateMultiple<T>(Func<int, string> selector, int size) where T : class {
		var array = new T[size];
		for (int i = 0; i < size; i++) {
			array[i] = CreateSingle<T>(selector(i));
		}
		return array;
	}

	public static TType[] CreateMultiple<TContent, TType>(Func<TContent, string> output) where TContent : ILoadable where TType : class {
		return ModContent.GetContent<TContent>().Select(x => CreateSingle<TType>(output(x))).ToArray();
	}

	public static Array CreateMultidimensional<T>(Func<int, string> selector, params int[] size) where T : class {
		var mdArray = Array.CreateInstance(typeof(T), size);
		RecursiveFill(new int[size.Length], size, 0);

		int GetIndex(int[] indices, int[] size) {
			int index = 0;
			for (int i = 0, c = indices.Length; i < c; i++) {
				index = index * size[i] + indices[i];
			}
			return index;
		}
		void RecursiveFill(int[] indices, int[] size, int dimension) {
			if (dimension == size.Length) {
				mdArray.SetValue(CreateSingle<T>(selector(GetIndex(indices, size))), indices);
				return;
			}
			for (int i = 0, c = size[dimension]; i < c; i++) {
				indices[dimension] = i;
				RecursiveFill(indices, size, dimension + 1);
			}
		}

		return mdArray;
	}

	public void Unload() {
	}
}
