using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosts : MonoBehaviour {

    GameObject prefab;

    float timer;
    public bool countdown;

    List<GameObject> boosts = new List<GameObject>();
    List<int> boostsIndicies = new List<int>();

    public void SpawnBoosts(int _x, int _y)
    {
        for(int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(_x, _y, 6/*(int)(Random.value * 100) - 50, (int)(Random.value * 100) - 50, 6*/);
            boosts.Add(Instantiate(prefab, pos, gameObject.transform.rotation, gameObject.transform));
            boosts[i].name = "" + i;
            boostsIndicies.Add(i);
        }
    }

    public void RespawnBoosts(int _x, int _y)
    {

    }

    private void Update()
    {
        if(countdown && timer > 0)
            timer -= Time.deltaTime;
        else if(timer <= 0)
        {
            timer = 30;
        }
    }

}
