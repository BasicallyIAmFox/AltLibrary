using AltLibrary.Common.CID;
using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public partial interface IAltBiome : IAAltType, ILocalizedModType {
	internal static List<IAltBiome> altBiomes = new(8);

	int GetAltBlock(int baseTile);
}
public abstract partial class AltBiome<T> : AAltType<AltBiome<T>, T, IAltBiome>, IAltBiome
	where T : BiomeGroup {
	public override string LocalizationCategory => "AltBiome";

	public sealed override void SetupContent() {
		DataHandler = new DataHandler();
		base.SetupContent();
	}

	public virtual int GetAltBlock(int baseTile) {
		var data = DataHandler.Get<ConversionData>();
		return baseTile switch {
			TileID.Stone => data.Stone == 0 ? data.Stone : ConversionInheritanceData.Keep,
			TileID.Grass => data.Grass == 0 ? data.Grass : ConversionInheritanceData.Keep,
			TileID.JungleGrass => data.JungleGrass == 0 ? data.JungleGrass : ConversionInheritanceData.Keep,
			TileID.GolfGrass => data.MowedGrass == 0 ? data.MowedGrass : ConversionInheritanceData.Keep,
			TileID.IceBlock => data.Ice == 0 ? data.Ice : ConversionInheritanceData.Keep,
			TileID.Sand => data.Sand == 0 ? data.Sand : ConversionInheritanceData.Keep,
			TileID.HardenedSand => data.HardSand == 0 ? data.HardSand : ConversionInheritanceData.Keep,
			TileID.Sandstone => data.Sandstone == 0 ? data.Sandstone : ConversionInheritanceData.Keep,
			TileID.CorruptThorns or TileID.CrimsonThorns => data.ThornBush == 0 ? data.ThornBush : ConversionInheritanceData.Keep,
			_ => ConversionInheritanceData.Keep,
		};
	}

	private protected override List<IAltBiome> GetListOfTypes() => IAltBiome.altBiomes;
}
