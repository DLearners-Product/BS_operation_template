using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActivityContentManager)), CanEditMultipleObjects]
public class ActivityContentManagerEditor : Editor
{
    int questionCount=0;
    QuestionType qType;
    ActivityContentManager contentManager;
    SerializedObject getTarget;

    SerializedProperty staticQA;
    SerializedProperty dynamicQA;
    SerializedProperty defaultQA;

    private void OnEnable()
    {
        contentManager = (ActivityContentManager)target;

        questionCount = contentManager.questionCount;
        qType = contentManager.questionType;

        getTarget = new SerializedObject(contentManager);

        staticQA = getTarget.FindProperty("staticQA");
        dynamicQA = getTarget.FindProperty("dynamicQA");
        defaultQA = getTarget.FindProperty("activityQAData");

        Debug.Log($":  {staticQA.GetType()}");
    }

    public override void OnInspectorGUI()
    {
        getTarget.Update();

        using(new EditorGUILayout.VerticalScope("HelpBox")){
            contentManager.questionCount = (int)EditorGUILayout.IntField("Question Count", contentManager.questionCount);

            contentManager.questionType = (QuestionType)EditorGUILayout.EnumPopup("Obstacle Type", contentManager.questionType);

            EditorGUILayout.Space();

            switch (contentManager.questionType)
            {
                case QuestionType.Static:
                    // staticQA.GetField("InstantiateObject", )
                    staticQA.serializedObject.targetObject.GetType().GetField("");
                    EditorGUILayout.PropertyField(staticQA);
                    break;
                case QuestionType.Dynamic:
                    EditorGUILayout.PropertyField(dynamicQA);
                    break;
                default:
                    EditorGUILayout.PropertyField(defaultQA);
                    break;
            }
        }

        EditorGUILayout.Space();

        getTarget.ApplyModifiedProperties();
    }

    void UpdateInspector(){

    }
}