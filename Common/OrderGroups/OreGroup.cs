using AltLibrary.Common.AltTypes;
using System;

namespace AltLibrary.Common.OrderGroups;

public abstract class OreGroup : AOrderGroup<OreGroup, IAltOre> {
	private protected override Type GetMainSubclass() {
		return typeof(AltOre<>);
	}
}
