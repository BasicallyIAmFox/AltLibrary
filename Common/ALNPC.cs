using AltLibrary.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common
{
    internal partial class ALNPC : GlobalNPC
    {
        public override bool IsCloneable => true;
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances => true;

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.KingSlime)
            {
                return StarTracker_PreAI(npc);
            }
            return true;
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.KingSlime && starTracker)
            {
                return StarTracker_PreDraw(npc, spriteBatch, screenPos, drawColor);
            }
            return true;
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.KingSlime && starTracker)
            {
                StarTracker_PostDraw(spriteBatch);
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                if (Main.drunkWorld)
                {
                    WorldBiomeGeneration.WofKilledTimes++;
                    if (WorldBiomeGeneration.WofKilledTimes > 1)
                    {
                        StartHardmode();
                    }
                }
                else if (WorldBiomeGeneration.WofKilledTimes == 0)
                {
                    WorldBiomeGeneration.WofKilledTimes = 1;
                }
            }
        }

        public static void StartHardmode()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.smCallBack), 1);
            }
        }
    }
}
