using AltLibrary.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary.Common.Assets;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class AssetFactory {
	private static readonly Dictionary<Type, IProcessor> processes = new(4);

	private static void Load() {
		LibUtils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IProcessor)), (current, mod) => {
			var process = Activator.CreateInstance(current) as IProcessor;
			processes.TryAdd(process.ProcessType, process);
		});
	}

	public static T CreateSingle<T>(string path) where T : class {
		if (processes.TryGetValue(typeof(T), out IProcessor process)) {
			return process.Load(path) as T;
		}

		// oi shut up
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
		throw new ArgumentOutOfRangeException(nameof(processes), $"Couldn't find Processor that could load {typeof(T).FullName}!");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
	}

	public static T[] CreateMultiple<T>(Func<int, string> selector, int size) where T : class {
		var array = new T[size];

		int i = 0;
		for (; i < size - 4; i += 4) {
			array[i] = CreateSingle<T>(selector(i));
			array[i + 1] = CreateSingle<T>(selector(i + 1));
			array[i + 2] = CreateSingle<T>(selector(i + 2));
			array[i + 3] = CreateSingle<T>(selector(i + 3));
		}
		for (; i < size; i++) {
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

		static int GetIndex(int[] indices, int[] size) {
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
}
