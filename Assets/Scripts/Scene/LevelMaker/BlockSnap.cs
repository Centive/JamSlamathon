using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSnap : MonoBehaviour
{
    public float cell_size = 1f;
    private Vector3 pos = Vector3.zero;
    
    void Update()
    {
        pos.x = Mathf.Round(transform.position.x / cell_size) * cell_size;
        pos.y = Mathf.Round(transform.position.y / cell_size) * cell_size;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
