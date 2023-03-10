using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class magicHandler : MonoBehaviour
{
    public bool isCarving;
    public int layersToCull = 7;


    private cursorChanger cursorHandler;
    private runeCarver carver;
    private bookHandeler handeler;
    public Vector3 originOfObject;
    private int originLayer;
    private void Awake()
    {
        cursorHandler = GetComponent<cursorChanger>();
        carver = GetComponent<runeCarver>();
        handeler = GetComponent<bookHandeler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && cursorHandler.cursorImage.sprite == cursorHandler.cursors[1] && !isCarving)
        {
            startCarve();
        }
    }

    void startCarve()
    {
        isCarving = true;
        originOfObject = cursorHandler.hit.transform.position;
        originLayer = cursorHandler.hit.transform.gameObject.layer;
        Camera.main.GetComponent<cameraControl>().canLook = false;

        GameObject.Find("UiManagment").transform.Find("Canvas").gameObject.SetActive(true);
        GameObject.Find("UiManagment").transform.Find("OnScreenUi").gameObject.SetActive(false);


        carver.enabled = true;
        handeler.enabled = true;

        carver.objToCarve = cursorHandler.hit.transform.gameObject;
        carver.objToCarve.layer = layersToCull;

        carver.objToCarve.transform.parent = GameObject.Find("UiManagment").transform.Find("Canvas/ObjectToCarvePos").transform;

        carver.objToCarve.transform.position = carver.objToCarve.transform.parent.position;
        carver.objToCarve.transform.LookAt(Camera.main.transform.position);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        handeler.bookObject.transform.parent.transform.Find("Open Book").GetComponent<Button>().onClick.Invoke();
    }

    public void endCarve()
    {
        isCarving = false;
        Camera.main.GetComponent<cameraControl>().canLook = true;
        handeler.bookObject.transform.parent.transform.Find("Close Book").GetComponent<Button>().onClick.Invoke();

        


        carver.enabled = false;
        handeler.enabled = false;


        carver.objToCarve.transform.parent = GameObject.Find("Enviroment").transform.Find("Interactables/AliveObj").transform;

        
        carver.objToCarve.layer = originLayer;
        carver.objToCarve.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        carver.objToCarve.tag = "canBeKilled";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("UiManagment").transform.Find("Canvas").gameObject.SetActive(false);
        GameObject.Find("UiManagment").transform.Find("OnScreenUi").gameObject.SetActive(true);
    }
}
