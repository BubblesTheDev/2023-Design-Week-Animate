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

    private void Awake()
    {
        createRunesForPages();
    }

    private void Update()
    {
        if (carverScript.objToCarve != null && !bookObject.activeSelf) bookObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.F1)) currentRunePageIndex = 0;
        if (Input.GetKeyDown(KeyCode.F2)) currentRunePageIndex = 1;
        if (Input.GetKeyDown(KeyCode.F3)) currentRunePageIndex = 2;
    }

    void createRunesForPages()
    {
        for (int i = 0; i < runePageData.Count; i++)
        {
            for (int x = 0; x < runePageData[i].runes.Count; x++)
            {
                GameObject temp = Instantiate(runeSelectorPrefab, GameObject.Find("Content").transform.position, Quaternion.identity, GameObject.Find("Content").transform);
                temp.name = "Rune Button " + runePageData[i].runes[x].name;
                temp.transform.Find("RunePicture").gameObject.GetComponent<Image>().sprite = runePageData[i].runes[x].runeAsset;
                temp.GetComponent<Button>().onClick.AddListener( delegate { GameObject.Find("Game Manager").GetComponent<bookHandeler>().selectRune(); });
            }
        }
    }

    public void selectRune()
    {
        GameObject thingCalling = EventSystem.current.currentSelectedGameObject;
        if (runePageData[currentRunePageIndex].runes.Contains(runePageData[currentRunePageIndex].runes.Where(runeDataContainer => runeDataContainer.name == new string(thingCalling.name.Replace("Rune Button ", ""))).SingleOrDefault())){
            carverScript.runeSelected = runePageData[currentRunePageIndex].runes.Where(runeDataContainer => runeDataContainer.name == new string(thingCalling.name.Replace("Rune Button ", ""))).FirstOrDefault();
            carverScript.runeTraceImage.sprite = carverScript.runeSelected.runeAsset;
        }
    }
}
