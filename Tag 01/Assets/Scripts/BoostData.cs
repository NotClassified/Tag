using UnityEngine;

public class BoostData : MonoBehaviour {

    public string type;
    [SerializeField]
    MeshRenderer rend;

    public void SetBoostData(int _randomType)
    {
        if (_randomType == 0)
        {
            type = "speed";
            rend.material.color = Color.blue;
        }
        else
        {
            type = "jump";
            rend.material.color = Color.green;
        }
    }
}
