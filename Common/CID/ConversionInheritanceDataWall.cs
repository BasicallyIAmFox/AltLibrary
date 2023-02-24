using AltLibrary.Common.Solutions;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

public sealed class ConversionInheritanceDataWall : ConversionInheritanceData {
	public ConversionInheritanceDataWall() : base(WallLoader.WallCount) {
	}

	public override void Bake() {
		ISolution.solutions.ForEach(i => {
			for (int x = 0; x < WallLoader.WallCount; x++) {
				i.FillWallEntries(x, ref tiles[GetId(i.Type, x)]);
			}
		});
	}
}