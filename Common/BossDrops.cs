using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common
{
    internal class BossDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            List<AltBiome> HallowList = new();
            List<AltBiome> HellList = new();
            List<AltBiome> JungleList = new();
            List<AltBiome> EvilList = new();
            foreach (AltBiome biome in AltLibrary.biomes)
            {
                switch (biome.BiomeType)
                {
                    case BiomeType.Evil:
                        EvilList.Add(biome);
                        break;
                    case BiomeType.Hallow:
                        HallowList.Add(biome);
                        break;
                    case BiomeType.Hell:
                        HellList.Add(biome);
                        break;
                    case BiomeType.Jungle:
                        JungleList.Add(biome);
                        break;
                }
            }
            var hallowBarCondition = new LeadingConditionRule(new HallowedBarDropCondition());
            var hallowBarRule = hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30));
            void RegisterAltHallowDrops(NPCLoot loot)
            {
                foreach (AltBiome biome in HallowList)
                {
                    var altCondition = new LeadingConditionRule(new HallowedBarAltDropCondition(biome.FullName));
                    var altItemType = biome.MechDropItemType == null ? ItemID.HallowedBar : (int)biome.MechDropItemType;
                    var altDropRule = altCondition.OnSuccess(ItemDropRule.Common(altItemType, 1, 15, 30));
                    loot.Add(altDropRule);
                }
            }
            void RegisterAltEvilDrops(LeadingConditionRule condition, int itemType, int chanceDenominator, int dropMin, int dropMax, NPCLoot loot)
            {
                var altDropRule = condition.OnSuccess(ItemDropRule.Common(itemType, chanceDenominator, dropMin, dropMax));
                loot.Add(altDropRule);
            }

            var entries = npcLoot.Get(false);
            if (npc.type == NPCID.EyeofCthulhu)
            {
                foreach (var entry in entries)
                {
                    if (entry is ItemDropWithConditionRule conditionRule)
                    {
                        if (conditionRule.itemId == ItemID.DemoniteOre || conditionRule.itemId == ItemID.CrimtaneOre ||
                            conditionRule.itemId == ItemID.CorruptSeeds || conditionRule.itemId == ItemID.CrimsonSeeds
                            || conditionRule.itemId == ItemID.UnholyArrow)
                        {
                            npcLoot.Remove(entry);
                        }

                    }
                }
                var corroCondition = new LeadingConditionRule(new CorroCrimDropCondition());
                var crimCondition = new LeadingConditionRule(new CorroCrimDropCondition());
                var corroCondition2 = new LeadingConditionRule(new Conditions.IsCorruptionAndNotExpert());
                var crimCondition2 = new LeadingConditionRule(new Conditions.IsCrimsonAndNotExpert());

                corroCondition2.OnSuccess(corroCondition);
                corroCondition.OnSuccess(ItemDropRule.Common(ItemID.DemoniteOre, 1, 30, 90));
                corroCondition.OnSuccess(ItemDropRule.Common(ItemID.CorruptSeeds, 1, 1, 3));
                corroCondition.OnSuccess(ItemDropRule.Common(ItemID.UnholyArrow, 1, 20, 50));

                crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimtaneOre, 1, 30, 90));
                crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimsonSeeds, 1, 1, 3));

                npcLoot.Add(crimCondition2);
                npcLoot.Add(corroCondition2);

                foreach (var biome in EvilList)
                {
                    var altCondition = new LeadingConditionRule(new EvilAltDropCondition(biome.FullName));
                    var altItemType = biome.BiomeOreItem == null ? ItemID.DemoniteOre : (int)biome.BiomeOreItem;
                    RegisterAltEvilDrops(altCondition, altItemType, 1, 30, 90, npcLoot);
                    altItemType = biome.SeedType == null ? ItemID.CorruptSeeds : (int)biome.SeedType;
                    RegisterAltEvilDrops(altCondition, altItemType, 1, 1, 3, npcLoot);
                    altItemType = biome.ArrowType == null ? 0 : (int)biome.ArrowType;
                    if (altItemType != 0) RegisterAltEvilDrops(altCondition, altItemType, 1, 20, 50, npcLoot);
                }
            }
            if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
            {

                foreach (var entry in entries)
                {
                    if (entry is LeadingConditionRule leadingRule)
                    {
                        foreach (var chainedRule in leadingRule.ChainedRules)
                        {
                            if (chainedRule is LeadingConditionRule leadingRule2)
                            {
                                if (leadingRule2.condition is Conditions.NotExpert)
                                {
                                    foreach (var chainedRule2 in leadingRule2.ChainedRules)
                                    {
                                        if (chainedRule2 is CommonDrop normalDropRule && normalDropRule.itemId == ItemID.HallowedBar)
                                        {
                                            leadingRule2.ChainedRules.Remove(chainedRule2);
                                            leadingRule2.OnSuccess(hallowBarRule);

                                            foreach (AltBiome biome in HallowList)
                                            {
                                                var altHallowBarCondition = new LeadingConditionRule(new HallowedBarAltDropCondition(biome.FullName));
                                                var altHallowBarType = biome.MechDropItemType == null ? ItemID.HallowedBar : (int)biome.MechDropItemType;
                                                var altHallowBarRule = altHallowBarCondition.OnSuccess(ItemDropRule.Common(altHallowBarType, 1, 15, 30));
                                                leadingRule2.OnSuccess(altHallowBarRule);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime)
            {
                foreach (var entry in entries)
                {
                    if (entry is ItemDropWithConditionRule conditionRule && conditionRule.itemId == ItemID.HallowedBar)
                    {
                        npcLoot.Remove(entry);
                        break;
                    }
                }
                npcLoot.Add(hallowBarRule);
                RegisterAltHallowDrops(npcLoot);
            }
        }
    }
    internal class HallowedBarDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
            {
                return (WorldBiomeManager.worldHallow == "" || WorldBiomeManager.worldHallow == null);
            }
            return false;
        }
        public bool CanShowItemDropInUI()
        {
            return WorldBiomeManager.worldHallow == "";
        }
        public string GetConditionDescription()
        {
            return "Drops in Hallowed worlds."; // TODO: translate
        }
    }

    internal class CorroCrimDropCondition : IItemDropRuleCondition
    {
        //public bool Crimson;
        //public CorroCrimDropCondition(bool isCrimson)
        //{
        //    Crimson = isCrimson;
        //}
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation && (WorldBiomeManager.worldEvil == "" || WorldBiomeManager.worldEvil == null))
            {
                //return WorldGen.crimson == Crimson;
                return true;
            }
            return false;
        }
        public bool CanShowItemDropInUI()
        {
            if (WorldBiomeManager.worldEvil == "" || WorldBiomeManager.worldEvil == null)
            {
                //return WorldGen.crimson == Crimson;
                return true;
            }
            return false;
        }
        public string GetConditionDescription()
        {
            string biome;
            if (WorldGen.crimson) biome = "Crimson";
            else biome = "Corruption";
            return "Drops in worlds with " + biome;
        }
    }

    internal class HallowedBarAltDropCondition : IItemDropRuleCondition
    {
        public string BiomeType;
        public HallowedBarAltDropCondition(string biomeType)
        {
            BiomeType = biomeType;
        }

        public bool CanDrop(DropAttemptInfo info) // this may have the same issue as EvilAltDropCondition
        {
            if (!info.IsInSimulation && !(BiomeType != null && BiomeType != ""))
            {
                if (WorldBiomeManager.worldHallow != null && WorldBiomeManager.worldHallow != "") return WorldBiomeManager.worldHallow == BiomeType;
            }
            return false;
        }

        public bool CanShowItemDropInUI()
        {
            return WorldBiomeManager.worldHallow == BiomeType;
        }

        public string GetConditionDescription()
        {
            return "Drops in worlds with a specific biome type"; // TODO: translate this and make it use the biomes display name
        }
    }

    internal class EvilAltDropCondition : IItemDropRuleCondition
    {
        public string BiomeType;
        public EvilAltDropCondition(string biomeType)
        {
            BiomeType = biomeType;
        }

        public bool CanDrop(DropAttemptInfo info) // why? why is this always returning true? 
        {
            if (!info.IsInSimulation && (BiomeType != null && BiomeType != ""))
            {
                if (WorldBiomeManager.worldEvil != null && WorldBiomeManager.worldEvil != "") return WorldBiomeManager.worldEvil == BiomeType;
            }
            return false;
        }

        public bool CanShowItemDropInUI()
        {
            return WorldBiomeManager.worldEvil == BiomeType;
        }

        public string GetConditionDescription()
        {
            return "Drops in worlds with a specific biome type"; // TODO: translate this and make it use the biomes display name
        }
    }
}
