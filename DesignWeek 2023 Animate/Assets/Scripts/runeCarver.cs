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
    [SerializeField] private Vector3 boundCorner1, boundCorner2;
    [SerializeField] private float maxDistanceBetweenPoints, maxNumOfPoints;
    [SerializeField] private float errorDistance;
    [SerializeField] private float screenDepthValue;

    [Header("General Settings")]
    public runeDataContainer currentSelectedPersonality;
    public runeDataContainer currentSelectedMotivation;
    public runeDataContainer currentSelectedJob;
    public bool drawnPersonality, drawnMotivation, drawnJob;

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1)) {
            carveRune();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1)) {
            removeLine();
        }
        runeInformation();
    }

    void removeLine() {
        carvingLine.positionCount = 0;
    }

    void carveRune() {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = screenDepthValue;

        if (Camera.main.ScreenToWorldPoint(screenPos).x > boundCorner1.x + carvingLine.startWidth/2
            && Camera.main.ScreenToWorldPoint(screenPos).x < boundCorner2.x - carvingLine.startWidth/2
            && Camera.main.ScreenToWorldPoint(screenPos).y < boundCorner1.y - carvingLine.startWidth/2
            && Camera.main.ScreenToWorldPoint(screenPos).y > boundCorner2.y + carvingLine.startWidth/2)
        {
            if (carvingLine.positionCount == 0)
            {
                carvingLine.positionCount++;
                carvingLine.SetPosition(0, Camera.main.ScreenToWorldPoint(screenPos));

            }
            else if (Vector3.Distance(Camera.main.ScreenToWorldPoint(screenPos), carvingLine.GetPosition(carvingLine.positionCount - 1)) >= maxDistanceBetweenPoints)
            {
                carvingLine.positionCount++;
                carvingLine.SetPosition(carvingLine.positionCount - 1, Camera.main.ScreenToWorldPoint(screenPos));
            }
            else if (carvingLine.positionCount > maxNumOfPoints) removeLine();
        }

    }

    void runeInformation()
    {
    }

    
}
