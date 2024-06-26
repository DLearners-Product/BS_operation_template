using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAManager : MonoBehaviour
{
    List<ActivityContent> currentSlideActivityContents = new List<ActivityContent>();
    ActivityContent[] activityContents;
    public static QAManager instance;
    Dictionary<string, Component> additionalField;
    int currentSlideNum;

    private void Awake() {
        if(instance == null)
            instance = this;
        additionalField = new Dictionary<string, Component>();
    }

    public void UpdateActivityQuestion(){
        currentSlideActivityContents?.Clear();
        currentSlideNum = Main_Blended.OBJ_main_blended.levelno;
        if(activityContents == null)
            activityContents = ActivityContentManager.instance.activityContents;
        
        foreach (var activityContent in activityContents)
        {
            if(activityContent.slideNo == currentSlideNum){
                currentSlideActivityContents.Add(activityContent);
            }
        }
    }

    public List<ActivityContent> GetCurrentActivityContents(){
        UpdateActivityQuestion();
        return currentSlideActivityContents;
    }

    public ActivityContent GetActivity(int activityNo){
        if(activityNo > (currentSlideActivityContents.Count - 1)) return null; 

        return currentSlideActivityContents[activityNo];
    }

    public Component[] GetSubQuestionsAt(int activityNo, int questionNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];

        if(!currentSlideActivityContent.hasSubquestion){
            throw new InvalidOperationException("GetSubQuestionsAt() function valid only for Activity with SubQuestion");
        }

        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                return currentSlideActivityContent.staticQAWithSQ.qaWithSubQuestion[questionNo].staticSubQA.GetQuestions();
            case QuestionType.Dynamic:
                // Dynamic Question Type Implementation come here
                return null;
            default:
                return null;
        }
    }

    public Component[] GetSubQuestionOptionsAt(int activityNo, int questionNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];

        if(!currentSlideActivityContent.hasSubquestion){
            throw new InvalidOperationException("GetSubQuestionOptionsAt() function valid only for Activity with SubQuestion");
        }

        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                return currentSlideActivityContent.staticQAWithSQ.qaWithSubQuestion[questionNo].staticSubQA.options;
            case QuestionType.Dynamic:
                // Dynamic options Type Implementation come here
                return null;
            default:
                return null;
        }
    }

    public Component GetQuestionAt(int activityNo, int questionNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];

        if(currentSlideActivityContent.hasSubquestion){
            throw new InvalidOperationException("For Activity with Sub questions use GetSubQuestionsAt() function to get question");
        }

        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                return currentSlideActivityContent.staticQA.questions[questionNo].question;
            case QuestionType.Dynamic:
                return currentSlideActivityContent.dynamicQA.questions[questionNo].question;
            default:
                return null;
        }
    }

    public Component[] GetAllQuestions(int activityNo){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];

        if(currentSlideActivityContent.hasSubquestion){
            throw new InvalidOperationException("For Activity with Sub questions use GetSubQuestionsAt() function to get question");
        }

        Component[] questions;
        switch(currentSlideActivityContent.questionType){
            case QuestionType.Static:
                questions = new Component[currentSlideActivityContent.staticQA.questions.Length];
                for(int i=0; i<questions.Length; i++){
                    questions[i] = currentSlideActivityContent.staticQA.questions[i].question;
                }
                return questions;
            case QuestionType.Dynamic:
                questions = new Component[currentSlideActivityContent.dynamicQA.questions.Length];
                for(int i=0; i<questions.Length; i++){
                    questions[i] = currentSlideActivityContent.dynamicQA.questions[i].question;
                }
                return questions;
            default:
                return null;
        }
    }

    public Component[] GetOption(int activityNo, int questionNo=0){
        ActivityContent currentSlideActivityContent = currentSlideActivityContents[activityNo];

        if(currentSlideActivityContent.hasSubquestion){
            throw new InvalidOperationException("For Activity with Sub questions use GetSubQuestionsOptionsAt() function to get options");
        }

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

        List<AdditionalComponent> additionalComponents = new List<AdditionalComponent>();
        if (currentSlideActivityContent.questionType == QuestionType.Static){
            additionalComponents = currentSlideActivityContent.staticQA.additionalFields;
        }else if(currentSlideActivityContent.questionType == QuestionType.Dynamic){
            additionalComponents = currentSlideActivityContent.dynamicQA.additionalFields;
        }

        for(int i=0; i<additionalComponents.Count; i++){
            if(!additionalField.ContainsKey(additionalComponents[i].key)) continue;

            additionalField.Add(additionalComponents[i].key, additionalComponents[i].value);
        }

        return additionalField;
    }
}
