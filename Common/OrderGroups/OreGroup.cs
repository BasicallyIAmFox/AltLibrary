using AltLibrary.Common.AltTypes;
using Microsoft.Xna.Framework;
using System;

namespace AltLibrary.Common.OrderGroups;

public abstract class OreGroup : AOrderGroup<OreGroup, IAltOre>, IStaticOrderGroup {
	public sealed override string Texture => GetTexture();

	public static Rectangle? GetSourceRectangle() => new(60, 0, 30, 30);
	public static string GetTexture() => DefaultTexture;

	private protected override Type GetMainSubclass() {
		return typeof(AltOre<>);
	}
}
