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

			try
			{
				c.GotoNext(MoveType.After, i => i.MatchStloc(2));

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

				c.GotoNext(MoveType.After,
					i => i.MatchLdcI4(11),
					i => i.MatchLdcI4(3),
					i => i.MatchLdcI4(7),
					i => i.MatchNewobj<Color>());

				c.EmitDelegate<Func<Color, Color>>((orig) =>
				{
					if (WorldBiomeManager.WorldHell != "")
					{
						return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).AltUnderworldColor;
					}
					return orig;
				});
			}
			catch
			{
			}
		}
	}
}
