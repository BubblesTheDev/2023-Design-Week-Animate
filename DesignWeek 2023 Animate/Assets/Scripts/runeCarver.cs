using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.AI;

public class runeCarver : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Image runeCircle;
    public Image runeTraceImage;

    [Space, Header("Drawing Settings")]
    public LineRenderer carvingLine;
    public GameObject objToCarve;
    public runeDataContainer runeSelected;
    [SerializeField] private GameObject boundRect;
    [SerializeField] private float maxDistanceBetweenPoints, maxNumOfPoints;
    [SerializeField] private float errorDistance;
    [SerializeField] private float screenDepthValue;

    [Space, Header("General Settings")]
    public runeDataContainer currentSelectedPersonality;
    public runeDataContainer currentSelectedMotivation;
    public runeDataContainer currentSelectedJob;
    [SerializeField] private bool isInCarveBox;
    public bool bookIsOpen;
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
            if (!bookIsOpen && carvingLine.positionCount >0) removeLine();
        }

        if (Input.GetKeyDown(KeyCode.Space) && runeSelected != null)
        {
            Vector3 screenPos = Input.mousePosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(boundRect.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out screenPos);

            GameObject temp = new GameObject("Place(" + GameObject.Find("Bound Rect").transform.Find(runeSelected.name + " RunePlaceMents").childCount.ToString() + ")");
            temp.transform.parent = GameObject.Find("Bound Rect").transform.Find(runeSelected.name + " RunePlaceMents");
            temp.transform.position = screenPos;
            print("Adding line place to " + runeSelected + "'s Line places at index " + (runeSelected.linePlacesHolder.transform.childCount - 1));
        }

    }

    public void changeBookState(bool stateToSet)
    {
        bookIsOpen = stateToSet;
    }

    void detectBounds()
    {
        Vector3 screenPos = Vector3.zero;
        isInCarveBox = RectTransformUtility.RectangleContainsScreenPoint(boundRect.GetComponent<RectTransform>(), Input.mousePosition, Camera.main);
    }

    void removeLine()
    {
        detectAccuracy();
        carvingLine.positionCount = 0;
    }

    void carveRune()
    {
        Vector3 screenPos = Input.mousePosition;

        if (isInCarveBox)
        {

            RectTransformUtility.ScreenPointToWorldPointInRectangle(boundRect.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out screenPos);
            if (carvingLine.positionCount == 0)
            {
                carvingLine.positionCount++;

                carvingLine.SetPosition(0, screenPos);

            }
            else if (Vector3.Distance(screenPos, carvingLine.GetPosition(carvingLine.positionCount - 1)) >= maxDistanceBetweenPoints)
            {
                carvingLine.positionCount++;
                carvingLine.SetPosition(carvingLine.positionCount - 1, screenPos);
            }
            else if (carvingLine.positionCount > maxNumOfPoints) removeLine();
        }

    }

    void detectAccuracy()
    {
        int numOfCorrect = 0;
        int lastCorrectPointIndex = 0;
        GameObject tempHolder = GameObject.Find(runeSelected.linePlacesHolder.name).gameObject;

        if (carvingLine.positionCount <= 2) return;
        if (Vector3.Distance(tempHolder.transform.GetChild(0).transform.position, carvingLine.GetPosition(0))
            < Vector3.Distance(tempHolder.transform.GetChild(tempHolder.transform.childCount - 1).position, carvingLine.GetPosition(0)))
        {
            for (int i = 0; i < tempHolder.transform.childCount-1;)
            {
                for (int x = 0 + lastCorrectPointIndex; x < carvingLine.positionCount - 1; x++)
                {
                    if (Vector3.Distance(carvingLine.GetPosition(x), tempHolder.transform.GetChild(i).position) < errorDistance)
                    {
                        lastCorrectPointIndex = x;
                        i++;
                        numOfCorrect++;
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

            for (int i = tempHolder.transform.childCount - 1; i > 0;)
            {

                for (int x = 0 + lastCorrectPointIndex; x < carvingLine.positionCount - 1; x++)
                {
                    if (Vector3.Distance(carvingLine.GetPosition(x), tempHolder.transform.GetChild(i).position) < errorDistance)
                    {
                        lastCorrectPointIndex = x;
                        i--;
                        numOfCorrect++;
                        break;
                    }

                    if (x >= carvingLine.positionCount - 2)
                    {
                        return;
                    }

                }


            }
        }

        if (numOfCorrect == tempHolder.transform.childCount-1)
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

    public void applyRune()
    {

        objectSoul temp = objToCarve.GetComponent<objectSoul>();
        temp.objPersonality = currentSelectedPersonality;
        temp.objMotivation = currentSelectedMotivation;
        temp.objJob = currentSelectedJob;
        temp.gameObject.GetComponent<NavMeshAgent>().enabled = true;



        NavMeshHit hit;
        NavMesh.SamplePosition(GameObject.Find("Game Manager").GetComponent<magicHandler>().originOfObject, out hit, 50f, objToCarve.GetComponent<NavMeshAgent>().areaMask);
        objToCarve.GetComponent<NavMeshAgent>().Warp(hit.position + Vector3.up*0.1f);


        for (int i = 0; i < temp.placerBasedAi.Count - 1; i++)
        {
            
            if (temp.placerBasedAi[i].GetType().ToString() == temp.objJob.name + "JobAI")
            {
                temp.placerBasedAi[i].enabled = true;
                temp.selectedPlacer = objToCarve.GetComponent<objectSoul>().placerBasedAi[i];
                break;
            }
        }
        for (int i = 0; i < temp.removerBasedAi.Count - 1; i++)
        {
            if (temp.removerBasedAi[i].GetType().ToString() == temp.objJob.name + "JobAI")
            {
                temp.removerBasedAi[i].enabled = true;
                temp.selectedRemover = objToCarve.GetComponent<objectSoul>().removerBasedAi[i];
                break;
            }
        }


        temp.objMoveSpeed *= currentSelectedPersonality.moveSpeed;
        temp.sightRadius *= currentSelectedPersonality.sightRadius;
        temp.minTimeBetweenChoice = currentSelectedMotivation.minTimeBetweenActions;
        temp.maxTimeBetweenChoice = currentSelectedMotivation.maxTimeBetweenActions;

        if (temp.selectedPlacer)
        {

        }
        else if (temp.selectedRemover)
        {

        }
    }

    public void completeAnimate()
    {
        applyRune();
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

        GetComponent<magicHandler>().endCarve();
    }

    IEnumerator resetRune(runeDataContainer runeToSpawn)
    {
        runeSelected = null;
        runeTraceImage.sprite = null;

        if (drawnPersonality && drawnMotivation && drawnJob) {
            handeler.bookObject.transform.parent.transform.Find("Open Book").GetComponent<Button>().onClick.Invoke();

            yield return new WaitForSeconds(4.5f);

            handeler.bookObject.transform.Find("HiddenPage").gameObject.SetActive(true);
            handeler.bookObject.transform.Find("LeftPage").gameObject.SetActive(false);

            handeler.bookObject.transform.Find("HiddenPage/Rune Name").GetComponent<Text>().text = currentSelectedPersonality.name + " " + currentSelectedMotivation.name + " " + currentSelectedJob.name;
            handeler.bookObject.transform.Find("HiddenPage/Rune Description").GetComponent<Text>().text = currentSelectedPersonality.runeDescription + " \n\n " + currentSelectedMotivation.runeDescription + " \n\n " + currentSelectedJob.runeDescription;

            handeler.bookObject.transform.Find("HiddenPage/RuneHolder/Personality Rune Img").GetComponent<Image>().sprite = currentSelectedPersonality.runeAsset;
            handeler.bookObject.transform.Find("HiddenPage/RuneHolder/Motivation Rune Img").GetComponent<Image>().sprite = currentSelectedMotivation.runeAsset;
            handeler.bookObject.transform.Find("HiddenPage/RuneHolder/Job Rune Img").GetComponent<Image>().sprite = currentSelectedJob.runeAsset;
            yield return null;
        } else switch (runeToSpawn.runeType) {
            case runeTypes.personality:
            if (currentSelectedMotivation != null && !drawnMotivation) runeSelected = currentSelectedMotivation;
            else if (currentSelectedJob != null && !drawnJob) runeSelected = currentSelectedJob;
            else { 
                yield return new WaitForSeconds(1.5f);
                handeler.bookObject.transform.parent.transform.Find("Open Book").GetComponent<Button>().onClick.Invoke(); 
            }
            break;
            case runeTypes.motivation:
            if (currentSelectedJob != null && !drawnJob) runeSelected = currentSelectedJob;
            else if (currentSelectedPersonality != null && !drawnPersonality) runeSelected = currentSelectedPersonality;
            else {
                yield return new WaitForSeconds(1.5f);
                handeler.bookObject.transform.parent.transform.Find("Open Book").GetComponent<Button>().onClick.Invoke();
            }
            break;
            case runeTypes.job:
            if (currentSelectedPersonality != null && !drawnPersonality) runeSelected = currentSelectedPersonality;
            else if (currentSelectedMotivation != null && !drawnMotivation) runeSelected = currentSelectedMotivation;
            else {
                yield return new WaitForSeconds(1.5f);
                handeler.bookObject.transform.parent.transform.Find("Open Book").GetComponent<Button>().onClick.Invoke();
            }
            break;
        }
        if (runeSelected != null)
        {
            runeTraceImage.sprite = runeSelected.runeAsset;
            if (GameObject.Find("Bound Rect").transform.childCount > 0) Destroy(GameObject.Find("Bound Rect").transform.GetChild(0).gameObject);
            GameObject temp = Instantiate(runeSelected.linePlacesHolder, GameObject.Find("Bound Rect").transform);
            temp.name = temp.name.Replace("(Clone)", "");
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

            

            foreach (Transform item in runeSelected.linePlacesHolder.transform)
            {
                

                Gizmos.DrawWireSphere(item.position, errorDistance);
            }
        }
    }

}
