using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AltLibrary.Common.AltBiomes;
using System;

namespace AltLibrary.Common
{
    internal class Spreading : GlobalTile
    {
        [Obsolete]
        public override void RandomUpdate(int i, int j, int type)
        {
            bool isSpreadingTile = false;
            AltBiome toSpread;
            foreach (AltBiome biome in AltLibrary.biomes) 
            {
                if ((biome.BiomeType == BiomeType.Evil || biome.BiomeType == BiomeType.Hallow) && biome.SpreadingTiles.Contains(type))
                {
                    isSpreadingTile = true;
                    toSpread = biome;
                    break;
                }
            }
            if (isSpreadingTile)
            {
                var xdif = Main.rand.Next(-3, 3);
                var ydif = Main.rand.Next(-3, 3);
                var targetX = i + xdif;
                var targetY = j + ydif;
                var target = Main.tile[targetX, targetY];

                if ((i + xdif > 0 && j + ydif > 0) && (i + xdif < Main.maxTilesX && j + ydif < Main.maxTilesY))
                {
                    int newTileType;
                    //if (TileID.Sets.Conversion.Stone)
                    //{

                    //}
                }
            }
            //if (type == TileID.Gold)
            //{
            //    var xdif = Main.rand.Next(-3, 3);
            //    var ydif = Main.rand.Next(-3, 3);
            //    var target = Main.tile[i + xdif, j + ydif];
            
        }
    }
    }
}
