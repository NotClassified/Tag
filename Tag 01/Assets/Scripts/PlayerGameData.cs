using UnityEngine;
using UnityEngine.Networking;

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
    void CmdSetName() { RpcSetName(); }
    [ClientRpc]
    void RpcSetName() {
        playerName = inputName;

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
