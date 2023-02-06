using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.OrderGroups;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary.Common.Cache;

[Cache(nameof(Load))]
public static class OGICallCache {
	internal static MethodInfo ModContent_GetInstance;
	internal static Func<string>[] orderGroupInstanceCallsCache;
	internal static Func<Rectangle?>[] orderGroupInstanceCallsCache2;
	internal static Func<Color>[] orderGroupInstanceCallsCache3;
	internal static IAAltType[] sampleCache;

	public static void Load() {
		var allTypes = ModContent.GetContent<IAOrderGroup>().SeparateTypes(typeof(AOrderGroup<,>)).ToArray();
		orderGroupInstanceCallsCache = new Func<string>[allTypes.Length];
		orderGroupInstanceCallsCache2 = new Func<Rectangle?>[allTypes.Length];
		orderGroupInstanceCallsCache3 = new Func<Color>[allTypes.Length];
		sampleCache = new IAAltType[allTypes.Length];

		for (int i = 0, c = allTypes.Length; i < c; i++) {
			orderGroupInstanceCallsCache[i] = allTypes[i].Key
				.GetMethod(nameof(IStaticOrderGroup.GetTexture), BindingFlags.Static | BindingFlags.Public)
				.CreateDelegate<Func<string>>();

			orderGroupInstanceCallsCache2[i] = allTypes[i].Key
				.GetMethod(nameof(IStaticOrderGroup.GetSourceRectangle), BindingFlags.Static | BindingFlags.Public)
				.CreateDelegate<Func<Rectangle?>>();

			orderGroupInstanceCallsCache3[i] = allTypes[i].Key
				.GetMethod(nameof(IStaticOrderGroup.GetColor), BindingFlags.Static | BindingFlags.Public)
				.CreateDelegate<Func<Color>>();

			sampleCache[i] = allTypes[i].First().Elements[0];
		}

		orderGroupInstanceCallsCache = orderGroupInstanceCallsCache.Reverse().ToArray();
		orderGroupInstanceCallsCache2 = orderGroupInstanceCallsCache2.Reverse().ToArray();
		orderGroupInstanceCallsCache3 = orderGroupInstanceCallsCache3.Reverse().ToArray();
		sampleCache = sampleCache.Reverse().ToArray();
	}
}
