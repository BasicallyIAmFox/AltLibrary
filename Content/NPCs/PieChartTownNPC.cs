using AltLibrary.Common;
using AltLibrary.Common.Systems;
using AltLibrary.Core.Baking;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AltLibrary.Content.NPCs
{
	[AutoloadHead]
	internal class PieChartTownNPC : ModNPC
	{
		internal static int CurrentPage = 0;

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void Load()
		{
			CurrentPage = 0;
			IL.Terraria.Main.GUIChatDrawInner += Main_GUIChatDrawInner;
		}

		private void Main_GUIChatDrawInner(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.npcChatCornerItem)),
				i => i.MatchBrfalse(out _),
				i => i.MatchLdloca(21)))
			{
				AltLibrary.Instance.Logger.Info("10 $ 1");
				return;
			}

			if (!c.TryGotoNext(i => i.MatchStloc(24)))
			{
				AltLibrary.Instance.Logger.Info("10 $ 2");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 24);
			c.Emit(OpCodes.Ldloc, 22);
			c.EmitDelegate<Func<Texture2D, Item, Texture2D>>((value, item) =>
			{
				if (Main.npc[Main.LocalPlayer.talkNPC].type == Type && item.type == ItemID.DirtBlock && AnalystShopLoader.MaxShopCount() >= 1)
				{
					return ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/ButtonCorrupt", AssetRequestMode.ImmediateLoad).Value;
				}
				return value;
			});
			c.Emit(OpCodes.Stloc, 24);

			if (!c.TryGotoNext(i => i.MatchCallvirt<Item>("get_Name")))
			{
				AltLibrary.Instance.Logger.Info("10 $ 3");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 22);
			c.EmitDelegate<Func<string, Item, string>>((value, item) =>
			{
				if (Main.npc[Main.LocalPlayer.talkNPC].type == Type && item.type == ItemID.DirtBlock && AnalystShopLoader.MaxShopCount() >= 1)
				{
					return Language.GetTextValue("Mods.AltLibrary.AnalysisNext");
				}
				return value;
			});

			if (!c.TryGotoNext(i => i.MatchLdcI4(-11)))
			{
				AltLibrary.Instance.Logger.Info("10 $ 4");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 22);
			c.EmitDelegate<Func<int, Item, int>>((rare, item) =>
			{
				if (Main.npc[Main.LocalPlayer.talkNPC].type == Type && item.type == ItemID.DirtBlock && AnalystShopLoader.MaxShopCount() >= 1)
				{
					return ItemRarityID.Blue;
				}
				return rare;
			});

			if (!c.TryGotoNext(i => i.MatchCall(out _)))
			{
				AltLibrary.Instance.Logger.Info("10 $ 5");
				return;
			}

			c.Index++;
			c.EmitDelegate(() =>
			{
				if (Main.npc[Main.LocalPlayer.talkNPC].type == Type && Main.mouseLeft && Main.mouseLeftRelease && AnalystShopLoader.MaxShopCount() >= 1)
				{
					CurrentPage++;
					if (CurrentPage >= AnalystShopLoader.MaxShopCount())
						CurrentPage = 0;
					Main.mouseLeftRelease = false;
				}
			});
		}

		public override void Unload()
		{
			IL.Terraria.Main.GUIChatDrawInner -= Main_GUIChatDrawInner;
			CurrentPage = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Analyst");

			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Mechanic];

			NPCID.Sets.ExtraFramesCount[Type] = 9;
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700;
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90;
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0)
			{
				Velocity = 1f,
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPC.Happiness // if/when cyber becomes a mod, make analyst love cyber
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Like)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Steampunker, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Cyborg, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Like)
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)
			;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money) => false;

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Mechanic;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Mods.AltLibrary.Bestiary.PieChartTownNPC")
			});
		}

		public override ITownNPCProfile TownNPCProfile() => new PieChartTownNPCProfile();

		public override List<string> SetNPCNameList()
		{
			return new()
			{
				"Kenzie",
				"Harley",
				"Tristen",
				"Nicole",
				"Jaden",
				"Marie",
				"Flo",
				"Susan",
				"Penny"
			};
		}

		public override string GetChat()
		{
			CurrentPage = 0;
			WeightedRandom<string> chat = new();
			for (int i = 0; i < ALUtils.AdvancedGetSizeOfCategory("Mods.AltLibrary.Analyser.Dialog", out LocalizedText[] texts); i++)
			{
				chat.Add(texts[i].Value);
			}
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("LegacyInterface.28") + (AnalystShopLoader.MaxShopCount() > 1 ? " " + Language.GetTextValue("Mods.AltLibrary.AnalysisPage", CurrentPage + 1) : "");
			button2 = Language.GetTextValue("Mods.AltLibrary.Analysis");
		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			nextSlot = 0;

			List<int> sellableItems = AnalystShopLoader.SellableItems();
			int i = 0;
			int startOffset = CurrentPage * 40;
			if (startOffset < 0)
				startOffset = 0;

			foreach (int type in sellableItems)
			{
				if (++i < startOffset)
					continue;
				if (nextSlot >= 40)
					break;
				shop.item[nextSlot].SetDefaults(type);
				nextSlot++;
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			Main.LocalPlayer.GetModPlayer<ALPlayer>().IsAnalysing = false;
			Main.LocalPlayer.GetModPlayer<ALPlayer>().IsAnalysingClick = false;
			if (firstButton)
			{
				shop = true;
			}
			else
			{
				Main.LocalPlayer.GetModPlayer<ALPlayer>().IsAnalysingClick = true;
				WorldBiomeManager.AnalysisTiles();
			}
		}

		public override bool CanGoToStatue(bool toQueenStatue) => true;
	}

	internal class PieChartTownNPCProfile : ITownNPCProfile
	{
		public int GetHeadTextureIndex(NPC npc)
		{
			return ModContent.GetModHeadSlot("AltLibrary/Content/NPCs/PieChartTownNPC_Head");
		}

		public string GetNameForVariant(NPC npc)
		{
			return npc.getNewNPCName();
		}

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>("AltLibrary/Content/NPCs/PieChartTownNPC");
			return ModContent.Request<Texture2D>("AltLibrary/Content/NPCs/PieChartTownNPC");
		}

		public int RollVariation()
		{
			return 0;
		}
	}

	internal class AnalystNPCRelationships : GlobalNPC
	{
		public override void SetStaticDefaults()
		{
			int analystType = ModContent.NPCType<PieChartTownNPC>();

			var otherNPC = NPCHappiness.Get(NPCID.Steampunker);
			otherNPC.SetNPCAffection(analystType, AffectionLevel.Love);

			otherNPC = NPCHappiness.Get(NPCID.Dryad);
			otherNPC.SetNPCAffection(analystType, AffectionLevel.Hate);

			otherNPC = NPCHappiness.Get(NPCID.TaxCollector);
			otherNPC.SetNPCAffection(analystType, AffectionLevel.Dislike);

			otherNPC = NPCHappiness.Get(NPCID.Clothier);
			otherNPC.SetNPCAffection(analystType, AffectionLevel.Like);
		}
	}
}
