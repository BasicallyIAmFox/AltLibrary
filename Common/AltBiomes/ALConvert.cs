using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    [Obsolete("WIP, not recomended to use right now.")]
    public static class ALConvert
    {
        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="fullName"></param>
        public static void SimulateThrownWater(Projectile projectile, string fullName)
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert(fullName, i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="mod"></param>
        /// <param name="name"></param>
        public static void SimulateThrownWater(Projectile projectile, Mod mod, string name)
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert(mod, name, i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projectile"></param>
        public static void SimulateThrownWater<T>(Projectile projectile) where T : AltBiome
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert<T>(i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="biome"></param>
        public static void SimulateThrownWater(Projectile projectile, AltBiome biome)
        {
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert(biome, i, j, 4);
        }

        /// <summary>
        /// Makes throwing water converting effect.
        /// </summary>
        /// <typeparam name="TProj"></typeparam>
        /// <typeparam name="TBiome"></typeparam>
        public static void SimulateThrownWater<TProj, TBiome>() where TBiome : AltBiome where TProj : Projectile
        {
            Projectile projectile = ContentInstance<TProj>.Instance;
            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
            Convert<TBiome>(i, j, 4);
        }

        /// <summary>
        /// Makes Solution AI for your custom solutions.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="mod"></param>
        /// <param name="name"></param>
        /// <param name="dustType"></param>
        public static void SimulateSolution(ref Projectile projectile, Mod mod, string name, int dustType) => SimulateSolution(ref projectile, AltLibrary.Biomes.Find(x => x.Mod == mod && x.Name == name), dustType);

        /// <summary>
        /// Makes Solution AI for your custom solutions.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="fullname"></param>
        /// <param name="dustType"></param>
        public static void SimulateSolution(ref Projectile projectile, string fullname, int dustType) => SimulateSolution(ref projectile, AltLibrary.Biomes.Find(x => x.FullName == fullname), dustType);

        /// <summary>
        /// Makes Solution AI for your custom solutions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="projectile"></param>
        /// <param name="dustType"></param>
        public static void SimulateSolution<T>(ref Projectile projectile, int dustType) where T : AltBiome => SimulateSolution(ref projectile, ContentInstance<T>.Instance, dustType);

        /// <summary>
        /// Makes Solution AI for your custom solutions.
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="biome"></param>
        /// <param name="dustType"></param>
        public static void SimulateSolution(ref Projectile projectile, AltBiome biome, int dustType)
        {
            if (projectile.owner == Main.myPlayer)
                Convert(biome, (int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);

            if (projectile.timeLeft > 133)
                projectile.timeLeft = 133;

            if (projectile.ai[0] > 7f)
            {
                float dustScale = 1f;

                if (projectile.ai[0] == 8f)
                    dustScale = 0.2f;
                else if (projectile.ai[0] == 9f)
                    dustScale = 0.4f;
                else if (projectile.ai[0] == 10f)
                    dustScale = 0.6f;
                else if (projectile.ai[0] == 11f)
                    dustScale = 0.8f;

                projectile.ai[0] += 1f;

                for (int i = 0; i < 1; i++)
                {
                    int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.scale *= 1.75f;
                    dust.velocity.X *= 2f;
                    dust.velocity.Y *= 2f;
                    dust.scale *= dustScale;
                }
            }
            else
                projectile.ai[0] += 1f;

            projectile.rotation += 0.3f * projectile.direction;
        }

        public static void Convert(string fullName, int i, int j, int size = 4) => Convert(AltLibrary.Biomes.Find(x => x.FullName == fullName), i, j, size);

        public static void Convert(Mod mod, string name, int i, int j, int size = 4) => Convert(AltLibrary.Biomes.Find(x => x.Mod == mod && x.Name == name), i, j, size);

        public static void Convert<T>(int i, int j, int size = 4) where T : AltBiome => Convert(ContentInstance<T>.Instance, i, j, size);

        public static void Convert(AltBiome biome, int i, int j, int size = 4)
        {
            if (biome is null)
                throw new ArgumentNullException(nameof(biome), "Can't be null!");

            for (int k = i - size; k <= i + size; k++)
            {
                for (int l = j - size; l <= j + size; l++)
                {
                    if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6)
                    {
                        int type = Main.tile[k, l].TileType;
                        int wall = Main.tile[k, l].WallType;

                        // Key = original (int)
                        // Value = 
                        Dictionary<int, List<int>> keys = new()
                        {
                            { TileID.Stone, CreateAndGet(nameof(AltBiome.BiomeStone), biome) },
                            { TileID.Grass, CreateAndGet(nameof(AltBiome.BiomeGrass), biome) },
                            { TileID.IceBlock, CreateAndGet(nameof(AltBiome.BiomeIce), biome) },
                            { TileID.Sand, CreateAndGet(nameof(AltBiome.BiomeSand), biome) },
                            { TileID.HardenedSand, CreateAndGet(nameof(AltBiome.BiomeHardenedSand), biome) },
                            { TileID.Sandstone, CreateAndGet(nameof(AltBiome.BiomeSandstone), biome) },
                            { TileID.CorruptThorns, CreateAndGet(nameof(AltBiome.BiomeThornBush), biome) },
                        };

                        if ((Main.tileMoss[type] || TileID.Sets.Conversion.Stone[type]) && type != biome.BiomeStone.GetValueOrDefault() && biome.BiomeStone.HasValue)
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeStone.Value;
                            WorldGen.SquareWallFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l);
                        }
                        foreach (KeyValuePair<int, List<int>> key in keys)
                        {
                            if (type == key.Key)
                            {
                                Main.tile[k, l].TileType = (ushort)biome.BiomeStone.Value;
                                WorldGen.SquareWallFrame(k, l);
                                NetMessage.SendTileSquare(-1, k, l);
                            }
                        }

                        /*if ((Main.tileMoss[type] || TileID.Sets.Conversion.Stone[type]) && type != biome.BiomeStone.GetValueOrDefault() || keys.ContainsKey(type) && biome.BiomeStone.HasValue)
                        {
                            keys.TryGetValue(type, out List<int> a);
                            Main.tile[k, l].TileType = keys.TryGetValue();
                            WorldGen.SquareWallFrame(k, l);
                            NetMessage.SendTileSquare(-1, k, l);
                        }
                        else if (TileID.Sets.Conversion.Grass[type] && type != biome.BiomeGrass.GetValueOrDefault() || grass.Contains(type))
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeGrass.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (TileID.Sets.Conversion.Ice[type] && type != biome.BiomeIce.GetValueOrDefault() || ice.Contains(type))
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeIce.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (TileID.Sets.Conversion.Sand[type] && type != biome.BiomeSand.GetValueOrDefault() || sand.Contains(type))
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeSand.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (TileID.Sets.Conversion.HardenedSand[type] && type != biome.BiomeHardenedSand.GetValueOrDefault() || hardSand.Contains(type))
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeHardenedSand.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (TileID.Sets.Conversion.Sandstone[type] && type != biome.BiomeSandstone.GetValueOrDefault() || sandstone.Contains(type))
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeSandstone.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        else if (TileID.Sets.Conversion.Thorn[type] && type != biome.BiomeSandstone.GetValueOrDefault() || thorns.Contains(type))
                        {
                            Main.tile[k, l].TileType = (ushort)biome.BiomeThornBush.Value;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }
                        if (biome.MudToDirt && type == TileID.Mud && (Main.tile[k - 1, l].TileType == biome.BiomeGrass || Main.tile[k + 1, l].TileType == biome.BiomeGrass || Main.tile[k, l - 1].TileType == biome.BiomeGrass || Main.tile[k, l + 1].TileType == biome.BiomeGrass))
                        {
                            Main.tile[k, l].TileType = TileID.Dirt;
                            WorldGen.SquareTileFrame(k, l, true);
                            NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
                        }*/

                        foreach (KeyValuePair<int, int> pair in biome.SpecialConversion)
                        {
                            if (type == pair.Key)
                            {
                                Main.tile[k, l].TileType = (ushort)pair.Value;
                            }
                        }
                        foreach (KeyValuePair<ushort, ushort> pair in biome.WallContext.wallsReplacement)
                        {
                            if (wall == pair.Key)
                            {
                                Main.tile[k, l].WallType = pair.Value;
                            }
                        }
                    }
                }
            }
        }

        private static List<int> CreateAndGet(string name, AltBiome biome)
        {
            List<int> var = new();
            foreach (AltBiome altBiome in AltLibrary.Biomes)
            {
                int? @int = (int?)typeof(AltBiome).GetField(name).GetValue(altBiome);
                if (altBiome.FullName != biome.FullName && @int.HasValue)
                {
                    var.Add(@int.Value);
                }
            }
            return var;
        }
    }
}
