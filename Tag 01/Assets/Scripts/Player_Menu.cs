using UnityEngine;

public class Player_Menu : MonoBehaviour {

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject[] pauseMenuItems;
    [SerializeField]
    Player_Movement pm;
    [SerializeField]
    PlayerTag pt;

    public int startTagTimer;
    public int tagTimer;

    public bool gameState = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuItems[1].SetActive(true);
            pauseMenuItems[2].SetActive(false);
            pauseMenuItems[3].SetActive(false);
            pauseMenuItems[4].SetActive(false);
            pauseMenuItems[5].SetActive(false);
            pauseMenuItems[6].SetActive(false);
            pauseMenuItems[7].SetActive(false);
            if (pt.tagged)
                pauseMenuItems[8].SetActive(true);
            else
                pauseMenuItems[8].SetActive(false);
            if (!gameState)
                pauseMenuItems[0].SetActive(true);
            else
                pauseMenuItems[0].SetActive(false);
        }
    }

    public void GoToSettings()
    {
        if (!gameState)
        {
            pauseMenuItems[0].SetActive(false);
            pauseMenuItems[1].SetActive(false);
            pauseMenuItems[2].SetActive(true);
            pauseMenuItems[3].SetActive(true);
            pauseMenuItems[4].SetActive(true);
            pauseMenuItems[5].SetActive(true);
            pauseMenuItems[6].SetActive(true);
            pauseMenuItems[7].SetActive(true);
            pauseMenuItems[8].SetActive(false);
        }
    }

    public void GoBack()
    {
        pauseMenuItems[1].SetActive(true);
        pauseMenuItems[2].SetActive(false);
        pauseMenuItems[3].SetActive(false);
        pauseMenuItems[4].SetActive(false);
        pauseMenuItems[5].SetActive(false);
        pauseMenuItems[6].SetActive(false);
        pauseMenuItems[7].SetActive(false);
        if (pt.tagged)
            pauseMenuItems[8].SetActive(true);
        else
            pauseMenuItems[8].SetActive(false);
        if (!gameState)
            pauseMenuItems[0].SetActive(true);
        else
            pauseMenuItems[8].SetActive(false);
    }

    public void SetStartTagTimer(string _startTagTimer)
    {
        if (_startTagTimer != "")
            startTagTimer = int.Parse(_startTagTimer);
        else
            startTagTimer = 5;
    }

    public void SetTagTimer(string _tagTimer)
    {
        if (_tagTimer != "")
            tagTimer = int.Parse(_tagTimer);
        else
            tagTimer = 30;
    }

    public void SetViewSpeed(string _viewSpeed)
    {
        pm.viewSpeed = float.Parse(_viewSpeed);
    }

    public void GameStopButton()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&
            !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Space) && 
            !Input.GetKey(KeyCode.Tab) && !Input.GetKey(KeyCode.T) && gameState && pt.tagged)
        {
            pauseMenu.SetActive(false);
            pt.CmdGameOver();
        }
    }
}
