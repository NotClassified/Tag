using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerGameData : NetworkBehaviour {

    ScoreBoard sb;
    
    public string playerName;
    public int numTags;
    public int numTagged;
    public int numLosts;


    private void Start()
    {
        sb = GameObject.Find("Score Board").GetComponent<ScoreBoard>();
    }
    
    [Command]
    void CmdSetName(string _name) { RpcSetName(_name); }
    [ClientRpc]
    void RpcSetName(string _name)
    {
        if(_name != "")
            playerName = _name;
        else
            playerName = "Unknown";
    }

    public void SetBoard()
    {
        sb.SetBoard();
        for (int i = 0; i < sb.clientNames.Count; i++)
        {
            sb.boardItems.Add(Instantiate(sb.prefab));
            sb.boardItems[i].transform.SetParent(sb.content.transform);
            sb.boardItems[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            sb.boardItems[i].transform.GetChild(0).GetComponent<Text>().text = sb.clientNames[i];
            sb.boardItems[i].transform.GetChild(1).GetComponent<Text>().text = "" + sb.clientTags[i];
            sb.boardItems[i].transform.GetChild(2).GetComponent<Text>().text = "" + sb.clientTagged[i];
            sb.boardItems[i].transform.GetChild(3).GetComponent<Text>().text = "" + sb.clientLosts[i];
        }
    }

    public void SetScore(int _tags, int _tagged, int _losts)
    {
        if (hasAuthority)
        {
            numTags += _tags;
            numTagged += _tagged;
            numLosts += _losts;
            CmdSetScores(numTags, numTagged, numLosts);
        }
    }
    [Command]
    void CmdSetScores(int _tags, int _tagged, int _losts) { RpcSetScores(_tags, _tagged, _losts); }
    [ClientRpc]
    void RpcSetScores(int _tags, int _tagged, int _losts)
    {
        sb.boardItems[sb.clientNames.IndexOf(playerName)].transform.GetChild(1).GetComponent<Text>().text = "" + _tags;
        sb.boardItems[sb.clientNames.IndexOf(playerName)].transform.GetChild(2).GetComponent<Text>().text = "" + _tagged;
        sb.boardItems[sb.clientNames.IndexOf(playerName)].transform.GetChild(3).GetComponent<Text>().text = "" + _losts;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            sb.canvas.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            sb.canvas.SetActive(false);
    }
}
