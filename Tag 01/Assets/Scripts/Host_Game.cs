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

    [SerializeField]
    GameObject quitButton;
    [SerializeField]
    GameObject quitValidation;

    public void Start()
    {
        cam.SetActive(true);
        quitButton.SetActive(true);
        quitValidation.SetActive(false);
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

    public void QuitButton()
    {
        quitButton.SetActive(false);
        quitValidation.SetActive(true);
    }
    public void QuitAnswer(string _answer)
    {
        if(_answer == "Yes")
        {
            Application.Quit();
        }
        if (_answer == "No")
        {
            quitButton.SetActive(true);
            quitValidation.SetActive(false);
        }
    }
}
