﻿
using Assets.Code.World;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public delegate void CameraMoveEvent(Vector3 oldPosition, Vector3 newPosition);
    public delegate void ClickTileEvent(ClientTile tile);
    public delegate void SelectPartyEvent(ClientParty party);
    public delegate void PlayerLoginEvent(ClientPlayer player);
    public delegate void PartyFinishedMoveEvent(ClientParty party, ClientTile oldTile, ClientTile newTile);
    public delegate void StartMovementRequest(ClientParty party, List<ClientTile> Path);

    public class ClientEvents
    {
        public static event CameraMoveEvent OnCameraMove;
        public static event ClickTileEvent OnClickTile;
        public static event SelectPartyEvent OnSelectParty;
        public static event PlayerLoginEvent OnPlayerLogin;
        public static event PartyFinishedMoveEvent OnPartyFinishedMove;
        public static event StartMovementRequest OnStartMovementRequest;

        public static void CameraMove(Vector3 oldPosition, Vector3 newPosition)
        {
            OnCameraMove?.Invoke(oldPosition, newPosition);
        }

        public static void ClickTile(ClientTile tile)
        {
            OnClickTile?.Invoke(tile);
        }

        public static void SelectParty(ClientParty party)
        {
            OnSelectParty?.Invoke(party);
        }

        public static void PlayerLogin(ClientPlayer player)
        {
            OnPlayerLogin?.Invoke(player);
        }

        public static void PartyFinishedMove(ClientParty p, ClientTile o, ClientTile n)
        {
            OnPartyFinishedMove?.Invoke(p, o, n);
        }

        public static void StartMovementRequest(ClientParty p, List<ClientTile> path)
        {
            OnStartMovementRequest?.Invoke(p, path);
        }
    }
}
