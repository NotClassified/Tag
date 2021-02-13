using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddColliders : MonoBehaviour {

    public int numChilds;
    public GameObject child;
    public Mesh childMesh;

    void Start () {
        numChilds = gameObject.transform.childCount;
        for(int i = 0; i < numChilds; i++)
        {
            child = gameObject.transform.GetChild(i).gameObject;
            child.AddComponent<MeshCollider>();
            childMesh = child.GetComponent<MeshFilter>().sharedMesh;
            child.GetComponent<MeshCollider>().sharedMesh = childMesh;
            child.GetComponent<MeshCollider>().convex = true;
        }
    }
}
