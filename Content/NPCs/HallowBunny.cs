using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Content.NPCs
{
	internal class HallowBunny : ModNPC
	{
		public override string Texture => "AltLibrary/Assets/HallowBunny";

		public override bool IsLoadingEnabled(Mod mod) => AltLibraryServerConfig.Config.SecretFeatures;

		public override void Load() => AltLibrary.NPCsToNowShowUp.Add(Type);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallow Bunny");
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
			Main.npcCatchable[Type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Bunny);
			NPC.width = NPC.height = 30;
			NPC.lifeMax *= 5;
			NPC.catchItem = (short)ModContent.ItemType<Items.HallowBunny>();
			NPC.friendly = true;
			AIType = NPCID.Bunny;
			AnimationType = NPCID.Bunny;
		}

		public override bool? CanBeCaughtBy(Item item, Player player) => AltLibrary.HallowBunnyUnlocked;

		public override void AI()
		{
			if (AltLibrary.HallowBunnyUnlocked)
			{
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active && player.statLife > 0 && player.Distance(NPC.position) <= 500f && Main.time % Main.rand.Next(80, 150) <= Main.rand.Next(0, 5) && Collision.CanHit(player, NPC))
					{
						List<int> pairs = new()
						{
							0,
							1,
							2,
							3,
							4,
							6
						};
						if (player.name == "FoxXD_") pairs.Add(5);

						string reason = Language.GetTextValue($"Mods.AltLibrary.BunReason.{Main.rand.Next(pairs)}", player.name);
						player.Hurt(PlayerDeathReason.ByCustomReason(reason), Main.tenthAnniversaryWorld ? 2 : 100 + Main.rand.Next(0, 100), -1);
						player.immuneTime = 2;
					}
				}
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.life > 0 && npc.Distance(NPC.position) <= 500f && Main.time % Main.rand.Next(80, 150) <= Main.rand.Next(0, 5) && Collision.CanHit(npc, NPC) && npc.type != Type && !npc.dontTakeDamage && !npc.dontTakeDamageFromHostiles)
					{
						npc.StrikeNPC(Main.tenthAnniversaryWorld ? 2 : 20 + Main.rand.Next(0, 20), 0f, -1, Main.rand.Next(100) < 25);
						for (int j = 0; j < byte.MaxValue; j++)
						{
							npc.immune[j] = 2;
						}
					}
				}
			}
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (AltLibrary.HallowBunnyUnlocked)
			{
				knockback = 0f;
				damage = 1;
				crit = false;
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (AltLibrary.HallowBunnyUnlocked)
			{
				knockback = 0f;
				damage = 1;
				crit = false;
			}
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return true;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return AltLibrary.HallowBunnyUnlocked ? 30f : 0f;
		}

		private class HallowGlobal : GlobalNPC
		{
			public override bool IsLoadingEnabled(Mod mod) => AltLibraryServerConfig.Config.SecretFeatures;

			public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == ModContent.NPCType<HallowBunny>();

			public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
			{
				if (AltLibrary.HallowBunnyUnlocked)
				{
					pool.Clear();
					pool.Add(ModContent.NPCType<HallowBunny>(), 30f);
				}
			}

			public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
			{
				if (AltLibrary.HallowBunnyUnlocked)
				{
					spawnRate /= 30;
					maxSpawns *= 30;
				}
			}
		}
	}
}
