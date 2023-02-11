using BossesAsNPCs.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossesAsNPCs
{
	public class BossesAsNPCsRecipes : ModSystem
	{
		public override void AddRecipes()
		{
			if (ModContent.GetInstance<BossesAsNPCsConfigServer>().CatchNPCs)
			{
				Recipe.Create(ItemID.SlimeBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtKingSlime>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtEyeOfCthulhu>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.LesionBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtEaterOfWorlds>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtBrainOfCthulhu>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.Hive, 25)
					.AddIngredient(ModContent.ItemType<CaughtQueenBee>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.BoneBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtSkeletron>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtDeerclops>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtWallOfFlesh>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.PinkSlimeBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtQueenSlime>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.TungstenBrick, 25)
					.AddIngredient(ModContent.ItemType<CaughtTheDestroyer>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.IronBrick, 25)
					.AddIngredient(ModContent.ItemType<CaughtRetinazer>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.LeadBrick, 25)
					.AddIngredient(ModContent.ItemType<CaughtSpazmatism>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.SilverBrick, 25)
					.AddIngredient(ModContent.ItemType<CaughtSkeletronPrime>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.Hay, 25)
					.AddIngredient(ModContent.ItemType<CaughtPlantera>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.LihzahrdBrick, 25)
					.AddIngredient(ModContent.ItemType<CaughtGolem>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtEmpressOfLight>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtDukeFishron>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtBetsy>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtLunaticCultist>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtMoonLord>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.ReefBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtDreadnautilus>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.FleshBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtMothron>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.Pumpkin, 25)
					.AddIngredient(ModContent.ItemType<CaughtPumpking>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.IceBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtIceQueen>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.MartianConduitPlating, 25)
					.AddIngredient(ModContent.ItemType<CaughtMartianSaucer>())
					.AddTile(TileID.MeatGrinder)
					.Register();
				Recipe.Create(ItemID.LivingFireBlock, 25)
					.AddIngredient(ModContent.ItemType<CaughtTorchGod>())
					.AddTile(TileID.MeatGrinder)
					.Register();
			}
		}
	}
}