using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace BossesAsNPCs.Items.Vanity
{
    public class VanityBase : ModItem
    {
		public override bool IsLoadingEnabled(Mod mod) => GetType() != typeof(VanityBase);
		public override string Texture => Type == ModContent.ItemType<VanityBase>() ? null : (GetType().Namespace + "." + Name).Replace('.', '/');
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}
	}
}