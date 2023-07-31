using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataPlayer))]
public class DataPlayerEditor : Editor
{
    private SerializedProperty targetObjectProperty;
    private SerializedProperty positionProperty;
    private SerializedObject hapticControllerSerializedObject;
    private SerializedProperty forceXProperty;
    private SerializedProperty forceYProperty;
    private SerializedProperty forceZProperty;
    private SerializedProperty csvFileProperty;


    private void OnEnable()
    {
        targetObjectProperty = serializedObject.FindProperty("targetObject");
        positionProperty = serializedObject.FindProperty("position");
        DataPlayer dataPlayer = (DataPlayer)target;
        hapticControllerSerializedObject = new SerializedObject(dataPlayer.hapticController);
        forceXProperty = hapticControllerSerializedObject.FindProperty("forceX");
        forceYProperty = hapticControllerSerializedObject.FindProperty("forceY");
        forceZProperty = hapticControllerSerializedObject.FindProperty("forceZ");
        csvFileProperty = serializedObject.FindProperty("csvFile"); // �߰��� �κ�
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(targetObjectProperty);
        EditorGUILayout.PropertyField(positionProperty);

        hapticControllerSerializedObject.Update();
        EditorGUILayout.PropertyField(forceXProperty);
        EditorGUILayout.PropertyField(forceYProperty);
        EditorGUILayout.PropertyField(forceZProperty);
        hapticControllerSerializedObject.ApplyModifiedProperties();

        // csvFileProperty�� null�� �ƴ��� Ȯ�� �� PropertyField�� ǥ���մϴ�.
        if (csvFileProperty != null)
        {
            EditorGUILayout.PropertyField(csvFileProperty);
        }
        else
        {
            Debug.LogError("csvFileProperty is null.");
        }

        serializedObject.ApplyModifiedProperties();

        DataPlayer dataPlayer = (DataPlayer)target;

        if (GUILayout.Button("Play"))
        {
            dataPlayer.Play();
        }
        if (GUILayout.Button("Stop"))
        {
            dataPlayer.Stop();
        }
    }
}
