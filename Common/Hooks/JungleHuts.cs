using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.GameContent.Biomes.CaveHouse;

namespace AltLibrary.Common.Hooks
{
    internal class JungleHuts
    {
        public static void Init()
        {
            IL.Terraria.GameContent.Biomes.CaveHouse.HouseUtils.CreateBuilder += HouseUtils_CreateBuilder;
        }

        private static void HouseUtils_CreateBuilder(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchNewobj(typeof(JungleHouseBuilder).GetConstructor(BindingFlags.Public | BindingFlags.Instance, new Type[] { typeof(IEnumerable<Rectangle>) }))))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdloc(0)))
                return;

            var label = il.DefineLabel();

            c.Emit(OpCodes.Ldsfld, typeof(WorldBiomeManager).GetField(nameof(WorldBiomeManager.worldJungle), BindingFlags.Public | BindingFlags.Static));
            c.Emit(OpCodes.Ldstr, "");
            c.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }));
            c.Emit(OpCodes.Brfalse_S, label);

            c.Emit(OpCodes.Ldsfld, typeof(HouseBuilder).GetField("Invalid", BindingFlags.Public | BindingFlags.Static));
            c.Emit(OpCodes.Ret);

            c.MarkLabel(label);
        }
    }
}
