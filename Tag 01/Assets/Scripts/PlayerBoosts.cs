﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerBoosts : NetworkBehaviour {

    [SerializeField]
    GameObject ui;
    Text boostNam;
    bool haveBoost = false;

    [SerializeField]
    GameObject prefab;
    Transform parent;

    [SerializeField]
    List<GameObject> boosts = new List<GameObject>();
    /*
    float timer;
    public bool countdown;
    */
    private void Start()
    {
        boostNam = ui.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
        boostNam.text = "";
        parent = GameObject.Find("Boosts").transform;
    }
    
    [Command]
    public void CmdSpawnBoosts(Vector3[] _randomPos, int[] _randomType) { RpcSpawnBoosts(_randomPos, _randomType); }
    [ClientRpc]
    void RpcSpawnBoosts(Vector3[] _randomPos, int[] _randomType)
    {
        foreach (Transform child in parent.transform)
            Destroy(child.gameObject);
        boosts.Clear();
        for (int i = 0; i < _randomPos.Length; i++)
        {
            boosts.Add(Instantiate(prefab, _randomPos[i], gameObject.transform.rotation, parent));
            //NetworkServer.Spawn(boosts[i]);
            boosts[i].name = "" + i;
            boosts[i].GetComponent<BoostData>().SetBoostData(_randomType[i]);
        }
        GameObject[] clients = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject client in clients)
        {
            client.GetComponent<PlayerBoosts>().boosts = boosts;
        }
    }

    /*
    private void Update()
    {
        if (countdown && timer > 0)
            timer -= Time.deltaTime;
        else if (timer <= 0)
        {
            timer = 30;
        }
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boost"))
        {
            if (collision.gameObject.GetComponent<BoostData>().type.Equals("speed") && !haveBoost)
            {
                boostNam.text = "Speed";
                haveBoost = true;
            }
            else if (collision.gameObject.GetComponent<BoostData>().type.Equals("jump") && !haveBoost)
            {
                boostNam.text = "Jump";
                haveBoost = true;
            }
            Vector3 randomPos = new Vector3((int)(Random.value * 100) - 50, 6, (int)(Random.value * 100) - 50);
            for (int i = 0; i < boosts.Count; i++)
            {
                if (randomPos == boosts[i].transform.position && i != int.Parse(collision.gameObject.name))
                {
                    randomPos = new Vector3((int)(Random.value * 100) - 50, 6, (int)(Random.value * 100) - 50);
                    i = 0;
                }
            }
            int randomType = Mathf.RoundToInt(Random.value);
            CmdRespawnBoost(int.Parse(collision.gameObject.name), randomPos, randomType);
        }
    }
    
    [Command]
    void CmdRespawnBoost(int _indexCollision, Vector3 _randomPos, int _randomType) { RpcRespawnBoost(_indexCollision, _randomPos, _randomType); }
    [ClientRpc]
    void RpcRespawnBoost(int _indexCollision, Vector3 _randomPos, int _randomType)
    {
        boosts[_indexCollision].GetComponent<BoostData>().SetBoostData(_randomType);
        boosts[_indexCollision].transform.position = _randomPos;
    }
}
