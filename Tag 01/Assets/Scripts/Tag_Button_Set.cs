using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tag_Button_Set : NetworkBehaviour {
    

    public GameObject tag_b;
    public GameObject not_tag_b;
    public GameObject start_b;
    public GameObject restart_b;
    public GameObject spawnpoint;

    public GameObject playerclone;
    public GameObject timer;
    public GameObject g_timer;
    public GameObject you_lostg;
    public float countdown;
    public bool iscounting = false;
    public Text textTimer;
    public float g_countdown;
    public bool g_iscounting = false;
    public Text g_textTimer;
    public Text you_lost;

    private void Start()
    {
        timer.SetActive(false);
    }
    

    public void Tag_Button()
    {
            playerclone = GameObject.Find("Local");
            Debug.Log("Action1t");
        
        if (playerclone.CompareTag("Player"))
        {
            Debug.Log("Action2t");
            playerclone.GetComponent<Tag_Manager>().Tag();
        }
    }

    public void Not_Tag_Button()
    {
        playerclone = GameObject.Find("Local");
        Debug.Log("Action1n");

        if (playerclone.CompareTag("Player"))
        {
            Debug.Log("Action2n");
            playerclone.GetComponent<Tag_Manager>().Not_Tag();
        }
    }

    public void Start_Button()
    {
        playerclone = GameObject.Find("Local");
        Debug.Log("Action1s");

        if (playerclone.CompareTag("Player"))
        {
            Debug.Log("Action2s");
            playerclone.GetComponent<Tag_Manager>().Start_Game();
        }
        countdown = 5;
        iscounting = true;
    }

    public void Restart_Button()
    {
        playerclone = GameObject.Find("Local");
        Debug.Log("Action1r");

        if (playerclone.CompareTag("Player"))
        {
            Debug.Log("Action2r");
            playerclone.GetComponent<Tag_Manager>().Restart_Game();
        }
    }

    public void Game_Count_Down()
    {
        g_countdown = 30f;
        g_iscounting = true;
    }

    void Update()
    {
        if(iscounting == true)
        {
            timer.SetActive(true);
            countdown -= Time.deltaTime;
            textTimer.text = Mathf.RoundToInt(countdown) + "";
        }

        if(countdown < 0)
        {
            iscounting = false;
            timer.SetActive(false);
        }
        if (g_iscounting == true)
        {
            g_timer.SetActive(true);
            g_countdown -= Time.deltaTime;
            g_textTimer.text = Mathf.RoundToInt(g_countdown) + "";
        }

        if (g_countdown < 0)
        {
            g_iscounting = false;
            playerclone.GetComponent<Tag_Manager>().Stop_Game();
        }
    }

}
