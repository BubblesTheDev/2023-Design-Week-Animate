using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMent : MonoBehaviour
{
    public float speed = 5;
    Vector3 moveVector;

    float movX, movZ;

    public cameraControl cameraControl;

    private void Awake()
    {
        cameraControl = Camera.main.GetComponent<cameraControl>();
    }


    private void Update()
    {
        movX = Input.GetAxis("Horizontal");
        movZ = Input.GetAxis("Vertical");

        moveVector = new Vector3(movX, 0 , movZ);
        if (cameraControl.canLook) transform.Translate(moveVector.normalized * speed * Time.deltaTime);
    }
}
