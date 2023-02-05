using AltLibrary.Common.Attributes;
using AltLibrary.Common.OrderGroups;
using System.Diagnostics;
using System.Reflection;
using System;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework;

namespace AltLibrary.Common.Cache;

[Cache]
public static class OGICallCache {
	internal static MethodInfo ModContent_GetInstance;
	internal static Func<string>[] orderGroupInstanceCallsCache;
	internal static Func<Rectangle>[] orderGroupInstanceCallsCache2;

	public static void Load() {
		var allTypes = ModContent.GetContent<IAOrderGroup>().SeparateTypes(typeof(AOrderGroup<,>)).ToArray();
		orderGroupInstanceCallsCache = new Func<string>[allTypes.Length];
		orderGroupInstanceCallsCache2 = new Func<Rectangle>[allTypes.Length];
		for (int i = 0, c = allTypes.Length; i < c; i++) {
			orderGroupInstanceCallsCache[i] = allTypes[i].Key
				.GetMethod(nameof(IStaticOrderGroup.GetTexture), BindingFlags.Static | BindingFlags.Public)
				.CreateDelegate<Func<string>>();

			orderGroupInstanceCallsCache2[i] = allTypes[i].Key
				.GetMethod(nameof(IStaticOrderGroup.GetSourceRectangle), BindingFlags.Static | BindingFlags.Public)
				.CreateDelegate<Func<Rectangle>>();
		}
	}
}
