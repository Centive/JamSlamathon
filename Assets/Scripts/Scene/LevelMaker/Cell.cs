using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private new Renderer renderer;
    private Color startColor;
    private bool[] isFull = new bool[3] { false, false, false};

    void Start()
    {
        renderer = GetComponent<Renderer>();
        startColor = renderer.material.color;
    }

    public bool IsOccupiedAt(int zAxis)
    {
        return Physics2D.OverlapPoint(transform.position);
    }
    //Highlight block on mouse hover
    void OnMouseEnter()
    {
        startColor = renderer.material.color;
        renderer.material.color = Color.yellow;
    }
    void OnMouseExit()
    {
        renderer.material.color = startColor;
    }
}
