using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;

namespace AltLibrary.Common.AltTypes;

public interface IAltOre : IAAltType {
	internal static List<IAltOre> altOres = new(8);
}
public abstract class AltOre<T> : AAltType<AltOre<T>, T, IAltOre>, IAltOre where T : OreGroup {
	public sealed override void SetupContent() {
		DataHandler = new OreDataHandler();
		base.SetupContent();
	}

	private protected override List<IAltOre> GetListOfTypes() => IAltOre.altOres;
}
