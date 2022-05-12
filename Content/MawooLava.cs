using AltLibrary.Common.AltLiquidStyles;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace AltLibrary.Content
{
    internal class MawooLava : AltLiquidStyle
    {
        public override Func<bool> IsActive => () => true;

        public override void SetStaticDefaults()
        {
            LiquidStyle = LiquidStyle.Honey;
            LavaImmuneTexture = "AltLibrary/Content/MawooLava_Immune";
            LavaColor = new Color(0.01f, 0.01f, 0.01f);
            MapColor = new(255, 127, 63);
            LavaContactDamage = 250;
            LavaDebuff = BuffID.Venom;
            LavaDebuffTime = 40;
        }
    }
}
