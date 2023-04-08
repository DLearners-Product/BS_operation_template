using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ActivityContentManager : MonoBehaviour
{
    [SerializeField] int questionCount;
    [SerializeField] private QuestionType questionType;
    public ActivityQAManager activityQAData;

    private void OnValidate()
    {
        Debug.Log($"Came here");
        UpdateInspector();
    }

    void UpdateInspector(){
        switch(questionType){
            case QuestionType.Static:
                Debug.Log($"Question type is static "+(activityQAData.GetType() == typeof(StaticQA)));
                Debug.Log($"Question type is static "+(activityQAData.GetType() == typeof(DynamicQA)));
                Debug.Log($"Question type is static "+(activityQAData == null));

                activityQAData = new StaticQA(questionCount);
                break;
            
            case QuestionType.Dynamic:
                Debug.Log($"Question type is dynamic "+(activityQAData.GetType() == typeof(DynamicQA)));
                activityQAData = new DynamicQA(questionCount);
                break;

            default:
                Debug.Log($"Question type is none");
                activityQAData = null;
                break;
        }
    }
}
