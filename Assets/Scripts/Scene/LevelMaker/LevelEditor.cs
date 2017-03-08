using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class LevelEditor : GameManager
{
    private class RoomManager
    {
        public void LoadManager()
        {
            UnityEngine.Object[] listRooms = Resources.LoadAll("Rooms/", typeof(TextAsset));
            room_index = listRooms.Length;
        }
        public void NewRoom(Vector2 size)
        {
            room = new Room(room_index, Vector3.zero, size);
        }
        public void DeleteRoom()
        {
            room = null;
        }
        public void SaveRoom()
        {
            if (room == null)
            {
                print("Cannot save if there is no room");
                return;
            }
            room.Save(save_Path + "Room_" + room_index + ".xml");
        }
        public Room LoadRoom(string path)
        {
            return room = Room.Load(Path.Combine(Application.dataPath, path));
        }
        public Room GetRoom() { return room; }

        public void AddBlock(Vector3 pos, int index)
        {
            if (room == null)
            {
                print("Cannot add a block, when room is null");
                return;
            }

            //Add a block in the list
            room.blocks.Add(new Block(index, pos));
        }
        public void RemoveBlock(Vector3 pos)
        {
            if (room == null)
            {
                print("Cannot remove a block, when room is null");
            }

            //Search for the block in the list
            foreach (Block block in room.blocks)
            {
                //If found, then remove it
                if (pos == block.GetPosition())
                {
                    room.blocks.Remove(block);
                    break;
                }
            }
        }
        public void AddProp(Vector3 pos, int index)
        {
            if (room == null)
            {
                print("Cannot add a prop, when room is null");
                return;
            }

            //Add a block in the list
            room.props.Add(new Prop(index, pos));
        }
        public void RemoveProp(Vector3 pos)
        {
            if (room == null)
            {
                print("Cannot remove a prop, when room is null");
            }

            //Search for the block in the list
            foreach (Prop prop in room.props)
            {
                //If found, then remove it
                if (pos == prop.GetPosition())
                {
                    room.props.Remove(prop);
                    break;
                }
            }
        }

        private int room_index = 0;
        private Room room = null;
        private string save_Path = "../JamSlamathon/Assets/Resources/Rooms/";
    }
    public class GridEditor
    {
        public void LoadEditor()
        {
            //Get the cell prefab to create the grid
            cell_Prefab = Resources.Load("Prefabs/LevelEditor/Cell") as GameObject;

            //Create containers for holding gameobjects
            cell_Container = new GameObject("Cell_Container");
            block_Container = new GameObject("Block_Container");
            prop_Container = new GameObject("Prop_Container");
            room_Container = new GameObject("Room");

            //Set positions
            cell_Container.transform.position
                = room_Container.transform.position = Vector3.zero;

            //Set their parent transform
            prop_Container.transform.parent =
                block_Container.transform.parent = room_Container.transform;
        }
        public void InitGrid(Vector2 size)
        {
            //Clear props, blocks, and cells in the scene
            ClearAll();

            //Set the size
            gridSize = size;

            //Fill the block holder
            blocks = new GameObject[(int)gridSize.x, (int)gridSize.y, 4];
            for (int z = 0; z < 3; z++)
                for (int y = 0; y < (int)gridSize.y; y++)
                    for (int x = 0; x < (int)gridSize.x; x++)
                        blocks[x, y, z] = null;

            //Create the prop holder
            props = new GameObject[,,] { };

            //Create grid cells
            Vector3 startPos = Vector3.zero;
            for (int y = 0; y < gridSize.y; y++)
                for (int x = 0; x < gridSize.x; x++)
                {
                    Vector3 newPos = new Vector3(startPos.x + (float)x, startPos.y + (float)y, startPos.z);
                    GameObject newCell = Instantiate(cell_Prefab, newPos, Quaternion.identity);
                    newCell.transform.parent = cell_Container.transform;
                }
        }
        public void InitByRoom(Room room)
        {
            InitGrid(room.GetSize());

            //Add blocks
            foreach (Block block in room.blocks)
            {
                GameObject newBlock = Resources.Load("Prefabs/Blocks/Block_" + block.index) as GameObject;
                string[] coord = block.sPosition.Split(',');
                Vector3 position = new Vector3
                    (float.Parse(coord[0])
                    , float.Parse(coord[1])
                    , float.Parse(coord[2]));

                AddBlock(newBlock, position);
            }

            //Add Props
            foreach (Prop prop in room.props)
            {
                GameObject newProp = Resources.Load("Prefabs/Props/Prop_" + prop.index) as GameObject;
                string[] coord = prop.sPosition.Split(',');
                Vector3 position = new Vector3
                    (float.Parse(coord[0])
                    , float.Parse(coord[1])
                    , float.Parse(coord[2]));

                AddProp(newProp, position);
            }
        }
        public void InitEditComp(EditType type)
        {
            switch(type)
            {
                case EditType.Block:
                    {

                        break;
                    }
                case EditType.Prop:
                    {
                        break;
                    }
            }
        } //Need fix
        public void AddBlock(GameObject block, Vector3 pos)
        {
            //Check if theres block in that position
            if (blocks[(int)pos.x, (int)pos.y, (int)pos.z] != null)
            {
                print("Cannot add, since it is occupied by a block");
                return;
            }

            //Create the obj at the position
            blocks[(int)pos.x, (int)pos.y, (int)pos.z] = Instantiate(block, pos, Quaternion.identity);
            blocks[(int)pos.x, (int)pos.y, (int)pos.z].transform.parent = block_Container.transform;
            blocks[(int)pos.x, (int)pos.y, (int)pos.z].layer = ((int)pos.z + 10);
        }
        public void RemoveBlockAtPosition(Vector3 pos)
        {
            //Check if theres no block in that position
            if (blocks[(int)pos.x, (int)pos.y, (int)pos.z] == null)
            {
                print("Cannot remove, since block doesn't exist");
                return;
            }

            //Destroy the gameobject
            Destroy(blocks[(int)pos.x, (int)pos.y, (int)pos.z]);
        }
        public void AddProp(GameObject prop, Vector3 pos)
        {
            GameObject newProp = Instantiate(prop, pos, Quaternion.identity);
            newProp.transform.parent = prop_Container.transform;
        }   // Need Fix
        public void RemovePropAtPosition(Vector3 pos)
        {
            // Nothing
        }       // Need fix
        public void ShowCells(bool hide)
        {
            cell_Container.SetActive(hide);
        }
        public void ClearBlocks()
        {
            if (block_Container == null)
            {
                print("Cannot clear if container is null");
                return;
            }

            blocks = null;
            foreach (Transform child in block_Container.transform)
            {
                Destroy(child.gameObject);
            }
        }
        public void ClearProps()
        {
            if (prop_Container == null)
            {
                print("Cannot clear if container is null");
                return;
            }

            props = null;
            foreach (Transform child in prop_Container.transform)
            {
                Destroy(child.gameObject);
            }
        }
        public void ClearCells()
        {
            if (prop_Container == null)
            {
                print("Cannot clear if container is null");
                return;
            }

            foreach (Transform child in cell_Container.transform)
            {
                Destroy(child.gameObject);
            }
        }
        public void ClearAll()
        {
            ClearBlocks();
            ClearProps();
            ClearCells();
        }
        //
        private Vector2 gridSize            = Vector2.zero;
        private GameObject cell_Prefab      = null;
        public GameObject cell_Container    = null;
        private GameObject block_Container  = null;
        private GameObject prop_Container   = null;
        private GameObject room_Container   = null;
        private GameObject[,,] blocks       = null;
        private GameObject[,,] props        = null;
    }

    public enum Actions
    {
        Place,
        Delete
    }
    public enum EditType
    {
        Block,
        Prop
    }
    public enum CompositionPlane
    {
        Foreground,
        Middleground,
        Background
    }

    public Actions action = Actions.Place;
    public EditType editType = EditType.Block;
    public CompositionPlane workLayer = CompositionPlane.Middleground;

    public UnityEngine.Object[] availableBlocks = null;
    public UnityEngine.Object[] availableProps = null;
    public UnityEngine.Object[] availableEntities = null;

    public Vector2 size = new Vector2(32f, 32f);
    public GameObject selectedObject = null;
    public GridEditor gridEditor = new GridEditor();
    private RoomManager roomManager = new RoomManager();


    private bool IsEdit = false;
    private bool IsPlayTest = false;

    void Start()
    {
        availableBlocks = Resources.LoadAll("Prefabs/Blocks/", typeof(GameObject));
        availableProps = Resources.LoadAll("Prefabs/Props/", typeof(GameObject));
        availableEntities = Resources.LoadAll("Prefabs/Entities/", typeof(GameObject));
        selectedObject = availableBlocks[0] as GameObject;

        workLayer = CompositionPlane.Foreground;
        gridEditor.LoadEditor();
        roomManager.LoadManager();

        gridEditor.InitGrid(size);
        roomManager.NewRoom(size);

        ChangePlane(workLayer);


        //GUI
        selStrings = new string[availableBlocks.Length];
        for (int i = 0; i < availableBlocks.Length; i++)
            selStrings[i] = availableBlocks[i].name;
    }
    void Update()
    {
        if (gameState == GameState.Level_Edit)
        {
            switch (action)
            {
                case Actions.Place:
                    {
                        if (editType == EditType.Block) { PlaceBlock(); }
                        //if (editType == EditType.Prop) { PlaceProp(); }
                        break;
                    }
                case Actions.Delete:
                    {
                        if (editType == EditType.Block) { RemoveBlock(); }
                        //if (editType == EditType.Prop) { RemoveProp(); }
                        break;
                    }
            }
        }
    }

    void PlaceBlock()
    {
        if (Input.GetMouseButton(0))
        {
            Transform hit = CastRay();
            if (hit)
            {
                //Add to scene
                gridEditor.AddBlock(selectedObject, new Vector3(hit.position.x, hit.position.y, (float)workLayer));

                //Save to room [Extract index from the selected block]
                int moveToPos = selectedObject.name.LastIndexOf("_") + 1;
                int index = Int32.Parse(
                    selectedObject.name.Substring(
                        moveToPos,
                        selectedObject.name.Length - moveToPos));
                roomManager.AddBlock(new Vector3(hit.position.x, hit.position.y, (float)workLayer), index);
            }
        }
    }
    void RemoveBlock()
    {
        if (Input.GetMouseButton(0))
        {
            Transform hit = CastRay();
            if (hit)
            {
                gridEditor.RemoveBlockAtPosition(new Vector3(hit.position.x, hit.position.y, (float)workLayer));

                //Save to room [Extract index from the selected block]
                int moveToPos = selectedObject.name.LastIndexOf("_") + 1;
                int index = Int32.Parse(
                    selectedObject.name.Substring(
                        moveToPos,
                        selectedObject.name.Length - moveToPos));
                roomManager.RemoveBlock(new Vector3(hit.position.x, hit.position.y, (float)workLayer));
            }
        }
    }

    void SaveRoom()
    {
        roomManager.SaveRoom();
    }
    void LoadRoom()
    {
        string path = Path.Combine(Application.dataPath, "../Assets/Resources/Rooms/" + roomName + ".xml");
        Room loadRoom = roomManager.LoadRoom(path);
        gridEditor.InitByRoom(loadRoom);
    }

    void ChangePlane(CompositionPlane plane)
    {
        workLayer = plane;
        gridEditor.cell_Container.transform.position = new Vector3(0f, 0f, (float)workLayer);
    }

    Transform CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        return (hit.collider != null) ? hit.collider.transform : null;
    }

    void OnGUI()
    {
        //Load UI
        if (GUI.Button(new Rect(0f, 0f, 80f, 20f), "Load"))
        {
            LoadRoom();
        }
        roomName = GUI.TextField(new Rect(80f, 0f, 100f, 20f), roomName, 10);

        //Save UI
        if (GUI.Button(new Rect(0f, 20f, 80f, 20f), "Save")) { SaveRoom(); }

        //tools window
        if (GUI.Button(new Rect(0f, 40f, 80f, 20f), "Tools")) { toolWindow = (toolWindow) ? false : true; }
        if (toolWindow) { wTools = GUI.Window(0, wTools, ToolsWindow, "Tools"); }

        //blocks window
        if (GUI.Button(new Rect(0f, 60f, 80f, 20f), "Blocks")) { blockWindow = (blockWindow) ? false : true; }
        if (blockWindow) { wBlocks = GUI.Window(1, wBlocks, BlocksWindow, "Blocks"); }
    }
    private string roomName = "";
    private bool toolWindow = true;
    private Rect wTools = new Rect(100f, 0f, 210f, 70);
    void ToolsWindow(int id)
    {
        if (GUI.Button(new Rect(0f, 20f, 70f, 20f), "Fground")) { ChangePlane(CompositionPlane.Foreground); }
        if (GUI.Button(new Rect(70f, 20f, 70f, 20f), "Mground")) { ChangePlane(CompositionPlane.Middleground); }
        if (GUI.Button(new Rect(140f, 20f, 70f, 20f), "Bground")) { ChangePlane(CompositionPlane.Background); }

        if (GUI.Button(new Rect(0f, 40f, 70f, 20f), "Place")) { action = Actions.Place; }
        if (GUI.Button(new Rect(70f, 40f, 70f, 20f), "Delete")) { action = Actions.Delete; }
        GUI.DragWindow();
    }

    private bool blockWindow = true;
    private Rect wBlocks = new Rect(100f, 100f, 210f, 300f);
    public int selGridInt = 0;
    public string[] selStrings;
    void BlocksWindow(int id)
    {
        selGridInt = GUI.SelectionGrid(new Rect(25, 25, 150, 50), selGridInt, selStrings, selStrings.Length - 1);
        selectedObject = availableBlocks[selGridInt] as GameObject;
        GUI.DragWindow();
    }

    //

    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //

    //
    //
    //public GameObject selectedObject = null;

    //private RoomManager roomManager = new RoomManager();
    //private GridEditor gridEditor = new GridEditor();

    //void Start()
    //{
    //    roomManager.LoadRoomManager();
    //    gridEditor.LoadGridEditor();

    //    
    //    
    //    

    //}

    //void InitNewRoom()
    //{
    //    roomManager.NewRoom();
    //    gridEditor.InitGrid();
    //}

    //void ClearRoom()
    //{
    //    roomManager.ClearBlocks();
    //    gridEditor.InitGrid();
    //}
    //void DeleteRoom()
    //{
    //    roomManager.DeleteRoom();
    //    gridEditor.DeleteGrid();
    //}
    //void SaveRoom()
    //{
    //    roomManager.SaveRoom();
    //}

    //void PlaceBlock()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        GameObject cell = CastRay();
    //        if(cell)
    //        {
    //            //Check if the block being placed, CAN be placed.
    //            if (!gridEditor.IsOccupied((int)cell.transform.position.x, (int)cell.transform.position.y, (int)workLayer))
    //            {
    //                int moveToPos = selectedObject.name.LastIndexOf("_") + 1;
    //                int index = Int32.Parse(
    //                    selectedObject.name.Substring(
    //                        moveToPos,
    //                        selectedObject.name.Length - moveToPos));

    //                roomManager.AddBlock(index, cell.transform.position);
    //                gridEditor.AddBlock(
    //                    selectedObject
    //                    , (int)cell.transform.position.x
    //                    , (int)cell.transform.position.y
    //                    , (int)workLayer);
    //                //    //Get index
    //                //    int moveToPos = selectedObject.name.LastIndexOf("_") + 1;
    //                //    int index = Int32.Parse(
    //                //        selectedObject.name.Substring(
    //                //            moveToPos, 
    //                //            selectedObject.name.Length - moveToPos));
    //                //    //Get position
    //                //    Vector3 position = 
    //                //        cellEdit.transform.position + new Vector3(0, 0, (float)workLayer);
    //                //    //Add block
    //                //    roomManager.AddBlock(index, position, selectedObject, mask);
    //            }
    //            //if(cellEdit.IsOccupied(mask))
    //            //{
    //            //    string b = "im a nut case";
    //            //}
    //        }
    //        //GameObject hitObj = CastRay();
    //        //if (hitObj && !Cast2DRay())
    //        //{
    //        //    if (hitObj.tag == "CellEditor")
    //        //    {
    //        //        int moveToPos = selectedObject.name.LastIndexOf("_") + 1;
    //        //        int index = Int32.Parse(selectedObject.name.Substring(moveToPos, selectedObject.name.Length - moveToPos));
    //        //
    //        //        hitObj.transform.position += new Vector3(0, 0, (float)workLayer);
    //        //        if (roomManager.AddBlock(index, hitObj.transform.position, selectedObject))
    //        //        {
    //        //            //Destroy the cell in that position
    //        //            //Destroy(hitObj);
    //        //        }
    //        //    }
    //        //}
    //    }
    //}
    //void DeleteBlock()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        //GameObject hitObj = Cast2DRay();

    //        //if (hitObj 
    //        //    && roomManager.RemoveBlock(hitObj.transform.position))
    //        //{
    //        //    gridEditor.AddCell(hitObj.transform.position);
    //        //    Destroy(hitObj);
    //        //}
    //    }
    //}

    //void PlaceProp()
    //{
    //    if(Input.GetMouseButtonDown(0))
    //    {
    //        //Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        //position.z = (float)workLayer;

    //        //Instantiate(selectedObject, position, Quaternion.identity);
    //    }
    //}
    //void DeleteProp()
    //{

    //}

    //void ChangeEditType(EditType type)
    //{
    //    editType = type;

    //    //Show the grid depending on the type
    //    switch(editType)
    //    {
    //        case EditType.Block:
    //            {
    //                gridEditor.ShowGrid(true);
    //                break;
    //            }
    //        case EditType.Prop:
    //            {
    //                gridEditor.ShowGrid(false);
    //                break;
    //            }
    //    }
    //}

    ////GameObject Cast2DRay()
    ////{
    ////    Vector3 mousePos = Input.mousePosition;
    ////    mousePos.z = -(Camera.main.transform.position.z + 1);

    ////    Vector2 pos = Camera.main.ScreenToWorldPoint(mousePos);

    ////    Collider2D col = Physics2D.OverlapPoint(pos);

    ////    return (col != null) ? col.gameObject : null;
    ////}
    //
    //
    //
    //
    //

    //
    //

    ////UI stuff
    //private Rect editWindow = new Rect(120, 60, 200, 500);
    //private bool isEditWindow = false;
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 120, 30), "Init Room"))
    //    {
    //        InitNewRoom();
    //        isEditWindow = true;
    //    }
    //    if (isEditWindow) { editWindow = GUI.Window(0, editWindow, EditWindow, "Edit Tools"); }

    //    //if (GUI.Button(new Rect(0, 30, 120, 30), "Create Room"))
    //    //{
    //    //    print("Clicked on: Create Room");
    //    //    CreateNewRoom();
    //    //}
    //    //if (GUI.Button(new Rect(0, 60, 120, 30), "Clear Room"))
    //    //{
    //    //    print("Clicked on: Clear Room");
    //    //    ClearRoom();
    //    //}
    //    //GUI.Box(new Rect(0, 90, 250, 60), "ToolBar");
    //    //string[] editNames = new string[] { "Place", "Delete", "Edit"};
    //    //action = (Actions)GUI.Toolbar(new Rect(0, 110, 250, 30), (int)action, editNames);

    //    //string[] workLayerNames = new string[] { "Background", "Middleground", "Foreground" };
    //    //workLayer = (LayerState)GUI.Toolbar(new Rect(0, 110, 250, 30), (int)workLayer, workLayerNames);


    //    //if (GUI.Button(new Rect(0, 60, 120, 30), "New Room"))
    //    //{
    //    //    popUp_CreateRoom = GUI.Window(0, popUp_CreateRoom, myWindow, "New Room");
    //    //}

    //}
    //void EditWindow(int id)
    //{
    //    string[] editNames = new string[] { "Place", "Delete" };
    //    action = (Actions)GUI.Toolbar(new Rect(10, 40, 150, 20), (int)action, editNames);

    //    if (GUI.Button(new Rect(10, 60, 100, 20), "Clear Room")) { ClearRoom(); }
    //    if (GUI.Button(new Rect(10, 90, 100, 20), "Save Room")) { SaveRoom(); }
    //    if (GUI.Button(new Rect(10, 120, 100, 20), "Delete Room")) { DeleteRoom(); isEditWindow = false; }
    //}
}