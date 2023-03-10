using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(menuName = "Create New Rune", fileName = "New Rune")]
public class runeDataContainer : ScriptableObject
{
    public runeTypes runeType;
    public string runeDescription;

    public Sprite runeAsset;
    public GameObject linePlacesHolder;

    public float minTimeBetweenActions = 3f, maxTimeBetweenActions = 5f;
    public float sightRadius = 1f;
    public float moveSpeed = 1f;
    public List<GameObject> buildingsToPlace;
    public List<string> stringsToAttack;
}

[System.Serializable]
public enum runeTypes {
    personality,
    motivation,
    job
}

[CustomEditor(typeof(runeDataContainer))]
public class runeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        runeDataContainer container = (runeDataContainer)target;

        SerializedProperty m_runeAssetSprite = serializedObject.FindProperty("runeAsset");
        SerializedProperty m_buildingsToPlace = serializedObject.FindProperty("buildingsToPlace");
        SerializedProperty m_stringsToAttack = serializedObject.FindProperty("stringsToAttack");

        container.runeType = (runeTypes)EditorGUILayout.EnumPopup("Rune Type: ", container.runeType);
        EditorGUILayout.LabelField("Rune Description: ");
        container.runeDescription = EditorGUILayout.TextArea(container.runeDescription, GUILayout.Height(50));

        switch (container.runeType)
        {
            case runeTypes.job:
                EditorGUILayout.PropertyField(m_buildingsToPlace, new GUIContent("Buildings To Place: "));
                EditorGUILayout.PropertyField(m_stringsToAttack, new GUIContent("Names Of Objects To Attack: "));
                break;
            case runeTypes.motivation:
                container.minTimeBetweenActions = EditorGUILayout.FloatField("Min Time Between Actions: ", container.minTimeBetweenActions);
                container.maxTimeBetweenActions = EditorGUILayout.FloatField("Max Time Between Actions: ", container.maxTimeBetweenActions);
                break;
            case runeTypes.personality:
                container.moveSpeed = EditorGUILayout.FloatField("Move Speed Modifier: ", container.moveSpeed);
                container.sightRadius = EditorGUILayout.FloatField("Sight Radius Modifier: ", container.sightRadius);
                break;
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.PropertyField(m_runeAssetSprite,new GUIContent("Rune Asset Sprite: "));
        container.linePlacesHolder = (GameObject)EditorGUILayout.ObjectField("Line Place Holder: ", container.linePlacesHolder, typeof(GameObject));
        EditorGUILayout.Space(20f);
        if (GUILayout.Button("Save")) {
            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssets();
        }
        serializedObject.ApplyModifiedProperties();
        
    }
}
