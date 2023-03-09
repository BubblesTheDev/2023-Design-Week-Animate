using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class runeCarver : MonoBehaviour
{
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
    private int lastCorrectPointIndex;
    private bool isInCarveBox;
    [HideInInspector] public bool bookIsOpen;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            if (objToCarve != null && runeSelected != null)
            {
                switch (runeSelected.runeType)
                {
                    case (runeTypes.personality):
                        if (!drawnPersonality)
                        {
                            carveRune();
                        }
                        break;
                    case (runeTypes.motivation):
                        if (!drawnMotivation)
                        {
                            carveRune();
                        }
                        break;
                    case (runeTypes.job):
                        if (!drawnJob)
                        {
                            carveRune();
                        }
                        break;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (isInCarveBox) removeLine();
        }

        if (Input.GetKeyDown(KeyCode.Space) && runeSelected != null)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = screenDepthValue;
            runeSelected.linePlaces.Add(Camera.main.ScreenToWorldPoint(screenPos));
            print("Adding line place to " + runeSelected + "'s Line places at index " + (runeSelected.linePlaces.Count - 1));
        }
    }

    void detectBounds()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = screenDepthValue;

        if (Camera.main.ScreenToWorldPoint(screenPos).x > boundCorner1.x + carvingLine.startWidth / 2
            && Camera.main.ScreenToWorldPoint(screenPos).x < boundCorner2.x - carvingLine.startWidth / 2
            && Camera.main.ScreenToWorldPoint(screenPos).y < boundCorner1.y - carvingLine.startWidth / 2
            && Camera.main.ScreenToWorldPoint(screenPos).y > boundCorner2.y + carvingLine.startWidth / 2) isInCarveBox = true;
        else isInCarveBox = false;
    }

    void removeLine()
    {
        carvingLine.positionCount = 0;
        detectAccuracy();
    }

    void carveRune()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = screenDepthValue;

        if (isInCarveBox)
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

    void detectAccuracy()
    {
        List<bool> correctPoints = new List<bool>(runeSelected.linePlaces.Count);
        for (int i = 0; i < runeSelected.linePlaces.Count - 1; i++)
        {
            print(runeSelected.linePlaces[i]);
            for (int x = 0 + lastCorrectPointIndex; x < carvingLine.positionCount - 1; x++)
            {
                print(carvingLine.GetPosition(x));
                if (Vector3.Distance(carvingLine.GetPosition(x), runeSelected.linePlaces[i]) < errorDistance)
                {
                    lastCorrectPointIndex = x;
                    correctPoints[i] = true;
                    break;
                }
            }
        }
    }

}
