using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging_Stuff : MonoBehaviour
{
    void Debug2DBoxCast(Rect Box, Vector2 Direction, float Distance)
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS((Vector2)this.transform.position + Box.center, this.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, Box.size);
        Gizmos.matrix = Matrix4x4.TRS((Vector2)this.transform.position + Box.center + (Direction.normalized * Distance), this.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, Box.size);
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS((Vector2)this.transform.position + Box.center, Quaternion.identity, Vector3.one);
        Gizmos.DrawLine(Vector2.zero, Direction.normalized * Distance);
    }
}
