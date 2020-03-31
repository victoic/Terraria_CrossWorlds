using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;

namespace CrossWorlds
{
    public class CrossWorldsConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [DefaultValue(false)]
        [Label("Sync this World?")]
        [Tooltip("Set to true to synchronize time and moon phase of this world.")]
        public bool SyncThisWorld;
        public override void OnChanged()
        {
            //Main.time = WorldTime;
            //Main.dayTime = dayTime;
        }
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return false;
        }
        
        public string SerializeConfig(List<string> WorldsList)
        {
            var Json = "{\n";
            Json += "\t\"WorldsList\": [\n";
            foreach (string world in WorldsList)
            {
                Json += "\t\t\"" + world + "\",\n";
            }
            Json += "\t],\n";
            Json += "}";
            return Json;
        }

        public void SendJSON(string path, List<string> WorldsList)
        {
            var jsonConfig = SerializeConfig(WorldsList);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(jsonConfig);
            }
        }
    }
}