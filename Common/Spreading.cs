using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AltLibrary.Common.AltBiomes;
using System;

namespace AltLibrary.Common
{
    internal class Spreading : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
            // REMOVE THIS 'RETURN' WHEN FIXING SPREADING OR/AND FIXED
            // REMOVE THIS 'RETURN' WHEN FIXING SPREADING OR/AND FIXED
            // REMOVE THIS 'RETURN' WHEN FIXING SPREADING OR/AND FIXED
            // REMOVE THIS 'RETURN' WHEN FIXING SPREADING OR/AND FIXED
            // REMOVE THIS 'RETURN' WHEN FIXING SPREADING OR/AND FIXED
            return;

            if (Main.tile[i, j].IsActuated)
            {
                return;
            }
            bool isSpreadingTile = false;
            bool isOreGrowingTile = false;
            bool isJungleSpreadingOre = false;
            bool isGrass = false;
            AltBiome biomeToSpread = null;
            foreach (AltBiome biome in AltLibrary.biomes) 
            {
                if (type == biome.BiomeGrass) isGrass = true;
                if ((biome.BiomeType == BiomeType.Evil || biome.BiomeType == BiomeType.Hallow) && biome.SpreadingTiles.Contains(type))
                {
                    isSpreadingTile = true;
                    biomeToSpread = biome;
                    break;
                }
                if (biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass == type)
                {
                    if (type == biome.BiomeGrass)
                    {
                        isOreGrowingTile = true;
                        biomeToSpread = biome;
                        break;
                    } 
                    else if (type == biome.BiomeOre)
                    {
                        isJungleSpreadingOre = true;
                        biomeToSpread = biome;
                        break;
                    }
                    
                }
            }
            if (isSpreadingTile && Main.hardMode && WorldGen.AllowedToSpreadInfections && !(NPC.downedPlantBoss && !WorldGen.genRand.NextBool(2)))
            {
                var flag = true;
                while (flag)
                {
                    flag = false;
                    var xdif = WorldGen.genRand.Next(-3, 4);
                    var ydif = WorldGen.genRand.Next(-3, 4);
                    var targetX = i + xdif;
                    var targetY = j + ydif;

                    if (WorldGen.InWorld(targetX, targetY))
                    {
                        var target = Main.tile[targetX, targetY];
                        var canSpread = target.HasUnactuatedTile;
                        var oldTileType = target.TileType;
                        ushort newTileType = 0;

                        if (biomeToSpread.BiomeType == BiomeType.Evil && (WorldGen.CountNearBlocksTypes(targetX, targetY, 2, 1, TileID.Sunflower) > 0) && NearbyEvilSlowingOres(targetX, targetY)) continue;
                        // TODO: IL edit vanilla Evils to also check for Chlorophyte alts in addition to Chlorophyte 
                        if (canSpread)
                        {
                            if (biomeToSpread.SpecialConversion.ContainsKey(oldTileType)) newTileType = (ushort)biomeToSpread.SpecialConversion[oldTileType];
                            else if (biomeToSpread.MudToDirt)
                            {
                                if (oldTileType == TileID.Mud) newTileType = TileID.Dirt;
                            }
                            else
                            {
                                if (oldTileType == TileID.IceBlock) newTileType = (ushort)biomeToSpread.BiomeIce; // if I knew of a better way to do this, believe me, I would use it
                                if (oldTileType == TileID.HardenedSand) newTileType = (ushort)biomeToSpread.BiomeHardenedSand; // hey jackass use a switch case its literally just ints
                                if (oldTileType == TileID.Sandstone) newTileType = (ushort)biomeToSpread.BiomeSandstone;
                                if (oldTileType == TileID.Sand) newTileType = (ushort)biomeToSpread.BiomeSand;
                                if (oldTileType == TileID.Stone) newTileType = (ushort)biomeToSpread.BiomeStone;
                                if (oldTileType == TileID.Grass) newTileType = (ushort)biomeToSpread.BiomeGrass;
                                if (oldTileType == TileID.GolfGrass)
                                {
                                    if (biomeToSpread.BiomeMowedGrass != null) newTileType = (ushort)biomeToSpread.BiomeMowedGrass;
                                    else newTileType = (ushort)biomeToSpread.BiomeGrass;
                                }

                                if (AltLibrary.jungleGrass.Contains(oldTileType) && biomeToSpread.BiomeJungleGrass != null) newTileType = (ushort)biomeToSpread.BiomeJungleGrass;
                                if (AltLibrary.jungleThorns.Contains(oldTileType) && biomeToSpread.BiomeThornBush != null) newTileType = (ushort)biomeToSpread.BiomeThornBush;
                            }


                            if (newTileType != 0 && newTileType != oldTileType)
                            {
                                if (WorldGen.genRand.NextBool(2)) flag = true;
                                target.TileType = newTileType;
                                WorldGen.SquareTileFrame(targetX, targetY);
                                if (Main.netMode == NetmodeID.Server) NetMessage.SendTileSquare(-1, targetX, targetY);
                            }
                        }
                    }
                }
            }
            
            if (isGrass)
            {
                if (biomeToSpread.BiomeType == BiomeType.Jungle)
                {
                    SpreadGrass(i, j, TileID.Mud, (int)biomeToSpread.BiomeJungleGrass);
                }
                else
                {
                    if  (j < (Main.worldSurface + Main.rockLayer) / 2)
                    {
                        SpreadGrass(i, j, TileID.Dirt, (int)biomeToSpread.BiomeGrass);
                    }
                    if (WorldGen.AllowedToSpreadInfections) SpreadGrass(i, j, TileID.Grass, (int)biomeToSpread.BiomeGrass);
                }
            }

            if (isOreGrowingTile || isJungleSpreadingOre)
            {  
                if (j > (Main.worldSurface + Main.rockLayer) / 2.0)
                {
                    if (isOreGrowingTile && WorldGen.genRand.NextBool(300))
                    {
                        var xdif = WorldGen.genRand.Next(-10, 11);
                        var ydif = WorldGen.genRand.Next(-10, 11);
                        var targetX = i + xdif;
                        var targetY = j + ydif;
                        var target = Main.tile[targetX, targetY];

                        if (WorldGen.InWorld(targetX, targetY) && Main.tile[targetX, targetY].TileType == TileID.Mud)
                        {
                            if (Main.tile[targetX, targetY - 1].IsActuated || 
                                (Main.tile[targetX, targetY - 1].TileType != TileID.Trees && Main.tile[targetX, targetY - 1].TileType != TileID.LifeFruit 
                                && !AltLibrary.planteraBulbs.Contains(Main.tile[targetX, targetY - 1].TileType))) // TODO: recreate vanilla WorldGen.Chlorophyte method for alts
                            {
                                target.TileType = 211;
                                WorldGen.SquareTileFrame(targetX, targetY);
                                if (Main.netMode == NetmodeID.Server) NetMessage.SendTileSquare(-1, targetX, targetY);
                            }
                        }
                    }
                    if (isJungleSpreadingOre && !WorldGen.genRand.NextBool(3))
                    {
                        var targX = i;
                        var targY = j;
                        var random = WorldGen.genRand.Next(4);
                        if (random == 0)
                        {
                            targX++;
                        }
                        if (random == 1)
                        {
                            targX--;
                        }
                        if (random == 2)
                        {
                            targY++;
                        }
                        if (random == 3)
                        {
                            targY--;
                        }
                        var target = Main.tile[targX, targY];
                        if (WorldGen.InWorld(targX, targY, 2) && !target.IsActuated && (target.TileType == TileID.Mud || target.TileType == biomeToSpread.BiomeGrass))
                        {
                            target.TileType = 211;
                            WorldGen.SquareTileFrame(targX, targY);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendTileSquare(-1, targX, targY);
                            }
                        }
                        bool flag = true;
                        while (flag)
                        {
                            flag = false;
                            targX = i + Main.rand.Next(-6, 7);
                            targY = j + Main.rand.Next(-6, 7);
                            target = Main.tile[targX, targY];
                            if (!WorldGen.InWorld(targX, targY, 2) || target.IsActuated)
                            {
                                continue;
                            }
                            if (TileID.Sets.Conversion.Grass[target.TileType] && !AltLibrary.jungleGrass.Contains(target.TileType))
                            {
                                target.TileType = (ushort)biomeToSpread.BiomeGrass;
                                WorldGen.SquareTileFrame(targX, targY);
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendTileSquare(-1, targX, targY);
                                }
                                flag = true;
                            }
                            else if (target.TileType == TileID.Dirt)
                            {
                                target.TileType = TileID.Mud;
                                WorldGen.SquareTileFrame(targX, targY);
                                if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.SendTileSquare(-1, targX, targY);
                                }
                                flag = true;
                            }
                        }
                    }
                }
            }
        }

        private static bool NearbyEvilSlowingOres(int i, int j)
        {
            float count = 0f;
            int worldEdgeDistance = 5;
            if (i <= worldEdgeDistance + 5 || i >= Main.maxTilesX - worldEdgeDistance - 5)
            {
                return false;
            }
            if (j <= worldEdgeDistance + 5 || j >= Main.maxTilesY - worldEdgeDistance - 5)
            {
                return false;
            }
            for (int k = i - worldEdgeDistance; k <= i + worldEdgeDistance; k++)
            {
                for (int l = j - worldEdgeDistance; l <= j + worldEdgeDistance; l++)
                {
                    if (!Main.tile[k, l].IsActuated && AltLibrary.evilStoppingOres.Contains(Main.tile[k, l].TileType))
                    {
                        count += 1f;
                        if (count >= 4f)
                        {
                            return true;
                        }
                    }
                }
            }
            if (count > 0f && (float)WorldGen.genRand.Next(5) < count)
            {
                return true;
            }
            return false;
        }

        public static void SpreadGrass(int i, int j, int dirt, int grass) // I made this grass spreading code years ago, and its probably not great but it works so /shrug 
        {
            int left = i - 1; // defining the bounds of the 3x3 space which will be checked for dirt
            int right = i + 1;
            int top = j - 1;
            int bottom = j + 1;
            if (left < 0) // making sure the coords to detect obstacles to are within the bounds of the world
            {
                left = 0;
            }
            if (right > Main.maxTilesX)
            {
                right = Main.maxTilesX;
            }
            if (top < 0)
            {
                top = 0;
            }
            if (bottom > Main.maxTilesY)
            {
                bottom = Main.maxTilesY;
            }
            for (int k = left; k <= right; k++)
            {
                for (int l = top; l <= bottom; l++)
                {
                    SpreadGrassPhase2(k, l, dirt, grass);
                }
            }
        }
        public static void SpreadGrassPhase2(int i, int j, int dirt, int grass)
        {
            int left = i - 1; // defining the bounds of the 3x3 space which will be checked for lava and air
            int right = i + 1;
            int top = j - 1;
            int bottom = j + 1;

            if (left < 0) // making sure the coords to detect obstacles are within the bounds of the world
            {
                left = 0;
            }
            if (right > Main.maxTilesX)
            {
                right = Main.maxTilesX;
            }
            if (top < 0)
            {
                top = 0;
            }
            if (bottom > Main.maxTilesY)
            {
                bottom = Main.maxTilesY;
            }

            bool haltSpread = true; // a boolean that determines if the grass can spread
            for (int k = left; k <= right; k++)
            {
                for (int l = top; l <= bottom; l++)
                {
                    if (!Main.tile[k, l].IsActuated || !Main.tileSolid[Main.tile[k, l].TileType]) // checking that at least one adjacent tile is air
                    {
                        haltSpread = false;
                    }
                    if (Main.tile[k, l].LiquidType == LiquidID.Lava && Main.tile[k, l].LiquidAmount > 0) // checking that none of the adjacent tiles contain lava
                    {
                        haltSpread = true;
                        break; // stops checking adjacent blocks if even one is lava
                    }
                } // effectively, what was just done is this; grass is halted by default, but if even one adjacent tile has air (or furniture, etc) then
            }     // grass is no longer halted. the lava check then comes after the air check so that if there *is* any lava touching the block, the grass will not spread
            if (!haltSpread && Main.tile[i, j].TileType == dirt) // checking if the grass is allowed to spread and if the block in question is dirt
            {                                                // add && (grass != <ID of evil grass> || Main.tile[i, j - 1].type != 27) to disallow spreading when a sunflower is on top
                Main.tile[i, j].TileType = (ushort)grass;
            }
        }
    }
}
