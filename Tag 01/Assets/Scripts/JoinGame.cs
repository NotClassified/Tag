using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefreb;

    [SerializeField]
    private Transform roomListParent;

    private NetworkManager nm;

    private void Start()
    {
        nm = NetworkManager.singleton;
        if (nm.matchMaker == null)
            nm.StartMatchMaker();
        RefreshRooms();
    }

    public void RefreshRooms()
    {
        ClearRooms();
        nm.matchMaker.ListMatches(0, 20, "", false, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";
        if (matchList == null)
        {
            status.text = "No Games Found";
            return;
        }
        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomList = Instantiate(roomListItemPrefreb);
            _roomList.transform.SetParent(roomListParent);
            _roomList.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            RoomListItem _roomListItem = _roomList.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.SetUp(match, JoinRoom);
            }

            roomList.Add(_roomList);
        }
        if(roomList.Count == 0)
        {
            status.text = "No Available Games";
        }
    }

    void ClearRooms()
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        nm.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, nm.OnMatchJoined);
        ClearRooms();
        status.text = "Joining Game...";
    }
}
