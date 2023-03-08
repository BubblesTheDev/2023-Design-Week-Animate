using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class bookHandeler : MonoBehaviour
{
    [Header("Scripting Data")]
    [SerializeField] private List<runePageContainer> runePageData;
    [SerializeField] private int currentRunePageIndex;
    [SerializeField] private runeCarver carverScript;

    [Space, Header("Graphic Data")]
    [SerializeField] private GameObject bookObject;
    [SerializeField] private GameObject runeSelectorPrefab;
    [SerializeField] private List<GameObject> pages;

    private void Awake()
    {

        createRunesForPages();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1)) setPage(0);
        if (Input.GetKeyDown(KeyCode.F2)) setPage(1);
        if (Input.GetKeyDown(KeyCode.F3)) setPage(2);


        handlePageGraphics();
    }

    void createRunesForPages()
    {
        for (int i = 0; i < runePageData.Count; i++)
        {
            for (int x = 0; x < runePageData[i].runes.Count; x++)
            {
                GameObject temp = Instantiate(runeSelectorPrefab, GameObject.Find("Content " + runePageData[i].name).transform.position, Quaternion.identity, GameObject.Find("Content " + runePageData[i].name).transform);
                temp.name = "Rune Button " + runePageData[i].runes[x].name;
                temp.transform.Find("Rune Name").gameObject.GetComponent<Text>().text = "Name: " + runePageData[i].runes[x].name;
                temp.transform.Find("Rune Description").gameObject.GetComponent<Text>().text = "Description: " + runePageData[i].runes[x].runeDescription;
                temp.transform.Find("RunePicture").gameObject.GetComponent<Image>().sprite = runePageData[i].runes[x].runeAsset;
                temp.GetComponent<Button>().onClick.AddListener( delegate { GameObject.Find("Game Manager").GetComponent<bookHandeler>().selectRune(); });
            }
        }
        bookObject.transform.Find("LeftPage").gameObject.SetActive(false);
        bookObject.transform.Find("RightPage").gameObject.SetActive(false);
    }

    public void selectRune()
    {
        GameObject thingCalling = EventSystem.current.currentSelectedGameObject;
        if (runePageData[currentRunePageIndex].runes.Contains(runePageData[currentRunePageIndex].runes.Where(runeDataContainer => runeDataContainer.name == new string(thingCalling.name.Replace("Rune Button ", ""))).SingleOrDefault())){
            carverScript.runeSelected = runePageData[currentRunePageIndex].runes.Where(runeDataContainer => runeDataContainer.name == new string(thingCalling.name.Replace("Rune Button ", ""))).FirstOrDefault();
            carverScript.runeTraceImage.sprite = carverScript.runeSelected.runeAsset;
        }
    }

    public void handlePageGraphics() {
        switch (currentRunePageIndex) {
            case 0:
            pages[0].SetActive(true);
            pages[1].SetActive(false);
            pages[2].SetActive(false);
            break;
            case 1:
            pages[1].SetActive(true);
            pages[2].SetActive(false);
            pages[0].SetActive(false);
            break;
            case 2:
            pages[2].SetActive(true);
            pages[0].SetActive(false);
            pages[1].SetActive(false);
            break;
        }
    }

    public void setPage(int pageIndex) {
        if (pageIndex + 1 > pages.Count) print("There is no page at the index: " + (pageIndex+1));
        else currentRunePageIndex = pageIndex;
    }

    public void enableBookWrapper() {
        StartCoroutine(enableBook());
    }

    public void disableBookWrapper() {
        StartCoroutine(disableBook());
    }

    public IEnumerator enableBook() {
        bookObject.SetActive(true);
        while(bookObject.transform.position != Vector3.zero) {
           bookObject.transform.Translate((Vector3.zero - bookObject.transform.position) * Time.deltaTime * 25f);
            bookObject.transform.localScale = Vector3.Lerp(Vector3.one * .25f, Vector3.one * 2, Time.deltaTime * 5f);
            yield return new WaitForEndOfFrame();
        }

        //Once book is in center of screen, play animation of opening
        //wait for animation to finish
        yield return new WaitForSeconds(5f);

        bookObject.transform.Find("Right Page").gameObject.SetActive(true);
        bookObject.transform.Find("Left Page").gameObject.SetActive(true);
    }

    public IEnumerator disableBook() {
        yield return new WaitForEndOfFrame();
    }
}
