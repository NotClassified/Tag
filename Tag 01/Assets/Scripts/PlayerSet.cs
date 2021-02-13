using UnityEngine;
using UnityEngine.Networking;

public class PlayerSet : NetworkBehaviour {

    [SerializeField]
    Behaviour[] components;

    [SerializeField]
    GameObject playerUI;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            playerUI.SetActive(false);
            for (int i = 0; i < components.Length; i++)
            {
                components[i].enabled = false;
            }
        }
    }
}
