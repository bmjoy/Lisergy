﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entity
{
    [Serializable]
    public class Gaia : PlayerEntity
    {

        public Gaia()
        {
            this.UserID = "gaia";
        }

        public override bool Online()
        {
            return false;
        }

        public override void Send<EventType>(EventType ev)
        {
        }
    }
}