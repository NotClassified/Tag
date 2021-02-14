using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerTag : NetworkBehaviour {

    //UI: Buttons and Texts
    public GameObject playerUI;
    public GameObject gameButtons;
    public GameObject beTaggedButton;
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
        beTaggedButton = gameButtons.transform.GetChild(0).gameObject;
        beTaggedButton.GetComponent<Button>().onClick.AddListener(BeTagged);
    }

    void BeTagged()
    {
        CmdBeTagged();
    }
    //sync tag state to server & clients
    [Command]
    void CmdBeTagged()
    {
        RpcBeTagged();
    }
    [ClientRpc]
    void RpcBeTagged()
    {
        //tag this player
        tagged = true;
        rend.material.color = Color.red;
        //get all clients to start game
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject client in clients)
        {
            client.GetComponent<PlayerTag>().StartGame();
        }
    }

    void StartGame()
    {
        if (isLocalPlayer)
        {
            //disable tag button and game over text & tag the tagged player & set UI references
            beTaggedButton.SetActive(false);
            gameOverText.text = "";
            if (tagged)
            {
                taggedText.text = "Tagged";
                TaggedTimerStart(15);
                CmdColl(true);
                collided = true;
                Invoke("CollOff", 15f);
            }
            pgd.SetBoard();
            CmdReferenceUI();
        }
    }

    [Command]
    void CmdReferenceUI()
    {
        RpcReferenceUI();
    }
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
                    CmdTag(false, true);
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

    void CollOff()
    {
        CmdColl(false);
    }
    //sync collide state to server & clients
    [Command]
    void CmdColl(bool _collided)
    {
        RpcColl(_collided);
    }
    [ClientRpc]
    void RpcColl(bool _collided)
    {
        collided = _collided;
    }

    //sync collided player referecnces to server & clients
    [Command]
    void CmdCollPlayer(GameObject _collision)
    {
        RpcCollPlayer(_collision);
    }
    [ClientRpc]
    void RpcCollPlayer(GameObject _collision)
    {
        rendC = _collision.transform.GetChild(1).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>();
        ptC = _collision.GetComponent<PlayerTag>();
        pgdC = _collision.gameObject.GetComponent<PlayerGameData>();
    }

    //sync tag state to server & clients
    [Command]
    void CmdTag(bool _tagged, bool _tag)
    {
        RpcTag(_tagged, _tag);
    }
    [ClientRpc]
    void RpcTag(bool _tagged, bool _tag)
    {
        rend.material.color = Color.black;
        rendC.material.color = Color.red;
        tagged = _tagged;
        ptC.collided = true;
        ptC.tagged = _tag;
        ptC.taggedText.text = "Tagged";
        ptC.TaggedTimerStart(5);
        pgd.SetScore(1, 0);
        pgdC.SetScore(0, 1);
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
            gameTimer = 30;
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
            CmdGameOver();
        }
    }
    [Command]
    void CmdGameOver()
    {
        RpcGameOver();
    }
    [ClientRpc]
    void RpcGameOver()
    {
        //untag tagged player
        tagged = false;
        rend.material.color = Color.black;
        clients = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject client in clients)
        {
            client.GetComponent<PlayerTag>().GameOver();
        }
    }
    void GameOver()
    {
        if (isLocalPlayer)
        {
            //show game over UI & enable tag button
            gameOverText.text = "Game Over\nName Lost";
            beTaggedButton.SetActive(true);
        }
    }
}