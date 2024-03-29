﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Battles.Actions
{
    [Serializable]
    public class AttackAction : BattleAction
    {
        public string DefenderID;

        [NonSerialized]
        private BattleUnit _defender;

        public BattleUnit Defender
        {
            get
            {
                if (_defender == null)
                    _defender = Battle.FindBattleUnit(DefenderID);
                return _defender;
            }
            set { _defender = value; DefenderID = value.UnitID; }
        }

        public AttackAction(TurnBattle battle, BattleUnit atk, BattleUnit def) : base(battle, atk)
        {
            this.Defender = def;
        }

        public override string ToString()
        {
            return $"<Attack From={UnitID} To={DefenderID}>";
        }
    }
}
