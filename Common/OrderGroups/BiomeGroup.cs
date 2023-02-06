﻿using AltLibrary.Common.AltTypes;
using Microsoft.Xna.Framework;
using System;

namespace AltLibrary.Common.OrderGroups;

public abstract class BiomeGroup : AOrderGroup<BiomeGroup, IAltBiome>, IStaticOrderGroup {
	public sealed override string Texture => GetTexture();

	public static Rectangle GetSourceRectangle() => new(0, 0, 30, 30);
	public static string GetTexture() => DefaultTexture;

	private protected override Type GetMainSubclass() {
		return typeof(AltBiome<>);
	}
}