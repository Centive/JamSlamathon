using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.Xml;
using System.IO;

[XmlRoot("Room")]
public class Room
{
    [XmlAttribute("Index")]
    public int index;

    [XmlAttribute("Size")]
    public string sSize;

    [XmlAttribute("Position")]
    public string sPosition;

    [XmlArray("Block_List")]
    [XmlArrayItem("Block")]
    public List<Block> blocks = new List<Block>();
    
    [XmlArray("Prop_List")]
    [XmlArrayItem("Prop")]
    public List<Prop> props = new List<Prop>();

    public Room() { }
    public Room(int i, Vector3 pos, Vector2 size)
    {
        string posToString = pos.ToString();
        if (posToString.StartsWith("(") && posToString.EndsWith(")")) { posToString = posToString.Substring(1, posToString.Length - 2); }

        string sizeToString = size.ToString();
        if (sizeToString.StartsWith("(") && sizeToString.EndsWith(")")) { sizeToString = sizeToString.Substring(1, sizeToString.Length - 2); }

        index = i;
        sPosition = posToString;
        sSize = sizeToString;
    }
    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Room));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static Room Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Room));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Room;
        }
    }
    public Room LoadFromText(string text)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Room));
        return serializer.Deserialize(new StringReader(text)) as Room;
    }
    public Vector2 GetSize()
    {
        // Remove the parentheses
        if (sSize.StartsWith("(") && sSize.EndsWith(")"))
        {
            sSize = sSize.Substring(1, sSize.Length - 2);
        }

        // split the items
        string[] sArray = sSize.Split(',');

        // store as a Vector3
        return new Vector2(float.Parse(sArray[0]), float.Parse(sArray[1]));
    }
}