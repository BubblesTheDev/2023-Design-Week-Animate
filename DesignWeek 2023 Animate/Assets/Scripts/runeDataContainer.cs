using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Rune", fileName = "New Rune")]
public class runeDataContainer : ScriptableObject
{
    [SerializeField] private runeTypes runeType;

    [SerializeField] private personalityTypes personality;
    [SerializeField] private motivationLevels motivation;
    [SerializeField] private jobs runeJob;
    [SerializeField] private Sprite runeAsset;
    [SerializeField] private List<GameObject> linePlaces;
}

public enum runeTypes {
    personality,
    motivation,
    job
}

public enum personalityTypes {
    None,
    Happy,
    Sad,
    Angry
}

public enum motivationLevels{
    None,
    High,
    Moderate,
    Low
}

public enum jobs {
    None,
    Builder,
    Breaker
}
