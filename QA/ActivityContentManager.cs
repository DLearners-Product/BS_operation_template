using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ActivityContentManager : MonoBehaviour
{
    public QuestionType questionType;
    public int questionCount, o_questionCount;
    // public QA[] qa;
    // public Component[] option;
    public StaticQA staticQA = new StaticQA();
    public DynamicQA dynamicQA = new DynamicQA();
    public ActivityQA defaultQA = null;

    private void OnValidate()
    {
        Debug.Log($"Question Type : {questionType}");

        Debug.Log($"Static Len : {staticQA.options.Length}");

        Debug.Log($"Dynamic Len : {dynamicQA.questions.Length}");
    }
}
