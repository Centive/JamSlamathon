using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //DEBUGGING
    public bool win = false;

    //Gameobjs
    public GameObject mPlayer_prefab;
    private GameObject mPlayer;

    //Components
    public Transform mCheckpoint;

    //Variables

    /* (SELECTING CHECKPOINTS):
     * Store a vector list of save positions so 
     * the player can select which last save pos 
     * they want to go back to.
     * 
     * Create a checkpoint manager script that
     * pushes the transform.position 
     */

    /* (SAVING PROGRESS):
     * if we are saving player's progress ie(increased health, or items)
     * must save player progress in a notepad or xml (Data driven)
     * so when we spawn the player we will read the information in the 
     * notepad, and copy it to the newly spawned player.
     */

    // Use this for initialization
    void Start()
    {
        //GAME START
        //-Set Checkpoint to origin
        //-Spawn player to save position (should be the first one) 
            // -Set mPlayer obj to point at the active player in scene
        mPlayer = Instantiate(mPlayer_prefab, mCheckpoint.position, Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //1. Check if player is dead in the game
            //Respawn to last save
        if(!mPlayer)
        {
            //Spawn another player 
                // -Set mPlayer obj to point at the active player in scene
            mPlayer = Instantiate(mPlayer_prefab, mCheckpoint.position, Quaternion.identity) as GameObject;
            Debug.Log("Loaded new :: " + mPlayer.name);
        }

        //2. Check if player has finished the game
            // PLACE HOLDER :: Reload the level (for now)
        if(win)
        {
            Debug.Log("Win condition is true, Reloading scene...");
            //Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            // FINISH       :: Load to main menu
            //...
    }
    
    //Checks for win condition
    bool IsWin()
    {
        //Check for win
        bool isWin = false;
        
        return isWin;
    }



    //Debugging funcs
    public void ToggleWin()
    {
        win = true;
    }
}