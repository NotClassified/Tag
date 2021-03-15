using UnityEngine;
using UnityEngine.Networking;
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
    public List<int> clientLosts = new List<int>();

    private void Start()
    {
        canvas.SetActive(false);
    }

    public void SetBoard()
    {
        boardItems.Clear();
        clientNames.Clear();
        clientTags.Clear();
        clientTagged.Clear();
        clientLosts.Clear();
        foreach (Transform child in content.transform)
            Destroy(child.gameObject);
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject client in clients)
        {
            clientNames.Add(client.GetComponent<PlayerGameData>().playerName);
            clientTags.Add(client.GetComponent<PlayerGameData>().numTags);
            clientTagged.Add(client.GetComponent<PlayerGameData>().numTagged);
            clientLosts.Add(client.GetComponent<PlayerGameData>().numLosts);
        }
    }
}
