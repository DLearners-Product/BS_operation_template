using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ActivityContentManager)), CanEditMultipleObjects]
public class ActivityContentManagerEditor : Editor
{
    ActivityContentManager contentManager;
    SerializedObject getTarget;

    SerializedProperty _activityContents;
    int arrSize;

    private void OnEnable()
    {
        contentManager = (ActivityContentManager)target;
        
        getTarget = new SerializedObject(contentManager);

        _activityContents = getTarget.FindProperty("activityContents");
    }

    public override void OnInspectorGUI()
    {
        getTarget.Update();

        arrSize = _activityContents.arraySize;
        arrSize = EditorGUILayout.IntField ("List Size", arrSize);

        // Debug.Log($"Array Size : {arrSize}");
        if(arrSize != _activityContents.arraySize){
            while(arrSize > _activityContents.arraySize){
                _activityContents.InsertArrayElementAtIndex(_activityContents.arraySize);
            }

            while(arrSize < _activityContents.arraySize){
                _activityContents.DeleteArrayElementAtIndex(_activityContents.arraySize - 1);
            }
        }

        for(int i=0; i<_activityContents.arraySize; i++){

            EditorGUILayout.Space();

            EditorGUILayout.LabelField($"Activity {(i + 1)}");

            SerializedProperty activityContent = _activityContents.GetArrayElementAtIndex(i);

            SerializedProperty questionType = activityContent.FindPropertyRelative("questionType");

            SerializedProperty slideNo = activityContent.FindPropertyRelative("slideNo");

            EditorGUILayout.PropertyField(slideNo);
            EditorGUILayout.PropertyField(questionType);

            // Debug.Log("--- > " + questionType.enumNames[questionType.enumValueIndex]);
            switch(questionType.enumValueIndex){

                case 1:
                    SerializedProperty staticQA = activityContent.FindPropertyRelative("staticQA");

                    MethodInfo staticMethodInfo = staticQA.serializedObject.targetObject.GetType().GetMethod("UpdateAsset");

                    staticMethodInfo?.Invoke(staticQA.serializedObject.targetObject, null);

                    EditorGUILayout.PropertyField(staticQA);
                    break;

                case 2:
                    SerializedProperty dynamicQA = activityContent.FindPropertyRelative("dynamicQA");

                    MethodInfo dynamicMethodInfo = dynamicQA.serializedObject.targetObject.GetType().GetMethod("UpdateAsset");

                    dynamicMethodInfo?.Invoke(dynamicQA.serializedObject.targetObject, null);

                    EditorGUILayout.PropertyField(dynamicQA);
                    break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();            
        }

        EditorGUILayout.Space();

        getTarget.ApplyModifiedProperties();
    }

    void UpdateInspector(){

    }
}