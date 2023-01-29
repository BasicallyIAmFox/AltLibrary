using AltLibrary.Common.AltBiomes;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace AltLibrary.Common.Hooks
{
	internal class DryadText
	{
		public static void Init()
		{
			EditsHelper.On<Lang>(nameof(Lang.GetDryadWorldStatusDialog), Lang_GetDryadWorldStatusDialog);
			EditsHelper.IL<WorldGen>(nameof(WorldGen.AddUpAlignmentCounts), WorldGen_AddUpAlignmentCounts);
		}

		public static void Unload()
		{
		}

		private static string Lang_GetDryadWorldStatusDialog(On_Lang.orig_GetDryadWorldStatusDialog orig, out bool worldIsEntirelyPure)
		{
			string text2;
			int tGood = WorldGen.tGood;
			int tEvil = WorldGen.tEvil + WorldGen.tBlood;
			worldIsEntirelyPure = false;
			if (tGood > 0 && tEvil > 0)
			{
				text2 = Language.GetTextValue("Mods.AltLibrary.DryadSpecialText.WorldStatusGoodEvil", Main.worldName, tGood, tEvil);
			}
			else if (tEvil > 0)
			{
				text2 = Language.GetTextValue("Mods.AltLibrary.DryadSpecialText.WorldStatusEvil", Main.worldName, tEvil);
			}
			else
			{
				if (tGood <= 0)
				{
					worldIsEntirelyPure = true;
					return Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
				}
				text2 = Language.GetTextValue("Mods.AltLibrary.DryadSpecialText.WorldStatusGood", Main.worldName, tGood);
			}
			string arg;
			if (tGood * 1.2 >= tEvil && tGood * 0.8 <= tEvil) {
				arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced");
			}
			else {
				if (tGood >= tEvil) {
					arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale");
				}
				else {
					if (tEvil > tGood + 20) {
						arg = (Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim"));
					}
					else {
						if (tEvil <= 5) {
							arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionClose");
						}
						else {
							arg = Language.GetTextValue("DryadSpecialText.WorldDescriptionWork");
						}
					}
				}
			}

			return text2 + " " + arg;
		}

		private static void WorldGen_AddUpAlignmentCounts(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
			{
				AltLibrary.Instance.Logger.Info("2 $ 1");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
			{
				AltLibrary.Instance.Logger.Info("2 $ 2");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdsfld(out _)))
			{
				AltLibrary.Instance.Logger.Info("2 $ 3");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchStsfld<WorldGen>(nameof(WorldGen.totalSolid2))))
			{
				AltLibrary.Instance.Logger.Info("2 $ 4");
				return;
			}

			c.Index++;
			c.EmitDelegate(() =>
			{
				WorldGen.totalGood2 += WorldGen.tileCounts[TileID.HallowHardenedSand] + WorldGen.tileCounts[TileID.HallowSandstone];
				WorldGen.totalEvil2 += WorldGen.tileCounts[TileID.CorruptHardenedSand] + WorldGen.tileCounts[TileID.CorruptSandstone];
				WorldGen.totalBlood2 += WorldGen.tileCounts[TileID.CrimsonHardenedSand] + WorldGen.tileCounts[TileID.CrimsonSandstone];

				WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.HardenedSand] + WorldGen.tileCounts[TileID.Sandstone];
				WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.HallowHardenedSand] + WorldGen.tileCounts[TileID.HallowSandstone];
				WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CorruptHardenedSand] + WorldGen.tileCounts[TileID.CorruptSandstone];
				WorldGen.totalSolid2 += WorldGen.tileCounts[TileID.CrimsonHardenedSand] + WorldGen.tileCounts[TileID.CrimsonSandstone];

				int hallow = 0;
				int evil = 0;
				foreach (AltBiome biome in AltLibrary.Biomes)
				{
					if (biome.BiomeType == BiomeType.Hallow)
					{
						if (biome.BiomeIce.HasValue)
						{
							hallow += WorldGen.tileCounts[biome.BiomeIce.Value];
						}
						if (biome.BiomeGrass.HasValue)
						{
							hallow += WorldGen.tileCounts[biome.BiomeGrass.Value];
						}
						if (biome.BiomeStone.HasValue)
						{
							hallow += WorldGen.tileCounts[biome.BiomeStone.Value];
						}
						if (biome.BiomeSand.HasValue)
						{
							hallow += WorldGen.tileCounts[biome.BiomeSand.Value];
						}
						if (biome.BiomeHardenedSand.HasValue)
						{
							hallow += WorldGen.tileCounts[biome.BiomeHardenedSand.Value];
						}
						if (biome.BiomeSandstone.HasValue)
						{
							hallow += WorldGen.tileCounts[biome.BiomeSandstone.Value];
						}
					}
					if (biome.BiomeType == BiomeType.Evil)
					{
						if (biome.BiomeIce.HasValue)
						{
							evil += WorldGen.tileCounts[biome.BiomeIce.Value];
						}
						if (biome.BiomeGrass.HasValue)
						{
							evil += WorldGen.tileCounts[biome.BiomeGrass.Value];
						}
						if (biome.BiomeStone.HasValue)
						{
							evil += WorldGen.tileCounts[biome.BiomeStone.Value];
						}
						if (biome.BiomeSand.HasValue)
						{
							evil += WorldGen.tileCounts[biome.BiomeSand.Value];
						}
						if (biome.BiomeHardenedSand.HasValue)
						{
							evil += WorldGen.tileCounts[biome.BiomeHardenedSand.Value];
						}
						if (biome.BiomeSandstone.HasValue)
						{
							evil += WorldGen.tileCounts[biome.BiomeSandstone.Value];
						}
					}
				}

				WorldGen.totalGood2 += hallow;
				WorldGen.totalEvil2 += evil;
				WorldGen.totalSolid2 += hallow + evil;
			});
		}
	}
}
