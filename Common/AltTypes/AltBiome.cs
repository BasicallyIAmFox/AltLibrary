using AltLibrary.Common.CID;
using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;
using Terraria.ID;

namespace AltLibrary.Common.AltTypes;

public interface IAltBiome : IAAltType {
	internal static List<IAltBiome> altBiomes = new(8);

	int GetAltBlock(in int baseTile, in ushort x, in ushort y);
}
public abstract class AltBiome<T> : AAltType<AltBiome<T>, T, IAltBiome>, IAltBiome where T : BiomeGroup {
	public sealed override void SetupContent() {
		DataHandler = new BiomeDataHandler();
		base.SetupContent();
	}

	public virtual int GetAltBlock(in int baseTile, in ushort x, in ushort y) {
		var data = DataHandler.Get<ConversionData>();
		return baseTile switch {
			TileID.Stone => data.Stone == 0 ? data.Stone : CIData.KEEP,
			TileID.Grass => data.Grass == 0 ? data.Grass : CIData.KEEP,
			TileID.JungleGrass => data.JungleGrass == 0 ? data.JungleGrass : CIData.KEEP,
			TileID.GolfGrass => data.MowedGrass == 0 ? data.MowedGrass : CIData.KEEP,
			TileID.IceBlock => data.Ice == 0 ? data.Ice : CIData.KEEP,
			TileID.Sand => data.Sand == 0 ? data.Sand : CIData.KEEP,
			TileID.HardenedSand => data.HardSand == 0 ? data.HardSand : CIData.KEEP,
			TileID.Sandstone => data.Sandstone == 0 ? data.Sandstone : CIData.KEEP,
			TileID.CorruptThorns => data.ThornBush == 0 ? data.ThornBush : CIData.KEEP,
			_ => CIData.KEEP,
		};
	}

	private protected override List<IAltBiome> GetListOfTypes() => IAltBiome.altBiomes;
}
