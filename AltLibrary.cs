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
		ILHelper.Load();
	}

	public override void PostSetupContent() {
		LibTils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPostContent)), (current, mod)
			=> ((IPostContent)Activator.CreateInstance(current)).Load(mod));

		ILHelper.PostLoad();
	}

	public override void Unload() {
		// I'm not exactly sure about re-creating instance twice, honestly...
		LibTils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPostContent)), (current, mod)
			=> ((IPostContent)Activator.CreateInstance(current)).Unload());

		ILHelper.Unload(); // Have to unload ILs and Detours manually.
		StaticCollector.Clean(this);
	}
}