using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Content.NPCs
{
    internal class HallowBunny : ModNPC
    {
        public override string Texture => "AltLibrary/Assets/HallowBunny";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hallow Bunny");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
            Main.npcCatchable[Type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bunny);
            NPC.lifeMax *= 5;
            NPC.catchItem = (short)ModContent.ItemType<Items.HallowBunny>();
            NPC.friendly = true;
            AIType = NPCID.Bunny;
            AnimationType = NPCID.Bunny;
        }

        private bool firstTick = false;
        public override void AI()
        {
            if (!firstTick)
            {
                List<string> pairs = new();
                pairs.Add("Charlie");
                pairs.Add("Freddy");
                pairs.Add("Kai");
                pairs.Add("Max");
                pairs.Add("Mickey");
                pairs.Add("Mike");
                pairs.Add("Jack");
                pairs.Add("Cleo");
                pairs.Add("Juni");
                pairs.Add("Nessie");
                pairs.Add("Betty");
                pairs.Add("Mary");
                pairs.Add("Linda");
                pairs.Add("Tootsie");
                pairs.Add("Ensie");
                pairs.Add("Hannah");
                NPC.GivenName = pairs[Main.rand.Next(pairs.Count)];
                firstTick = true;
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.DeadOrGhost && player.Distance(NPC.position) <= 500f && Main.time % Main.rand.Next(30, 150) == Main.rand.Next(0, 20) && Collision.CanHit(player, NPC))
                {
                    List<int> pairs = new();
                    pairs.Add(0);
                    pairs.Add(1);
                    pairs.Add(2);
                    pairs.Add(3);
                    pairs.Add(4);
                    if (player.name == "FoxXD_") pairs.Add(5);

                    string reason = Language.GetTextValue($"Mods.AltLibrary.BunReason.{pairs[Main.rand.Next(pairs.Count)]}", player.name);
                    player.Hurt(PlayerDeathReason.ByCustomReason(reason), Main.tenthAnniversaryWorld ? 2 : 100 + Main.rand.Next(0, 100), -1);
                    player.immuneTime = 2;
                }
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            knockback = 0f;
            damage = 1;
            crit = false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = 0f;
            damage = 1;
            crit = false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return AltLibrary.HallowBunnyUnlocked ? 30f : 0f;
        }

        private class HallowGlobal : GlobalNPC
        {
            public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == ModContent.NPCType<HallowBunny>();

            public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
            {
                if (AltLibrary.HallowBunnyUnlocked)
                {
                    pool.Clear();
                    pool.Add(ModContent.NPCType<HallowBunny>(), 30f);
                }
            }

            public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
            {
                if (AltLibrary.HallowBunnyUnlocked)
                {
                    spawnRate /= 30;
                    maxSpawns *= 30;
                }
            }
        }
    }
}
