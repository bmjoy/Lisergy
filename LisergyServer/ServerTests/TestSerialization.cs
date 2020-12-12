using Game;
using Game.Events;
using LisergyServer.Core;
using NUnit.Framework;
using ServerTests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class TestSerialization
    {

        [Serializable]
        public class TestTileEvent : GameEvent
        {
            public TestTileEvent(Tile t)
            {
                this.Tile = t;
            }

            public override EventID GetID() => EventID.AUTH;
            public Tile Tile;
        }

        [Test]
        public void TestSimpleSerialization()
        {
            Serialization.LoadSerializers();
            var authEvent = new AuthEvent()
            {
                Login = "wololo",
                Password = "walala"
            };
            var bytes = Serialization.FromEvent<AuthEvent>(authEvent);
            var event2 = Serialization.ToEvent<AuthEvent>(bytes);

            Assert.AreEqual(authEvent.Login, event2.Login);
            Assert.AreEqual(authEvent.Password, event2.Password);
        }

        [Test]
        public void TestTileSerialization()
        {
            var game = new TestGame();
            var player = game.GetTestPlayer();
            var tile = game.World.GetTile(1, 1);
            Serialization.LoadSerializers(typeof(TestTileEvent));

            var serialized = Serialization.FromEvent<TestTileEvent>(new TestTileEvent(tile));
            var unserialized = Serialization.ToEvent<TestTileEvent>(serialized);

            Assert.AreEqual(tile.BuildingID, unserialized.Tile.BuildingID);
            Assert.AreEqual(tile.ResourceData, unserialized.Tile.ResourceData);
            Assert.AreEqual(tile.TerrainData, unserialized.Tile.TerrainData);
            Assert.AreEqual(tile.UserID, unserialized.Tile.UserID);
            Assert.AreEqual(tile.X, unserialized.Tile.X);
            Assert.AreEqual(tile.Y, unserialized.Tile.Y);
        }

        [Test]
        public void TestTileWIthData()
        {
            var game = new TestGame();
            var player = game.GetTestPlayer();
            var tile = game.RandomNotBuiltTile();
            Serialization.LoadSerializers(typeof(TestTileEvent));
            var buildingSpec = TestGame.RandomBuildingSpec();

            tile.Building = new Building(buildingSpec.Id, player, tile);
           
            var serialized = Serialization.FromEvent<TestTileEvent>(new TestTileEvent(tile));
            var unserialized = Serialization.ToEvent<TestTileEvent>(serialized);

            Assert.AreEqual(tile.BuildingID, unserialized.Tile.BuildingID);
            Assert.AreEqual(tile.ResourceData, unserialized.Tile.ResourceData);
            Assert.AreEqual(tile.TerrainData, unserialized.Tile.TerrainData);
            Assert.AreEqual(tile.UserID, unserialized.Tile.UserID);
            Assert.AreEqual(tile.X, unserialized.Tile.X);
            Assert.AreEqual(tile.Y, unserialized.Tile.Y);
        }
    }
}