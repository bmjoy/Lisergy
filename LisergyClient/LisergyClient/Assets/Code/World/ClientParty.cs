﻿using Game;
using Game.Entity;
using UnityEngine;

namespace Assets.Code.World
{
    public class ClientParty : Party
    {
        private static GameObject _container;

        public GameObject GameObject { get; private set; }
        public ClientTile ClientTile { get => (ClientTile)this.Tile; }

        public ClientParty(PlayerEntity owner, Party partyFromNetwork) : base(owner, partyFromNetwork.PartyIndex)
        {
            GameObject = new GameObject($"{owner.UserID}-{Id}-{partyFromNetwork.PartyIndex}");
            GameObject.transform.SetParent(Container.transform);
          
            _id = partyFromNetwork.Id;
            _x = (ushort)partyFromNetwork.X;
            _y = (ushort)partyFromNetwork.Y;
            foreach (var unit in partyFromNetwork.GetUnits())
                    this.AddUnit(new ClientUnit(owner, unit));
            Render();
            StackLog.Debug($"Created new party instance {this}");
        }


        public override Tile Tile {
            get => base.Tile;
            set {
                var old = base.Tile;
                base.Tile = value;
                if(value != null)
                {
                    StackLog.Debug($"Moving {this} gameobject to {value}");
                    GameObject.transform.position = new Vector3(value.X, 0.1f, value.Y);
                }
                ClientEvents.PartyFinishedMove(this, (ClientTile)old, (ClientTile)base.Tile);
            }
        }

        private static GameObject Container { get
            {
                if (_container == null)
                    _container = new GameObject("Parties");
                return _container;
            }
        }

        public bool IsMine()
        {
            return this.OwnerID == MainBehaviour.Player.UserID;
        }

        public override void AddUnit(Unit u)
        {
            base.AddUnit(u);
        }

        public void Render()
        {
            foreach(var unit in GetUnits())
            {
                var unitObject = ((ClientUnit)unit).Render();
                unitObject.transform.SetParent(GameObject.transform);
            }  
        }


    }
}
