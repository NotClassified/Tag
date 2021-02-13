using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreBoard : NetworkBehaviour {

    public GameObject canvas;
    [SerializeField]
    GameObject content;
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    GameObject[] clients;
    [SerializeField]
    List<GameObject> boardItems = new List<GameObject>();

    List<string> clientNames = new List<string>();
    List<int> clientTags = new List<int>();
    List<int> clientTagged = new List<int>();
    
    private void Start()
    {
        canvas.SetActive(false);
    }

    [Command]
    public void CmdSetBoard() { RpcSetBoard(); }
    [ClientRpc]
    void RpcSetBoard()
    {
            Debug.Log("SB SetBoard");
            clients = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject client in clients)
            {
                clientNames.Add(client.GetComponent<PlayerGameData>().playerName);
                clientTags.Add(client.GetComponent<PlayerGameData>().numTags);
                clientTagged.Add(client.GetComponent<PlayerGameData>().numTagged);
            }
            for (int i = 0; i < clientNames.Count; i++)
            {
                boardItems.Add(Instantiate(prefab));
                boardItems[i].transform.SetParent(content.transform);
                boardItems[i].transform.GetChild(0).GetComponent<Text>().text = clientNames[i];
            }
    }

    [Command]
    public void CmdSetBoardScores(string _name, int _tags, int _tagged) { RpcSetBoardScores(_name, _tags, _tagged); }
    [ClientRpc]
    public void RpcSetBoardScores(string _name, int _tags, int _tagged)
    {
            Debug.Log("SB Set Scores");
            boardItems[clientNames.IndexOf(_name)].transform.GetChild(1).GetComponent<Text>().text = "" + _tags;
            boardItems[clientNames.IndexOf(_name)].transform.GetChild(2).GetComponent<Text>().text = "" + _tagged;
    }
}
