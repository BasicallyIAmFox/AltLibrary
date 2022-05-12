using AltLibrary.Common.AltLiquidStyles;
using Microsoft.Xna.Framework;
using System;

namespace AltLibrary.Content
{
    internal class MawooLava : AltLiquidStyle
    {
        public override Func<bool> IsActive => () => true;

        public override void SetStaticDefaults()
        {
            LiquidStyle = LiquidStyle.Lava;
            LavaImmuneTexture = "AltLibrary/Content/MawooLava_Immune";
            LavaColor = new Color(0.01f, 0.01f, 0.01f);
            MapColor = new(255, 127, 63);
        }
    }
}
