using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Block")]
public class Block
{
    [XmlAttribute("Index")]
    public int index;

    [XmlAttribute("Position")]
    public string sPosition;
    
    //Constructor
    public Block() { }
    public Block(int i, Vector3 pos)
    {
        string vTos = pos.ToString();

        if (vTos.StartsWith("(") && vTos.EndsWith(")")) { vTos = vTos.Substring(1, vTos.Length - 2); }
            

        index = i; sPosition = vTos;
    }
    public void SetBlock(int i, Vector3 pos)
    {
        index = i; sPosition = pos.ToString();
    }
    public Vector3 GetPosition()
    {
        // Remove the parentheses
        if (sPosition.StartsWith("(") && sPosition.EndsWith(")"))
        {
            sPosition = sPosition.Substring(1, sPosition.Length - 2);
        }

        // split the items
        string[] sArray = sPosition.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}