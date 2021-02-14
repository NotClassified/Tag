using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerBoosts : NetworkBehaviour {

    [SerializeField]
    GameObject ui;
    Text boostNam;

    private void Start()
    {
        boostNam = ui.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {

        }
    }
}
