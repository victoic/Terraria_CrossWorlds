using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace CrossWorlds
{
    public class SyncWorld : ModWorld
    {
        bool IsTimeSetted;
        bool LastSyncThisWorld;
        List<int> WorldIDs;
        BinaryWriter bw;
        BinaryReader br;
        string WorldIDsPath;
        public override void Initialize()
        {
            IsTimeSetted = false;
            this.WorldIDs = new List<int>();
            string WorldIDsFilename = mod.Name + "_syncWorlds.dat";
            this.WorldIDsPath = System.IO.Path.Combine(Terraria.ModLoader.Config.ConfigManager.ModConfigPath, WorldIDsFilename);
        }

        public override void PostUpdate()
        {
            base.PostUpdate();
            var config = GetInstance<CrossWorldsConfig>();
            if (!IsTimeSetted)
            {
                IsTimeSetted = true;
                this.ReadWorldIDs();
                if (this.CheckIfWorldSync())
                {
                    LastSyncThisWorld = config.SyncThisWorld = true;
                    this.SyncTimeAndMoon();
                } else
                {
                    LastSyncThisWorld = config.SyncThisWorld = false;
                }
            }
            if (config.SyncThisWorld != LastSyncThisWorld)
            {
                LastSyncThisWorld = config.SyncThisWorld;
                if (config.SyncThisWorld)
                {
                    this.AddWorldID(Main.worldID);
                    this.WriteWorldIDs();
                    this.SyncTimeAndMoon();
                }
                else
                {
                    this.RemoveWorldID(Main.worldID);
                    this.WriteWorldIDs();
                    this.SaveTimeAndMoon();
                }
            }
        }

        public override bool Autoload(ref string name)
        {
            return base.Autoload(ref name);
        }

        public void PreSaveAndQuit()
        {
            //var config = GetInstance<CrossWorldsConfig>();
            if (this.CheckIfWorldSync() && (Main.netMode != 1))
            {
                this.WriteWorldIDs();
                this.SaveTimeAndMoon();
            }
        }

        public void SyncTimeAndMoon()
        {
            string filename = mod.Name + ".dat";
            string fullpath = System.IO.Path.Combine(Terraria.ModLoader.Config.ConfigManager.ModConfigPath, filename);
            try
            {
                br = new BinaryReader(new FileStream(fullpath, FileMode.OpenOrCreate));
                if (br.BaseStream.Position != br.BaseStream.Length)
                {
                    Main.time = br.ReadDouble();
                    Main.moonPhase = br.ReadInt32();
                    Main.dayTime = br.ReadBoolean();
                }
                br.Close();
            }
            catch (IOException e)
            {

            }
        }

        public void SaveTimeAndMoon()
        {
            string filename = mod.Name + ".dat";
            string fullpath = System.IO.Path.Combine(Terraria.ModLoader.Config.ConfigManager.ModConfigPath, filename);
            try
            {
                bw = new BinaryWriter(new FileStream(fullpath, FileMode.OpenOrCreate));
                bw.Write(Main.time);
                bw.Write(Main.moonPhase);
                bw.Write(Main.dayTime);
                bw.Close();
            }
            catch (IOException e)
            {

            }
        }

        public void ReadWorldIDs()
        {
            if (File.Exists(this.WorldIDsPath)){
                using (StreamReader reader = new StreamReader(this.WorldIDsPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        int newID = int.Parse(line);
                        this.WorldIDs.Add(newID);
                    }
                }
            } else
            {
                this.WriteWorldIDs();
            }
        }

        public void WriteWorldIDs()
        {
            using (StreamWriter writer = new StreamWriter(this.WorldIDsPath, false))
            {
                foreach (int ID in this.WorldIDs)
                {
                    writer.WriteLine(ID);
                }
            }
        }

        public void AddWorldID(int ID)
        {
            if (WorldIDs.Contains(ID) == false)
            {
                WorldIDs.Add(ID);
            }
        }

        public void RemoveWorldID(int ID)
        {
            if (WorldIDs.Contains(ID) == true)
            {
                WorldIDs.Remove(ID);
            }
        }

        public bool CheckIfWorldSync()
        {
            if (this.WorldIDs.Contains(Main.worldID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
