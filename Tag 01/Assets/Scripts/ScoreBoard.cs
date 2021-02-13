using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreBoard : NetworkBehaviour {

    public GameObject canvas;
    public GameObject content;
    public GameObject prefab;

    [SerializeField]
    GameObject[] clients;
    public List<GameObject> boardItems = new List<GameObject>();

    public List<string> clientNames = new List<string>();
    public List<int> clientTags = new List<int>();
    public List<int> clientTagged = new List<int>();
    
    private void Start()
    {
        canvas.SetActive(false);
    }

    public void SetBoard()
    {
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject client in clients)
        {
            clientNames.Add(client.GetComponent<PlayerGameData>().playerName);
            clientTags.Add(client.GetComponent<PlayerGameData>().numTags);
            clientTagged.Add(client.GetComponent<PlayerGameData>().numTagged);
        }
    }

    public void SetBoardScores(string _name, int _tags, int _tagged)
    {
        boardItems[clientNames.IndexOf(_name)].transform.GetChild(1).GetComponent<Text>().text = "" + _tags;
        boardItems[clientNames.IndexOf(_name)].transform.GetChild(2).GetComponent<Text>().text = "" + _tagged;
    }
}
