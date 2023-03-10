using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtObject : MonoBehaviour
{
    public GameObject objToLookAt;
    public bool lookAway, lookAtCam;


    private void Awake()
    {
        if (lookAtCam) objToLookAt = Camera.main.gameObject;
    }
    private void Update() {
        if (lookAway) transform.rotation = Quaternion.LookRotation(transform.position - objToLookAt.transform.position);
        else transform.LookAt(objToLookAt.transform.position);

    }
}
