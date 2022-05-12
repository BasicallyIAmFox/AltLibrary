using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltLiquidStyles
{
    [Obsolete("This class is being heavily in DEVELOPMENT and EXPERIMENTAL! Major changes can happen in any update. Risky to use.")]
    public abstract class AltLiquidStyle : ModTexturedType
    {
        /// <summary>
        /// Should only be used only with World-like fields. (will never change in worlds)
        /// </summary>
        public virtual Func<bool> IsActive => () => false;
        public override string Texture => base.Texture;
        public virtual string LiquidTexture => base.Texture + "_Liquid";
        public virtual string SlopeTexture => base.Texture + "_Slope";
        public virtual string WaterfallTexture => base.Texture + "_Waterfall";
        internal Asset<Texture2D>[] Textures = null;
        internal Asset<Texture2D>[] GetTextures() => Textures;

        public Func<Player, PlayerDeathReason> LavaDeathReason;
        public int? LavaDebuff = null;
        public int LavaContactDamage = 80;
        public int LavaDebuffTime = 0;
        public string LavaImmuneTexture = null;
        public int? HoneyBuff = null;
        public int HoneyBuffTime = 0;
        public int? ObsidianWater = null;
        public int? HoneyWater = null;
        public int? CrispyHoneyWater = null;
        public int? BucketReplacement = null;
        public int? BottomlessBucketReplacement = null;
        public int? GoreDropletID = null;
        public LiquidStyle LiquidStyle = LiquidStyle.Lava;
        public Color LavaColor = new();
        public Color MapColor = new();

        internal int Type { get; set; }

        public sealed override void SetupContent()
        {
            SetStaticDefaults();
        }

        public override void SetStaticDefaults()
        {
        }

        protected sealed override void Register()
        {
            ModTypeLookup<AltLiquidStyle>.Register(this);
            Textures = new Asset<Texture2D>[4];
            Textures[0] = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad);
            Textures[1] = ModContent.Request<Texture2D>(SlopeTexture, AssetRequestMode.ImmediateLoad);
            Textures[2] = ModContent.Request<Texture2D>(WaterfallTexture, AssetRequestMode.ImmediateLoad);
            Textures[3] = ModContent.Request<Texture2D>(LiquidTexture, AssetRequestMode.ImmediateLoad);
            if (LiquidStyle != LiquidStyle.Lava && LiquidStyle != LiquidStyle.Honey)
            {
                throw new ArgumentOutOfRangeException(nameof(LiquidStyle), "Invalid option");
            }
            AltLibrary.LiquidStyles.Add(this);
            Type = AltLibrary.LiquidStyles.Count;
        }
    }
}
