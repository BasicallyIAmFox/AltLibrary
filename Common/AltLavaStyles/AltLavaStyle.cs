using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltLavaStyles
{
    [Obsolete("This class is being heavily in DEVELOPMENT and EXPERIMENTAL! Major changes can happen in any update. Risky to use.")]
    public abstract class AltLavaStyle : ModTexturedType
    {
        /// <summary>
        /// Should only be used only with World-like fields. (will never change in worlds)
        /// </summary>
        public virtual Func<bool> IsActive => () => false;
        public override string Texture => base.Texture;
        public virtual string SlopeTexture => base.Texture + "_Slope";
        public virtual string WaterfallTexture => base.Texture + "_Waterfall";


        public Color LiquidColor = new();
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
            ModTypeLookup<AltLavaStyle>.Register(this);
            AltLibrary.LavaStyles.Add(this);
            Type = AltLibrary.LavaStyles.Count;
        }
    }
}
