using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Host_Game : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager nm;
    
    public GameObject main_menu;
    public GameObject cam;

    [SerializeField]
    private Text status;

    private void Start()
    {
        main_menu.SetActive(true);
        nm = NetworkManager.singleton;
        if(nm.matchMaker == null)
        {
            nm.StartMatchMaker();
        }
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    public void CreateRoom()
    {
        status.text = "Creating Room";
        if (roomName != "" && roomName != null)
        {
            Debug.Log(roomName + " has " + roomSize + "players");
            nm.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, nm.OnMatchCreate);
        }
    }
    public void OnEnterGame()
    {
        status.text = "";
        cam.SetActive(false);
        main_menu.SetActive(false);
    }
}
