using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AltLibrary.Common
{
	internal class ALPlayer : ModPlayer
	{
		public bool HasObtainedHallowBunnyAtleastOnce = false;
		public bool IsAnalysing = false;
		public bool IsAnalysingClick = false;

		public override void Unload()
		{
			HasObtainedHallowBunnyAtleastOnce = false;
			IsAnalysing = false;
			IsAnalysingClick = false;
		}

		public override void PostUpdate()
		{
			if (Player.HasItem(ModContent.ItemType<Content.Items.HallowBunny>()) && !HasObtainedHallowBunnyAtleastOnce)
			{
				HasObtainedHallowBunnyAtleastOnce = true;
			}
			if (Player.talkNPC == -1)
			{
				IsAnalysing = false;
				IsAnalysingClick = false;
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
