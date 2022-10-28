using AltLibrary.Common.AltBiomes;
using AltLibrary.Content.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.Systems
{
	internal class RecipeGroups : ModSystem
	{
		internal static RecipeGroup EvilOres;
		internal static RecipeGroup EvilBars;
		internal static RecipeGroup HallowBars;
		internal static RecipeGroup HellBars;
		internal static RecipeGroup JungleBars;
		internal static RecipeGroup MushroomBars;
		internal static RecipeGroup EvilSwords;
		internal static RecipeGroup HallowSwords;
		internal static RecipeGroup HellSwords;
		internal static RecipeGroup JungleSwords;
		internal static RecipeGroup ComboSwords;
		internal static RecipeGroup TrueComboSwords;
		internal static RecipeGroup TrueHallowSwords;
		internal static RecipeGroup RottenChunks;
		internal static RecipeGroup PixieDusts;
		internal static RecipeGroup UnicornHorns;
		internal static RecipeGroup CrystalShards;
		internal static RecipeGroup CursedFlames;
		internal static RecipeGroup ShadowScales;
		internal static RecipeGroup JungleSpores;
		internal static RecipeGroup Deathweed;
		internal static RecipeGroup Fireblossom;
		internal static RecipeGroup Moonglow;
		internal static RecipeGroup Hellforges;
		internal static RecipeGroup CopperBars;
		internal static RecipeGroup IronOres;
		internal static RecipeGroup IronBars;
		internal static RecipeGroup SilverBars;
		internal static RecipeGroup GoldOres;
		internal static RecipeGroup GoldBars;
		internal static RecipeGroup CobaltBars;
		internal static RecipeGroup MythrilBars;
		internal static RecipeGroup AdamantiteBars;
		internal static RecipeGroup GoldCandles;
		internal static RecipeGroup CopperWatches;
		internal static RecipeGroup SilverWatches;
		internal static RecipeGroup GoldWatches;
		internal static RecipeGroup SoulsOfEvil;

		public override void Unload()
		{
			EvilOres = null;
			EvilBars = null;
			HallowBars = null;
			HellBars = null;
			JungleBars = null;
			MushroomBars = null;
			EvilSwords = null;
			HallowSwords = null;
			HellSwords = null;
			JungleSwords = null;
			ComboSwords = null;
			TrueComboSwords = null;
			TrueHallowSwords = null;
			RottenChunks = null;
			PixieDusts = null;
			UnicornHorns = null;
			CrystalShards = null;
			CursedFlames = null;
			ShadowScales = null;
			JungleSpores = null;
			Deathweed = null;
			Fireblossom = null;
			Moonglow = null;
			Hellforges = null;
			CopperBars = null;
			IronBars = null;
			SilverBars = null;
			GoldBars = null;
			CobaltBars = null;
			MythrilBars = null;
			AdamantiteBars = null;
			GoldCandles = null;
			CopperWatches = null;
			SilverWatches = null;
			GoldWatches = null;
			GoldOres = null;
			IronOres = null;
			SoulsOfEvil = null;
		}

		public override void AddRecipeGroups()
		{
			List<AltBiome> Hell = AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Hell);
			List<AltBiome> Light = AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Hallow);
			List<AltBiome> Evil = AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Evil);
			List<AltBiome> Tropic = AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Jungle);

			List<int> array = new() { ItemID.DemoniteOre, ItemID.CrimtaneOre };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.EvilOre != -1)
				{
					array.Add(x.MaterialContext.EvilOre);
				}
			});
			EvilOres = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilOres")}", array.ToArray());
			RecipeGroup.RegisterGroup("EvilOres", EvilOres);

			array = new List<int>() { ItemID.DemoniteBar, ItemID.CrimtaneBar };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.EvilBar != -1)
				{
					array.Add(x.MaterialContext.EvilBar);
				}
			});
			EvilBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("EvilBars", EvilBars);

			array = new List<int>() { ItemID.HallowedBar };
			Light.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.LightBar != -1)
				{
					array.Add(x.MaterialContext.LightBar);
				}
			});
			HallowBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("HallowBars", HallowBars);

			array = new List<int>() { ItemID.HellstoneBar };
			Hell.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.UnderworldBar != -1)
				{
					array.Add(x.MaterialContext.UnderworldBar);
				}
			});
			HellBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("HellBars", HellBars);

			array = new List<int>() { ItemID.ChlorophyteBar };
			Tropic.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.TropicalBar != -1)
				{
					array.Add(x.MaterialContext.TropicalBar);
				}
			});
			JungleBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("JungleBars", JungleBars);

			array = new List<int>() { ItemID.ShroomiteBar };
			Tropic.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.MushroomBar != -1)
				{
					array.Add(x.MaterialContext.MushroomBar);
				}
			});
			MushroomBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MushroomBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("MushroomBars", MushroomBars);

			array = new List<int>() { ItemID.LightsBane, ItemID.BloodButcherer };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.EvilSword != -1)
				{
					array.Add(x.MaterialContext.EvilSword);
				}
			});
			EvilSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("EvilSwords", EvilSwords);

			array = new List<int>() { ItemID.Excalibur };
			Light.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.LightSword != -1)
				{
					array.Add(x.MaterialContext.LightSword);
				}
			});
			HallowSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("HallowSwords", HallowSwords);

			array = new List<int>() { ItemID.FieryGreatsword };
			Hell.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.UnderworldSword != -1)
				{
					array.Add(x.MaterialContext.UnderworldSword);
				}
			});
			HellSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("HellSwords", HellSwords);

			array = new List<int>() { ItemID.BladeofGrass };
			Tropic.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.TropicalSword != -1)
				{
					array.Add(x.MaterialContext.TropicalSword);
				}
			});
			JungleSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("JungleSwords", JungleSwords);

			array = new List<int>() { ItemID.NightsEdge };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.CombinationSword != -1)
				{
					array.Add(x.MaterialContext.CombinationSword);
				}
			});
			ComboSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ComboSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("ComboSwords", ComboSwords);

			array = new List<int>() { ItemID.TrueNightsEdge };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.TrueCombinationSword != -1)
				{
					array.Add(x.MaterialContext.TrueCombinationSword);
				}
			});
			TrueComboSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueComboSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("TrueComboSwords", TrueComboSwords);

			array = new List<int>() { ItemID.TrueExcalibur };
			Light.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.TrueLightSword != -1)
				{
					array.Add(x.MaterialContext.TrueLightSword);
				}
			});
			TrueHallowSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueHallowSwords")}", array.ToArray());
			RecipeGroup.RegisterGroup("TrueHallowSwords", TrueHallowSwords);

			array = new List<int>() { ItemID.RottenChunk, ItemID.Vertebrae };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.VileInnard != -1)
				{
					array.Add(x.MaterialContext.VileInnard);
				}
			});
			RottenChunks = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.RottenChunks")}", array.ToArray());
			RecipeGroup.RegisterGroup("RottenChunks", RottenChunks);

			array = new List<int>() { ItemID.PixieDust };
			Light.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.LightResidue != -1)
				{
					array.Add(x.MaterialContext.LightResidue);
				}
			});
			PixieDusts = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.PixieDusts")}", array.ToArray());
			RecipeGroup.RegisterGroup("PixieDusts", PixieDusts);

			array = new List<int>() { ItemID.UnicornHorn };
			Light.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.LightInnard != -1)
				{
					array.Add(x.MaterialContext.LightInnard);
				}
			});
			UnicornHorns = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.UnicornHorns")}", array.ToArray());
			RecipeGroup.RegisterGroup("UnicornHorns", UnicornHorns);

			array = new List<int>() { ItemID.CrystalShard };
			Light.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.LightComponent != -1)
				{
					array.Add(x.MaterialContext.LightComponent);
				}
			});
			CrystalShards = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CrystalShards")}", array.ToArray());
			RecipeGroup.RegisterGroup("CrystalShards", CrystalShards);

			array = new List<int>() { ItemID.CursedFlame, ItemID.Ichor };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.VileComponent != -1)
				{
					array.Add(x.MaterialContext.VileComponent);
				}
			});
			CursedFlames = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CursedFlames")}", array.ToArray());
			RecipeGroup.RegisterGroup("CursedFlames", CursedFlames);

			array = new List<int>() { ItemID.ShadowScale, ItemID.TissueSample };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.EvilBossDrop != -1)
				{
					array.Add(x.MaterialContext.EvilBossDrop);
				}
			});
			ShadowScales = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ShadowScales")}", array.ToArray());
			RecipeGroup.RegisterGroup("ShadowScales", ShadowScales);

			array = new List<int>() { ItemID.JungleSpores };
			Tropic.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.TropicalComponent != -1)
				{
					array.Add(x.MaterialContext.TropicalComponent);
				}
			});
			JungleSpores = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSpores")}", array.ToArray());
			RecipeGroup.RegisterGroup("JungleSpores", JungleSpores);

			array = new List<int>() { ItemID.Deathweed };
			Evil.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.EvilHerb != -1)
				{
					array.Add(x.MaterialContext.EvilHerb);
				}
			});
			Deathweed = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Deathweed")}", array.ToArray());
			RecipeGroup.RegisterGroup("Deathweed", Deathweed);

			array = new List<int>() { ItemID.Fireblossom };
			Hell.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.UnderworldHerb != -1)
				{
					array.Add(x.MaterialContext.UnderworldHerb);
				}
			});
			Fireblossom = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Fireblossom")}", array.ToArray());
			RecipeGroup.RegisterGroup("Fireblossom", Fireblossom);

			array = new List<int>() { ItemID.Moonglow };
			Tropic.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.TropicalHerb != -1)
				{
					array.Add(x.MaterialContext.TropicalHerb);
				}
			});
			Moonglow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Moonglow")}", array.ToArray());
			RecipeGroup.RegisterGroup("Moonglow", Moonglow);

			array = new List<int>() { ItemID.Hellforge };
			Hell.ForEach(x =>
			{
				if (x.MaterialContext != null && x.MaterialContext.UnderworldForge != -1)
				{
					array.Add(x.MaterialContext.UnderworldForge);
				}
			});
			Hellforges = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Hellforges")}", array.ToArray());
			RecipeGroup.RegisterGroup("Hellforges", Hellforges);

			array = new List<int>() { ItemID.CopperBar, ItemID.TinBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Copper).ForEach(x => array.Add(x.bar));
			CopperBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CopperBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("CopperBars", CopperBars);

			array = new List<int>() { ItemID.IronOre, ItemID.LeadOre };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Iron).ForEach(x => array.Add(x.ore));
			IronOres = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.IronOres")}", array.ToArray());
			RecipeGroup.RegisterGroup("IronOres", IronOres);

			array = new List<int>() { ItemID.IronBar, ItemID.LeadBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Iron).ForEach(x => array.Add(x.bar));
			IronBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.IronBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("IronBars", IronBars);

			array = new List<int>() { ItemID.SilverBar, ItemID.TungstenBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Silver).ForEach(x => array.Add(x.bar));
			SilverBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.SilverBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("SilverBars", SilverBars);

			array = new List<int>() { ItemID.GoldOre, ItemID.PlatinumOre };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Gold).ForEach(x => array.Add(x.ore));
			GoldOres = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldOres")}", array.ToArray());
			RecipeGroup.RegisterGroup("GoldOres", GoldOres);

			array = new List<int>() { ItemID.GoldBar, ItemID.PlatinumBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Gold).ForEach(x => array.Add(x.bar));
			GoldBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("GoldBars", GoldBars);

			array = new List<int>() { ItemID.CobaltBar, ItemID.PalladiumBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Cobalt).ForEach(x => array.Add(x.bar));
			CobaltBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CobaltBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("CobaltBars", CobaltBars);

			array = new List<int>() { ItemID.MythrilBar, ItemID.OrichalcumBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Mythril).ForEach(x => array.Add(x.bar));
			MythrilBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MythrilBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("MythrilBars", MythrilBars);

			array = new List<int>() { ItemID.AdamantiteBar, ItemID.TitaniumBar };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Adamantite).ForEach(x => array.Add(x.bar));
			AdamantiteBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.AdamantiteBars")}", array.ToArray());
			RecipeGroup.RegisterGroup("AdamantiteBars", AdamantiteBars);

			array = new List<int>() { ItemID.Candle, ItemID.PlatinumCandle };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Gold && x.Candle.HasValue).ForEach(x => array.Add(x.Candle.Value));
			GoldCandles = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldCandles")}", array.ToArray());
			RecipeGroup.RegisterGroup("GoldCandles", GoldCandles);

			array = new List<int>() { ItemID.CopperWatch, ItemID.TinWatch };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Copper && x.Watch.HasValue).ForEach(x => array.Add(x.Watch.Value));
			CopperWatches = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CopperWatches")}", array.ToArray());
			RecipeGroup.RegisterGroup("CopperWatches", CopperWatches);

			array = new List<int>() { ItemID.SilverWatch, ItemID.TungstenWatch };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Silver && x.Watch.HasValue).ForEach(x => array.Add(x.Watch.Value));
			SilverWatches = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.SilverWatches")}", array.ToArray());
			RecipeGroup.RegisterGroup("SilverWatches", SilverWatches);

			array = new List<int>() { ItemID.GoldWatch, ItemID.PlatinumWatch };
			AltLibrary.Ores.FindAll(x => x.OreType == OreType.Gold && x.Watch.HasValue).ForEach(x => array.Add(x.Watch.Value));
			GoldWatches = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldWatches")}", array.ToArray());
			RecipeGroup.RegisterGroup("GoldWatches", GoldWatches);
		}
	}
}
