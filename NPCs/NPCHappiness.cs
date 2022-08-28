using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using BossesAsNPCs.NPCs.TownNPCs;
using Terraria.Localization;
using Terraria.GameContent.Personalities;

namespace BossesAsNPCs.NPCs
{
	//This only is for happiness. Shop changes are in BossesAsNPCsNPCs.cs
	public class BossesAsNPCsNPCHappiness : GlobalNPC
	{
		public override void SetStaticDefaults()
		{
			bool townNPCsCrossModSupport = ModContent.GetInstance<BossesAsNPCsConfigServer>().TownNPCsCrossModSupport;

			int kingSlime = ModContent.NPCType<KingSlime>(); // Get NPC's type
			int eyeOfCthulhu = ModContent.NPCType<EyeOfCthulhu>();
			int eaterOfWorlds = ModContent.NPCType<EaterOfWorlds>();
			int brainOfCthulhu = ModContent.NPCType<BrainOfCthulhu>();
			int queenBee = ModContent.NPCType<QueenBee>();
			int skeletron = ModContent.NPCType<Skeletron>();
			int deerclops = ModContent.NPCType<Deerclops>();
			int wallOfFlesh = ModContent.NPCType<WallOfFlesh>();
			int queenSlime = ModContent.NPCType<QueenSlime>();
			int theDestroyer = ModContent.NPCType<TheDestroyer>();
			int retinazer = ModContent.NPCType<Retinazer>();
			int spazmatism = ModContent.NPCType<Spazmatism>();
			int skeletronPrime = ModContent.NPCType<SkeletronPrime>();
			int plantera = ModContent.NPCType<Plantera>();
			int golem = ModContent.NPCType<Golem>();
			int empressOfLight = ModContent.NPCType<EmpressOfLight>();
			int dukeFishron = ModContent.NPCType<DukeFishron>();
			int betsy = ModContent.NPCType<Betsy>();
			int lunaticCultist = ModContent.NPCType<LunaticCultist>();
			int moonLord = ModContent.NPCType<MoonLord>();
			int dreadnautilus = ModContent.NPCType<Dreadnautilus>();
			int mothron = ModContent.NPCType<Mothron>();
			int pumpking = ModContent.NPCType<Pumpking>();
			int iceQueen = ModContent.NPCType<IceQueen>();
			int martianSaucer = ModContent.NPCType<MartianSaucer>();
			int torchGod = ModContent.NPCType<TorchGod>();

			var guide = NPCHappiness.Get(NPCID.Guide); // Get the key into the NPC's happiness
			var merchant = NPCHappiness.Get(NPCID.Merchant);
			var nurse = NPCHappiness.Get(NPCID.Nurse);
			var demolitionist = NPCHappiness.Get(NPCID.Demolitionist);
			var dyeTrader = NPCHappiness.Get(NPCID.DyeTrader);
			var angler = NPCHappiness.Get(NPCID.Angler);
			var zoologist = NPCHappiness.Get(NPCID.BestiaryGirl);
			var dryad = NPCHappiness.Get(NPCID.Dryad);
			var painter = NPCHappiness.Get(NPCID.Painter);
			var golfer = NPCHappiness.Get(NPCID.Golfer);
			var armsDealer = NPCHappiness.Get(NPCID.ArmsDealer);
			var tavernkeep = NPCHappiness.Get(NPCID.DD2Bartender);
			var stylist = NPCHappiness.Get(NPCID.Stylist);
			var goblinTinkerer = NPCHappiness.Get(NPCID.GoblinTinkerer);
			var witchDoctor = NPCHappiness.Get(NPCID.WitchDoctor);
			var clothier = NPCHappiness.Get(NPCID.Clothier);
			var mechanic = NPCHappiness.Get(NPCID.Mechanic);
			var partyGirl = NPCHappiness.Get(NPCID.PartyGirl);
			var wizard = NPCHappiness.Get(NPCID.Wizard);
			var taxCollector = NPCHappiness.Get(NPCID.TaxCollector);
			var truffle = NPCHappiness.Get(NPCID.Truffle);
			var pirate = NPCHappiness.Get(NPCID.Pirate);
			var steampunker = NPCHappiness.Get(NPCID.Steampunker);
			var cyborg = NPCHappiness.Get(NPCID.Cyborg);
			var santa = NPCHappiness.Get(NPCID.SantaClaus);
			//Princess automatically Loves everyone

			guide.SetNPCAffection(kingSlime, AffectionLevel.Like); // Make the Guide like King Slime!
			guide.SetNPCAffection(eyeOfCthulhu, AffectionLevel.Like);
			guide.SetNPCAffection(torchGod, AffectionLevel.Like);
			guide.SetNPCAffection(wallOfFlesh, AffectionLevel.Hate);

			merchant.SetNPCAffection(brainOfCthulhu, AffectionLevel.Like);
			merchant.SetNPCAffection(iceQueen, AffectionLevel.Like);
			merchant.SetNPCAffection(mothron, AffectionLevel.Like);
			merchant.SetNPCAffection(torchGod, AffectionLevel.Dislike);

			nurse.SetNPCAffection(queenSlime, AffectionLevel.Like);
			nurse.SetNPCAffection(empressOfLight, AffectionLevel.Like);
			nurse.SetNPCAffection(theDestroyer, AffectionLevel.Dislike);
			nurse.SetNPCAffection(retinazer, AffectionLevel.Dislike);
			nurse.SetNPCAffection(spazmatism, AffectionLevel.Dislike);
			nurse.SetNPCAffection(skeletronPrime, AffectionLevel.Dislike);
			nurse.SetNPCAffection(martianSaucer, AffectionLevel.Dislike);

			demolitionist.SetNPCAffection(martianSaucer, AffectionLevel.Like);
			demolitionist.SetNPCAffection(skeletronPrime, AffectionLevel.Like);
			demolitionist.SetNPCAffection(torchGod, AffectionLevel.Like);
			demolitionist.SetNPCAffection(eaterOfWorlds, AffectionLevel.Dislike);
			demolitionist.SetNPCAffection(brainOfCthulhu, AffectionLevel.Dislike);

			dyeTrader.SetNPCAffection(plantera, AffectionLevel.Like);
			dyeTrader.SetNPCAffection(kingSlime, AffectionLevel.Like);
			dyeTrader.SetNPCAffection(mothron, AffectionLevel.Like);
			dyeTrader.SetNPCAffection(torchGod, AffectionLevel.Like);
			dyeTrader.SetNPCAffection(dreadnautilus, AffectionLevel.Dislike);

			angler.SetNPCAffection(dukeFishron, AffectionLevel.Like);
			angler.SetNPCAffection(dreadnautilus, AffectionLevel.Like);
			angler.SetNPCAffection(queenBee, AffectionLevel.Dislike);
			angler.SetNPCAffection(mothron, AffectionLevel.Dislike);

			zoologist.SetNPCAffection(dukeFishron, AffectionLevel.Like);
			zoologist.SetNPCAffection(queenBee, AffectionLevel.Like);
			zoologist.SetNPCAffection(betsy, AffectionLevel.Like);
			zoologist.SetNPCAffection(deerclops, AffectionLevel.Like);
			zoologist.SetNPCAffection(mothron, AffectionLevel.Like);
			zoologist.SetNPCAffection(dreadnautilus, AffectionLevel.Like);
			zoologist.SetNPCAffection(wallOfFlesh, AffectionLevel.Dislike);

			dryad.SetNPCAffection(empressOfLight, AffectionLevel.Love);
			dryad.SetNPCAffection(plantera, AffectionLevel.Love);
			dryad.SetNPCAffection(golem, AffectionLevel.Like);
			dryad.SetNPCAffection(queenSlime, AffectionLevel.Like);
			dryad.SetNPCAffection(queenBee, AffectionLevel.Like);
			dryad.SetNPCAffection(dreadnautilus, AffectionLevel.Dislike);
			dryad.SetNPCAffection(eaterOfWorlds, AffectionLevel.Hate);
			dryad.SetNPCAffection(brainOfCthulhu, AffectionLevel.Hate);
			dryad.SetNPCAffection(moonLord, AffectionLevel.Hate);

			painter.SetNPCAffection(empressOfLight, AffectionLevel.Like);
			painter.SetNPCAffection(iceQueen, AffectionLevel.Like);
			painter.SetNPCAffection(eyeOfCthulhu, AffectionLevel.Like);
			painter.SetNPCAffection(martianSaucer, AffectionLevel.Dislike);

			golfer.SetNPCAffection(dukeFishron, AffectionLevel.Love);
			golfer.SetNPCAffection(kingSlime, AffectionLevel.Like);
			golfer.SetNPCAffection(wallOfFlesh, AffectionLevel.Dislike);

			armsDealer.SetNPCAffection(eaterOfWorlds, AffectionLevel.Like);
			armsDealer.SetNPCAffection(brainOfCthulhu, AffectionLevel.Like);
			armsDealer.SetNPCAffection(pumpking, AffectionLevel.Like);

			tavernkeep.SetNPCAffection(eaterOfWorlds, AffectionLevel.Like);
			tavernkeep.SetNPCAffection(brainOfCthulhu, AffectionLevel.Like);
			tavernkeep.SetNPCAffection(dreadnautilus, AffectionLevel.Like);
			tavernkeep.SetNPCAffection(lunaticCultist, AffectionLevel.Like);
			tavernkeep.SetNPCAffection(torchGod, AffectionLevel.Like);
			tavernkeep.SetNPCAffection(golem, AffectionLevel.Dislike);

			stylist.SetNPCAffection(empressOfLight, AffectionLevel.Love);
			stylist.SetNPCAffection(queenSlime, AffectionLevel.Like);
			stylist.SetNPCAffection(plantera, AffectionLevel.Like);
			stylist.SetNPCAffection(queenBee, AffectionLevel.Dislike);
			stylist.SetNPCAffection(wallOfFlesh, AffectionLevel.Dislike);
			stylist.SetNPCAffection(pumpking, AffectionLevel.Dislike);
			stylist.SetNPCAffection(mothron, AffectionLevel.Dislike);
			stylist.SetNPCAffection(eyeOfCthulhu, AffectionLevel.Hate);
			stylist.SetNPCAffection(eaterOfWorlds, AffectionLevel.Hate);
			stylist.SetNPCAffection(brainOfCthulhu, AffectionLevel.Hate);
			stylist.SetNPCAffection(skeletron, AffectionLevel.Hate);
			stylist.SetNPCAffection(theDestroyer, AffectionLevel.Hate);
			stylist.SetNPCAffection(retinazer, AffectionLevel.Hate);
			stylist.SetNPCAffection(spazmatism, AffectionLevel.Hate);
			stylist.SetNPCAffection(skeletronPrime, AffectionLevel.Hate);
			stylist.SetNPCAffection(martianSaucer, AffectionLevel.Hate);

			goblinTinkerer.SetNPCAffection(theDestroyer, AffectionLevel.Like);
			goblinTinkerer.SetNPCAffection(retinazer, AffectionLevel.Like);
			goblinTinkerer.SetNPCAffection(spazmatism, AffectionLevel.Like);
			goblinTinkerer.SetNPCAffection(skeletronPrime, AffectionLevel.Like);
			goblinTinkerer.SetNPCAffection(martianSaucer, AffectionLevel.Like);
			goblinTinkerer.SetNPCAffection(torchGod, AffectionLevel.Dislike);

			witchDoctor.SetNPCAffection(golem, AffectionLevel.Love);
			witchDoctor.SetNPCAffection(queenBee, AffectionLevel.Like);
			witchDoctor.SetNPCAffection(mothron, AffectionLevel.Like);
			witchDoctor.SetNPCAffection(lunaticCultist, AffectionLevel.Dislike);
			witchDoctor.SetNPCAffection(moonLord, AffectionLevel.Dislike);

			clothier.SetNPCAffection(empressOfLight, AffectionLevel.Like);
			clothier.SetNPCAffection(pumpking, AffectionLevel.Like);
			clothier.SetNPCAffection(kingSlime, AffectionLevel.Like);
			clothier.SetNPCAffection(queenSlime, AffectionLevel.Like);
			clothier.SetNPCAffection(skeletronPrime, AffectionLevel.Dislike);
			clothier.SetNPCAffection(lunaticCultist, AffectionLevel.Dislike);
			clothier.SetNPCAffection(skeletron, AffectionLevel.Hate);

			mechanic.SetNPCAffection(martianSaucer, AffectionLevel.Like);
			mechanic.SetNPCAffection(theDestroyer, AffectionLevel.Dislike);
			mechanic.SetNPCAffection(retinazer, AffectionLevel.Dislike);
			mechanic.SetNPCAffection(spazmatism, AffectionLevel.Dislike);
			mechanic.SetNPCAffection(skeletronPrime, AffectionLevel.Dislike);
			mechanic.SetNPCAffection(lunaticCultist, AffectionLevel.Dislike);

			partyGirl.SetNPCAffection(empressOfLight, AffectionLevel.Like);
			partyGirl.SetNPCAffection(betsy, AffectionLevel.Like);
			partyGirl.SetNPCAffection(queenSlime, AffectionLevel.Like);
			partyGirl.SetNPCAffection(mothron, AffectionLevel.Like);
			partyGirl.SetNPCAffection(dreadnautilus, AffectionLevel.Dislike);

			wizard.SetNPCAffection(empressOfLight, AffectionLevel.Like);
			wizard.SetNPCAffection(lunaticCultist, AffectionLevel.Like);
			wizard.SetNPCAffection(moonLord, AffectionLevel.Like);
			wizard.SetNPCAffection(martianSaucer, AffectionLevel.Dislike);

			taxCollector.SetNPCAffection(wallOfFlesh, AffectionLevel.Dislike);
			taxCollector.SetNPCAffection(martianSaucer, AffectionLevel.Dislike);
			taxCollector.SetNPCAffection(deerclops, AffectionLevel.Dislike);

			truffle.SetNPCAffection(skeletron, AffectionLevel.Love);
			truffle.SetNPCAffection(plantera, AffectionLevel.Like);
			truffle.SetNPCAffection(dukeFishron, AffectionLevel.Dislike);
			truffle.SetNPCAffection(torchGod, AffectionLevel.Dislike);

			pirate.SetNPCAffection(dukeFishron, AffectionLevel.Like);
			pirate.SetNPCAffection(theDestroyer, AffectionLevel.Dislike);
			pirate.SetNPCAffection(dreadnautilus, AffectionLevel.Dislike);

			steampunker.SetNPCAffection(theDestroyer, AffectionLevel.Like);
			steampunker.SetNPCAffection(retinazer, AffectionLevel.Like);
			steampunker.SetNPCAffection(spazmatism, AffectionLevel.Like);
			steampunker.SetNPCAffection(skeletronPrime, AffectionLevel.Like);
			steampunker.SetNPCAffection(martianSaucer, AffectionLevel.Like);
			steampunker.SetNPCAffection(pumpking, AffectionLevel.Dislike);
			steampunker.SetNPCAffection(eyeOfCthulhu, AffectionLevel.Dislike);

			cyborg.SetNPCAffection(theDestroyer, AffectionLevel.Like);
			cyborg.SetNPCAffection(retinazer, AffectionLevel.Like);
			cyborg.SetNPCAffection(spazmatism, AffectionLevel.Like);
			cyborg.SetNPCAffection(skeletronPrime, AffectionLevel.Like);
			cyborg.SetNPCAffection(martianSaucer, AffectionLevel.Like);
			cyborg.SetNPCAffection(iceQueen, AffectionLevel.Dislike);
			cyborg.SetNPCAffection(pumpking, AffectionLevel.Dislike);
			cyborg.SetNPCAffection(empressOfLight, AffectionLevel.Dislike);

			santa.SetNPCAffection(deerclops, AffectionLevel.Like);
			santa.SetNPCAffection(iceQueen, AffectionLevel.Like);
			santa.SetNPCAffection(pumpking, AffectionLevel.Dislike);
			santa.SetNPCAffection(moonLord, AffectionLevel.Hate);

			if (ModLoader.TryGetMod("FishermanNPC", out Mod fishermanNPC) && townNPCsCrossModSupport)
			{
				int fishermanType = fishermanNPC.Find<ModNPC>("Fisherman").Type;
				var fishermanHappiness = NPCHappiness.Get(fishermanNPC.Find<ModNPC>("Fisherman").Type);

				var dreadnautilusHappiness = NPCHappiness.Get(dreadnautilus);

				fishermanHappiness.SetNPCAffection(dreadnautilus, AffectionLevel.Like);
				dreadnautilusHappiness.SetNPCAffection(fishermanType, AffectionLevel.Love);
			}

			if (ModLoader.TryGetMod("TorchMerchant", out Mod torchSeller) && townNPCsCrossModSupport)
			{
				int torchManType = torchSeller.Find<ModNPC>("TorchSellerNPC").Type;
				var torchManHappiness = NPCHappiness.Get(torchManType);

				var mothronHappiness = NPCHappiness.Get(mothron);
				mothronHappiness.SetNPCAffection(torchManType, AffectionLevel.Love);

				var moonLordHappiness = NPCHappiness.Get(moonLord);
				moonLordHappiness.SetNPCAffection(torchManType, AffectionLevel.Dislike);

				var torchGodHappiness = NPCHappiness.Get(torchGod);
				torchGodHappiness.SetNPCAffection(torchManType, AffectionLevel.Love);
				torchManHappiness.SetNPCAffection(torchGod, AffectionLevel.Love);
			}
		}
	}
}