using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //---------------GAME MANAGER STUFF--------------------
    public static GameManager instance;
    public void MakeSingleton()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //-----------------------------------------------------



    //------------------PLAYER STUFF-----------------------
    public static Player playerInstance;
    //-----------------------------------------------------








    //--------------------------------------MAIN METHODS-------------------------------------------
    public UIMgr UIManager;
    public AudioMgr AudioManager;

    bool isPaused = false;

    private void Awake()
    {
        MakeSingleton();
        Props.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInstance.HP = Props.GetValue("starthp");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            ResumeGame();
        }
        else if(Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        UIManager.PauseGame();
        AudioManager.ChangeSnapshot("InPauseVolume", 0.35f);
    }

    void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        UIManager.ResumeGame();
        AudioManager.ChangeSnapshot("InGameVolumes", 0.35f);
    }
}
