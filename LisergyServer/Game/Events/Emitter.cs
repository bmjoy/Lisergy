﻿using Game;
using Game.Events;
using Game.Events.ServerEvents;

namespace LisergyServer.Core
{
    public class EventEmitter
    {
        // TODO MAKE THIS WITH REFLECTION PLX
        public static void CallEventFromBytes(PlayerEntity owner, byte [] message)
        {
            var eventId = (EventID)message[0];
            Log.Debug($"Received {eventId.ToString()} event from network, calling handler");

            // SERVER EVENTS (from server to client/server)
            if(eventId == EventID.AUTH_RESULT)
            {
                var ev = Serialization.ToEvent<AuthResultEvent>(message);
                ev.ClientPlayer = owner;
                ev.FromNetwork = true;
                NetworkEvents.AuthResult(ev);
            }
            else if (eventId == EventID.TILE_VISIBLE)
            {
                var ev = Serialization.ToEvent<TileVisibleEvent>(message);
                ev.ClientPlayer = owner;
                ev.FromNetwork = true;
                NetworkEvents.TileVisible(ev);
            }
            else if (eventId == EventID.SPEC_RESPONSE)
            {
                var ev = Serialization.ToEvent<GameSpecResponse>(message);
                ev.ClientPlayer = owner;
                ev.FromNetwork = true;
                NetworkEvents.SpecResponse(ev);
            }
            else if (eventId == EventID.PARTY_VISIBLE)
            {
                var ev = Serialization.ToEvent<PartyVisibleEvent>(message);
                ev.ClientPlayer = owner;
                ev.FromNetwork = true;
                NetworkEvents.PartyVisible(ev);
            }


            // CLIENT EVENTS (coming from client to server)
            else if(eventId == EventID.JOIN)
            {
                var ev = Serialization.ToEvent<JoinWorldEvent>(message);
                ev.ClientPlayer = owner;
                ev.FromNetwork = true;
                NetworkEvents.JoinWorld(ev);
            }
            else if (eventId == EventID.PARTY_MOVE)
            {
                var ev = Serialization.ToEvent<MoveRequestEvent>(message);
                ev.ClientPlayer = owner;
                ev.FromNetwork = true;
                NetworkEvents.RequestPartyMove(ev);
            }
        } 
    }
}
