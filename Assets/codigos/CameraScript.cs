using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    /*
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    */


    public Transform PlayerTransform;
    private Vector3 _cameraOffset;
    [Range(0.01f,1.0f)]
    public float SmoothFactor = 0.1f;
    public float RotationSpeed = 5.0f;




    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset  = transform.position - PlayerTransform.position;
    }

    void LateUpdate()
    {
        Quaternion camTurnAngleX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
        Quaternion camTurnAngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * -RotationSpeed, Vector3.right);

       
        _cameraOffset = camTurnAngleY * camTurnAngleX * _cameraOffset;

        Vector3 newPos = PlayerTransform.position+_cameraOffset;
        transform.position = Vector3.Slerp(transform.position,newPos,SmoothFactor);

        transform.LookAt(PlayerTransform);

       
        /*
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch,yaw,0.0f);
        */
    }
}
