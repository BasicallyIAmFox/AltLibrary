using AltLibrary.Common;
using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pie Charter");

			Main.npcFrameCount[Type] = 25;

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

			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate)
			;
		}

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
			AnimationType = NPCID.Guide;
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
				"Ahabrammamaobaama",
            };
        }

        public override string GetChat()
		{
			WeightedRandom<string> chat = new();
			chat.Add("Sometimes I feel like I'm different from everyone else here.");
			return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = Language.GetTextValue("Mods.AltLibrary.Analysis");
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

		public override bool CanGoToStatue(bool toKingStatue) => true;
	}

    internal class PieChartTownNPCProfile : ITownNPCProfile
    {
        public int GetHeadTextureIndex(NPC npc)
        {
			return ModContent.GetModHeadSlot("AltLibrary/Content/NPCs/PieChartTownNPC_Head"); ;
		}

        public string GetNameForVariant(NPC npc)
        {
			return npc.getNewNPCName();
		}

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>("AltLibrary/Content/NPCs/PieChartTownNPC");

			if (npc.altTexture == 1)
				return ModContent.Request<Texture2D>("AltLibrary/Content/NPCs/PieChartTownNPC_Party");

			return ModContent.Request<Texture2D>("AltLibrary/Content/NPCs/PieChartTownNPC");
		}

        public int RollVariation()
        {
			return 0;
        }
    }
}
