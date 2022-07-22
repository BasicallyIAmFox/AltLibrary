#if TML_2022_06
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Terraria;
using Terraria.ID;
#endif
using Terraria.ModLoader;

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
#endif
    }
}
