using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace CrossWorlds
{
	public class CrossWorlds : Mod
	{
		public CrossWorlds()
		{
			
		}

		public override void PreSaveAndQuit()
		{
			var syncWorld = ModContent.GetInstance<SyncWorld>();
			base.PreSaveAndQuit();
			syncWorld.PreSaveAndQuit();
		}
	}
}