using UnityEngine;
using UnityEngine.Networking;

public class PlayerGameData : NetworkBehaviour {
    
    ScoreBoard sb;

    public string inputName;
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
    void RpcSetName(string _name)
    {
        playerName = _name;
        Debug.Log("PGD SetName: " + playerName);
    }

    public void SetScores(int _tags, int _tagged)
    {
        Debug.Log("PGD Set Scores");
        numTags += _tags;
        numTagged += _tagged;
        sb.CmdSetBoardScores(playerName, numTags, numTagged);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            sb.canvas.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            sb.canvas.SetActive(false);
    }
}
