using AltLibrary.Common.Data;
using AltLibrary.Common.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content;

public sealed class NPCDropReplacements : GlobalItem {
	public override void OnSpawn(Item item, IEntitySource source) {
		var matcher = new ItemTypeMatcher(item.type);
		if (source is EntitySource_Loot { Entity: NPC { type: NPCID.Retinazer or NPCID.Spazmatism or NPCID.SkeletronPrime or NPCID.TheDestroyer } }
				or EntitySource_ItemOpen { ItemType: int and (ItemID.TwinsBossBag or ItemID.SkeletronPrimeBossBag or ItemID.DestroyerBossBag) }) {
			var conversionData = WorldDataManager.GetHallow().ItemReplacements;

			matcher.ReplaceByIfMatch(item, conversionData[ItemID.HallowedBar], ItemID.HallowedBar);
		}
		else if (source is EntitySource_Loot { Entity: NPC { type: NPCID.EyeofCthulhu } }
				or EntitySource_ItemOpen { ItemType: ItemID.EyeOfCthulhuBossBag }) {
			var conversionData = WorldDataManager.GetEvil().ItemReplacements;

			matcher.ReplaceByIfMatch(item, conversionData[ItemID.DemoniteOre], ItemID.DemoniteOre, ItemID.CrimtaneOre);
			matcher.ReplaceByIfMatch(item, conversionData[ItemID.CorruptSeeds], ItemID.CorruptSeeds, ItemID.CrimsonSeeds);
			matcher.ReplaceByIfMatch(item, conversionData[ItemID.UnholyArrow], ItemID.UnholyArrow);
		}
	}

	private record struct ItemTypeMatcher(int ItemType) {
		public bool Match(params int[] itemTypes) {
			for (int i = 0; i < itemTypes.Length; i++) {
				if (itemTypes[i] == ItemType) {
					return true;
				}
			}
			return false;
		}

		public void ReplaceByIfMatch(Item item, int itemType, params int[] itemTypes) {
			if (Match(itemTypes)) {
				ReplaceBy(item, itemType);
			}
		}

		private static void ReplaceBy(Item item, int itemType) {
			item.TurnToAir();
			item.SetDefaults(itemType);
		}
	}
}
