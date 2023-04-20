using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAManager : MonoBehaviour
{
    List<ActivityContent> currentSlideActivityContents;
    ActivityContent[] activityContents;
    public static QAManager instance;
    Dictionary<string, Component> additionalField;
    int currentSlideNum;
    
    void Start()
    {
        if(instance == null)
            instance = this;

        currentSlideActivityContents = new List<ActivityContent>();
        additionalField = new Dictionary<string, Component>();
        activityContents = ActivityContentManager.instance.activityContents;
        Debug.Log(activityContents);
    }

    public void UpdateActivityQuestion(){
        currentSlideActivityContents.Clear();
        currentSlideNum = Main_Blended.OBJ_main_blended.levelno;
        foreach (var activityContent in activityContents)
        {
            if(activityContent.slideNo == currentSlideNum){
                currentSlideActivityContents.Add(activityContent);
            }
        }
    }

    ActivityContent GetActivity(int activityNo){
        if(activityNo > (currentSlideActivityContents.Count - 1)) return null;
        
        return currentSlideActivityContents[activityNo];
    }

    public Component GetQuestionAt(int activityNo, int questionNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];
        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                return currentSlideActivityContent.staticQA.questions[questionNo].question;
            case QuestionType.Dynamic:
                return currentSlideActivityContent.dynamicQA.questions[questionNo].question;
            default:
                return null;
        }
    }

    public Component[] GetOption(int activityNo, int questionNo=0){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];
        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                return currentSlideActivityContent.staticQA.options;
            case QuestionType.Dynamic:
                return currentSlideActivityContent.dynamicQA.questions[questionNo].options;
            default:
                return null;
        }
    }

    public Component[] GetAnswer(int activityNo, int questionNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];
        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                return currentSlideActivityContent.staticQA.questions[questionNo].answers;
            case QuestionType.Dynamic:
                return currentSlideActivityContent.dynamicQA.questions[questionNo].GetAnswers();
            default:
                return null;
        }
    }
    public Dictionary<string, Component> GetAdditionalField(int activityNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];
        List<AdditionalComponent> additionalComponents = currentSlideActivityContent.staticQA.additionalFields;

        for(int i=0; i<additionalComponents.Count; i++){
            additionalField.Add(additionalComponents[i].key, additionalComponents[i].value);
        }

        return additionalField;
    }
}
