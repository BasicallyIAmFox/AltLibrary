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

		public static void Unload()
		{
			IL.Terraria.GameContent.Biomes.CaveHouse.HouseUtils.CreateBuilder -= HouseUtils_CreateBuilder;
		}

		private static void HouseUtils_CreateBuilder(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchNewobj(typeof(JungleHouseBuilder).GetConstructor(BindingFlags.Public | BindingFlags.Instance, new Type[] { typeof(IEnumerable<Rectangle>) }))))
			{
				AltLibrary.Instance.Logger.Info("j $ 1");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdloc(0)))
			{
				AltLibrary.Instance.Logger.Info("j $ 2");
				return;
			}

			var label = il.DefineLabel();

			c.EmitDelegate(() => WorldBiomeManager.WorldJungle == "");
			c.Emit(OpCodes.Brfalse_S, label);

			c.EmitDelegate(() => HouseBuilder.Invalid);
			c.Emit(OpCodes.Ret);

			c.MarkLabel(label);
		}
	}
}
