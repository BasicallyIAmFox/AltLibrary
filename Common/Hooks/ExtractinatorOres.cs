using AltLibrary.Common.AltOres;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	public static class ExtractinatorOres
	{
		private static readonly string NoteToEveryone = "Yes, this is a detour, and it WILL have incompatibilities with ILs, but it was made for the sake of other detours.";

		public static List<int> Gems;
		public static List<int> Ores;

		internal static void Load()
		{
			Gems = new()
			{
				ItemID.Amethyst,
				ItemID.Topaz,
				ItemID.Sapphire,
				ItemID.Emerald,
				ItemID.Ruby,
				ItemID.Diamond,
			};

			Ores = new()
			{
				ItemID.CopperOre,
				ItemID.TinOre,
				ItemID.IronOre,
				ItemID.LeadOre,
				ItemID.SilverOre,
				ItemID.TungstenOre,
				ItemID.GoldOre,
				ItemID.PlatinumOre
			};

			foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType >= OreType.Copper && x.OreType <= OreType.Gold || x.IncludeInExtractinator))
				Ores.Add(o.ore);

			On.Terraria.Player.ExtractinatorUse += Player_ExtractinatorUse1;
		}

		internal static void Unload()
		{
			On.Terraria.Player.ExtractinatorUse -= Player_ExtractinatorUse1;

			Gems = null;
			Ores = null;
		}

		private static void Player_ExtractinatorUse1(On.Terraria.Player.orig_ExtractinatorUse orig, Player self, int extractType)
		{
			int mosquito = 5000;
			int num8 = 25;
			int num5 = 50;
			int studryFossil = -1;
			if (extractType == ItemID.DesertFossil)
			{
				mosquito /= 3;
				num8 *= 2;
				num5 = 20;
				studryFossil = 10;
			}

			int itemType;
			int itemStack = 1;
			if (studryFossil != -1 && Main.rand.NextBool(studryFossil))
			{
				itemType = ItemID.FossilOre;
				if (Main.rand.NextBool(5))
					itemStack += Main.rand.Next(2);

				if (Main.rand.NextBool(10))
					itemStack += Main.rand.Next(3);

				if (Main.rand.NextBool(15))
					itemStack += Main.rand.Next(4);
			}
			else if (Main.rand.NextBool(2))
			{
				if (Main.rand.NextBool(12000))
				{
					itemType = ItemID.PlatinumCoin;
					if (Main.rand.NextBool(14))
						itemStack += Main.rand.Next(0, 2);
					if (Main.rand.NextBool(14))
						itemStack += Main.rand.Next(0, 2);
					if (Main.rand.NextBool(14))
						itemStack += Main.rand.Next(0, 2);
				}
				else if (Main.rand.NextBool(800))
				{
					itemType = ItemID.GoldCoin;
					if (Main.rand.NextBool(6))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(6))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(6))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(6))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(6))
						itemStack += Main.rand.Next(1, 20);
				}
				else if (Main.rand.NextBool(60))
				{
					itemType = ItemID.SilverCoin;
					if (Main.rand.NextBool(4))
						itemStack += Main.rand.Next(5, 26);
					if (Main.rand.NextBool(4))
						itemStack += Main.rand.Next(5, 26);
					if (Main.rand.NextBool(4))
						itemStack += Main.rand.Next(5, 26);
					if (Main.rand.NextBool(4))
						itemStack += Main.rand.Next(5, 25);
				}
				else
				{
					itemType = ItemID.CopperCoin;
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(10, 26);
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(10, 26);
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(10, 26);
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(10, 25);
				}
			}
			else if (mosquito != -1 && Main.rand.NextBool(mosquito))
			{
				itemType = ItemID.AmberMosquito;
			}
			else if (num8 != -1 && Main.rand.NextBool(num8))
			{
				itemType = Main.rand.Next(Gems);
				if (Main.rand.NextBool(20))
					itemStack += Main.rand.Next(0, 2);
				if (Main.rand.NextBool(30))
					itemStack += Main.rand.Next(0, 3);
				if (Main.rand.NextBool(40))
					itemStack += Main.rand.Next(0, 4);
				if (Main.rand.NextBool(50))
					itemStack += Main.rand.Next(0, 5);
				if (Main.rand.NextBool(60))
					itemStack += Main.rand.Next(0, 6);
			}
			else if (num5 != -1 && Main.rand.NextBool(num5))
			{
				itemType = ItemID.Amber;
				if (Main.rand.NextBool(20))
					itemStack += Main.rand.Next(0, 2);
				if (Main.rand.NextBool(30))
					itemStack += Main.rand.Next(0, 3);
				if (Main.rand.NextBool(40))
					itemStack += Main.rand.Next(0, 4);
				if (Main.rand.NextBool(50))
					itemStack += Main.rand.Next(0, 5);
				if (Main.rand.NextBool(60))
					itemStack += Main.rand.Next(0, 6);
			}
			else if (Main.rand.NextBool(3))
			{
				if (Main.rand.NextBool(5000))
				{
					itemType = ItemID.PlatinumCoin;
					if (Main.rand.NextBool(10))
						itemStack += Main.rand.Next(0, 3);
					if (Main.rand.NextBool(10))
						itemStack += Main.rand.Next(0, 3);
					if (Main.rand.NextBool(10))
						itemStack += Main.rand.Next(0, 3);
					if (Main.rand.NextBool(10))
						itemStack += Main.rand.Next(0, 3);
					if (Main.rand.NextBool(10))
						itemStack += Main.rand.Next(0, 3);
				}
				else if (Main.rand.NextBool(400))
				{
					itemType = ItemID.GoldCoin;
					if (Main.rand.NextBool(5))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(5))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(5))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(5))
						itemStack += Main.rand.Next(1, 21);
					if (Main.rand.NextBool(5))
						itemStack += Main.rand.Next(1, 20);
				}
				else if (Main.rand.NextBool(30))
				{
					itemType = ItemID.SilverCoin;
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(5, 26);
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(5, 26);
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(5, 26);
					if (Main.rand.NextBool(3))
						itemStack += Main.rand.Next(5, 25);
				}
				else
				{
					itemType = ItemID.CopperCoin;
					if (Main.rand.NextBool(2))
						itemStack += Main.rand.Next(10, 26);
					if (Main.rand.NextBool(2))
						itemStack += Main.rand.Next(10, 26);
					if (Main.rand.NextBool(2))
						itemStack += Main.rand.Next(10, 26);
					if (Main.rand.NextBool(2))
						itemStack += Main.rand.Next(10, 25);
				}
			}
			else
			{
				itemType = Main.rand.Next(Ores);
				if (Main.rand.NextBool(20))
					itemStack += Main.rand.Next(0, 2);
				if (Main.rand.NextBool(30))
					itemStack += Main.rand.Next(0, 3);
				if (Main.rand.NextBool(40))
					itemStack += Main.rand.Next(0, 4);
				if (Main.rand.NextBool(50))
					itemStack += Main.rand.Next(0, 5);
				if (Main.rand.NextBool(60))
					itemStack += Main.rand.Next(0, 6);
			}
			ItemLoader.ExtractinatorUse(ref itemType, ref itemStack, extractType);

			if (itemType > 0)
			{
				Vector2 position = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
				if (Main.SmartCursorIsUsed || PlayerInput.UsingGamepad)
					position = self.Center;

				int itemIndex = Item.NewItem(self.GetSource_TileInteraction(Player.tileTargetX, Player.tileTargetY), (int)position.X, (int)position.Y, 1, 1, itemType, itemStack, pfix: -1);
				if (Main.netMode == NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, itemIndex, 1f);
			}
		}
	}
}
