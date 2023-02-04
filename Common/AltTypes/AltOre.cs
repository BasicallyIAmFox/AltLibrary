using AltLibrary.Common.MaterialContexts;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;

namespace AltLibrary.Common.AltTypes;

public interface IAltOre : IAAltType {
	internal static List<IAltOre> altOres = new(8);

	int OreTile { get; }
	int OreBar { get; }
	int OreItem { get; }
}
public abstract class AltOre<T> : AAltType<AltOre<T>, T, IAltOre, OreMaterialContext>, IAltOre where T : OreGroup {
	public int OreTile { get; protected set; }
	public int OreBar { get; protected set; }
	public int OreItem { get; protected set; }

	private protected override List<IAltOre> GetListOfTypes() => IAltOre.altOres;
}
