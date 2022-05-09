using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
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

                npcLoot.Add(corroCrimCondition);

                var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());

                foreach (AltBiome biome in EvilList)
                {
                    var biomeDropRule = new LeadingConditionRule(new EvilAltDropCondition(biome));
                    if (biome.BiomeOreItem != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.BiomeOreItem, 1, 30, 90));
                    if (biome.SeedType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.SeedType, 1, 1, 3));
                    if (biome.ArrowType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.ArrowType, 20, 50));
                    expertCondition.OnSuccess(biomeDropRule);
                }
                npcLoot.Add(expertCondition);
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                foreach (var entry in entries)
                {
                    if (entry is ItemDropWithConditionRule rule && rule.itemId == ItemID.Pwnhammer)
                    {
                        npcLoot.Remove(rule);
                    }
                }
                var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());
                var hallowBarCondition = new LeadingConditionRule(new HallowedBarDropCondition());
                expertCondition.OnSuccess(hallowBarCondition);
                hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.Pwnhammer));
                foreach (AltBiome biome in HallowList)
                {
                    var biomeDropRule = new LeadingConditionRule(new HallowedBarAltDropCondition(biome));
                    biomeDropRule.OnSuccess(ItemDropRule.Common(biome.HammerType));
                    expertCondition.OnSuccess(biomeDropRule);
                }
                npcLoot.Add(expertCondition);
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
                var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());
                var hallowBarCondition = new LeadingConditionRule(new HallowedBarDropCondition());
                expertCondition.OnSuccess(hallowBarCondition);
                hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30));

                foreach (AltBiome biome in HallowList)
                {
                    var biomeDropRule = new LeadingConditionRule(new HallowedBarAltDropCondition(biome));
                    if (biome.MechDropItemType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.MechDropItemType, 1, 15, 30));
                    expertCondition.OnSuccess(biomeDropRule);
                }
                npcLoot.Add(expertCondition);
            }
        }
    }
    internal class HallowedBarDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation && (WorldBiomeManager.worldHallow == "" || WorldBiomeManager.worldHallow == null))
            {
                return true;
            }
            return false;
        }
        public bool CanShowItemDropInUI()
        {
            return WorldBiomeManager.worldHallow == "";
        }
        public string GetConditionDescription()
        {
            return $"{Language.GetTextValue("Mods.AltLibrary.DropRule.Base")} {Language.GetTextValue("Mods.AltLibrary.Biomes.HallowName")}";
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
            if (WorldGen.crimson) biome = Language.GetTextValue("Mods.AltLibrary.Biomes.CrimsonName");
            else biome = Language.GetTextValue("Mods.AltLibrary.Biomes.CorruptName");
            return Language.GetTextValue("Mods.AltLibrary.DropRule.Base", biome);
        }
    }

    internal class HallowedBarAltDropCondition : IItemDropRuleCondition
    {
        public AltBiome BiomeType;
        public HallowedBarAltDropCondition(AltBiome biomeType)
        {
            BiomeType = biomeType;
        }

        public bool CanDrop(DropAttemptInfo info) // this may have the same issue as EvilAltDropCondition
        {
            if (!info.IsInSimulation && (BiomeType.FullName != null && BiomeType.FullName != ""))
            {
                if (WorldBiomeManager.worldHallow == BiomeType.FullName) return WorldBiomeManager.worldHallow == BiomeType.FullName;
            }
            return false;
        }

        public bool CanShowItemDropInUI()
        {
            return WorldBiomeManager.worldHallow == BiomeType.FullName;
        }

        public string GetConditionDescription()
        {
            return Language.GetTextValue("Mods.AltLibrary.DropRule.Base", BiomeType.DisplayName != null ? BiomeType.DisplayName : BiomeType.Name);
        }
    }

    internal class EvilAltDropCondition : IItemDropRuleCondition
    {
        public AltBiome BiomeType;
        public EvilAltDropCondition(AltBiome biomeType)
        {
            BiomeType = biomeType;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation && (BiomeType.FullName != null && BiomeType.FullName != ""))
            {
                if (WorldBiomeManager.worldEvil != "") return WorldBiomeManager.worldEvil == BiomeType.FullName;
            }
            return false;
        }

        public bool CanShowItemDropInUI()
        {
            return WorldBiomeManager.worldEvil == BiomeType.FullName;
        }

        public string GetConditionDescription()
        {
            return Language.GetTextValue("Mods.AltLibrary.DropRule.Base", BiomeType.DisplayName != null ? BiomeType.DisplayName : BiomeType.Name);
        }
    }

    internal class BossBags : GlobalItem
    {
        public override bool PreOpenVanillaBag(string context, Player player, int arg)
        {
            if (WorldBiomeManager.worldHallow != "")
            {
                if (arg == ItemID.TwinsBossBag || arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.DestroyerBossBag)
                {
                    NPCLoader.blockLoot.Add(ItemID.HallowedBar);
                }
                if (arg == ItemID.WallOfFleshBossBag && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).HammerType != ItemID.Pwnhammer)
                {
                    NPCLoader.blockLoot.Add(ItemID.Pwnhammer);
                }
            }  
            if (arg == ItemID.EyeOfCthulhuBossBag)
            {
                if (WorldBiomeManager.worldEvil != "")
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
            if (WorldBiomeManager.worldHallow != "")
            {
                var biome = ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow);
                if (arg == ItemID.TwinsBossBag || arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.DestroyerBossBag)
                {
                    var amount = Main.rand.Next(15, 31);
                    player.QuickSpawnItem(source, (int)biome.MechDropItemType, amount);
                }
                if (arg == ItemID.WallOfFleshBossBag)
                {
                    player.QuickSpawnItem(source, (int)biome.HammerType);
                }
            }
            if (arg == ItemID.EyeOfCthulhuBossBag && WorldBiomeManager.worldEvil != "")
            {
                var biome = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil);
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
    }
}
