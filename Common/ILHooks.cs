﻿using AltLibrary.Common;
using AltLibrary.Common.Hooks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Core
{
    internal class ILHooks
    {
        public static Asset<Texture2D> EmptyAsset => ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Outer Empty");

        public static void OnInitialize()
        {
            On.Terraria.Main.EraseWorld += Main_EraseWorld;
            WorldIcons.Init();
            OuterVisual.Init();
            EvenMoreWorldGen.Init();
            UnderworldVisual.Init();
            UIWorldCreationEdits.Init();
            HardmodeWorldGen.Init();
            TwinsRules.Init();
        }

        private static void Main_EraseWorld(On.Terraria.Main.orig_EraseWorld orig, int i)
        {
            Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
            var path = Path.ChangeExtension(Main.WorldList[i].Path, ".twld");
            if (tempDict.ContainsKey(path))
            {
                tempDict.Remove(path);
                AltLibraryConfig.Config.SetWorldData(tempDict);
                AltLibraryConfig.Save(AltLibraryConfig.Config);
            }
            orig(i);
        }
    }
}
