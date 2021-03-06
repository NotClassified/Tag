﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerTag : NetworkBehaviour {

    //UI: Buttons and Texts
    public GameObject playerUI;
    public GameObject gameButtons;
    public Text taggedText;
    Text timerText;
    Text gameTimerText;
    Text gameOverText;
    //Timers:
    float timer;
    bool countdown = false;
    float gameTimer;
    bool gameCountdown;

    GameObject[] clients;
    
    public SkinnedMeshRenderer rend; //this renderer
    public SkinnedMeshRenderer rendC; //collided player renderer
    
    public bool tagged = false;
    public bool collided;
    
    [SerializeField]
    PlayerGameData pgd;
    [SerializeField]
    PlayerBoosts pb;
    [SerializeField]
    Player_Menu ppm;
    public PlayerGameData pgdC;
    public PlayerTag pt; //this PlayerTag
    public PlayerTag ptC; //collided player PlayerTag

    public Player_Movement pm;

    private void Start()
    {
        rend = gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();
        pt = gameObject.GetComponent<PlayerTag>();
    }

    public override void OnStartLocalPlayer()
    {
        gameButtons = playerUI.transform.GetChild(0).gameObject;
        taggedText = gameButtons.transform.GetChild(1).gameObject.GetComponent<Text>();
        taggedText.text = "";
        timerText = gameButtons.transform.GetChild(3).gameObject.GetComponent<Text>();
        timerText.text = "";
        gameTimerText = gameButtons.transform.GetChild(4).gameObject.GetComponent<Text>();
        gameTimerText.text = "";
        gameOverText = gameButtons.transform.GetChild(5).gameObject.GetComponent<Text>();
        gameOverText.text = "";
    }

    public void BeTagged()
    {
        //pass array with random postions for boosts & array of random types
        Vector3[] randomPos = new Vector3[12 - GameObject.FindGameObjectsWithTag("Player").Length];
        for (int i = 0; i < randomPos.Length; i++)
        {
            int x = (int)(Random.value * 100) - 50;
            int z = (int)(Random.value * 100) - 50;
            randomPos[i] = new Vector3(x, 6, z);
            for (int j = i - 1; j >= 0; j--)
            {
                if (randomPos[i] == randomPos[j])
                {
                    x = (int)(Random.value * 100) - 50;
                    z = (int)(Random.value * 100) - 50;
                    randomPos[i] = new Vector3(x, 6, z);
                    j = i - 1;
                }
            }
        }
        int[] randomType = new int[randomPos.Length];
        for (int i = 0; i < randomPos.Length; i++)
        {
            randomType[i] = Mathf.RoundToInt(Random.value);
        }
        pb.CmdSpawnBoosts(randomPos, randomType);
        CmdBeTagged(ppm.startTagTimer, ppm.tagTimer);
    }
    //sync tag state to server & clients
    [Command]
    void CmdBeTagged(int _startTagTimer, int _tagTimer) { RpcBeTagged(_startTagTimer, _tagTimer); }
    [ClientRpc]
    void RpcBeTagged(int _startTagTimer, int _tagTimer)
    {
        //tag this player
        tagged = true;
        rend.material.color = Color.red;
        //get all clients to start game & spawn boosts
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject client in clients)
        {
            client.GetComponent<PlayerTag>().StartGame(_startTagTimer, _tagTimer);
        }
    }

    void StartGame(int _startTagTimer, int _tagTimer)
    {
        if (isLocalPlayer)
        {
            //disable game over text & tag the tagged player
            gameOverText.text = "";
            pb.boostNam.text = "";
            pb.haveBoost = false;
            ppm.gameState = true;
            //set time variables
            ppm.startTagTimer = _startTagTimer;
            ppm.tagTimer = _tagTimer;
            if (tagged)
            {
                taggedText.text = "Tagged";
                TaggedTimerStart(ppm.startTagTimer);
                CmdColl(true);
                collided = true;
                Invoke("CollOff", (ppm.startTagTimer));
            }
            //set board data & set UI references
            pgd.SetBoard();
            CmdReferenceUI();
        }
    }

    [Command]
    void CmdReferenceUI() { RpcReferenceUI(); }
    [ClientRpc]
    void RpcReferenceUI()
    {
        gameButtons = playerUI.transform.GetChild(0).gameObject;
        taggedText = gameButtons.transform.GetChild(1).gameObject.GetComponent<Text>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check if this is the local player & this is colliding a player object
        if (isLocalPlayer && collision.gameObject.CompareTag("Player"))
        {
            //set references of the collided player
            CmdCollPlayer(collision.gameObject);
            rendC = collision.gameObject.transform.GetChild(1).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();
            ptC = collision.gameObject.GetComponent<PlayerTag>();
            pgdC = collision.gameObject.GetComponent<PlayerGameData>();
            //this is intial collision
            if (!collided)
            {
                //to make sure this function won't be called again
                CmdColl(true);
                collided = true;
                //this player is tagged but the collided player isn't
                if (tagged && !ptC.tagged)
                {
                    //untag this player & tag the collided player & set the collided player to collided to aviod repeat of this method
                    //change tagged text of this player
                    CmdTag(false, true, ppm.startTagTimer);
                    taggedText.text = "";
                    gameCountdown = false;
                    gameTimer = 0;
                    gameTimerText.text = "";
                }
                else if ((tagged && ptC.tagged) || (!tagged && !ptC.tagged))
                    CollOff();
            }
            Invoke("CollOff", 5f);
        }
    }

    void CollOff() { CmdColl(false); }
    //sync collide state to server & clients
    [Command]
    void CmdColl(bool _collided) { RpcColl(_collided); }
    [ClientRpc]
    void RpcColl(bool _collided) { collided = _collided; }

    //sync collided player referecnces to server & clients
    [Command]
    void CmdCollPlayer(GameObject _collision) { RpcCollPlayer(_collision); }
    [ClientRpc]
    void RpcCollPlayer(GameObject _collision)
    {
        rendC = _collision.transform.GetChild(1).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();
        ptC = _collision.GetComponent<PlayerTag>();
        pgdC = _collision.gameObject.GetComponent<PlayerGameData>();
    }

    //sync tag state to server & clients
    [Command]
    void CmdTag(bool _tagged, bool _tag, int _startTagTimer) { RpcTag(_tagged, _tag, _startTagTimer); }
    [ClientRpc]
    void RpcTag(bool _tagged, bool _tag, int _startTagTimer)
    {
        rend.material.color = Color.black;
        rendC.material.color = Color.red;
        tagged = _tagged;
        ptC.collided = true;
        ptC.tagged = _tag;
        ptC.taggedText.text = "Tagged";
        ptC.TaggedTimerStart(_startTagTimer);
        pgd.SetScore(1, 0, 0);
        pgdC.SetScore(0, 1, 0);
    }

    void TaggedTimerStart(int _time)
    {
        //disable player movement & set and start timer
        if (!countdown)
        {
            pm.origin_speed = 0;
            pm.diagnol_speed = 0;
            timer = _time;
            countdown = true;
        }
        //turn off timer & enable player movement
        else
        {
            pm.running = false;
            pm.origin_speed = 2;
            pm.diagnol_speed = pm.origin_speed / Mathf.Sqrt(2);
            countdown = false;
            gameTimer = ppm.tagTimer;
        }
    }
    void Update()
    {
        //countdown
        if (countdown && timer > 0)
        {
            timerText.text = "You Got Tagged\n" + Mathf.RoundToInt(timer);
            timer -= Time.deltaTime;
        }
        //countdown stop
        else if (countdown && timer <= 0)
        {
            timerText.text = "";
            TaggedTimerStart(0);
        }

        //game countdown
        if (gameTimer > 0)
        {
            gameTimerText.text = "" + Mathf.RoundToInt(gameTimer);
            gameTimer -= Time.deltaTime;
            gameCountdown = true;
        }
        //game countdown stop
        else if (gameTimer <= 0 && gameCountdown)
        {
            gameTimerText.text = "";
            gameCountdown = false;
            taggedText.text = "";
            pgd.SetScore(0, 0, 1);
            CmdGameOver();
        }
    }
    [Command]
    public void CmdGameOver() { RpcGameOver(); }
    [ClientRpc]
    void RpcGameOver()
    {
        //untag tagged player
        tagged = false;
        rend.material.color = Color.black;
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject client in clients)
        {
            client.GetComponent<PlayerTag>().GameOver(pgd.playerName);
        }
    }
    void GameOver(string _name)
    {
        if (isLocalPlayer)
        {
            //make sure player can move
            pm.running = false;
            pm.origin_speed = 2;
            pm.diagnol_speed = pm.origin_speed / Mathf.Sqrt(2);
            //reset timers
            timer = 0;
            countdown = false;
            timerText.text = "";
            gameTimer = 0;
            gameCountdown = false;
            gameTimerText.text = "";
            //show game over UI & enable tag button
            taggedText.text = "";
            gameOverText.text = "Game Over\n" + _name + " Lost";
            ppm.gameState = false;
            Invoke("DisableGameOverText", 3f);
        }
    }
    void DisableGameOverText() { gameOverText.text = ""; }
}