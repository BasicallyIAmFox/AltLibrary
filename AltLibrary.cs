using AltLibrary.Common;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.IL;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
	}

	public override void Unload() {
	}
}