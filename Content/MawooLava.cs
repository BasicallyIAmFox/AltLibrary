using AltLibrary.Common.AltLavaStyles;
using Microsoft.Xna.Framework;
using System;

namespace AltLibrary.Content
{
    internal class MawooLava : AltLavaStyle
    {
        public override Func<bool> IsActive => () => true;

        public override void SetStaticDefaults()
        {
            LiquidColor = new Color(0.1f, 0.1f, 0.1f);
        }
    }
}
