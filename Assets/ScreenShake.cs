using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public void Shake(Vector3 shakePos)
    {
        transform.position += shakePos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("shake");
            Shake(Vector3.right * 0.15f);
        }
    }
}