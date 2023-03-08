using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class runeCarver : MonoBehaviour {
    [Header("Graphics")]
    [SerializeField] private Image runeCircle;
    public Image runeTraceImage;

    [Header("Drawing Settings")]
    public LineRenderer carvingLine;
    public GameObject objToCarve;
    public runeDataContainer runeSelected;
    [SerializeField] private float maxDistanceBetweenPoints;
    [SerializeField] private runeTypes currentRuneToCarve;
    [SerializeField] private float errorDistance;

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) {
            carveRune();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1)) {
            removeLine();
        }
    }

    void removeLine() {
        carvingLine.positionCount = 0;
    }

    void carveRune() {

        if (carvingLine.positionCount == 0) {
            carvingLine.positionCount++;
            carvingLine.SetPosition(0, Camera.main.ScreenToViewportPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.nearClipPlane)));

        } else if (Vector3.Distance(Camera.main.ScreenToViewportPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.nearClipPlane)), carvingLine.GetPosition(carvingLine.positionCount - 1)) >= maxDistanceBetweenPoints) {
            carvingLine.positionCount++;
            carvingLine.SetPosition(carvingLine.positionCount - 1, Camera.main.ScreenToViewportPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.nearClipPlane)));
        } else if (carvingLine.positionCount > 50) removeLine();
        
    }

    
}
