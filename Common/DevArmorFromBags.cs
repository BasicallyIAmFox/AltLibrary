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
                            player.QuickSpawnItem(source, ModContent.GetInstance<FoxMask>().Type, 1);
                            player.QuickSpawnItem(source, ModContent.GetInstance<FoxShirt>().Type, 1);
                            player.QuickSpawnItem(source, ModContent.GetInstance<FoxPants>().Type, 1);
                            break;
                    }
                }
            }
        }
    }
}