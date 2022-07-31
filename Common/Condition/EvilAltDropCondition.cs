using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AltLibrary.Common.Condition
{
	internal class EvilAltDropCondition : IItemDropRuleCondition
	{
		public AltBiome BiomeType;
		public EvilAltDropCondition(AltBiome biomeType)
		{
			BiomeType = biomeType;
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation && BiomeType.FullName != null && BiomeType.FullName != "")
			{
				if (WorldBiomeManager.WorldEvil != "") return WorldBiomeManager.WorldEvil == BiomeType.FullName;
			}
			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return WorldBiomeManager.WorldEvil == BiomeType.FullName;
		}

		public string GetConditionDescription()
		{
			return Language.GetTextValue("Mods.AltLibrary.DropRule.Base", BiomeType.DisplayName != null ? BiomeType.DisplayName.GetTranslation(Language.ActiveCulture) : BiomeType.Name);
		}
	}
}
