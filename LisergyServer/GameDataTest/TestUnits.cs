﻿using Game.Entity;
using GameData;
using GameData.buffs;
using GameData.Specs;
using System;
using System.Collections.Generic;

namespace GameDataTest
{
    public class TestUnits
    {
        private static void AddUnit(GameSpec spec, UnitSpec unitSpec)
        {
            var id = (ushort)spec.Units.Count;
            unitSpec.UnitSpecID = id;
            spec.Units[id] = unitSpec;
        }

        public static readonly int THIEF = 0;
        public static readonly int KNIGHT = 1;
        public static readonly int MAGE = 2;

        private static UnitStats BaseStats = new UnitStats(new Dictionary<Stat, short>()
            {
                    { Stat.SPEED, 5 },
                    { Stat.ACCURACY, 5 },
                    { Stat.DEF, 5 },
                    { Stat.MDEF, 5 },
                    { Stat.ATK, 5 },
                    { Stat.MATK, 5 },
                    { Stat.MHP, 100 },
                    { Stat.MMP, 100 },
        });

        private static UnitStats AddToBase(params (Stat, short)[] stats)
        {
            var st = new UnitStats();
            st.SetStats(BaseStats);
            foreach(var item in stats)
            {
                var value = st.GetStat(item.Item1);
                value += item.Item2;
                st.SetStat(item.Item1, value);
            }
                
            return st;
        }

        public static void Generate(GameSpec spec)
        {
            var tuple = (Stat.ATK, 5);
            AddUnit(spec, new UnitSpec()
            {
                Art = new ArtSpec()
                {
                    Type = ArtType.SPRITE_SHEET,
                    Name = "Soldier"
                },
                Name = "Thief",
                FaceArt = new ArtSpec()
                {
                    Type = ArtType.SPECIFIC_SPRITE,
                    Name = "Faces",
                    Index = 2
                },
                LOS = 2,
                Stats = AddToBase(
                    (Stat.SPEED, 2),
                    (Stat.DEF, -2),
                    (Stat.MDEF, -1),
                    (Stat.ATK, 1),
                    (Stat.MATK, -2)
                )
            });
            AddUnit(spec, new UnitSpec()
            {
                Art = new ArtSpec()
                {
                    Type = ArtType.SPRITE_SHEET,
                    Name = "Knight"
                },
                Name = "Knight",
                LOS = 1,
                FaceArt = new ArtSpec()
                {
                    Type = ArtType.SPECIFIC_SPRITE,
                    Name = "Faces",
                    Index = 1
                },
                Stats = AddToBase(
                    (Stat.ATK, 1),
                    (Stat.MDEF, -1),
                    (Stat.SPEED, -3),
                    (Stat.DEF, 2),
                    (Stat.ACCURACY, 1),
                    (Stat.MATK, -1),
                    (Stat.MHP, 10)
                )
            });
            AddUnit(spec, new UnitSpec()
            {
                Art = new ArtSpec()
                {
                    Type = ArtType.SPRITE_SHEET,
                    Name = "Mage"
                },
                Name = "Mage",
                FaceArt = new ArtSpec()
                {
                    Type = ArtType.SPECIFIC_SPRITE,
                    Name = "Faces",
                    Index = 0
                },
                LOS = 3,
                Stats = AddToBase(
                    (Stat.ATK, -2),
                    (Stat.MATK, 2),
                    (Stat.MDEF, 1),
                    (Stat.SPEED, -1),
                    (Stat.DEF, -2),
                    (Stat.ACCURACY, 2),
                    (Stat.MHP, -20),
                    (Stat.MMP, 20)
                )
            });
        }
    }
}
