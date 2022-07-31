using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	public class AltOreInsideBodies
	{
		public static List<int> ores;

		internal static void Load()
		{
			On.Terraria.NPC.AI_001_Slimes_GenerateItemInsideBody += NPC_AI_001_Slimes_GenerateItemInsideBody;
			IL.Terraria.GameContent.ItemDropRules.SlimeBodyItemDropRule.CanDrop += SlimeBodyItemDropRule_CanDrop;
		}

		private static void SlimeBodyItemDropRule_CanDrop(ILContext il)
		{
			ILCursor c = new(il);
			try
			{
				c.GotoNext(MoveType.After,
					i => i.MatchLdcR4(ItemID.Count));

				c.Emit(OpCodes.Pop);
				c.EmitDelegate<Func<float>>(() => ItemLoader.ItemCount);
			}
			catch (Exception e)
			{
				AltLibrary.Instance.Logger.Error($"[Slime Body Modification]\n{e.Message}\n{e.StackTrace}");
			}
		}

		private static int NPC_AI_001_Slimes_GenerateItemInsideBody(On.Terraria.NPC.orig_AI_001_Slimes_GenerateItemInsideBody orig, bool isBallooned)
		{
			int item = orig(isBallooned);
			if (item >= ItemID.IronOre && item <= ItemID.SilverOre || item >= ItemID.TinOre && item <= ItemID.PlatinumOre)
				item = Main.rand.Next(ores);

			return item;
		}

		internal static void Unload()
		{
			On.Terraria.NPC.AI_001_Slimes_GenerateItemInsideBody -= NPC_AI_001_Slimes_GenerateItemInsideBody;
			IL.Terraria.GameContent.ItemDropRules.SlimeBodyItemDropRule.CanDrop -= SlimeBodyItemDropRule_CanDrop;
		}
	}
}
