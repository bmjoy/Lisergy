﻿using Game;
using UnityEngine;

namespace Assets.Code.World
{
    public class ClientBuilding : Building
    {
        public GameObject Object;

        public ClientBuilding(byte id, ClientPlayer owner, ClientTile tile): base(id, owner)
        {
            var prefab = Resources.Load("prefabs/buildings/"+id);
            StackLog.Debug("Instantiating BUILDING");
            Object = MainBehaviour.Instantiate(prefab, ((ClientChunk)tile.Chunk).ChunkObject.transform) as GameObject;
        }
    }
}
