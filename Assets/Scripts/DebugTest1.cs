using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest1 : MonoBehaviour
{
    /* Notes! 1
     * 
     * When creating a room with a collection of blocks with props:
     * - Make sure to set the child object's local position to zero so that
     * when it will spawn at the correct position relative to it's parent.
     * 
     * Like
     * Room [parent of Blocks]
     *  Block [parent of props][child of room]
     *      Props [child of block]
     * 
     * CODE:
     * //Start
        public GameObject parentObj;
        public GameObject newObj;
        public Vector3 parentObj_Position = Vector3.zero;
        public Vector3 newObj_Position = Vector3.zero;

        void Start()
        {
            parentObj = new GameObject("ParentObj");
            parentObj.transform.position = parentObj_Position;
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                newObj = new GameObject("newObj");
                newObj.transform.parent = parentObj.transform;
                newObj.transform.localPosition = new Vector3(0, 0);
            }
        }
     * //End
     */
     

    void Start()
    {
    }

    void Update()
    {
    }
}