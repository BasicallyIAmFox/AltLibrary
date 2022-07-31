using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Common.Hooks
{
	internal class MimicSummon
	{
		internal static Dictionary<string, Struct_31213> MimicPairs;

		public static void Init()
		{
			On.Terraria.NPC.BigMimicSummonCheck += NPC_BigMimicSummonCheck;
		}

		public static void SetupContent()
		{
		}

		private static bool Cond(AltBiome biome, bool isEvil)
		{
			return isEvil ? !WorldGen.crimson && WorldBiomeManager.WorldEvil == biome.FullName : (!AltLibrary.Biomes.Any(x => x.MimicKeyType == ItemID.LightKey) || WorldBiomeManager.WorldHallow == biome.FullName);
		}

		public static void Unload()
		{
			On.Terraria.NPC.BigMimicSummonCheck -= NPC_BigMimicSummonCheck;
			MimicPairs = null;
		}

		internal struct Struct_31213
		{
			internal int field_74123;
			internal int field_30363;
			internal Func<bool> condition;

			public Struct_31213(int key, int mimic)
			{
				field_74123 = key;
				field_30363 = mimic;
				condition = () => true;
			}

			public Struct_31213(int key, int mimic, Func<bool> cond)
			{
				field_74123 = key;
				field_30363 = mimic;
				condition = cond ?? throw new ArgumentNullException(nameof(cond));
			}
		}

		private static bool NPC_BigMimicSummonCheck(On.Terraria.NPC.orig_BigMimicSummonCheck orig, int x, int y, Player user)
		{
			if (MimicPairs == null)
			{
				MimicPairs = new()
				{
					["Corruption"] = new(ItemID.NightKey, NPCID.BigMimicCorruption, () => WorldBiomeManager.IsCorruption),
					["Crimson"] = new(ItemID.NightKey, NPCID.BigMimicCrimson, () => WorldBiomeManager.IsCrimson),
					["Hallow"] = new(ItemID.LightKey, NPCID.BigMimicHallow, () => !AltLibrary.Biomes.Any(x => x.MimicKeyType == ItemID.LightKey) || WorldBiomeManager.WorldHallow == ""),
				};
				foreach (AltBiome biome in AltLibrary.Biomes)
				{
					if (biome.BiomeType <= BiomeType.Hallow && biome.MimicKeyType.HasValue && biome.MimicType.HasValue)
					{
						bool isEvil = biome.BiomeType == BiomeType.Evil;
						MimicPairs.TryAdd($"{biome.FullName}", new(biome.MimicKeyType.Value, biome.MimicType.Value, () => Cond(biome, isEvil)));
					}
				}
			}

			if (Main.netMode == NetmodeID.MultiplayerClient || !Main.hardMode)
				return false;

			int chestIndex = Chest.FindChest(x, y);
			if (chestIndex < 0)
				return false;

			List<AltBiome> EvilHallow = new();
			AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow || x.BiomeType == BiomeType.Evil)
				.ToList().ForEach(x => EvilHallow.Add(x));
			Dictionary<string, (int, int)> keys = new()
			{
				{ "Terraria/Hallow", (NPCID.BigMimicHallow, 0) },
				{ "Terraria/Evil", (MimicPairs["Crimson"].condition() ? NPCID.BigMimicCrimson : NPCID.BigMimicCorruption, 0) }
			};
			foreach (AltBiome biome in EvilHallow)
			{
				keys.TryAdd(biome.FullName, (biome.MimicType.GetValueOrDefault(), 0));
			}

			int emptiness = 0;
			int leading = 0;
			for (int i = 0; i < 40; i++)
			{
				ushort chestTile = Main.tile[Main.chest[chestIndex].x, Main.chest[chestIndex].y].TileType;
				int chestFrame = Main.tile[Main.chest[chestIndex].x, Main.chest[chestIndex].y].TileFrameX / 36;
				if (TileID.Sets.BasicChest[chestTile] && (chestTile != 21 || chestFrame < 5 || chestFrame > 6) && Main.chest[chestIndex].item[i] != null && Main.chest[chestIndex].item[i].type > ItemID.None)
				{
					foreach (var pair in from KeyValuePair<string, Struct_31213> pair in MimicPairs
										 where pair.Value.condition() && pair.Key != "Corruption" && pair.Key != "Crimson" && pair.Key != "Hallow"
											&& Main.chest[chestIndex].item[i].type == pair.Value.field_74123
										 select pair)
					{
						(int, int) p = keys[pair.Key];
						p.Item2 += Main.chest[chestIndex].item[i].stack;
						keys[pair.Key] = p;
						leading = Main.chest[chestIndex].item[i].type;
					}

					if (Main.chest[chestIndex].item[i].type == ItemID.LightKey)
					{
						(int, int) p = keys["Terraria/Hallow"];
						p.Item2 += Main.chest[chestIndex].item[i].stack;
						keys["Terraria/Hallow"] = p;
						leading = Main.chest[chestIndex].item[i].type;
					}
					else if (Main.chest[chestIndex].item[i].type == ItemID.NightKey)
					{
						(int, int) p = keys["Terraria/Evil"];
						p.Item2 += Main.chest[chestIndex].item[i].stack;
						keys["Terraria/Evil"] = p;
						leading = Main.chest[chestIndex].item[i].type;
					}
					else if (leading == 0)
					{
						emptiness++;
					}
				}
			}

			int total = 0;
			string index = null;
			foreach (KeyValuePair<string, (int, int)> p in keys)
			{
				total += keys[p.Key].Item2;
			}
			if (total == 1)
			{
				foreach (KeyValuePair<string, Struct_31213> pair in MimicPairs)
				{
					if (pair.Value.field_74123 == leading)
					{
						index = pair.Key;
					}
				}
			}

			if (emptiness == 0 && index != null)
			{
				if (TileID.Sets.BasicChest[Main.tile[x, y].TileType])
				{
					if (Main.tile[x, y].TileFrameX % 36 != 0)
					{
						x--;
					}
					if (Main.tile[x, y].TileFrameY % 36 != 0)
					{
						y--;
					}
					int number = Chest.FindChest(x, y);
					for (int j = 0; j < 40; j++)
					{
						Main.chest[chestIndex].item[j] = new Item();
					}
					Chest.DestroyChest(x, y);
					for (int k = x; k <= x + 1; k++)
					{
						for (int l = y; l <= y + 1; l++)
						{
							if (TileID.Sets.BasicChest[Main.tile[k, l].TileType])
							{
								Main.tile[k, l].ClearTile();
							}
						}
					}
					int number2 = 1;
					if (Main.tile[x, y].TileType == 467)
					{
						number2 = 5;
					}
					if (Main.tile[x, y].TileType >= 625)
					{
						number2 = 101;
					}
					NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, number2, x, y, 0f, number, Main.tile[x, y].TileType, 0);
					NetMessage.SendTileSquare(-1, x, y, 3);
				}
				int mimicID = MimicPairs[index].field_30363;
				int mimicIndex = NPC.NewNPC(user.GetSource_TileInteraction(x, y), x * 16 + 16, y * 16 + 32, mimicID, 0, 0f, 0f, 0f, 0f, 255);
				Main.npc[mimicIndex].whoAmI = mimicIndex;
				NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, mimicIndex, 0f, 0f, 0f, 0, 0, 0);
				Main.npc[mimicIndex].BigMimicSpawnSmoke();
			}
			return false;
		}
	}
}
