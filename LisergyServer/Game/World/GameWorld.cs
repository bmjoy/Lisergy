﻿using Game.Generator;
using Game.World;
using System;
using System.Collections.Generic;

namespace Game
{
    public class GameWorld
    {
        // Amount of tiles the chunk length
        public static readonly int CHUNK_SIZE = 8;

        // Bitshift needed to get the chunk of a given tile
        public static readonly int CHUNK_SIZE_BITSHIFT = CHUNK_SIZE.BitsRequired() - 1;

        // how many tiles is the area of a chunk
        public static readonly int TILES_IN_CHUNK = CHUNK_SIZE * CHUNK_SIZE;

        // how many chunks are "player reserved chunks" per player
        public static readonly int PLAYERS_CHUNKS = 2;

        private int _sizeX;
        private int _sizeY;

        public WorldPlayers Players { get; set; }
        public ChunkMap ChunkMap { get; set; }

        public ushort Seed { get; set; }
        public int SizeX { get => _sizeX; set => _sizeX = value; }
        public int SizeY { get => _sizeY; set => _sizeY = value; }

        public bool ValidCoords(int x, int y)
        {
            return x >= 0 && x < SizeX && y >= 0 && y < SizeY;
        }

        public virtual void CreateWorld(int playerCount)
        {
            var amtOfChunks = playerCount * GameWorld.PLAYERS_CHUNKS;
            var amtOfTiles = amtOfChunks * GameWorld.TILES_IN_CHUNK;
            var arraySize = (int)Math.Ceiling(Math.Sqrt(amtOfTiles));
            var extraNeeded = GameWorld.CHUNK_SIZE - arraySize % GameWorld.CHUNK_SIZE;
            SizeX = arraySize + extraNeeded;
            SizeY = arraySize + extraNeeded;
            Players = new WorldPlayers(playerCount);
            ChunkMap = new ChunkMap(SizeX / GameWorld.CHUNK_SIZE, SizeY / GameWorld.CHUNK_SIZE);
        }

        public virtual void PlaceNewPlayer(PlayerEntity player, Tile t = null)
        {
            var newbieChunk = ChunkMap.GetUnnocupiedNewbieChunk();
            if (newbieChunk == null)
            {
                throw new Exception("No more room for newbie players in this world");
            }
            Players.Add(player);
            if(t == null)
            {
                t = Worldgen.FindTileWithId(newbieChunk.Tiles, 0);
            }
            byte castleID = StrategyGame.Specs.InitialBuilding;
            player.Build(castleID, t);

            ushort initialUnit = StrategyGame.Specs.InitialUnit;
            var unit = player.RecruitUnit(initialUnit);

            var party = player.Parties[0];
            player.PlaceUnitInParty(unit, party);
            player.DeployParty(party, t.GetNeighbor(Direction.EAST));
            Log.Debug($"Placed new player in {t}");
            return;
        }

        public virtual IEnumerable<Tile> AllTiles()
        {
            foreach (var chunk in ChunkMap.AllChunks())
                foreach (var tile in chunk.AllTiles())
                    yield return tile;
        }

        public virtual Chunk GetTileChunk(int tileX, int tileY)
        {
            int chunkX = tileX >> CHUNK_SIZE_BITSHIFT;
            var chunkY = tileY >> CHUNK_SIZE_BITSHIFT;
            return ChunkMap.GetChunk(chunkX, chunkY);
        }

        public virtual Tile GetTile(int tileX, int tileY)
        {
            var internalTileX = tileX % CHUNK_SIZE;
            var internalTileY = tileY % CHUNK_SIZE;
            return GetTileChunk(tileX, tileY).GetTile(internalTileX, internalTileY);
        }
    }
}
