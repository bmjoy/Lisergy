﻿using System.Reflection;

namespace Game.Events.Bus
{
    public class RegisteredListener
    {
        public IEventListener Listener;
        public MethodInfo Method;

        public void Call(BaseEvent ev)
        {
            Method.Invoke(Listener, new object[] { ev });
        }
    }
}
