﻿using Game;
using UnityEngine;

namespace Assets.Code.World
{
    public class ClientBuilding : Building, IGameObject
    {
        private GameObject _gameObject;

        public GameObject GetGameObject() => _gameObject;

        public ClientBuilding(byte id, ClientPlayer owner, ClientTile tile): base(id, owner)
        {
            var prefab = Resources.Load("prefabs/buildings/"+id);
            StackLog.Debug("Instantiating BUILDING");
            _gameObject = MainBehaviour.Instantiate(prefab, ((ClientChunk)tile.Chunk).GetGameObject().transform) as GameObject;
        }

        public bool IsMine()
        {
            return Owner == MainBehaviour.Player;
        }
    }
}
