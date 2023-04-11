using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ActivityContentManager)), CanEditMultipleObjects]
public class ActivityContentManagerEditor : Editor
{
    ActivityContent[] activityContents;
    // QuestionType qType;
    ActivityContentManager contentManager;
    SerializedObject getTarget;

    SerializedProperty staticQA;
    SerializedProperty dynamicQA;
    SerializedProperty defaultQA;
    SerializedProperty _activityContents;
    int arrSize;

    private void OnEnable()
    {
        contentManager = (ActivityContentManager)target;
        
        activityContents = contentManager.activityContents;

        getTarget = new SerializedObject(contentManager);

        _activityContents = getTarget.FindProperty("activityContents");
    }

    public override void OnInspectorGUI()
    {
        getTarget.Update();

        using(new EditorGUILayout.VerticalScope("HelpBox")){
            arrSize = _activityContents.arraySize;
            arrSize = EditorGUILayout.IntField ("List Size", arrSize);

            Debug.Log($"Array Size : {arrSize}");
            if(arrSize != _activityContents.arraySize){
                while(arrSize > _activityContents.arraySize){
                    _activityContents.InsertArrayElementAtIndex(_activityContents.arraySize);
                }

                while(arrSize < _activityContents.arraySize){
                    _activityContents.DeleteArrayElementAtIndex(_activityContents.arraySize - 1);
                }
            }

            // for(int i=0; i<contentManager.activityContents.Length; i++){

            //     SerializedProperty activityContent = _activityContents.GetArrayElementAtIndex(i);

            //     SerializedProperty questionType = activityContent.FindPropertyRelative("questionType");

            //     activity.questionType = (QuestionType)EditorGUILayout.EnumPopup("Question Type", activityContent.questionType);

            //     EditorGUILayout.Space();

                
            //     // switch (activityContent.questionType)
            //     // {
            //     //     case QuestionType.Static:

            //     //         MethodInfo staticMethodInfo = staticQA.serializedObject.targetObject.GetType().GetMethod("UpdateAsset");

            //     //         staticMethodInfo?.Invoke(staticQA.serializedObject.targetObject, null);

            //     //         EditorGUILayout.PropertyField(getTarget.);
            //     //         break;
            //     //     case QuestionType.Dynamic:

            //     //         MethodInfo dynamicMethodInfo = dynamicQA.serializedObject.targetObject.GetType().GetMethod("UpdateAsset");

            //     //         dynamicMethodInfo?.Invoke(dynamicQA.serializedObject.targetObject, null);

            //     //         EditorGUILayout.PropertyField(dynamicQA);
            //     //         break;
            //     //     default:
            //     //         EditorGUILayout.PropertyField(defaultQA);
            //     //         break;
            //     // }
            // }
        }

        EditorGUILayout.Space();

        getTarget.ApplyModifiedProperties();
    }

    void UpdateInspector(){

    }
}