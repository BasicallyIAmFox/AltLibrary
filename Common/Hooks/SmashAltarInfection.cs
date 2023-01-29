using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Core.Baking;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	internal class SmashAltarInfection
	{
		public static void Init()
		{
			EditsHelper.IL<WorldGen>(nameof(WorldGen.SmashAltar), WorldGen_SmashAltar);
		}

		public static void Unload()
		{
		}

		private static void WorldGen_SmashAltar(ILContext il)
		{
			ILCursor c = new(il);

			if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
			{
				AltLibrary.Instance.Logger.Info("n $ 0");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc_0);
			c.EmitDelegate(DrunkenBaking.GetDrunkSmashingData);

			for (int j = 0; j < 3; j++)
			{
				if (j == 1)
				{
					if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
					{
						AltLibrary.Instance.Logger.Info("n $ -1 " + j);
						return;
					}

					c.Index++;
					c.Emit(OpCodes.Pop);
					c.EmitDelegate(() => false);
				}
				else if (j == 2)
				{
					if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
					{
						AltLibrary.Instance.Logger.Info("n $ -2 " + j);
						return;
					}

					c.Index++;
					c.Emit(OpCodes.Pop);
					c.EmitDelegate(() => false);
				}

				if (!c.TryGotoNext(i => i.MatchLdsfld<Lang>(nameof(Lang.misc)),
					i => i.MatchLdloc(j == 0 ? 11 : (j == 1 ? 17 : 26)),
					i => i.MatchLdelemRef(),
					i => i.MatchCallvirt<LocalizedText>("get_Value")))
				{
					AltLibrary.Instance.Logger.Info("n $ 1 " + j);
					return;
				}

				c.Index += 4;
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_I4, j);
				c.EmitDelegate(DrunkenBaking.GetSmashAltarText);

				if (!c.TryGotoNext(i => i.MatchLdsfld<Lang>(nameof(Lang.misc)),
					i => i.MatchLdloc(j == 0 ? 11 : (j == 1 ? 17 : 26)),
					i => i.MatchLdelemRef(),
					i => i.MatchLdfld<LocalizedText>(nameof(LocalizedText.Key)),
					i => i.MatchCall(out _),
					i => i.MatchCall<NetworkText>(nameof(NetworkText.FromKey))))
				{
					AltLibrary.Instance.Logger.Info("n $ 2 " + j);
					return;
				}

				c.Index += 6;
				c.Emit(OpCodes.Pop);
				c.Emit(OpCodes.Ldc_I4, j);
				c.EmitDelegate<Func<int, NetworkText>>((j) => NetworkText.FromLiteral(DrunkenBaking.GetSmashAltarText(j)));
			}
		}
	}
}
