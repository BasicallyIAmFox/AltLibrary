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
            int causedIssueBy = -1;
            if (evil != "" && ModContent.Find<AltBiome>(evil).BiomeType != BiomeType.Evil)
            {
                List<string> checks = new() { hallow, hell, jungle };
                foreach (string check in checks)
                {
                    if (check != "" && ModContent.Find<AltBiome>(check).BiomeType == BiomeType.Evil)
                    {
                        causedIssueBy = (int)AltLibrary.biomes.First(x => x.FullName == check).BiomeType;
                        break;
                    }
                }
            }
            if (hallow != "" && ModContent.Find<AltBiome>(hallow).BiomeType != BiomeType.Hallow)
            {
                List<string> checks = new() { evil, hell, jungle };
                foreach (string check in checks)
                {
                    if (check != "" && ModContent.Find<AltBiome>(check).BiomeType == BiomeType.Hallow)
                    {
                        causedIssueBy = (int)AltLibrary.biomes.First(x => x.FullName == check).BiomeType;
                        break;
                    }
                }
            }
            if (hell != "" && ModContent.Find<AltBiome>(hell).BiomeType != BiomeType.Hell)
            {
                List<string> checks = new() { evil, hallow, jungle };
                foreach (string check in checks)
                {
                    if (check != "" && ModContent.Find<AltBiome>(check).BiomeType == BiomeType.Hell)
                    {
                        causedIssueBy = (int)AltLibrary.biomes.First(x => x.FullName == check).BiomeType;
                        break;
                    }
                }
            }
            if (jungle != "" && ModContent.Find<AltBiome>(jungle).BiomeType != BiomeType.Jungle)
            {
                List<string> checks = new() { evil, hallow, hell };
                foreach (string check in checks)
                {
                    if (check != "" && ModContent.Find<AltBiome>(check).BiomeType == BiomeType.Jungle)
                    {
                        causedIssueBy = (int)AltLibrary.biomes.First(x => x.FullName == check).BiomeType;
                        break;
                    }
                }
            }
            if (causedIssueBy == -1)
            {
                AltLibrary.Instance.Logger.Info("Checking issues done! Enjoy game!");
            }
            else
            {
                string inside = causedIssueBy == 0 ? "evil" : (causedIssueBy == 1 ? "hallow" : (causedIssueBy == 2 ? "hell" : "jungle"));

                AltLibrary.Instance.Logger.Info($"Found error in {inside}! Fixing...");

                switch (inside)
                {
                    case "evil":
                        {
                            switch (causedIssueBy)
                            {
                                case 1:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case 2:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hell;
                                        WorldBiomeManager.worldHell = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case 3:
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
                                case 0:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case 2:
                                    {
                                        string evil2 = hell;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldHell = hallow2;
                                        break;
                                    }
                                case 3:
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
                                case 0:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = hell;
                                        WorldBiomeManager.worldHell = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case 1:
                                    {
                                        string evil2 = hell;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldHell = hallow2;
                                        break;
                                    }
                                case 3:
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
                    case "jungle":
                        {
                            switch (causedIssueBy)
                            {
                                case 0:
                                    {
                                        string evil2 = evil;
                                        string hallow2 = jungle;
                                        WorldBiomeManager.worldJungle = evil2;
                                        WorldBiomeManager.worldEvil = hallow2;
                                        break;
                                    }
                                case 1:
                                    {
                                        string evil2 = jungle;
                                        string hallow2 = hallow;
                                        WorldBiomeManager.worldHallow = evil2;
                                        WorldBiomeManager.worldJungle = hallow2;
                                        break;
                                    }
                                case 2:
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
