#if TML_2022_06
using AltLibrary.Common.Systems;
#else 
using Terraria.GameContent.ItemDropRules;
using AltLibrary.Common.Condition;
using System.Collections.Generic;
#endif
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AltLibrary.Common.AltBiomes;

namespace AltLibrary.Common
{
    internal class BossBags : GlobalItem
    {
#if TML_2022_06
        public override bool PreOpenVanillaBag(string context, Player player, int arg)
        {
            if (WorldBiomeManager.WorldHallow != "")
            {
                if (arg == ItemID.TwinsBossBag || arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.DestroyerBossBag)
                {
                    NPCLoader.blockLoot.Add(ItemID.HallowedBar);
                }
                if (arg == ItemID.WallOfFleshBossBag && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).HammerType != ItemID.Pwnhammer)
                {
                    NPCLoader.blockLoot.Add(ItemID.Pwnhammer);
                }
            }
            if (arg == ItemID.EyeOfCthulhuBossBag)
            {
                if (WorldBiomeManager.WorldEvil != "")
                {
                    NPCLoader.blockLoot.Add(ItemID.DemoniteOre);
                    NPCLoader.blockLoot.Add(ItemID.CrimtaneOre);
                    NPCLoader.blockLoot.Add(ItemID.CorruptSeeds);
                    NPCLoader.blockLoot.Add(ItemID.CrimsonSeeds);
                    NPCLoader.blockLoot.Add(ItemID.UnholyArrow);
                }
            }

            return base.PreOpenVanillaBag(context, player, arg);
        }

        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            var source = player.GetSource_OpenItem(arg);
            if (WorldBiomeManager.WorldHallow != "")
            {
                var biome = ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow);
                if (arg == ItemID.TwinsBossBag || arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.DestroyerBossBag)
                {
                    var amount = Main.rand.Next(15, 31);
                    player.QuickSpawnItem(source, (int)biome.MechDropItemType, amount);
                }
                if (arg == ItemID.WallOfFleshBossBag)
                {
                    player.QuickSpawnItem(source, biome.HammerType);
                }
            }
            if (arg == ItemID.EyeOfCthulhuBossBag && WorldBiomeManager.WorldEvil != "")
            {
                var biome = ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil);
                if (biome.BiomeOreItem != null)
                {
                    var amount = Main.rand.Next(30, 90);
                    player.QuickSpawnItem(source, (int)biome.BiomeOreItem, amount);
                }
                if (biome.SeedType != null)
                {
                    var amount = Main.rand.Next(1, 4);
                    player.QuickSpawnItem(source, (int)biome.SeedType, amount);
                }
                if (biome.ArrowType != null)
                {
                    var amount = Main.rand.Next(20, 51);
                    player.QuickSpawnItem(source, (int)biome.ArrowType, amount);
                }
            }
        }
#else
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            List<AltBiome> HallowList = new();
            List<AltBiome> HellList = new();
            List<AltBiome> JungleList = new();
            List<AltBiome> EvilList = new();
            foreach (AltBiome biome in AltLibrary.Biomes)
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

            var entries = itemLoot.Get(false);
            switch (item.type)
            {
                case ItemID.EyeOfCthulhuBossBag:
                    {
                        foreach (var entry in entries)
                        {
                            if (entry is ItemDropWithConditionRule conditionRule)
                            {
                                if (conditionRule.itemId == ItemID.DemoniteOre || conditionRule.itemId == ItemID.CrimtaneOre ||
                                    conditionRule.itemId == ItemID.CorruptSeeds || conditionRule.itemId == ItemID.CrimsonSeeds
                                    || conditionRule.itemId == ItemID.UnholyArrow)
                                {
                                    itemLoot.Remove(entry);
                                }
                            }
                        }

                        var corroCrimCondition = new LeadingConditionRule(new CorroCrimDropCondition());
                        var corroCondition = new LeadingConditionRule(new Conditions.IsCorruptionAndNotExpert());
                        var crimCondition = new LeadingConditionRule(new Conditions.IsCrimsonAndNotExpert());

                        corroCrimCondition.OnSuccess(corroCondition);
                        corroCrimCondition.OnSuccess(crimCondition);
                        corroCondition.OnSuccess(ItemDropRule.Common(ItemID.DemoniteOre, 1, 30, 90));
                        corroCondition.OnSuccess(ItemDropRule.Common(ItemID.CorruptSeeds, 1, 1, 3));
                        corroCondition.OnSuccess(ItemDropRule.Common(ItemID.UnholyArrow, 1, 20, 50));

                        crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimtaneOre, 1, 30, 90));
                        crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimsonSeeds, 1, 1, 3));

                        itemLoot.Add(corroCrimCondition);

                        var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());

                        foreach (AltBiome biome in EvilList)
                        {
                            var biomeDropRule = new LeadingConditionRule(new EvilAltDropCondition(biome));
                            if (biome.BiomeOreItem != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.BiomeOreItem, 1, 30, 90));
                            if (biome.SeedType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.SeedType, 1, 1, 3));
                            if (biome.ArrowType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.ArrowType, 20, 50));
                            expertCondition.OnSuccess(biomeDropRule);
                        }
                        itemLoot.Add(expertCondition);

                        break;
                    }
                case ItemID.WallOfFleshBossBag:
                    {
                        foreach (var entry in entries)
                        {
                            if (entry is ItemDropWithConditionRule rule && rule.itemId == ItemID.Pwnhammer)
                            {
                                itemLoot.Remove(rule);
                            }
                        }
                        var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());
                        var hallowBarCondition = new LeadingConditionRule(new HallowDropCondition());
                        expertCondition.OnSuccess(hallowBarCondition);
                        hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.Pwnhammer));
                        foreach (AltBiome biome in HallowList)
                        {
                            var biomeDropRule = new LeadingConditionRule(new HallowAltDropCondition(biome));
                            biomeDropRule.OnSuccess(ItemDropRule.Common(biome.HammerType));
                            expertCondition.OnSuccess(biomeDropRule);
                        }
                        itemLoot.Add(expertCondition);
                        break;
                    }
                case ItemID.TwinsBossBag:
                case ItemID.DestroyerBossBag:
                case ItemID.SkeletronPrimeBossBag:
                    {
                        foreach (var entry in entries)
                        {
                            if (entry is ItemDropWithConditionRule conditionRule && conditionRule.itemId == ItemID.HallowedBar)
                            {
                                itemLoot.Remove(entry);
                                break;
                            }
                        }
                        var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());
                        var hallowBarCondition = new LeadingConditionRule(new HallowDropCondition());
                        expertCondition.OnSuccess(hallowBarCondition);
                        hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30));

                        foreach (AltBiome biome in HallowList)
                        {
                            var biomeDropRule = new LeadingConditionRule(new HallowAltDropCondition(biome));
                            if (biome.MechDropItemType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.MechDropItemType, 1, 15, 30));
                            expertCondition.OnSuccess(biomeDropRule);
                        }
                        itemLoot.Add(expertCondition);
                        break;
                    }
            }
        }
#endif
    }
}
