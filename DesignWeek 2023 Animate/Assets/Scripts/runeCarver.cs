using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class runeCarver : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private Image runeCircle;
    [SerializeField] private GameObject runeBook;
    [SerializeField] private List<GameObject> bookPages;

    [Header("Drawing Settings")]
    public LineRenderer carvingLine;
    public GameObject objToCarve;
    public runeDataContainer runeSelected;
    [SerializeField] private float maxDistanceBetweenPoints;
    [SerializeField] private runeTypes currentRuneToCarve;
    [SerializeField] private float errorDistance;

    private void Update() {
        
    }

    void removeLine() {
        carvingLine.positionCount = 0;
    }


}
