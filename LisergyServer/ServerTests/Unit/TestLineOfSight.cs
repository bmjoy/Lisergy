using Game;
using Game.Events.ServerEvents;
using NUnit.Framework;
using ServerTests;
using System.Linq;
using Game.World;

namespace Tests
{
    public class TestLineOfSight
    {
        private TestGame Game;

        [SetUp]
        public void Setup()
        {
            Game = new TestGame(createPlayer:false);
        }

        [Test]
        public void TestLOSEvents()
        {
            Game.CreatePlayer();
            var initialBuildingSpec = StrategyGame.Specs.GetBuildingSpec(StrategyGame.Specs.InitialBuilding);
            var player = Game.GetTestPlayer();
            var events = Game.ReceivedEvents
                .Where(e => e is TileVisibleEvent)
                .Select(e => (TileVisibleEvent)e)
                .ToList();

            var range = initialBuildingSpec.LOS * 2 + 1;
            Assert.That(events.Count == range * range);
        }

        [Test]
        public void TestInitialLOS()
        {
            Game.CreatePlayer();
            var player = Game.GetTestPlayer();
 
            var initialBuildingSpec = StrategyGame.Specs.GetBuildingSpec(StrategyGame.Specs.InitialBuilding);
            var building = player.Buildings.FirstOrDefault();
            var tile = building.Tile;
            var areaTiles = tile.GetAOE(initialBuildingSpec.LOS).ToList();

            Assert.AreEqual(player.VisibleTiles.Count, areaTiles.Count());

            foreach (var seenTile in areaTiles)
            {
                Assert.True(seenTile.IsVisibleTo(player));
                Assert.True(seenTile.EntitiesViewing.Contains(building));
                Assert.True(player.VisibleTiles.Contains(seenTile));
            }
        }   

        [Test]
        public void TestTileAOE()
        {
            var tile = Game.World.GetTile(12, 12);

            ushort aoe = 4;
            var range = aoe * 2 + 1;
            var los = tile.GetAOE(aoe).ToList();

            Assert.That(los.Count() == range * range);
        }

        [Test]
        public void TestNotSendingEventsWhenNotReallyMoving()
        {
            Game.CreatePlayer();
            var player = Game.GetTestPlayer();
            player.ReceivedEvents.Clear();

            var party = player.Parties.First();

            party.Tile = party.Tile;

            Assert.AreEqual(0, player.ReceivedEvents.Count);
        }

        [Test]
        public void TestSendingEventsWhenExploring()
        {
            StrategyGame.Specs.InitialUnit = 2; // Mage with 3+ LOS
            Game.CreatePlayer(0, 0); // placing new user in the corner
            var player = Game.GetTestPlayer();
            player.ReceivedEvents.Clear();
            var party = player.Parties.First();

            party.Tile = party.Tile.GetNeighbor(Direction.EAST);

            /*          
                                o o o o E
                                o o o o E   
                                o o o o E    <- Explores this new row
            P Moving right ->   P o o o E 
              
            */ 
            Assert.AreEqual(party.GetLineOfSight() + 1, player.ReceivedEventsOfType<TileVisibleEvent>().Count);
        }

        [Test]
        public void TestNotSendingVisibleEventsWhenAlreadyExplored()
        {
            Game.CreatePlayer(0, 0);
            var player = Game.GetTestPlayer();
            var party = player.Parties.First();
            party.Tile = party.Tile.GetNeighbor(Direction.EAST);
            player.ReceivedEvents.Clear();

            party.Tile = party.Tile.GetNeighbor(Direction.WEST);

            Assert.AreEqual(0, player.ReceivedEventsOfType<TileVisibleEvent>().Count);
        }
    }
}