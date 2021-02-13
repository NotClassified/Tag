using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDele(MatchInfoSnapshot _match);
    private JoinRoomDele joinRoomCallBack;

    [SerializeField]
    private Text roomNameText;

    private MatchInfoSnapshot match;

    public void SetUp (MatchInfoSnapshot _match, JoinRoomDele _joinRoomCallBack)
    {
        match = _match;
        joinRoomCallBack = _joinRoomCallBack;
        roomNameText.text = match.name + " (" + match.maxSize + "/" + match.maxSize + ")";
    }

    public void JoinRoom()
    {
        joinRoomCallBack.Invoke(match);
    }
}
