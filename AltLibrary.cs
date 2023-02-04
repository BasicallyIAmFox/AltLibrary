using AltLibrary.Common;
using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
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
		LibTils.ForEachContent<AltBiome<GoodBiomeGroup>>(biome => {
			Console.WriteLine(biome.FullName);

			var data = biome.DataHandler.Get<ConversionData>();
			Console.WriteLine($"Stone: {data.Stone}");
			Console.WriteLine($"Grass: {data.Grass}");
			Console.WriteLine($"Ice: {data.Ice}");
		});
#endif
	}

	public override void Unload() {
		StaticCollector.Clean(this);
		Instance = null;
	}
}