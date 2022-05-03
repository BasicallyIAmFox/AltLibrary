using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary.Common.Systems
{
    internal class RewriterSystem : ModSystem
    {
        public override void OnWorldLoad()
        {
            string evil = WorldBiomeManager.worldEvil;
            string hallow = WorldBiomeManager.worldHallow;
            string hell = WorldBiomeManager.worldHell;
            string jungle = WorldBiomeManager.worldJungle;
            BiomeType causedIssueBy = (BiomeType)(-1);
            if (ModContent.Find<AltBiome>(evil).BiomeType != BiomeType.Evil)
            {
                List<string> checks = new() { hallow, hell, jungle };
                foreach (string check in checks)
                {
                    if (check != "" && ModContent.Find<AltBiome>(check).BiomeType == BiomeType.Evil)
                    {
                        causedIssueBy = AltLibrary.biomes.First(x => x.FullName == check).BiomeType;
                        break;
                    }
                }
            }
            if (causedIssueBy == (BiomeType)(-1))
            {
                AltLibrary.Instance.Logger.Info("Checking issues done! Enjoy game!");
            }
            else
            {
                string inside = causedIssueBy == BiomeType.Evil ? "evil" : (causedIssueBy == BiomeType.Hallow ? "hallow" : (causedIssueBy == BiomeType.Hell ? "hell" : "jungle"));

                AltLibrary.Instance.Logger.Info($"Found error in {inside}! Fixing...");

                switch (inside)
                {
                    case "evil":
                        {
                            switch (causedIssueBy)
                            {
                                case BiomeType.Hallow:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case BiomeType.Hell:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hell;
                                        WorldBiomeManager.worldHell = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case BiomeType.Jungle:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = jungle;
                                        WorldBiomeManager.worldJungle = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                            }
                            break;
                        }
                    case "hallow":
                        {
                            switch (causedIssueBy)
                            {
                                case BiomeType.Evil:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case BiomeType.Hell:
                                    {
                                        string evil2 = hell;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldHell = hallow2;
                                        break;
                                    }
                                case BiomeType.Jungle:
                                    {
                                        string evil2 = jungle;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldJungle = hallow2;
                                        break;
                                    }
                            }
                            break;
                        }
                    case "hell":
                        {
                            switch (causedIssueBy)
                            {
                                case BiomeType.Evil:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hell;
                                        WorldBiomeManager.worldHell = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case BiomeType.Hallow:
                                    {
                                        string evil2 = hell;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldHell = hallow2;
                                        break;
                                    }
                                case BiomeType.Jungle:
                                    {
                                        string evil2 = jungle;
                                        string hallow2 = hell;
                                        WorldBiomeManager.worldHell = evil2;
                                        WorldBiomeManager.worldJungle = hallow2;
                                        break;
                                    }
                            }
                            break;
                        }
                }

                AltLibrary.Instance.Logger.Info($"Error fixed! Report it if it's wrong.");
            }
        }
    }
}
