using AltLibrary.Common;
using AltLibrary.Common.AltTypes;
using System;
using Terraria.ModLoader;

namespace AltLibrary;

public class AltLibrary : Mod {
	public static AltLibrary Instance { get; set; }

	public AltLibrary() {
		Instance = this;
	}

	public override void Load() {
	}

	public override void PostSetupContent() {
#if DEBUG
		LibTils.ForEachContent<IAltOre>(ore => {
			Console.WriteLine(ore.FullName);
		});
		LibTils.ForEachContent<IAltBiome>(biome => {
			Console.WriteLine(biome.FullName);
		});
#endif
	}

	public override void Unload() {
		StaticCollector.Clean(this);
		Instance = null;
	}
}