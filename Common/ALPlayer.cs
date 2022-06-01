using AltLibrary.Content.Items;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AltLibrary.Common
{
    internal class ALPlayer : ModPlayer
    {
        public bool HasObtainedHallowBunnyAtleastOnce = false;

        public override void PostUpdate()
        {
            if (Player.HasItem(ModContent.ItemType<HallowBunny>()) && !HasObtainedHallowBunnyAtleastOnce)
            {
                HasObtainedHallowBunnyAtleastOnce = true;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("AltLibrary:" + nameof(HasObtainedHallowBunnyAtleastOnce), HasObtainedHallowBunnyAtleastOnce);
        }

        public override void LoadData(TagCompound tag)
        {
            HasObtainedHallowBunnyAtleastOnce = tag.GetBool("AltLibrary:" + nameof(HasObtainedHallowBunnyAtleastOnce));
        }
    }
}
