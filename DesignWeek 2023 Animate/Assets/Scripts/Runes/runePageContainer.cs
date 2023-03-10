using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create Rune Page", fileName = "New Rune Page")]
public class runePageContainer : ScriptableObject
{
    public List<runeDataContainer> runes;
    public runeTypes pageStorageType;
}
