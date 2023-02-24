using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

public sealed class ConversionInheritanceDataTile : ConversionInheritanceData {
	public ConversionInheritanceDataTile() : base(TileLoader.TileCount) {
	}

	public override void Bake() {
		ISolution.solutions.ForEach(i => {
			for (int x = 0; x < TileLoader.TileCount; x++) {
				i.FillTileEntries(x, ref tiles[GetId(i.Type, x)]);
			}
		});

		for (int x = 0; x < TileLoader.TileCount; x++) {
			if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				Set<GreenSolution>(x, TileID.GolfGrass);
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass) {
				Set<GreenSolution>(x, TileID.Grass);
			}
			else if (TileID.Sets.Conversion.JungleGrass[x] && x != TileID.JungleGrass) {
				Set<GreenSolution>(x, TileID.JungleGrass);
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x]) && x != TileID.Stone) {
				Set<GreenSolution>(x, TileID.Stone);
			}
			else if (TileID.Sets.Conversion.Ice[x] && x != TileID.IceBlock) {
				Set<GreenSolution>(x, TileID.IceBlock);
			}
			else if (TileID.Sets.Conversion.Sandstone[x] && x != TileID.Sandstone) {
				Set<GreenSolution>(x, TileID.Sandstone);
			}
			else if (TileID.Sets.Conversion.HardenedSand[x] && x != TileID.HardenedSand) {
				Set<GreenSolution>(x, TileID.HardenedSand);
			}
			else if (TileID.Sets.Conversion.Sand[x] && x != TileID.Sand) {
				Set<GreenSolution>(x, TileID.Sand);
			}
			else if (TileID.Sets.Conversion.Thorn[x] && x != TileID.JungleThorns) {
				Set<GreenSolution>(x, TileID.JungleThorns);
			}

			// Brown Solution
			if ((TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.Stone) {
				Set<BrownSolution>(x, TileID.Dirt);
			}
			else if (TileID.Sets.Conversion.GolfGrass[x] && x != TileID.GolfGrass) {
				Set<BrownSolution>(x, TileID.GolfGrass);
			}
			else if (TileID.Sets.Conversion.Grass[x] && x != TileID.Grass && x != TileID.GolfGrass) {
				Set<BrownSolution>(x, TileID.Grass);
			}

			// White Solution
			if ((TileID.Sets.Conversion.Grass[x] || TileID.Sets.Conversion.Sand[x] || TileID.Sets.Conversion.HardenedSand[x] || TileID.Sets.Conversion.Snow[x] || TileID.Sets.Conversion.Dirt[x]) && x != TileID.SnowBlock) {
				Set<WhiteSolution>(x, TileID.SnowBlock);
			}
			else if ((Main.tileMoss[x] || TileID.Sets.Conversion.Stone[x] || TileID.Sets.Conversion.Ice[x] || TileID.Sets.Conversion.Sandstone[x]) && x != TileID.IceBlock) {
				Set<WhiteSolution>(x, TileID.IceBlock);
			}

			// TODO: Add Yellow Solution
		}
	}
}
