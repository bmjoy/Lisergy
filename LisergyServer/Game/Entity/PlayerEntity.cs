﻿using Game.Entity;
using Game.Events;
using Game.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public abstract class PlayerEntity
    {
        public string UserID;

        public HashSet<Unit> Units = new HashSet<Unit>();
        public HashSet<Building> Buildings = new HashSet<Building>();
        public HashSet<Tile> VisibleTiles = new HashSet<Tile>();
        public HashSet<Tile> OnceExplored = new HashSet<Tile>();

        public Party[] Parties;

        public List<BattleResultEvent> Battles = new List<BattleResultEvent>();

        public PlayerEntity()
        {
            this.UserID = Guid.NewGuid().ToString();
            Parties = new Party[4]
            {
                new Party(this, 0),new Party(this, 1),new Party(this, 2),new Party(this, 3),
            };
        }

        public PlayerEntity(string id)
        {
            this.UserID = id;
        }

        public Building GetCenter()
        {
            return Buildings.First(b => b.SpecID == StrategyGame.Specs.InitialBuilding);
        }

        public Unit RecruitUnit(ushort unitSpecId)
        {
            var unit = new Unit(unitSpecId);
            unit.SetSpecStats();
            this.Units.Add(unit);
            Log.Debug($"{UserID} recruited {unitSpecId}");
            return unit;
        }

        public void PlaceUnitInParty(Unit u, Party newParty)
        {
            if (u.Party != null)
                u.Party.RemoveUnit(u);
            u.Party = newParty;
            newParty.AddUnit(u);
            Log.Debug($"{UserID} moved unit {u.SpecId} to party {newParty.PartyIndex}");
        }

        public void Build(byte id, Tile t)
        {
            var building = new Building(id, this);
            this.Buildings.Add(building);
            t.StaticEntity = building;
            Log.Debug($"Player {UserID} built {id}");
        }

        public abstract void Send<EventType>(EventType ev) where EventType : BaseEvent;

        public abstract bool Online();

        public override string ToString()
        {
            return $"<Player id={UserID.ToString()}>";
        }
    }
}
