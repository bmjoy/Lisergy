﻿using Game;
using Game.Entity;
using UnityEngine;

namespace Assets.Code.World
{
    public class ClientTile : Tile
    {
        public GameObject GameObj { get => _gameObj; private set => _gameObj = value; }

        private GameObject _gameObj;

        public bool Decorated;

        public ClientWorld ClientWorld { get => (ClientWorld)this.World; }

        public ClientTile(ClientChunk c, int x, int y) : base(c, x, y) { }

        public void SetVisible(bool visible)
        {
            if (GameObj == null)
                return;

            if (GameObj.activeSelf == visible)
                return;

            StackLog.Debug($"Changing activaction of {this} to {visible}");
            GameObj.SetActive(visible); // mandando unit parece q caga isso
        }

        public override void SetSeenBy(ExploringEntity entity)
        {
            base.SetSeenBy(entity);
            StackLog.Debug($"{entity} sees {this}");
            if (entity.Owner == MainBehaviour.Player)
            {
                SetVisible(true);
                foreach (var party in this.Parties)
                    ((ClientParty)party).GameObject.SetActive(true);
                if (this.Building != null)
                    ((ClientBuilding)this.Building).GameObject.SetActive(true);
            }
        }

        public override void SetUnseenBy(ExploringEntity unexplorer)
        {
            base.SetUnseenBy(unexplorer);

            if (unexplorer.Owner != MainBehaviour.Player)
                return;

            if (!this.IsVisibleTo(MainBehaviour.Player))
            {
                SetVisible(false);
                foreach (var party in this.Parties)
                    ((ClientParty)party).GameObject.SetActive(false);
                if (this.Building != null)
                    ((ClientBuilding)this.Building).GameObject.SetActive(false);
            }
        }

        public void RenderTile(byte tileID)
        {
            var tileSpec = StrategyGame.Specs.GetTileSpec(tileID);
            foreach (var art in tileSpec.Arts)
            {
                if (GameObj == null)
                {
                    var prefab = Resources.Load("prefabs/tiles/" + art.Name);
                    var parent = ((ClientChunk)this.Chunk).ChunkObject.transform;
                    GameObj = MainBehaviour.Instantiate(prefab, parent) as GameObject;
                    GameObj.name = $"Tile_{X}-{Y}";
                    GameObj.transform.position = new Vector3(X, 0, Y);
                    var tileBhv = GameObj.GetComponent<TileRandomizerBehaviour>();
                    base.TileId = tileID;
                    tileBhv.CreateTileDecoration(this);
                    return;
                }
            }
        }

        public override byte TileId
        {
            get { return base.TileId; }
            set
            {
                StackLog.Debug($"Updating {this} tileid to {value}");
                RenderTile(value);
                base.TileId = value;
            }
        }

        public override Building Building
        {
            get { return base.Building; }
            set
            {
                if (value != null)
                {
                    var clientBuilding = value as ClientBuilding;
                    clientBuilding.GameObject.transform.position = new Vector3(X, 0, Y);
                }
                using (new StackLog($"[Building] New {value} on {this}"))
                {
                    base.Building = value;
                }
            }
        }
    }
}
