//using UnityEngine;
//using System.Collections;
//using UnityEngine.SceneManagement;
//public class GameManager : MonoBehaviour
//{
//    //singleton
//    public static GameManager instance = null;
//    public enum GameState
//    {
//        None,
//        Intro,
//        Profiles,
//        Main_Menu,
//        Pause_Menu,
//        Options,
//        Gameplay,
//        Quick_Saving,
//        Cutscene
//    }
//    public GameState curGameState { get; set; }
//    void Awake()
//    {
//        //Check if there are any gamemanagers
//        if (instance == null)
//        {
//            //if not then set it
//            instance = this;
//        }
//        else if (instance != this)
//        {
//            //if there are then destroy this
//            Destroy(gameObject);
//        }
//        //must exist through scenes
//        DontDestroyOnLoad(gameObject);
//    }
//    //Puts the singleton back to null when exiting the app
//    public void OnApplicationQuit()
//    {
//        instance = null;
//    }
//    public void UpdateGameState()
//    {
//        switch (curGameState)
//        {
//            case GameState.None:
//                {
//                    //Literally none
//                    break;
//                }
//            case GameState.Intro:
//                {
//                    //Play intro or skip
//                    break;
//                }
//            case GameState.Profiles:
//                {
//                    //Create new game or load a game file
//                    break;
//                }
//            case GameState.Main_Menu:
//                {
//                    //Main menu selection screen
//                    break;
//                }
//            case GameState.Pause_Menu:
//                {
//                    //Pause menu selection screen
//                    break;
//                }
//            case GameState.Options:
//                {
//                    //Adjust audio, video, and controls
//                    break;
//                }
//            case GameState.Gameplay:
//                {
//                    //Gameplay happens
//                    break;
//                }
//            case GameState.Quick_Saving:
//                {
//                    //Game wait until saving is done
//                    break;
//                }
//            case GameState.Cutscene:
//                {
//                    //No gameplay, lore is happening...
//                    break;
//                }
//        }
//    }
//    void Update()
//    {
//    }
//}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private LevelEditor levelEditor = null;
    public CameraBehavior camera = null;
    public GameObject player;

    public enum GameState
    {
        None,
        Play,
        Level_Edit,
        Level_Gen
    }

    public static GameState gameState = GameState.Level_Edit;

    void Start()
    {
        levelEditor = GetComponent<LevelEditor>();
        camera = Camera.main.GetComponent<CameraBehavior>();
       
    }

    void Update()
    {
        
    }

    public void PlayTest()
    {
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            Instantiate(player, new Vector3(0, 1, 1), Quaternion.identity);
            levelEditor.enabled = false;
            levelEditor.gridEditor.ShowCells(false);
            camera.cameraState = CameraBehavior.CameraState.FollowPlayer;
            camera.gameObject.transform.position = camera.position;
            camera.gameObject.transform.eulerAngles = camera.rotation;
            Camera.main.orthographic = true;
        }
    }

    public void EditLevel()
    {
        Camera.main.orthographic = false;
        levelEditor.enabled = true;
        levelEditor.gridEditor.ShowCells(true);
        camera.cameraState = CameraBehavior.CameraState.ControlCamera;
        camera.gameObject.transform.position = camera.position;
        camera.gameObject.transform.eulerAngles = camera.rotation;

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    void OnGUI()
    {
        //Play
        if (GUI.Button(new Rect(0f, 80f, 80f, 20f), "Play")) { PlayTest(); }
        //Play
        if (GUI.Button(new Rect(0f, 100f, 80f, 20f), "Edit")) { EditLevel(); }
    }
}