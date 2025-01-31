using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(CropData))]
[CanEditMultipleObjects]
public class CICropData : Editor
{
    private SerializedProperty cropName;
    private SerializedProperty cropDescription;
    private SerializedProperty type;
    private SerializedProperty cropHealth;
    private SerializedProperty cropWalkSpeed;
    private SerializedProperty cropRunSpeed;
    private SerializedProperty cooldownTime;
    private SerializedProperty damage;
    private SerializedProperty cropIcon;
    private void OnEnable()
    {
        cropName = serializedObject.FindProperty("cropName");
        cropDescription = serializedObject.FindProperty("cropDescription");
        type = serializedObject.FindProperty("type");
        cropHealth = serializedObject.FindProperty("cropHealth");
        cropWalkSpeed = serializedObject.FindProperty("cropWalkSpeed");
        cropRunSpeed = serializedObject.FindProperty("cropRunSpeed");
        cooldownTime = serializedObject.FindProperty("cooldownTime");
        damage = serializedObject.FindProperty("damage");
        cropIcon = serializedObject.FindProperty("cropIcon");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();
        //before
        base.OnInspectorGUI();

        //after

        //EditorGUILayout.LabelField(cropName.stringValue.ToUpper(), EditorStyles.boldLabel); 
        //Space(2);

        //EditorGUILayout.LabelField("CROP NAME", EditorStyles.boldLabel);
        //cropName.stringValue = EditorGUILayout.TextField(cropName.stringValue, GUILayout.Width(250));
        //Space(2);

        //EditorGUILayout.LabelField("CROP DESCRIPTION", EditorStyles.boldLabel);
        //cropDescription.stringValue = EditorGUILayout.TextArea(cropDescription.stringValue, GUILayout.Height(70));

        //serializedObject.ApplyModifiedProperties();
    }

    private void Space(int spacesToAdd)
    {
        for (int i = 0; i < spacesToAdd; i++)
        {
            EditorGUILayout.Space();
        }
    }

}
#endif