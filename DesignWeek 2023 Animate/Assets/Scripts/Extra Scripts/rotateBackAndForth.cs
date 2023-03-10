using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateBackAndForth : MonoBehaviour
{
    public float minRotSpeed, maxRotSpeed;
    public float rotAngleY;
    float rotSpeed;

    private void Awake()
    {
        rotSpeed = Random.Range(minRotSpeed, maxRotSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float rY = Mathf.SmoothStep(-rotAngleY, rotAngleY, Mathf.PingPong(Time.time * rotSpeed, 1));
        transform.rotation = Quaternion.Euler(0, 0, rY);
    }
}
