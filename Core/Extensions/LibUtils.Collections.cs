using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AltLibrary;

internal static partial class LibUtils {
	public static T[] AddSize<T>(this T[] array, int extraLength) {
		Array.Resize(ref array, array.Length + extraLength);
		return array;
	}

	public static T[] AddSizeIfNotEnoughLength<T>(this T[] array, int extraLength) {
		return array.AddSizeIfNotEnoughLength(0, extraLength, extraLength);
	}

	public static T[] AddSizeIfNotEnoughLength<T>(this T[] array, int expectedLengthToBePut, int extraLength) {
		return array.AddSizeIfNotEnoughLength(0, expectedLengthToBePut, extraLength);
	}

	public static T[] AddSizeIfNotEnoughLength<T>(this T[] array, int startIndex, int expectedLengthToBePut, int extraLength) {
		if (startIndex + expectedLengthToBePut >= array.Length) {
			Array.Resize(ref array, array.Length + extraLength);
		}
		return array;
	}

	public static TType[] AddSelf<TType>(this TType[] collection, TType value) => collection.AddSizeIfNotEnoughLength(1).Insert(collection.Length - 1, value).ToArray();
	public static TType[] AddSelf<TType>(this TType[] collection, IEnumerable<TType> values) {
		var asArray = values.ToArray();
		var length = asArray.Length;

		return collection.AddSizeIfNotEnoughLength(length).Insert(collection.Length - length, asArray);
	}

	public static TCollection AddSelf<TCollection, TType>(this TCollection collection, TType value) where TCollection : ICollection<TType> {
		collection.Add(value);
		return collection;
	}

	public static TCollection AddSelf<TCollection, TType>(this TCollection collection, IEnumerable<TType> values) where TCollection : ICollection<TType> {
		foreach (var v in values) {
			collection.Add(v);
		}
		return collection;
	}

	public static int AddIndexReturn<T>(this ICollection<T> values, T value) {
		values.Add(value);
		return values.Count - 1;
	}

	public static T[] Insert<T>(this T[] array, int index, T value) {
		array[index] = value;
		return array;
	}

	public static T[] Insert<T>(this T[] array, int startIndex, T[] arrayValue) {
		var count = startIndex + arrayValue.Length;
		if ((uint)count > array.Length) {
			throw new IndexOutOfRangeException();
		}

		ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(array);
		ref var valueRef = ref MemoryMarshal.GetArrayDataReference(arrayValue);

		var i = startIndex;
		for (int c = count - (count % 4); i < c; i += 4) {
			Unsafe.Add(ref arrayRef, i) = Unsafe.Add(ref valueRef, i - startIndex);
			Unsafe.Add(ref arrayRef, i + 1) = Unsafe.Add(ref valueRef, i - startIndex + 1);
			Unsafe.Add(ref arrayRef, i + 2) = Unsafe.Add(ref valueRef, i - startIndex + 2);
			Unsafe.Add(ref arrayRef, i + 3) = Unsafe.Add(ref valueRef, i - startIndex + 3);
		}
		for (; i < count; i++) {
			Unsafe.Add(ref arrayRef, i) = Unsafe.Add(ref valueRef, i - startIndex);
		}
		return array;
	}
}
