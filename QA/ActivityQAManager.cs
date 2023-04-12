using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ActivityQA
{
    protected int questionCount;
    public FileType questionType;
    public FileType optionType;

    public ActivityQA(){}

    public ActivityQA(int count){
        this.questionCount = count;
    }

    public abstract void Update();
}

[Serializable]
public class QA{
    public Component question;
    public Component[] answers;
    
    public void Update(){
        question.UpdateDimension();
        foreach(var answer in answers){
            answer.UpdateDimension();
        }
    }
}

[Serializable]
public class QAO{
    public Component question;
    public Component[] answers;
    public Component[] options;

    public void Update(){
        question.UpdateDimension();
        foreach(var answer in answers){
            answer.UpdateDimension();
        }

        foreach(var option in options){
            option.UpdateDimension();
        }
    }
}

[Serializable]
public class DynamicQA : ActivityQA{
    public QAO[] questions;

    public DynamicQA() : base(){}

    public DynamicQA(int count) : base(count){
        InstantiateObject();
    }

    void InstantiateObject(){
        questions = new QAO[questionCount];

        for(int i=0; i<questionCount; i++){
            questions[i] = new QAO();
        }
    }

    void UpdateAsset(){
        foreach(var question in questions){
            question.Update();
        }
    }

    public override void Update(){
        if(questions == null || questions.Length == 0){
            InstantiateObject();
        }else{
            UpdateAsset();
        }
    }
}

[Serializable]
public class StaticQA : ActivityQA{
    public QA[] questions;
    public Component[] options;
    
    public StaticQA() : base(){}

    public StaticQA(int count) : base(count){
        InstantiateObject();
    }

    void InstantiateObject(){
        questions = new QA[questionCount];
        options = new Component[questionCount];

        for(int i=0; i<questionCount; i++){
            options[i] = new Component();
        }
    }

    void UpdateAsset(){
        foreach(var question in questions){
            question.Update();
        }
        foreach(var option in options){
            option.UpdateDimension();
        }
    }

    public override void Update(){
        if(questions == null || questions.Length == 0){
            InstantiateObject();
        }else{
            UpdateAsset();
        }
    }
}

[Serializable]
public class ActivityContent{
    public QuestionType questionType;
    public int slideNo;
    public StaticQA staticQA = new StaticQA();
    public DynamicQA dynamicQA = new DynamicQA();

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