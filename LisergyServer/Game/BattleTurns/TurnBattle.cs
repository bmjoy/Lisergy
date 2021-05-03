﻿
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Battles
{
    public class TurnBattle
    {
        protected BattleTeam _attacker;
        protected BattleTeam _defender;

        protected SortedSet<BattleUnit> _actionQueue = new SortedSet<BattleUnit>();
        protected TurnBattleResult _log = new TurnBattleResult();

        public Guid ID { get; private set; }

        public TurnBattle(Guid id, BattleTeam attacker, BattleTeam defender)
        {
            this.ID = id;
            this._attacker = _log.Attacker = attacker;
            this._defender = _log.Defender = defender;

            _actionQueue.UnionWith(attacker.Units);
            _actionQueue.UnionWith(defender.Units);
        }

        public TurnBattleResult Run()
        {
            while(_log.Winner == null && DoRound()) {
                _log.Winner = HasWinner();
            }
            return _log;
        }

        protected virtual bool DoRound()
        {
            _log.NextTurn();
            var actingUnit = _actionQueue.First();
            TakeAction(actingUnit);
            if (!_actionQueue.Remove(actingUnit))
                throw new Exception("Error removing unit");
            actingUnit.RT += actingUnit.GetMaxRT();
            _actionQueue.Add(actingUnit);
            return true;
        }

        protected virtual BattleTeam HasWinner()
        {
            if (_attacker.AllDead) return _defender;
            else if (_defender.AllDead) return _attacker;
            return null;
        }

        protected virtual void TakeAction(BattleUnit unit)
        {
            var enemyTeam = GetOpposingTeam(unit);
            var enemy = enemyTeam.Random();
            _log.AddAction(unit.Attack(enemy));
        }

        protected virtual BattleTeam GetOpposingTeam(BattleUnit unit)
        {
            if (unit.Team == _attacker) return _defender;
            return _attacker;
        }

    }
}
