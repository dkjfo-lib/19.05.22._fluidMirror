using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform viewTarget;
    public Transform cameraTrans;

    public float scrollSpeed = 5f;
    public float moveRate = 0.125f;

    public Vector3 cameraTargetPosition;

    void Start()
    {
        cameraTargetPosition = cameraTrans.localPosition;
    }
    
    void Update()
    {
        cameraTrans.LookAt(viewTarget);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraTargetPosition += Vector3.up * scroll * scrollSpeed;
        cameraTrans.localPosition = Vector3.Lerp(cameraTrans.localPosition, cameraTargetPosition, moveRate);
    }
}
