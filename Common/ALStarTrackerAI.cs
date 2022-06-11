using AltLibrary.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common
{
    internal partial class ALNPC
    {
        internal bool starTracker;
        internal float[] starTrackerAI;

        private bool StarTracker_PreAI(NPC npc)
        {
            if (!starTracker)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (/*p.type == ProjectileID.FallingStar && p.friendly && p.Colliding(p.getRect(), npc.getRect())*/ true)
                    {
                        starTrackerAI = new float[4];
                        starTracker = true;
                        npc.ai[0] = 0f;
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.ai[3] = 0f;
                        npc.localAI[1] = 0;
                        npc.localAI[2] = 0;
                        npc.localAI[0] = !Main.getGoodWorld ? 0.9f : 0.8f;
                        npc.velocity.X = 0f;
                        npc.life = npc.lifeMax;
                    }
                }
            }
            else
            {
                StarTracker_AI(npc);
            }
            return !starTracker;
        }

        private void StarTracker_AI(NPC npc)
        {
            npc.GivenName = "Star Tracker";

            const float fallingStarSpeed = 7f;
            const float distanceToDespawn = 3000f;
            bool expert = Main.expertMode;
            bool ftw = Main.getGoodWorld;

            Player player = Main.player[npc.target];
            npc.TargetClosest();
            if (Main.player[npc.target].DeadOrGhost || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > distanceToDespawn)
            {
                npc.TargetClosest();
                if (player.DeadOrGhost || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > distanceToDespawn)
                {
                    npc.EncourageDespawn(10);
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                }
            }
            npc.scale = npc.localAI[0];

            if (player.DeadOrGhost || Vector2.Distance(npc.Center, Main.player[npc.target].Center) > distanceToDespawn)
                npc.ai[0] = -1;
            else
                npc.timeLeft = 999;

            switch (npc.ai[0])
            {
                case 0f: // jump and shoot spikes
                    {
                        float speed = 1f;
                        float[] atWhatAdd = new float[9] { 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f };
                        for (int i = 0; i < atWhatAdd.Length; i++)
                            if (npc.life <= npc.lifeMax * atWhatAdd[i])
                                speed -= 0.04f;

                        if (++npc.ai[1] >= 120f * speed)
                        {
                            int rand = 0;
                            if (player.Distance(npc.Center) <= 400f)
                                rand = 1;
                            if (player.Distance(npc.Center) >= 800f)
                                rand = 2;
                            switch (rand)
                            {
                                case 2:
                                    npc.velocity.X = 16f * npc.direction;
                                    npc.velocity.Y += -7.5f;
                                    npc.ai[1] = 20f;
                                    StarTracker_ShootSpikes(npc);
                                    break;
                                case 1:
                                    npc.velocity.X = 12f * npc.direction;
                                    npc.velocity.Y += -12.5f;
                                    npc.ai[1] = -20f;
                                    StarTracker_ShootSpikes(npc);
                                    break;
                                default:
                                    npc.velocity.X = 14f * npc.direction;
                                    npc.velocity.Y += -10f;
                                    npc.ai[1] = 0f;
                                    StarTracker_ShootSpikes(npc);
                                    break;
                            }
                            npc.ai[2]++;
                        }
                        if (npc.ai[2] >= 3f + npc.localAI[3])
                        {
                            npc.ai[0]++;
                            if (starTrackerAI[2] == 2f)
                            {
                                npc.ai[0] = 2f;
                                starTrackerAI[2]++;
                            }
                            if (starTrackerAI[2] == 4f)
                            {
                                npc.ai[0] = 3f;
                                starTrackerAI[2] = 0f;
                            }
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.localAI[3] = 0f;
                            starTrackerAI[0] = Main.rand.Next(219, 224);
                            if ((expert || ftw) && Main.rand.NextBool(4))
                                starTrackerAI[0] = DustID.FireworkFountain_Pink;
                        }
                    }
                    break;
                case 1f: // teleports above player with some firework on space where tp
                    {
                        npc.ai[3] += 0.33f;
                        if (npc.ai[3] >= 20f)
                        {
                            Dust.NewDust(new(npc.ai[1], npc.ai[2]), 8, 8, (int)starTrackerAI[0]);
                        }
                        if (npc.ai[3] < 30f)
                        {
                            float xOffset = 0f;
                            float yOffset = npc.localAI[2] = Main.rand.Next(350, 450);
                            float localExtra = 0f;
                            float localExtra2 = 0f;
                            switch (starTrackerAI[0])
                            {
                                case DustID.FireworkFountain_Green:
                                    localExtra = 5f;
                                    xOffset = 50f;
                                    yOffset -= 75f;
                                    break;
                                case DustID.FireworkFountain_Blue:
                                    localExtra = -5f;
                                    xOffset = -50f;
                                    yOffset -= 75f;
                                    break;
                                case DustID.FireworkFountain_Yellow:
                                    localExtra2 = -2.5f;
                                    yOffset -= 150f;
                                    break;
                            }
                            npc.ai[1] = player.Center.X - xOffset;
                            npc.ai[2] = player.Center.Y - yOffset;
                            npc.localAI[1] = 0f;
                            if (yOffset <= 375)
                                npc.localAI[1] = 5f;
                            if (yOffset >= 425)
                                npc.localAI[1] = -5f;
                            if (localExtra != 0f)
                                npc.localAI[1] = localExtra;
                            if (localExtra2 != 0f)
                                starTrackerAI[1] = localExtra2;
                        }
                        if (npc.ai[3] >= 45f)
                        {
                            npc.Center = new(npc.ai[1], npc.ai[2] - npc.localAI[1]);
                            npc.velocity = new(npc.localAI[1], starTrackerAI[1] != 0f ? npc.localAI[1] : starTrackerAI[1]);
                            if (expert || ftw)
                            {
                                StarTracker_ShootSpikes(npc);
                            }
                            if (starTrackerAI[0] == DustID.FireworkFountain_Red)
                            {
                                for (int i = 0; i < Main.rand.Next(2, 6); i++)
                                {
                                    int index = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X - 8 * Main.rand.Next(-4, 4), (int)npc.Center.Y - 8 * Main.rand.Next(-4, 4), NPCID.SlimeSpiked);
                                    Main.npc[index].velocity.Y = 6f;
                                }
                            }
                            else if (starTrackerAI[0] == DustID.FireworkFountain_Pink)
                            {
                                int heal = npc.lifeMax / 100 * Main.rand.Next(5, 11);
                                npc.HealEffect(heal);
                                npc.life += heal;
                                if (npc.life > npc.lifeMax)
                                    npc.life = npc.lifeMax;
                            }
                            npc.ai[0] = 0f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.localAI[1] = 0f;
                            npc.localAI[2] = 0f;
                            npc.localAI[3] = Main.rand.Next(-2, 2);
                            starTrackerAI[2]++;
                            starTrackerAI[1]++;
                        }
                    }
                    break;
                case 2f: // shoot 2-4 rows of falling stars from above
                    {
                        int direction = 1;
                        if (Main.rand.NextBool(2))
                            direction = -1;
                        npc.ai[1]++;
                        if (npc.ai[1] == 1f)
                        {
                            npc.velocity.Y = -2f;
                        }
                        else
                        {
                            npc.velocity.Y = MathF.Sin(npc.velocity.Y);
                        }
                        if (npc.ai[1] >= 25f && npc.ai[1] <= 35f || npc.ai[1] >= 45f && npc.ai[1] <= 55f)
                        {
                            Dust.NewDust(new(npc.Center.X - 25f * Main.rand.NextFloat(-10f, 10f), npc.Center.Y - 25f * Main.rand.NextFloat(-10f, 10f)), 8, 8, DustID.FireworkFountain_Yellow);
                        }
                        if (npc.ai[1] == 75f)
                        {
                            int rows = 2;
                            if (expert)
                                rows++;
                            if (ftw)
                                rows++;
                            for (int j = 0; j < rows; j++)
                            {
                                for (int i = -10; i <= 10; i++)
                                {
                                    Vector2 vector2 = new(i * 300f, 500f + 150f * j + Main.screenPosition.Y / 4);
                                    if (j % 2 == 1)
                                        vector2.X -= (expert && ftw ? (j == 0 ? 25f : (j == 1 ? 75f : (j == 2 ? 150f : 225f))) : 150f) * direction;
                                    int index = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center - vector2, new(fallingStarSpeed * direction, fallingStarSpeed), ProjectileID.FallingStar, npc.damage / 5, 0f);
                                    Main.projectile[index].hostile = true;
                                    Main.projectile[index].friendly = false;
                                    Main.projectile[index].tileCollide = false;
                                    Main.projectile[index].timeLeft = 900;
                                }
                            }
                        }
                        if (npc.ai[1] >= 140f)
                        {
                            npc.ai[0] = 0f;
                            npc.ai[1] = 0f;
                            npc.velocity.Y = 10f;
                        }
                    }
                    break;
                case 3f: // shoots fireworks from the ground, with additional behavior from 0f
                    {
                        starTrackerAI[3]++;

                        if (starTrackerAI[3] <= 600f)
                        {
                            if (Main.rand.NextBool(!ftw ? 30 : 15))
                            {
                                Vector2 proSpeed = new(0f, -15f);
                                int dustID = Main.rand.Next(219, 223);
                                if (ftw && dustID != DustID.FireworkFountain_Red && Main.rand.NextFloat() <= 0.33f)
                                    dustID = DustID.FireworkFountain_Red;

                                if (dustID == DustID.FireworkFountain_Blue)
                                    proSpeed.X += 5f;
                                if (dustID == DustID.FireworkFountain_Green)
                                    proSpeed.X -= 5f;
                                if (dustID == DustID.FireworkFountain_Yellow)
                                    proSpeed.Y += 2.5f;

                                int index = Projectile.NewProjectile(npc.GetSource_FromAI(),
                                    new(Main.rand.NextFloat(player.Center.X - Main.ScreenSize.X / 2, player.Center.X + Main.ScreenSize.X / 2),
                                    player.Center.Y + Main.ScreenSize.Y), proSpeed, ModContent.ProjectileType<StarTrackerFirework>(), npc.damage / 7, 0f);
                                (Main.projectile[index].ModProjectile as StarTrackerFirework).dustToSummon = dustID;
                                Main.projectile[index].localAI[0] = Main.rand.Next(60, 120);
                            }
                        }
                        else
                        {
                            starTrackerAI[3] = 0f;
                            npc.ai[0] = 1f;
                            npc.ai[1] = 0f;
                        }

                        #region case 0f
                        float speed = 1f;
                        float[] atWhatAdd = new float[9] { 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f };
                        for (int i = 0; i < atWhatAdd.Length; i++)
                            if (npc.life <= npc.lifeMax * atWhatAdd[i])
                                speed -= 0.04f;
                        if (++npc.ai[1] >= 120f * speed)
                        {
                            int rand = 0;
                            if (player.Distance(npc.Center) <= 400f)
                                rand = 1;
                            if (player.Distance(npc.Center) >= 800f)
                                rand = 2;
                            switch (rand)
                            {
                                case 2:
                                    npc.velocity.X = 16f * npc.direction;
                                    npc.velocity.Y += -7.5f;
                                    npc.ai[1] = 20f;
                                    StarTracker_ShootSpikes(npc);
                                    break;
                                case 1:
                                    npc.velocity.X = 12f * npc.direction;
                                    npc.velocity.Y += -12.5f;
                                    npc.ai[1] = -20f;
                                    StarTracker_ShootSpikes(npc);
                                    break;
                                default:
                                    npc.velocity.X = 14f * npc.direction;
                                    npc.velocity.Y += -10f;
                                    npc.ai[1] = 0f;
                                    StarTracker_ShootSpikes(npc);
                                    break;
                            }
                        }
                        #endregion
                    }
                    break;
                case -1f: // despawn
                    npc.dontCountMe = true;
                    npc.dontTakeDamage = true;
                    npc.localAI[0] -= 0.05f;
                    if (npc.localAI[0] <= 0f)
                    {
                        npc.localAI[0] = 0.06f;
                        npc.EncourageDespawn(10);
                        npc.noGravity = false;
                        npc.noTileCollide = true;
                    }
                    break;
            }
            npc.velocity.X *= 0.98f;
        }

        private static void StarTracker_ShootSpikes(NPC npc)
        {
            int spawned = 0;
            StarTracker_SpikeStats(npc, out int cap, out float chance);
            for (float i = 0; i < MathF.PI * 2; i += 0.3f)
            {
                if (spawned < cap && Main.rand.NextFloat() <= chance)
                {
                    ValueTuple<float, float> angle = MathF.SinCos(i);
                    int index = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, new(angle.Item2 * 15f, angle.Item1 * 15f), ProjectileID.SpikedSlimeSpike, npc.damage / 10, 0f);
                    Main.projectile[index].tileCollide = false;
                    Main.projectile[index].timeLeft = 450;
                    spawned++;
                }
            }
        }

        private static void StarTracker_SpikeStats(NPC npc, out int cap, out float chance)
        {
            cap = 15;
            float[] atWhatAdd = new float[3] { 0.75f, 0.5f, 0.25f };
            chance = 0.75f;
            for (int i = 0; i < atWhatAdd.Length; i++)
                if (npc.life <= npc.lifeMax * atWhatAdd[i])
                    chance += 0.05f;
            if (Main.expertMode)
            {
                chance += 0.1f;
                cap += 3;
            }
            if (Main.getGoodWorld)
            {
                chance += 0.1f;
                cap += 3;
            }
            if (Main.GameMode == GameModeID.Creative)
            {
                cap -= 4;
                chance -= 0.25f;
            }
            if (Main.GameMode == GameModeID.Normal)
            {
                cap -= 2;
                chance -= 0.05f;
            }
        }

        private static bool StarTracker_PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = 0;
            if (npc.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            MiscShaderData shader = GameShaders.Misc["AltLibrary:StarTracker"];
            shader.ALUseImage0("Assets/StarTrackerExtra");
            shader.ALUseImage1("Assets/StarTrackerExtra");
            shader.Apply();

            Vector2 slimePos = new(npc.Center.X, npc.Center.Y + 8f + npc.gfxOffY);
            Main.EntitySpriteDraw(TextureAssets.Npc[npc.type].Value, slimePos - screenPos, npc.frame, npc.GetNPCColorTintedByBuffs(drawColor), npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects, 0);

            npc.alpha = 0;
            return true;
        }

        private static void StarTracker_PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            npc.alpha = 255;

            SpriteEffects spriteEffects = 0;
            if (npc.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

            Texture2D crown = TextureAssets.Extra[39].Value;
            Vector2 center = npc.Center;
            float yOffset = 0f;
            switch (npc.frame.Y / (TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type]))
            {
                case 0:
                    yOffset = 2f;
                    break;
                case 1:
                    yOffset = -6f;
                    break;
                case 2:
                    yOffset = 2f;
                    break;
                case 3:
                    yOffset = 10f;
                    break;
                case 4:
                    yOffset = 2f;
                    break;
                case 5:
                    yOffset = 0f;
                    break;
            }
            center.Y += npc.gfxOffY - (70f - yOffset) * npc.scale;
            Main.EntitySpriteDraw(crown, center - screenPos, null, npc.GetNPCColorTintedByBuffs(drawColor), 0f, crown.Size() / 2f, 1f, spriteEffects, 0);
        }
    }
}
