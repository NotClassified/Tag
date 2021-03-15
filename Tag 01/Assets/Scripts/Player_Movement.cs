using UnityEngine;
using UnityEngine.Networking;

public class Player_Movement : NetworkBehaviour {
    
    public float speed;
    public float origin_speed;
    public float diagnol_speed;
    public float jump_speed;
    public float velocityY;
    int localRotation;
    public float viewSpeed;
    public Rigidbody rb;
    Transform crb;
    public bool tagged;
    bool diagnolDirection;
    public bool running;
    bool wPressing;
    bool dPressing;
    bool aPressing;
    bool sPressing;
    public GameObject networkmanager;
    public NetworkIdentity nwID;
    AnimationStateController asc;
    public PlayerTag pt;
    GameObject character;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    Player_Menu ppm;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        gameObject.name = "Local";
        character = gameObject.transform.GetChild(1).gameObject;
        asc = character.GetComponent<AnimationStateController>();
        crb = character.GetComponent<Transform>();
        networkmanager = GameObject.Find("Network Manager");
        networkmanager.GetComponent<Host_Game>().OnEnterGame();
        diagnol_speed = speed / Mathf.Sqrt(2);
        origin_speed = speed;
    }
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeSelf)
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&
                !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Space) && 
                !Input.GetKey(KeyCode.Tab) && !Input.GetKey(KeyCode.T))
            {
                pauseMenu.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeSelf)
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&
                !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Space) && 
                !Input.GetKey(KeyCode.Tab) && !Input.GetKey(KeyCode.T))
            {
                pauseMenu.SetActive(false);
            }
        }
        if (pauseMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else if (!pauseMenu.activeSelf)
            Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKeyDown(KeyCode.T) && !ppm.gameState)
        {
            pt.BeTagged();
        }

        float x = Input.GetAxis("Mouse X");
        if (Input.GetKey(KeyCode.RightArrow))
            x = viewSpeed;
        if (Input.GetKey(KeyCode.LeftArrow))
            x = -viewSpeed;
        transform.Rotate( 0, x, 0);
        localRotation = (int)(transform.eulerAngles.y) + 180;
        if (Input.GetKey(KeyCode.W))
        {
            rb.transform.position += transform.forward * Time.deltaTime * speed;
            wPressing = true;
            if (!diagnolDirection)
                crb.eulerAngles = new Vector3(0, localRotation, 0);
        }
        else
            wPressing = false;

        if (Input.GetKey(KeyCode.D))
        {
            rb.transform.position += transform.right * Time.deltaTime * speed;
            dPressing = true;
            if (!diagnolDirection)
                crb.eulerAngles = new Vector3(0, localRotation + 90, 0);
        }
        else
            dPressing = false;

        if (Input.GetKey(KeyCode.A))
        {
            rb.transform.position -= transform.right * Time.deltaTime * speed;
            aPressing = true;
            if (!diagnolDirection)
                crb.eulerAngles = new Vector3(0, localRotation - 90, 0);
        }
        else
            aPressing = false;

        if (Input.GetKey(KeyCode.S))
        {
            rb.transform.position -= transform.forward * Time.deltaTime * speed;
            sPressing = true;
            if (!diagnolDirection)
                crb.eulerAngles = new Vector3(0, localRotation + 180, 0);
        }
        else
            sPressing = false;

        if(wPressing && dPressing)
            crb.eulerAngles = new Vector3(0, localRotation + 45, 0);
        if (dPressing && sPressing)
            crb.eulerAngles = new Vector3(0, localRotation + 135, 0);
        if (wPressing && aPressing)
            crb.eulerAngles = new Vector3(0, localRotation - 45, 0);
        if (aPressing && sPressing)
            crb.eulerAngles = new Vector3(0, localRotation - 135, 0);

        if(!(wPressing && dPressing) && !(dPressing && sPressing) && !(wPressing && aPressing) && !(aPressing && sPressing))
        {
            diagnolDirection = false;
            speed = origin_speed;
        }
        else
        {
            diagnolDirection = true;
            speed = diagnol_speed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))
            asc.AnimationStates("IsWalking", true);
        else
            asc.AnimationStates("IsWalking", false);

        velocityY = rb.velocity.y;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(velocityY < 0.001f && velocityY > -0.001f)
            {
                rb.AddForce(0f, jump_speed, 0f);
                asc.AnimationStates("InAir", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (velocityY < 0.01f && velocityY > -0.01f)
                asc.AnimationStates("Backflip", true);
            Invoke("EndBackflip", .5f);
        }
        if (velocityY < 0.00001f && velocityY > -0.00001f)
            asc.AnimationStates("InAir", false);
        else
            Invoke("GoInAir", 0.05f);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            asc.AnimationStates("IsRunning", true);
            if(origin_speed != 0 && diagnol_speed != 0)
            {
                running = true;
                origin_speed += 3;
                diagnol_speed = origin_speed / Mathf.Sqrt(2);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            asc.AnimationStates("IsRunning", false);
            if (origin_speed != 0 && diagnol_speed != 0 && running)
            {
                running = false;
                origin_speed -= 3;
                diagnol_speed = origin_speed / Mathf.Sqrt(2);
            }
        }
    }


    void GoInAir()
    {
        if(!(velocityY < 0.001f && velocityY > -0.001f))
            asc.AnimationStates("InAir", true);
    }
    void EndBackflip()
    {
        asc.AnimationStates("Backflip", false);
    }
}
