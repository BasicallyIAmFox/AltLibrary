using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Content.Items.AnalystItems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Core.Baking
{
	public static class AnalystShopLoader
	{
		internal static List<AnalystItem> Items;

		internal static void Load()
		{
			Items = new();

			AddAnalystItem(new AnalystItem(ModContent.ItemType<HallowFanBunnyMask>(), () => Main.hardMode && WorldBiomeManager.HallowBiomePercentage >= 0.1f));
		}

		public static bool AddAnalystItem(AnalystItem item)
		{
			if (!Items.Any(x => x.itemid == item.itemid))
			{
				Items.Add(item);
				return true;
			}
			return false;
		}

		public static int MaxShopCount() => SellableItems().Count / 40;

		internal static List<int> SellableItems()
		{
			List<int> items = new();
			foreach (AnalystItem item in Items)
			{
				if (item.availability.Invoke())
				{
					items.Add(item.itemid);
				}
			}
			return items;
		}

		internal static void Unload() => Items = null;
	}

	public struct AnalystItem
	{
		public int itemid;
		public Func<bool> availability;

		public AnalystItem()
		{
			throw new InvalidOperationException();
		}

		public AnalystItem(int itemid)
		{
			this.itemid = itemid;
			availability = () => true;
		}

		public AnalystItem(int itemid, Func<bool> availability)
		{
			this.itemid = itemid;
			this.availability = availability;
		}

		public AnalystItem(int itemid, AltBiome biome, float percentage)
		{
			this.itemid = itemid;
			float per = percentage;
			bool positive = percentage < 0f;
			availability = () =>
			{
				bool bl = WorldBiomeManager.AltBiomePercentages[biome.Type + 3] >= percentage;
				if (!positive)
				{
					bl = WorldBiomeManager.AltBiomePercentages[biome.Type + 3] <= -percentage;
				}
				return bl;
			};
		}

		public AnalystItem(AltBiome biome, int itemid, float percentage)
		{
			this.itemid = itemid;
			float per = percentage;
			bool positive = percentage < 0f;
			availability = () =>
			{
				bool bl = WorldBiomeManager.AltBiomePercentages[biome.Type + 3] >= percentage;
				if (!positive)
				{
					bl = WorldBiomeManager.AltBiomePercentages[biome.Type + 3] <= -percentage;
				}
				return bl;
			};
		}
	}
}
