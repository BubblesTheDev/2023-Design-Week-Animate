using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public bool canLook = true;
    public float minAngle ,maxAngle;
    public float sensX, sensY;


    float xRot, yRot;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        getMouseInput();
        if(canLook) controlCamera();
    }

    void getMouseInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        yRot += mouseX;
        xRot -= mouseY;

    }

    public void controlCamera()
    {

        xRot = Mathf.Clamp(xRot, minAngle, maxAngle);
        
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    }

}
