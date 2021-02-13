using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerGameData : NetworkBehaviour {

    ScoreBoard sb;

    string inputName;
    public string playerName;
    public int numTags;
    public int numTagged;

    private void Start()
    {
        sb = GameObject.Find("Score Board").GetComponent<ScoreBoard>();
    }

    public void SetName(string _name) { inputName = _name; }
    [Command]
    void CmdSetName() { RpcSetName(inputName); }
    [ClientRpc]
    void RpcSetName(string _name) {
        playerName = _name;
    }
    /*
    [Command]
    public void CmdSetBoard() { RpcSetBoard(); }
    [ClientRpc]*/
    public void /*Rpc*/SetBoard()
    {
        sb.SetBoard();
        for (int i = 0; i < sb.clientNames.Count; i++)
        {
            sb.boardItems.Add(Instantiate(sb.prefab));
            sb.boardItems[i].transform.SetParent(sb.content.transform);
            sb.boardItems[i].transform.GetChild(0).GetComponent<Text>().text = sb.clientNames[i];
        }
    }

    public void SetScores(int _tags, int _tagged)
    {
        numTags += _tags;
        numTagged += _tagged;
        sb.SetBoardScores(playerName, numTags, numTagged);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            sb.canvas.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            sb.canvas.SetActive(false);
    }
}
