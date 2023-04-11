using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [ExecuteInEditMode]
public class ActivityContentManager : MonoBehaviour
{
    public static ActivityContentManager instance;
    public QuestionType questionType;
    public int questionCount, o_questionCount;
    public StaticQA staticQA = new StaticQA();
    public DynamicQA dynamicQA = new DynamicQA();
    public ActivityQA defaultQA = null;

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void OnValidate()
    {

    }

    public void UpdateAsset(){
#if UNITY_EDITOR
        switch(questionType){
            case QuestionType.Static:
                staticQA.Update();
                break;
            case QuestionType.Dynamic:
                dynamicQA.Update();
                break;
        }
#endif
    }
}
