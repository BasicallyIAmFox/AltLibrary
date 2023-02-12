using AltLibrary.Common.Attributes;
using AltLibrary.Common.IL;
using AltLibrary.Common.IO;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.Utils;
using System;
using Terraria.Graphics.Light;

namespace AltLibrary.Common.AltTypes;

public partial interface IAltBiome {
	void ModifyUnderworldLighting(ref float r, ref float g, ref float b, ref bool shouldTilesAffectLighting);
}
public abstract partial class AltBiome<T> {
	public virtual void ModifyUnderworldLighting(ref float r, ref float g, ref float b, ref bool shouldTilesAffectLighting) {
	}
}
