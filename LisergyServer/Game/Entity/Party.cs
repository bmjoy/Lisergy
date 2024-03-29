﻿using Game.Events;
using Game.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Movement;
using Game.Battles;
using Game.Events.GameEvents;
using Game.Battle;

namespace Game.Entity
{
    [Serializable]
    public class Party : MovableWorldEntity, IBattleable
    {
        private byte _partyIndex;
        private Unit[] _units = new Unit[4] { null, null, null, null };

        private Inventory _cargo = new Inventory();

        public byte PartyIndex { get => _partyIndex; }
        public override TimeSpan GetMoveDelay() => TimeSpan.FromSeconds(0.25);

        public bool CanMove()
        {
            return !IsBattling;
        }

        public Party(PlayerEntity owner, byte partyIndex) : base(owner)
        {
            _partyIndex = partyIndex;
        }

        public override Tile Tile
        {
            get => base.Tile;
            set
            {
                base.Tile = value;
                if (value != null && value.StaticEntity != null)
                {
                    if (this.Course != null && this.Course.Intent == MovementIntent.Offensive && this.Course.IsLastMovement())
                    {
                        Tile.Game.GameEvents.RunCallbacks(new OffensiveMoveEvent()
                        {
                            Defender = value.StaticEntity,
                            Attacker = this
                        });
                    }
                }
            }
        }

      

        public override byte GetLineOfSight()
        {
            return _units.Where(u => u != null).Select(u => StrategyGame.Specs.Units[u.SpecId].LOS).Max();
        }

        public IEnumerable<Unit> GetValidUnits()
        {
            return _units.Where(u => u != null);
        }

        public Unit[] GetUnits()
        {
            return _units;
        }

        public virtual void SetUnit(Unit u, int index)
        {
            _units[index] = u;
        }

        public virtual void AddUnit(Unit u)
        {
            if (u.Party != null && u.Party != this)
                u.Party.RemoveUnit(u);
            var freeIndex = Array.IndexOf(_units, null);
            SetUnit(u, freeIndex);
        }

        public virtual void RemoveUnit(Unit u)
        {
            var index = Array.IndexOf(_units, u);
            _units[index] = null;
        }

        public override string ToString()
        {
            return $"<Party Battling={IsBattling} Id={Id} Index={PartyIndex} Owner={OwnerID}>";
        }

        public BattleTeam ToBattleTeam()
        {
            return new BattleTeam(this.Owner, this._units);
        }

        public void OnBattleStart(BattleStartEvent ev)
        {
            this.BattleID = ev.BattleID;
        }
    }
}
