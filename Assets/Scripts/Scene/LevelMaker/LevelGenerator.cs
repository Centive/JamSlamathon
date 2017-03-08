//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//public class LevelGenerator : MonoBehaviour
//{
//    public Texture2D bitmap;
//    public Vector2 bitmapSize;
//    public Object[] tilePrefabs;
//    public Object[] tilings;
//    public GameObject[] LevelObjects;
//    public Transform levelPosition;
//    public enum TileType
//    {
//        None = -1,
//        Ground,
//        Wall,
//        Door
//    }
//    void Start()
//    {
//        bitmap = null;
//        LoadLevel("Level1");
//    }   
//    void LoadLevel(string levelName)
//    {
//        bitmap = Resources.Load("Levels/" + levelName) as Texture2D;
//        bitmapSize = new Vector2(bitmap.width, bitmap.height);
//        tilePrefabs = Resources.LoadAll("Tileset/" + levelName, typeof(GameObject));
//        tilings = Resources.LoadAll("Textures/" + levelName, typeof(Texture));
//        //Check if the bitmap exists or the pathname is invalid
//        if (bitmap == null)
//        {
//            print("Invalid Bitmap");
//            return;
//        }
//        Color32[] pixels = bitmap.GetPixels32();
//        Vector3 startPos = new Vector3(-bitmapSize.x / 2.0f, -bitmapSize.y / 2.0f);
//        //Create the parent to hold all levelName assets
//        levelPosition = new GameObject("Level").transform;
//        //Load the tiles
//        for (int x = 0; x < bitmapSize.x; x++)
//        {
//            for (int y = 0; y < bitmapSize.y; y++)
//            {
//                Vector3 newPos = new Vector3(startPos.x + x, startPos.y + y);
//                Color32 color = pixels[x + y * bitmap.width];
//                int index = (int)GetTile(color);
//                if ( index != -1)
//                {
//                    GameObject clone = Instantiate(tilePrefabs[index], newPos, Quaternion.identity) as GameObject;
//                    clone.transform.parent = levelPosition.transform;
//                }
//            }
//        }
//    }
//    void SaveLevel(string name)
//    {
//        Texture2D newLevel = null;
//        Vector3 startPos = new Vector3(0, 0);
//        for (int x = 0; x < bitmap.width; x++)
//        {
//            for (int y = 0; y < bitmap.height; y++)
//            {
//                Vector3 newPos = new Vector3(startPos.x + x, startPos.y + y);
//                RaycastHit hit;
//                Ray ray = new Ray(newPos + new Vector3(0f, 0f, 3f), -Vector3.forward);
//                Physics.Raycast(ray, out hit);
//                Debug.DrawRay(ray.origin, -Vector3.forward, Color.red);
//                print("Found block at: " + ray.origin);
//            }
//        }
//        //Save texture to the path
//        AssetDatabase.CreateAsset(newLevel, "Resources/Levels");
//    }
//    TileType GetTile(Color32 color)
//    {
//        if (color.Equals(new Color32(255, 0, 0, 255)))
//        {
//            return TileType.Ground;
//        }
//        if (color.Equals(new Color32(0, 255, 0, 255)))
//        {
//            return TileType.Wall;
//        }
//        if (color.Equals(new Color32(0, 0, 255, 255)))
//        {
//            return TileType.Door;
//        }
//        return TileType.None;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    public Room room = new Room();
    public GameObject player_Prefab;
    private GameObject roomHolder = null;


    void Start()
    {
    }
    
    void NewRoom(int index)
    {
        if (roomHolder != null) { Destroy(roomHolder); }

        room = Room.Load(Path.Combine(Application.dataPath, "../Assets/Resources/Rooms/Room_" + index + ".xml"));
        roomHolder = new GameObject("roomHolder");
        roomHolder.transform.position = Vector3.zero;

        foreach (Block block in room.blocks)
        {
            GameObject newBlock = Resources.Load("Prefabs/Blocks/Block_" + block.index) as GameObject;
            string[] coord = block.sPosition.Split(',');
            Instantiate(newBlock, new Vector3(float.Parse(coord[0]), float.Parse(coord[1]), float.Parse(coord[2])), Quaternion.identity);
        }
        Instantiate(player_Prefab, new Vector3(0, 1,0), Quaternion.identity);
    }

    void OnGUI()
    {
        string sIndex = "1";
        GUI.TextField(new Rect(0, 0, 120, 30), sIndex);
        if (GUI.Button(new Rect(0, 30, 120, 30), "New Room"))
        {
            NewRoom(int.Parse(sIndex));
        }
    }
}