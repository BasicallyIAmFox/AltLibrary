using AltLibrary.Common;
using Terraria.ModLoader;

namespace AltLibrary;

public class AltLibrary : Mod {
	public static AltLibrary Instance { get; set; }

	public AltLibrary() {
		Instance = this;
	}

	public override void Load() {
	}

	public override void Unload() {
		StaticCollector.Clean();
		Instance = null;
	}
}