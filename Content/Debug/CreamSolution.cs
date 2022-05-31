using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.Debug
{
    internal class CreamSolution : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cream Solution");
            Tooltip.SetDefault("Used by the Clentaminator"
                + "\nSpreads the Confection");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<CreamSolutionPro>() - ProjectileID.PureSpray;
            Item.ammo = AmmoID.Solution;
            Item.width = 10;
            Item.height = 12;
            Item.value = Item.buyPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 999;
            Item.consumable = true;
        }
    }

    internal class CreamSolutionPro : ModProjectile
    {
        public ref float Progress => ref Projectile.ai[0];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Confection Spray");
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            int dustType = DustID.Cobalt;

            if (Projectile.owner == Main.myPlayer)
            {
                ALConvert.SimulateSolution<EpicSwag>(Projectile);
            }

            if (Projectile.timeLeft > 133)
            {
                Projectile.timeLeft = 133;
            }

            if (Progress > 7f)
            {
                float dustScale = 1f;

                if (Progress == 8f)
                {
                    dustScale = 0.2f;
                }
                else if (Progress == 9f)
                {
                    dustScale = 0.4f;
                }
                else if (Progress == 10f)
                {
                    dustScale = 0.6f;
                }
                else if (Progress == 11f)
                {
                    dustScale = 0.8f;
                }

                Progress += 1f;

                var dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

                dust.noGravity = true;
                dust.scale *= 1.75f;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
                dust.scale *= dustScale;
            }
            else
            {
                Progress += 1f;
            }

            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }
}
