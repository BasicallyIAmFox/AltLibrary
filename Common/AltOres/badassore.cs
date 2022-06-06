using System.Collections.Generic;

namespace AltLibrary.Common.AltOres
{
    internal class badassore : AltOre
    {
        public override void CustomSelection(List<AltOre> list)
        {
            base.CustomSelection(list);
        }

        public override void SetStaticDefaults()
        {
            OreType = OreType.Iron;
        }
    }
}
