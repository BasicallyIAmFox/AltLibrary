using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltLiquidStyles
{
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

        /// <summary>
        /// The Death Reason to be displayed in chat if a player is killed by this lava style
        /// </summary>
        public Func<Player, PlayerDeathReason> LavaDeathReason;
        /// <summary>
        /// The amount of damage to be dealt to a player in this lava style
        /// </summary>
        public int LavaContactDamage = 80;
        /// <summary>
        /// The icon that should appear to represent remaining immune time in this lava style (ie lava waders)
        /// </summary>
        public string LavaImmuneTexture = null;
        /// <summary>
        /// The BuffID that the player recieves upon entering this liquid style
        /// </summary>
        public int? LiquidBuff = null;
        /// <summary>
        /// The duration of the buff that the player recieves upon entering this liquid style
        /// </summary>
        public int LiquidBuffTime = 0;
        /// <summary>
        /// For Lava styles, the block that forms when this lava style comes into contact with water
        /// </summary>
        public int? ObsidianWater = null;
        /// <summary>
        /// For Honey styles, the block that forms when this honey style comes into contact with water
        /// </summary>
        public int? HoneyWater = null;
        /// <summary>
        /// For Lava styles, the block that forms when this lava style comes into contact with honey (of any style)
        /// </summary>
        public int? CrispyHoneyWater = null;
        /// <summary>
        /// The ItemID of the bucket of this liquid style, which should be functionally identical to the normal bucket of this fluid
        /// </summary>
        public int? BucketReplacement = null;
        /// <summary>
        /// The ItemID of the bottomless bucket of this liquid style, which should be functionally identical to the normal bottomless bucket of this fluid
        /// </summary>
        public int? BottomlessBucketReplacement = null;
        /// <summary>
        /// The GoreID of the particles to be emitted from this liquid style
        /// </summary>
        public int? GoreDropletID = null;
        /// <summary>
        /// Whether this LiquidStyle is for Honey or Lava
        /// </summary>
        public LiquidStyle LiquidStyle = LiquidStyle.Lava;
        /// <summary>
        /// The color of the light to be emitted by this lava style. Values should always be floats between 0f and 1f
        /// </summary>
        public Color LavaColor = new();
        /// <summary>
        /// The color that this fluid will appear as on the map and minimap
        /// </summary>
        public Color MapColor = new();

        public int Type { get; internal set; }

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
