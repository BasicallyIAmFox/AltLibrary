using AltLibrary.Common.CID;
using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using AltLibrary.Common.Solutions;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public partial interface IAltBiome : IAAltType, ILocalizedModType, ISolution {
	internal static List<IAltBiome> altBiomes = new(8);

	int[] ItemReplacements { get; }

	int GetAltBlock(int baseTile);
}
public abstract partial class AltBiome<T> : AAltType<AltBiome<T>, T, IAltBiome>, IAltBiome, ISolution
	where T : BiomeGroup {
	public override string LocalizationCategory => "AltBiome";

	public int[] ItemReplacements;

	int[] IAltBiome.ItemReplacements => ItemReplacements;

	public sealed override void SetupContent() {
		ItemReplacements = ItemID.Sets.Factory.CreateIntSet(ItemID.None);

		DataHandler = new DataHandler();
		base.SetupContent();

		ISolution.solutions.Add(this);
	}

	public virtual void FillTileEntries(int currentTileId, ref int tileEntry) {
	}

	public virtual void FillWallEntries(int currentWallId, ref int wallEntry) {
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
