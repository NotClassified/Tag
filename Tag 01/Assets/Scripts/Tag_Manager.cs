using UnityEngine;
using UnityEngine.Networking;

public class Tag_Manager : NetworkBehaviour {
    
    public bool enteredC;
    public Player_Movement pms;
    public Tag_Button_Set tbs;
    public GameObject gtbs;
    [SerializeField] Behaviour pm;

    public void Start()
    {
        gtbs = GameObject.Find("/Tag_Button Manager");
        tbs = gtbs.GetComponent<Tag_Button_Set>();
        tbs.tag_b.SetActive(true);
        tbs.not_tag_b.SetActive(true);
        tbs.start_b.SetActive(false);
    }
    public void Start_Game()
    {
        pm.enabled = false;
        tbs.start_b.SetActive(false);
        tbs.restart_b.SetActive(true);
        Invoke("Tag_Player_Out", 5);
    }

    public void Stop_Game()
    {
        tbs.you_lost.enabled = true;
        tbs.you_lostg.SetActive(true);
        tbs.g_timer.SetActive(false);
        tbs.g_textTimer.enabled = true;
        Invoke("Stop_Game2", 0.5f);
    }

    public void Stop_Game2()
    {
        tbs.restart_b.SetActive(true);
        pms.enabled = false;
    }

    public void Restart_Game()
    {
        tbs.g_countdown = 30f;
        gameObject.transform.position = tbs.spawnpoint.transform.position;
        gameObject.transform.rotation = tbs.spawnpoint.transform.rotation;
        tbs.playerclone.GetComponent<Player_Movement>().enabled = true;
        tbs.you_lost.enabled = false;
        tbs.you_lostg.SetActive(false);
        tbs.tag_b.SetActive(true);
        tbs.not_tag_b.SetActive(true);
        tbs.restart_b.SetActive(true);
    }

    void Tag_Player_Out()
    {
        pm.enabled = true;
        gtbs.GetComponent<Tag_Button_Set>().Game_Count_Down();
    }

    public void Tag()
    {
        tbs.start_b.SetActive(true);
        pms.gameObject.GetComponent<Player_Movement>().tagged = true;
        Debug.Log("Button Tag");
        Invoke("D_Tag_B", 0.2f);
    }
    
    public void Not_Tag()
    {
        pms.gameObject.GetComponent<Player_Movement>().tagged = false;
        Debug.Log("Button Not Tag");
        Invoke("D_Tag_B", 0.2f);
    }

    void D_Tag_B()
    {
        tbs.tag_b.SetActive(false);
        tbs.not_tag_b.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Tag1");

            if (pms.tagged == true && collision.gameObject.GetComponent<Player_Speed>().player_speed == 10)
            {
                Invoke("Tag_Player", 0.5f);
                enteredC = true;
            }

            if (pms.tagged == false && collision.gameObject.GetComponent<Player_Speed>().player_speed == 13)
            {
                Invoke("UnTag_Player", 0.5f);
                enteredC = true;
            }
        }
    }
    

    public void Tag_Player()
    {
        pms.tagged = false;
        Debug.Log("TAG2");
        Invoke("Enter_Expire", 0.5f);
        tbs.g_iscounting = false;
        tbs.g_textTimer.enabled = false;
        tbs.g_timer.SetActive(false);
    }

    public void UnTag_Player()
    {
        pms.tagged = true;
        Debug.Log("TAG33");
        pm.enabled = false;
        Invoke("Enter_Expire", 0.5f);
        tbs.iscounting = true;
        tbs.countdown = 5f;
        Invoke("Tag_Player_Out", 5f);
    }

    void Enter_Expire()
    {
        enteredC = false;
    }
    

}
