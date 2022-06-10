using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.Projectiles
{
    internal class StarTrackerFirework : ModProjectile
    {
        public int dustToSummon = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Firework");
        }

        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
        }

        public override void AI()
        {
            Dust.NewDust(Projectile.Center, 8, 8, dustToSummon);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active && !player.DeadOrGhost && player.Distance(Projectile.Center) <= 50f)
                {
                    player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, Projectile.whoAmI), Projectile.damage, 0, Crit: true);
                    player.AddBuff(BuffID.OnFire, 360);
                }
            }

            Tile tile = Main.tile[Projectile.position.ToTileCoordinates()];
            if (!tile.HasTile)
            {
                Dust.NewDust(Projectile.Center, 8, 8, DustID.Smoke);
                Projectile.ai[0]++;

                if (Projectile.ai[0] >= Projectile.localAI[0])
                {
                    for (int i = 0; i < Main.rand.Next(3, 6); i++)
                    {
                        float pos1 = 0f;
                        float pos2 = 0f;
                        while (pos1 <= 1f && pos1 >= 1f)
                        {
                            pos1 = Main.rand.NextFloat(-5f, 5f);
                        }
                        while (pos2 <= 1f && pos2 >= 1f)
                        {
                            pos2 = Main.rand.NextFloat(-5f, 5f);
                        }
                        Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, new(pos1, pos2), Main.rand.Next(61, 64), 2f);
                    }
                    if (dustToSummon == DustID.FireworkFountain_Red)
                    {
                        for (int i = 0; i < Main.rand.Next(1, 3); i++)
                        {
                            int npc = NPCID.BlueSlime;
                            if (Main.expertMode && !Main.getGoodWorld && Main.rand.NextBool(2))
                                npc = NPCID.SlimeSpiked;
                            if (Main.expertMode && Main.getGoodWorld && Main.rand.NextFloat() <= 0.75f)
                                npc = NPCID.SlimeSpiked;
                            NPC.NewNPC(Projectile.GetSource_FromAI(), (int)Projectile.Center.X, (int)Projectile.Center.Y, npc);
                        }
                    }
                    Projectile.Kill();
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 360);
            crit = true;
        }
    }
}
