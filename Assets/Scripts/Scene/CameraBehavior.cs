using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior
    : MonoBehaviour
{
    public enum CameraState
    {
        None,
        FollowPlayer,
        ControlCamera
    }

    public float smoothTime = 0.10f;
    public Vector3 offset = Vector3.zero;
    public Transform target = null;
    public float move_Speed = 0f;
    public float rotate_Speed = 0f;
    public float yaw = 0f;
    public float pitch = 0f;
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;

    public CameraState cameraState = CameraState.ControlCamera;

    void Start()
    {
        position = transform.position;
        rotation = transform.eulerAngles;
    }

    void FixedUpdate()
    { 
        switch(cameraState)
        {
            case CameraState.None:
                {
                    break;
                }
            case CameraState.FollowPlayer:
                {
                    FollowPlayer();
                    break;
                }
            case CameraState.ControlCamera:
                {
                    ControlCamera();
                    break;
                }
        }
    }
    
    private void FollowPlayer()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position - offset, target.position, smoothTime * Time.deltaTime);
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
    private void ControlCamera()
    {
        float xPos = Input.GetAxis("Horizontal") * move_Speed * Time.deltaTime;
        float zPos = Input.GetAxis("Vertical") * move_Speed * Time.deltaTime;
        Vector3 newPos = new Vector3(xPos, 0, zPos);
        newPos = Camera.main.transform.TransformDirection(newPos);
        transform.position += newPos;
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += new Vector3(0f, move_Speed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= new Vector3(0f, move_Speed * Time.deltaTime, 0f);
        }
        if (Input.GetMouseButton(1))
        {
            yaw += rotate_Speed * Input.GetAxis("Mouse X");
            pitch -= rotate_Speed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
    
}