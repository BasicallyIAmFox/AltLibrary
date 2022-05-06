using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class LivingMahogany
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.GenerateWorld += WorldGen_RemoveLivingMahoganyIfNotJungle;
        }

        private static void WorldGen_RemoveLivingMahoganyIfNotJungle(ILContext il)
        {
            //ILCursor c = new(il);
            //if (!c.TryGotoNext(i => i.MatchLdcI4()))
            //    return;
        }
    }
}
