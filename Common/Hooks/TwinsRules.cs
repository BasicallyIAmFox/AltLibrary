using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Condition;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Linq;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace AltLibrary.Common.Hooks
{
	internal class TwinsRules
	{
		public static void Init()
		{
			EditsHelper.IL<ItemDropDatabase>(nameof(ItemDropDatabase.RegisterBoss_Twins), ItemDropDatabase_RegisterBoss_Twins);
		}

		public static void Unload()
		{
		}

		private static void ItemDropDatabase_RegisterBoss_Twins(ILContext il)
		{
			ILCursor c = new(il);
			try
			{
				c.GotoNext(i => i.MatchLdcI4(ItemID.HallowedBar));
				c.GotoNext(MoveType.After, i => i.MatchPop());

				c.Emit(OpCodes.Ldloc, 1);
				c.EmitDelegate(LeadingConditionRule);
				c.Emit(OpCodes.Stloc, 1);
			}
			catch
			{
			}
		}

		private static LeadingConditionRule LeadingConditionRule(LeadingConditionRule leadCond)
		{
			leadCond.ChainedRules.RemoveAt(1);

			leadCond.OnSuccess(ItemDropRule.ByCondition(new HallowDropCondition(), ItemID.HallowedBar, 1, 15, 30));
			foreach (var biome in from AltBiome biome in AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow)
								  where biome.MechDropItemType != null && biome.MechDropItemType.HasValue
								  select biome)
			{
				leadCond.OnSuccess(ItemDropRule.ByCondition(new HallowAltDropCondition(biome), biome.MechDropItemType.Value, 1, 15, 30));
			}
			return leadCond;
		}
	}
}
