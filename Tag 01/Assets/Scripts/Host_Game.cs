using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Host_Game : MonoBehaviour {
    

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
            nm.matchMaker.CreateMatch(roomName, nm.matchSize, true, "", "", "", 0, 0, nm.OnMatchCreate);
        }
    }
    public void OnEnterGame()
    {
        status.text = "";
        cam.SetActive(false);
        main_menu.SetActive(false);
    }
}
