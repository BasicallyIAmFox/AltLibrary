using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	internal static class UnderworldVisual
	{
		public static void Init()
		{
			IL.Terraria.Main.DrawUnderworldBackgroudLayer += Main_DrawUnderworldBackgroudLayer;
		}

		public static void Unload()
		{
			IL.Terraria.Main.DrawUnderworldBackgroudLayer -= Main_DrawUnderworldBackgroudLayer;
		}

		private static void Main_DrawUnderworldBackgroudLayer(ILContext il)
		{
			var c = new ILCursor(il);
			if (!c.TryGotoNext(i => i.MatchStloc(2)))
			{
				AltLibrary.Instance.Logger.Info("r $ 1");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 0);
			c.Emit(OpCodes.Ldloc, 2);
			c.EmitDelegate<Func<int, Texture2D, Texture2D>>((index, orig) =>
			{
				if (WorldBiomeManager.WorldHell != "")
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).AltUnderworldBackgrounds[index].Value;
				}
				return orig;
			});
			c.Emit(OpCodes.Stloc, 2);

			if (!c.TryGotoNext(i => i.MatchLdcI4(11),
				i => i.MatchLdcI4(3),
				i => i.MatchLdcI4(7),
				i => i.MatchNewobj<Color>()))
			{
				AltLibrary.Instance.Logger.Info("r $ 2");
				return;
			}

			c.Index += 4;
			c.EmitDelegate<Func<Color, Color>>((orig) =>
			{
				if (WorldBiomeManager.WorldHell != "")
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).AltUnderworldColor;
				}
				return orig;
			});
		}
	}
}
