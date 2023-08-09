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

        using(new EditorGUI.DisabledScope(true)){
            arrSize = EditorGUILayout.IntField ("Total Activity", arrSize);
        }

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

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField($"Activity {(i + 1)}");
            if(GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(15))){
                _activityContents.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();

            SerializedProperty activityContent = _activityContents.GetArrayElementAtIndex(i);

            if(activityContent == null) continue;

            SerializedProperty questionType = activityContent.FindPropertyRelative("questionType");

            SerializedProperty slideNo = activityContent.FindPropertyRelative("slideNo");

            SerializedProperty activityName = activityContent.FindPropertyRelative("activityName");

            EditorGUILayout.PropertyField(slideNo);
            EditorGUILayout.PropertyField(activityName);
            EditorGUILayout.PropertyField(questionType, new GUIContent("QA Type"));

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

        if(GUILayout.Button("Add Activity")){
            _activityContents.InsertArrayElementAtIndex(_activityContents.arraySize);
        }

        getTarget.ApplyModifiedProperties();
    }

    void UpdateInspector(){

    }
}