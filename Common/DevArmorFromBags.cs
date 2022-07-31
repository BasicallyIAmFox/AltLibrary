using AltLibrary.Content.DevArmor.Cace;
using AltLibrary.Content.DevArmor.Fox;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common
{
	internal class DevArmorFromBags : GlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag")
			{
				IEntitySource source = player.GetSource_GiftOrReward(context);
				if (ItemID.Sets.BossBag[arg] && (!ItemID.Sets.PreHardmodeLikeBossBag[arg] || Main.tenthAnniversaryWorld) && Main.rand.NextBool(Main.tenthAnniversaryWorld ? 10 : 20))
				{
					switch (Main.rand.Next(2))
					{
						case 0:
							player.QuickSpawnItem(source, ModContent.ItemType<FoxMask>(), 1);
							player.QuickSpawnItem(source, ModContent.ItemType<FoxShirt>(), 1);
							player.QuickSpawnItem(source, ModContent.ItemType<FoxPants>(), 1);
							break;
						case 1:
							player.QuickSpawnItem(source, ModContent.ItemType<CaceEars>(), 1);
							break;
					}
				}
			}
		}
	}
}