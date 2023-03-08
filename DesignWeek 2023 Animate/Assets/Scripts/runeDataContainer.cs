using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(menuName = "Create New Rune", fileName = "New Rune")]
public class runeDataContainer : ScriptableObject
{
    public runeTypes runeType;
    public string runeDescription;

    public personalityTypes personality;
    public motivationLevels motivation;
    public jobs runeJob;
    public Sprite runeAsset;
    public List<GameObject> linePlaces;
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

[CustomEditor(typeof(runeDataContainer))]
public class runeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        runeDataContainer container = (runeDataContainer)target;

        SerializedProperty m_runeAssetSprite = serializedObject.FindProperty("runeAsset");
        SerializedProperty m_linePlaces = serializedObject.FindProperty("linePlaces");

        container.runeType = (runeTypes)EditorGUILayout.EnumPopup("Rune Type: ", container.runeType);
        EditorGUILayout.LabelField("Rune Description: ");
        container.runeDescription = EditorGUILayout.TextArea(container.runeDescription, GUILayout.Height(50));

        EditorGUILayout.Space(20);
        switch (container.runeType)
        {
            case runeTypes.job:
                container.runeJob = (jobs)EditorGUILayout.EnumPopup("Item's Job: ", container.runeJob);
                break;
            case runeTypes.motivation:
                container.motivation = (motivationLevels)EditorGUILayout.EnumPopup("Motivation Level: ", container.motivation);
                break;
            case runeTypes.personality:
                container.personality = (personalityTypes)EditorGUILayout.EnumPopup("Object's Personality: ", container.personality);
                break;
        }
        EditorGUILayout.PropertyField(m_runeAssetSprite,new GUIContent("Rune Asset Sprite: "));
        EditorGUILayout.PropertyField(m_linePlaces, new GUIContent("Rune line Points: "));
        serializedObject.ApplyModifiedProperties();
    }
}
