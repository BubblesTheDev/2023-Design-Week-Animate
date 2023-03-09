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

    [Space, Header("Drawing Settings")]
    public LineRenderer carvingLine;
    public GameObject objToCarve;
    public runeDataContainer runeSelected;
    [SerializeField] private Vector3 boundCorner1, boundCorner2;
    [SerializeField] private float maxDistanceBetweenPoints, maxNumOfPoints;
    [SerializeField] private float errorDistance;
    [SerializeField] private float screenDepthValue;

    [Space, Header("General Settings")]
    public runeDataContainer currentSelectedPersonality;
    public runeDataContainer currentSelectedMotivation;
    public runeDataContainer currentSelectedJob;
    private bool isInCarveBox;
    [HideInInspector] public bool bookIsOpen;
    [SerializeField] private bookHandeler handeler;

    [Space, Header("Rune Completion Settings")]
    [SerializeField] private float numOfAfterRunes;
    [SerializeField] private float distanceToRing;
    public bool drawnPersonality, drawnMotivation, drawnJob;


    private void Update()
    {
        detectBounds();
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
        {
            if (objToCarve != null && runeSelected != null && !bookIsOpen)
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
            if (isInCarveBox && !bookIsOpen) removeLine();
        }

        if (Input.GetKeyDown(KeyCode.Space) && runeSelected != null)
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = screenDepthValue;
            runeSelected.linePlaces.Add(Camera.main.ScreenToWorldPoint(screenPos));
            print("Adding line place to " + runeSelected + "'s Line places at index " + (runeSelected.linePlaces.Count - 1));
        }
    }

    public void changeBookState(bool stateToSet)
    {
        bookIsOpen = stateToSet;
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
        detectAccuracy();
        carvingLine.positionCount = 0;
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
        int numOfCorrect = 0;
        int lastCorrectPointIndex = 0;

        if (Vector3.Distance(runeSelected.linePlaces[0], carvingLine.GetPosition(0)) < Vector3.Distance(runeSelected.linePlaces[runeSelected.linePlaces.Count - 1], carvingLine.GetPosition(0)))
        {
            for (int i = 0; i < runeSelected.linePlaces.Count - 1;)
            {
                for (int x = 0 + lastCorrectPointIndex; x < carvingLine.positionCount - 1; x++)
                {
                    if (Vector3.Distance(carvingLine.GetPosition(x), runeSelected.linePlaces[i]) < errorDistance)
                    {
                        lastCorrectPointIndex = x;
                        i++;
                        numOfCorrect++;
                        //print("Correct at index: " + x + " of the line renderer, This is the " + i + "th Correct point of the rune");
                        break;
                    }

                    if (x == carvingLine.positionCount - 2)
                    {
                        return;
                    }

                }


            }
        }
        else
        {
            for (int i = runeSelected.linePlaces.Count - 1; i > 0;)
            {

                for (int x = 0 + lastCorrectPointIndex; x < carvingLine.positionCount - 1; x++)
                {
                    if (Vector3.Distance(carvingLine.GetPosition(x), runeSelected.linePlaces[i]) < errorDistance)
                    {
                        lastCorrectPointIndex = x;
                        i--;
                        numOfCorrect++;
                        //print("Correct at index: " + x + " of the line renderer, This is the " + i + "th Correct point of the rune");
                        break;
                    }

                    if (x == carvingLine.positionCount - 2)
                    {
                        return;
                    }

                }


            }
        }


        if (numOfCorrect == runeSelected.linePlaces.Count - 1)
        {
            switch (runeSelected.runeType)
            {
                case runeTypes.personality:
                    drawnPersonality = true;
                    lastCorrectPointIndex = 0;
                    break;
                case runeTypes.motivation:
                    drawnMotivation = true;
                    lastCorrectPointIndex = 0;
                    break;
                case runeTypes.job:
                    drawnJob = true;
                    lastCorrectPointIndex = 0;
                    break;
            }

            spawnCompletedRune(runeSelected);
            StartCoroutine(resetRune(runeSelected));
        }
    }

    void spawnCompletedRune(runeDataContainer runeToSpawn)
    {
        for (int i = 0; i < numOfAfterRunes; i++)
        {
            Vector3 posCenter = runeCircle.transform.position;

            Vector3 pos;
            float angle = 0;
            float singleAngleDistance = 360 / numOfAfterRunes;
            angle = (singleAngleDistance * i);

            switch (runeToSpawn.runeType)
            {
                case runeTypes.personality:
                    angle += (singleAngleDistance / 3) * 0;
                    break;
                case runeTypes.motivation:
                    angle += (singleAngleDistance / 3) * 1;
                    break;
                case runeTypes.job:
                    angle += (singleAngleDistance / 3) * 2;
                    break;
            }

            pos.x = posCenter.x + distanceToRing * Mathf.Sin(angle * Mathf.Deg2Rad);
            pos.y = posCenter.y + distanceToRing * Mathf.Cos(angle * Mathf.Deg2Rad);
            pos.z = posCenter.z;


            GameObject temp = new GameObject("Completed " + runeToSpawn.name + " Rune");
            temp.transform.parent = runeCircle.transform.Find("Completed Runes").transform;
            temp.transform.localPosition = pos;
            temp.transform.localScale = Vector3.one * .5f;

            Image tempImg = temp.AddComponent<Image>();
            tempImg.sprite = runeToSpawn.runeAsset;
        }

    }

    public void completeAnimate()
    {
        handeler.bookObject.transform.Find("RightPage").gameObject.SetActive(true);
        handeler.bookObject.transform.Find("LeftPage").gameObject.SetActive(true);
        handeler.bookObject.transform.Find("HiddenPage").gameObject.SetActive(false);

        handeler.bookObject.transform.Find("PageButtons/Page 1 Tab").gameObject.SetActive(true);
        handeler.bookObject.transform.Find("PageButtons/Page 2 Tab").gameObject.SetActive(true);
        handeler.bookObject.transform.Find("PageButtons/Page 3 Tab").gameObject.SetActive(true);

        {
            handeler.bookObject.transform.Find("RightPage/Personality Text").GetComponent<Text>().text = "Personality:";
            handeler.bookObject.transform.Find("RightPage/Motivation Text").GetComponent<Text>().text = "Motivation:";
            handeler.bookObject.transform.Find("RightPage/Job Text").GetComponent<Text>().text = "Job:";

            handeler.bookObject.transform.Find("RightPage/Selected Personality Rune").GetComponent<Image>().sprite = null;
            handeler.bookObject.transform.Find("RightPage/Selected Motivation Rune").GetComponent<Image>().sprite = null;
            handeler.bookObject.transform.Find("RightPage/Selected Job Rune").GetComponent<Image>().sprite = null;
        }

        handeler.currentRunePageIndex = 0;
        currentSelectedPersonality = null;
        currentSelectedMotivation = null;
        currentSelectedJob = null;
        drawnPersonality = false;
        drawnMotivation = false;
        drawnJob = false;

        foreach (Transform item in GameObject.Find("Completed Runes").transform)
        {
            Destroy(item.gameObject);
        }
    }

    IEnumerator resetRune(runeDataContainer runeToSpawn)
    {
        runeSelected = null;
        runeTraceImage.sprite = null;

        yield return new WaitForSeconds(0.5f);
        handeler.bookObject.transform.parent.transform.Find("Open Book").GetComponent<Button>().onClick.Invoke();


        if (drawnPersonality && drawnMotivation && drawnJob)
        {
            yield return new WaitForSeconds(3.5f);

            handeler.bookObject.transform.Find("HiddenPage").gameObject.SetActive(true);
            handeler.bookObject.transform.Find("LeftPage").gameObject.SetActive(false);

            handeler.bookObject.transform.Find("HiddenPage/Rune Name").GetComponent<Text>().text = currentSelectedPersonality.name + " " + currentSelectedMotivation.name + " " + currentSelectedJob.name;
            handeler.bookObject.transform.Find("HiddenPage/Rune Description").GetComponent<Text>().text = currentSelectedPersonality.runeDescription + " \n\n " + currentSelectedMotivation.runeDescription + " \n\n " + currentSelectedJob.runeDescription;

            handeler.bookObject.transform.Find("HiddenPage/RuneHolder/Personality Rune Img").GetComponent<Image>().sprite = currentSelectedPersonality.runeAsset;
            handeler.bookObject.transform.Find("HiddenPage/RuneHolder/Motivation Rune Img").GetComponent<Image>().sprite = currentSelectedMotivation.runeAsset;
            handeler.bookObject.transform.Find("HiddenPage/RuneHolder/Job Rune Img").GetComponent<Image>().sprite = currentSelectedJob.runeAsset;
            yield return null;
        }

        switch (runeToSpawn.runeType)
        {
            case runeTypes.personality:
                GameObject.Find("Canvas").transform.Find("Book/PageButtons/Page 1 Tab").gameObject.SetActive(false);
                if (!drawnMotivation) handeler.currentRunePageIndex = 1;
                else handeler.currentRunePageIndex = 2;
                break;
            case runeTypes.motivation:
                GameObject.Find("Canvas").transform.Find("Book/PageButtons/Page 2 Tab").gameObject.SetActive(false);
                if (!drawnJob) handeler.currentRunePageIndex = 2;
                else handeler.currentRunePageIndex = 0;
                break;
            case runeTypes.job:
                GameObject.Find("Canvas").transform.Find("Book/PageButtons/Page 3 Tab").gameObject.SetActive(false);
                if (!drawnPersonality) handeler.currentRunePageIndex = 0;
                else handeler.currentRunePageIndex = 1;
                break;
        }

        
    }

    private void OnDrawGizmos()
    {
        if (runeSelected != null)
        {
            foreach (Vector3 linePos in runeSelected.linePlaces)
            {
                Gizmos.DrawWireSphere(linePos, errorDistance);
            }
        }
    }

}
