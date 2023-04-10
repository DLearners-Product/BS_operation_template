using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActivityQA
{
    protected int questionCount;
    public FileType questionType;
    public FileType optionType;

    public ActivityQA(){
        Debug.Log($"Base class initialized");
    }

    public ActivityQA(int count){
        this.questionCount = count;
    }
}

[Serializable]
public class QA{
    public Component question;
    public Component answer;
    
    public void Update(){
        question.UpdateDimension();
        answer.UpdateDimension();
    }
}

[Serializable]
public class QAO{
    public Component question;
    public Component answer;
    public Component[] options;
}


[Serializable]
public class DynamicQA : ActivityQA{
    public QAO[] questions;

    public DynamicQA() : base(){
        Debug.Log($"Dynamic class initialized");
    }

    public DynamicQA(int count) : base(count){
        InstantiateObject();
    }

    void InstantiateObject(){
        questions = new QAO[questionCount];

        for(int i=0; i<questionCount; i++){
            questions[i] = new QAO();
        }
    }
}

[Serializable]
public class StaticQA : ActivityQA{
    public QA[] questions;
    public Component[] options;
    
    public StaticQA() : base(){
        Debug.Log($"Static class initialized");
    }

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
        // for(int i=0; i<questions.Length; i++){
        //     questions[i].UpdateDimen();
        // }
    }

    public void Update(){
        if(questions == null || questions.Length == 0){
            InstantiateObject();
        }else{

        }
    }
}