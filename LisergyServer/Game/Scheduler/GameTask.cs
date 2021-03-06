﻿using System;

namespace Game.Scheduler
{
    public abstract class GameTask : IComparable<GameTask>
    {
        private DateTime _start;
        private DateTime _executionTime;

        public GameTask(TimeSpan delay)
        {
            ID = Guid.NewGuid();
            Delay = delay;
            Start = GameScheduler.Now;
            GameScheduler.Add(this);
        }

        internal bool HasFinished;
        public Guid ID { get; private set; }
        public TimeSpan Delay { get; private set; }
        public DateTime Finish { get; private set; }
        public DateTime Start
        {
            get => _start;
            set {
                _start = value; Finish = _start + Delay;
            }
        }

      
        public bool Repeat;

        public bool IsDue() => Finish <= GameScheduler.Now;

        public abstract void Execute();

        public void Cancel()
        {
            GameScheduler.Cancel(this);
        }

        public int CompareTo(GameTask other)
        {
            if (other.ID == this.ID)
                return 0;
            return other.Finish > this.Finish ? -1 : 1;
        }

        public override string ToString()
        {
            return $"<Task {ID.ToString()} Start=<{Start}> End=<{Finish}>>";
        }
    }
}
