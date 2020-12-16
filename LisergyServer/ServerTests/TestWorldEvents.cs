using Game;
using NUnit.Framework;
using ServerTests;
using System.Linq;

namespace Tests
{
    public class TestChunk
    {
        private TestGame Game;

        [SetUp]
        public void Setup()
        {
            Game = new TestGame();
        }

        [Test]
        public void GetGetTileCoords()
        {
            // get a tile from a chunk in the middle
            var chunkX = 1;
            var tileX = 2;
            var tile = Game.World.GetTile(chunkX * GameWorld.CHUNK_SIZE + tileX, chunkX * GameWorld.CHUNK_SIZE + tileX);

            Assert.AreEqual(chunkX, tile.Chunk.X);
            Assert.AreEqual(chunkX, tile.Chunk.Y);

            Assert.AreEqual(chunkX * GameWorld.CHUNK_SIZE + tileX, tile.X);
            Assert.AreEqual(chunkX * GameWorld.CHUNK_SIZE + tileX, tile.Y);
        }
    }
}