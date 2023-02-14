using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public interface IAltOre : IAAltType, ILocalizedModType {
	internal static List<IAltOre> altOres = new(8);
}
public abstract class AltOre<T> : AAltType<AltOre<T>, T, IAltOre>, IAltOre where T : OreGroup {
	public override string LocalizationCategory => "AltOre";

	public sealed override void SetupContent() {
		DataHandler = new DataHandler();
		base.SetupContent();
	}

	private protected override List<IAltOre> GetListOfTypes() => IAltOre.altOres;
}
