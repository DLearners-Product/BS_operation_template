using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActivityQAManager
{
    protected int questionCount;
    public FileType questionType;
    public FileType optionType;

    public ActivityQAManager(int count){
        this.questionCount = count;
    }
}

[Serializable]
public class QA{
    public Component question;
    public Component answer;
}

[Serializable]
public class DynamicQA : ActivityQAManager{
    public Component question;
    public Component answer;
    [SerializeReference] public Component[] option;

    public DynamicQA(int count) : base(count){
        option = new Component[questionCount];

        for(int i=0; i<questionCount; i++){
            option[i] = new Component();
        }
    }
}

[Serializable]
public class StaticQA : ActivityQAManager{
    public QA[] questions;
    public Component[] options;
    
    public StaticQA(int count) : base(count){
        questions = new QA[questionCount];
        options = new Component[questionCount];

        for(int i=0; i<questionCount; i++){
            options[i] = new Component();
        }
    }
}