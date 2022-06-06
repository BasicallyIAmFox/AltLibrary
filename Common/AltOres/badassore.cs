using System.Collections.Generic;

namespace AltLibrary.Common.AltOres
{
    internal class badassore : AltOre
    {
        public override void CustomSelection(OreType thisOreType, List<AltOre> list)
        {
        }

        public override void SetStaticDefaults()
        {
            OreType = OreType.Copper;
        }
    }
}
