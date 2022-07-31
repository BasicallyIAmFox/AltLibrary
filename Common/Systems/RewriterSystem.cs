using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Systems
{
	internal class RewriterSystem : ModSystem
	{
		public override void OnWorldLoad()
		{
			if (WorldBiomeManager.WorldEvil == null) WorldBiomeManager.WorldEvil = "";
			if (WorldBiomeManager.WorldHallow == null) WorldBiomeManager.WorldHallow = "";
			if (WorldBiomeManager.WorldHell == null) WorldBiomeManager.WorldHell = "";
			if (WorldBiomeManager.WorldJungle == null) WorldBiomeManager.WorldJungle = "";
			if (WorldBiomeManager.drunkEvil == null) WorldBiomeManager.drunkEvil = "";

			string evil = WorldBiomeManager.WorldEvil;
			string hallow = WorldBiomeManager.WorldHallow;
			string hell = WorldBiomeManager.WorldHell;
			string jungle = WorldBiomeManager.WorldJungle;
			int[] causedIssueBy = new int[4] { -1, -1, -1, -1 };
			if (evil != "" && ModContent.Find<AltBiome>(evil).BiomeType != BiomeType.Evil)
			{
				List<string> checks = new() { hallow, hell, jungle };
				foreach (string check in checks)
				{
					if (check != "" && ModContent.Find<AltBiome>(check).BiomeType == BiomeType.Evil)
					{
						causedIssueBy[0] = (int)AltLibrary.Biomes.First(x => x.FullName == check).BiomeType;
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
						causedIssueBy[1] = (int)AltLibrary.Biomes.First(x => x.FullName == check).BiomeType;
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
						causedIssueBy[2] = (int)AltLibrary.Biomes.First(x => x.FullName == check).BiomeType;
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
						causedIssueBy[3] = (int)AltLibrary.Biomes.First(x => x.FullName == check).BiomeType;
						break;
					}
				}
			}
			for (int i = 0; i < 4; i++)
			{
				if (causedIssueBy[i] == -1)
				{
					AltLibrary.Instance.Logger.Info("Checking issues done! Enjoy game!");
				}
				else
				{
					string inside = causedIssueBy[i] == 0 ? "evil" : (causedIssueBy[i] == 1 ? "hallow" : (causedIssueBy[i] == 2 ? "hell" : "jungle"));

					AltLibrary.Instance.Logger.Info($"Found error in {inside}! Fixing...");

					switch (inside)
					{
						case "evil":
							{
								switch (causedIssueBy[i])
								{
									case 1:
										{
											string evil2 = evil;
											string hallow2 = hallow;
											WorldBiomeManager.WorldHallow = evil2;
											WorldBiomeManager.WorldEvil = hallow2;
											break;
										}
									case 2:
										{
											string evil2 = evil;
											string hallow2 = hell;
											WorldBiomeManager.WorldHell = evil2;
											WorldBiomeManager.WorldEvil = hallow2;
											break;
										}
									case 3:
										{
											string evil2 = evil;
											string hallow2 = jungle;
											WorldBiomeManager.WorldJungle = evil2;
											WorldBiomeManager.WorldEvil = hallow2;
											break;
										}
								}
								break;
							}
						case "hallow":
							{
								switch (causedIssueBy[i])
								{
									case 0:
										{
											string evil2 = evil;
											string hallow2 = hallow;
											WorldBiomeManager.WorldHallow = evil2;
											WorldBiomeManager.WorldEvil = hallow2;
											break;
										}
									case 2:
										{
											string evil2 = hell;
											string hallow2 = hallow;
											WorldBiomeManager.WorldHallow = evil2;
											WorldBiomeManager.WorldHell = hallow2;
											break;
										}
									case 3:
										{
											string evil2 = jungle;
											string hallow2 = hallow;
											WorldBiomeManager.WorldHallow = evil2;
											WorldBiomeManager.WorldJungle = hallow2;
											break;
										}
								}
								break;
							}
						case "hell":
							{
								switch (causedIssueBy[i])
								{
									case 0:
										{
											string evil2 = evil;
											string hallow2 = hell;
											WorldBiomeManager.WorldHell = evil2;
											WorldBiomeManager.WorldEvil = hallow2;
											break;
										}
									case 1:
										{
											string evil2 = hell;
											string hallow2 = hallow;
											WorldBiomeManager.WorldHallow = evil2;
											WorldBiomeManager.WorldHell = hallow2;
											break;
										}
									case 3:
										{
											string evil2 = jungle;
											string hallow2 = hell;
											WorldBiomeManager.WorldHell = evil2;
											WorldBiomeManager.WorldJungle = hallow2;
											break;
										}
								}
								break;
							}
						case "jungle":
							{
								switch (causedIssueBy[i])
								{
									case 0:
										{
											string evil2 = evil;
											string hallow2 = jungle;
											WorldBiomeManager.WorldJungle = evil2;
											WorldBiomeManager.WorldEvil = hallow2;
											break;
										}
									case 1:
										{
											string evil2 = jungle;
											string hallow2 = hallow;
											WorldBiomeManager.WorldHallow = evil2;
											WorldBiomeManager.WorldJungle = hallow2;
											break;
										}
									case 2:
										{
											string evil2 = jungle;
											string hallow2 = hell;
											WorldBiomeManager.WorldHell = evil2;
											WorldBiomeManager.WorldJungle = hallow2;
											break;
										}
								}
								break;
							}
					}

					AltLibrary.Instance.Logger.Info($"Error fixed! Report it if it's wrong.");
				}
			}

			if (WorldBiomeManager.WorldEvil != "" && WorldGen.crimson)
			{
				WorldGen.crimson = false;
			}
		}
	}
}
