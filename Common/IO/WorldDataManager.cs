using AltLibrary.Common.AltTypes;
using AltLibrary.Common.OrderGroups;
using AltLibrary.Content.Groups;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AltLibrary.Common.IO;

public sealed class WorldDataManager : ModSystem {
	private static Dictionary<BiomeGroup, string> biomeByGroup = new();
	private static Dictionary<OreGroup, string> oreByGroup = new();

	public const string BiomeDataKey = "BiomeData";
	public const string OreDataKey = "OreData";

	public override void SaveWorldData(TagCompound tag) {
		// Turning dictionary into dictionary is weird when reading out loud.
		tag[BiomeDataKey] = biomeByGroup.ToDictionary(x => x.Key, x => new ModTypeData<IAltBiome>(ModContent.Find<IAltBiome>(x.Value)));
		tag[OreDataKey] = oreByGroup.ToDictionary(x => x.Key, x => new ModTypeData<IAltOre>(ModContent.Find<IAltOre>(x.Value)));
	}

	public override void LoadWorldData(TagCompound tag) {
		if (tag.TryGet<Dictionary<BiomeGroup, ModTypeData<IAltBiome>>>(BiomeDataKey, out var biomeData)) {
			biomeByGroup = biomeData.ToDictionary(x => x.Key, x => x.Value.FullName);
		}
		if (tag.TryGet<Dictionary<OreGroup, ModTypeData<IAltOre>>>(OreDataKey, out var oreData)) {
			oreByGroup = oreData.ToDictionary(x => x.Key, x => x.Value.FullName);
		}
	}

	#region Biome Helper Methods
	public static IAltBiome GetEvil() => biomeByGroup[ModContent.GetInstance<EvilBiomeGroup>()].To<IAltBiome>();
	public static IAltBiome GetHallow() => biomeByGroup[ModContent.GetInstance<GoodBiomeGroup>()].To<IAltBiome>();
	public static IAltBiome GetTropics() => biomeByGroup[ModContent.GetInstance<TropicsBiomeGroup>()].To<IAltBiome>();
	public static IAltBiome GetUnderworld() => biomeByGroup[ModContent.GetInstance<UnderworldBiomeGroup>()].To<IAltBiome>();

	public static Biome GetEvil<Biome>() where Biome : AltBiome<EvilBiomeGroup> => GetBiome<EvilBiomeGroup, Biome>();
	public static Biome GetHallow<Biome>() where Biome : AltBiome<GoodBiomeGroup> => GetBiome<GoodBiomeGroup, Biome>();
	public static Biome GetTropics<Biome>() where Biome : AltBiome<TropicsBiomeGroup> => GetBiome<TropicsBiomeGroup, Biome>();
	public static Biome GetUnderworld<Biome>() where Biome : AltBiome<UnderworldBiomeGroup> => GetBiome<UnderworldBiomeGroup, Biome>();

	public static bool IsEvil<Biome>() where Biome : AltBiome<EvilBiomeGroup> => IsBiome<EvilBiomeGroup, Biome>();
	public static bool IsHallow<Biome>() where Biome : AltBiome<GoodBiomeGroup> => IsBiome<GoodBiomeGroup, Biome>();
	public static bool IsTropics<Biome>() where Biome : AltBiome<TropicsBiomeGroup> => IsBiome<TropicsBiomeGroup, Biome>();
	public static bool IsUnderworld<Biome>() where Biome : AltBiome<UnderworldBiomeGroup> => IsBiome<UnderworldBiomeGroup, Biome>();

	public static Biome GetBiome<Group, Biome>() where Group : BiomeGroup where Biome : AltBiome<Group>
		=> biomeByGroup[ModContent.GetInstance<Group>()].To<Biome>();
	public static bool IsBiome<Group, Biome>() where Group : BiomeGroup where Biome : AltBiome<Group>
		=> biomeByGroup[ModContent.GetInstance<Group>()] == ModContent.GetInstance<Biome>().FullName;
	#endregion

	#region Ore Helper Methods
	// I slightly feel yandere here for some reason.
	public static IAltOre GetCopper() => oreByGroup[ModContent.GetInstance<CopperOreGroup>()].To<IAltOre>();
	public static IAltOre GetIron() => oreByGroup[ModContent.GetInstance<IronOreGroup>()].To<IAltOre>();
	public static IAltOre GetSilver() => oreByGroup[ModContent.GetInstance<SilverOreGroup>()].To<IAltOre>();
	public static IAltOre GetGold() => oreByGroup[ModContent.GetInstance<GoldOreGroup>()].To<IAltOre>();
	public static IAltOre GetCobalt() => oreByGroup[ModContent.GetInstance<CobaltOreGroup>()].To<IAltOre>();
	public static IAltOre GetMythril() => oreByGroup[ModContent.GetInstance<MythrilOreGroup>()].To<IAltOre>();
	public static IAltOre GetAdamantite() => oreByGroup[ModContent.GetInstance<AdamantiteOreGroup>()].To<IAltOre>();

	public static Ore GetCopper<Ore>() where Ore : AltOre<CopperOreGroup> => GetOre<CopperOreGroup, Ore>();
	public static Ore GetIron<Ore>() where Ore : AltOre<IronOreGroup> => GetOre<IronOreGroup, Ore>();
	public static Ore GetSilver<Ore>() where Ore : AltOre<SilverOreGroup> => GetOre<SilverOreGroup, Ore>();
	public static Ore GetGold<Ore>() where Ore : AltOre<GoldOreGroup> => GetOre<GoldOreGroup, Ore>();
	public static Ore GetCobalt<Ore>() where Ore : AltOre<CobaltOreGroup> => GetOre<CobaltOreGroup, Ore>();
	public static Ore GetMythril<Ore>() where Ore : AltOre<MythrilOreGroup> => GetOre<MythrilOreGroup, Ore>();
	public static Ore GetAdamantite<Ore>() where Ore : AltOre<AdamantiteOreGroup> => GetOre<AdamantiteOreGroup, Ore>();

	public static bool IsCopper<Ore>() where Ore : AltOre<CopperOreGroup> => IsOre<CopperOreGroup, Ore>();
	public static bool IsIron<Ore>() where Ore : AltOre<IronOreGroup> => IsOre<IronOreGroup, Ore>();
	public static bool IsSilver<Ore>() where Ore : AltOre<SilverOreGroup> => IsOre<SilverOreGroup, Ore>();
	public static bool IsGold<Ore>() where Ore : AltOre<GoldOreGroup> => IsOre<GoldOreGroup, Ore>();
	public static bool IsCobalt<Ore>() where Ore : AltOre<CobaltOreGroup> => IsOre<CobaltOreGroup, Ore>();
	public static bool IsMythril<Ore>() where Ore : AltOre<MythrilOreGroup> => IsOre<MythrilOreGroup, Ore>();
	public static bool IsAdamantite<Ore>() where Ore : AltOre<AdamantiteOreGroup> => IsOre<AdamantiteOreGroup, Ore>();

	public static Ore GetOre<Group, Ore>() where Group : OreGroup where Ore : AltOre<Group>
		=> ModContent.TryFind<Ore>(oreByGroup[ModContent.GetInstance<Group>()], out var value) ? value : null;
	public static bool IsOre<Group, Ore>() where Group : OreGroup where Ore : AltOre<Group>
		=> oreByGroup[ModContent.GetInstance<Group>()] == ModContent.GetInstance<Ore>().FullName;
	#endregion
}
