using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using System;
using Terraria;

namespace AltLibrary.Content.Biomes;

public sealed class UnderworldBiome : AltBiome<UnderworldBiomeGroup> {
	public override void SetStaticDefaults() {
	}

	public override void ModifyUnderworldLighting(ref float r, ref float g, ref float b, ref bool shouldTilesAffectLighting) {
		float intensity = 0.55f + MathF.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.08f;

		r = intensity;
		g = intensity * 0.6f;
		b = intensity * 0.2f;

		shouldTilesAffectLighting = false;
	}
}