using AltLibrary.Common.Systems;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common {
	internal class ALNPC : GlobalNPC {
		public override void OnKill(NPC npc) {
			if (npc.type != NPCID.WallofFlesh) {
				return;
			}
			if (Main.drunkWorld) {
				if (++WorldBiomeGeneration.WofKilledTimes > 1) {
					StartHardmode();
				}
			}
			else if (WorldBiomeGeneration.WofKilledTimes == 0) {
				WorldBiomeGeneration.WofKilledTimes = 1;
			}
		}

		public static void StartHardmode() {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.smCallBack), 1);
		}
	}
}
