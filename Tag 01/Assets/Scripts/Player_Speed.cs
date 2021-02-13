using UnityEngine;
using UnityEngine.Networking;

public class Player_Speed : NetworkBehaviour {

    public float player_speed;
    public GameObject player;

	void FixedUpdate ()
    {
        Invoke("CmdSet_Speed", 2.5f);
	}

    [Command]
    void CmdSet_Speed()
    {
        player_speed = GetComponent<Player_Movement>().speed;
        Invoke("RpcSet_Speed", 2.5f);
    }

    [ClientRpc]
    void RpcSet_Speed()
    {
        player_speed = GetComponent<Player_Movement>().speed;
    }
}
