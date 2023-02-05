using AltLibrary.Common;
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
		LibTils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPostContent)), (current, mod)
			=> ((IPostContent)Activator.CreateInstance(current)).Load(mod));
	}

	public override void Unload() {
		// I'm not exactly sure about re-creating instance twice, honestly...
		LibTils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPostContent)), (current, mod)
			=> ((IPostContent)Activator.CreateInstance(current)).Unload());

		StaticCollector.Clean(this);
	}
}