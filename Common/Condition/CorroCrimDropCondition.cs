using AltLibrary.Common.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace AltLibrary.Common.Condition
{
	internal class CorroCrimDropCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			return !info.IsInSimulation && WorldBiomeManager.WorldEvil == "";
		}

		public bool CanShowItemDropInUI()
		{
			return WorldBiomeManager.WorldEvil == "";
		}

		public string GetConditionDescription()
		{
			string biome;
			if (WorldGen.crimson) biome = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.CrimsonBiome");
			else biome = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.CorruptBiome");
			return Language.GetTextValue("Mods.AltLibrary.DropRule.Base", biome);
		}
	}
}
