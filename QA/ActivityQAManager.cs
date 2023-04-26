using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region DIFFFRENT_TYPE_OF_ACTIVITY_QA
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
    public class DynamicQA : ActivityQA{
        public QAO[] questions;
        public List<AdditionalComponent> additionalFields;
        string qaData;

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

        string GetArrayData(QAO[] qas){
            string data = "";
            for (int i=0; i < qas.Length; i++)
            {
                data += qas[i].GetData();
                if(i < (qas.Length -1)){
                    data += ",";
                }
            }
            return data;
        }

        public string GetQAData(){
            qaData = "[";
            for(int i=0; i<questions.Length; i++){
                qaData += questions[i].GetData();
                if(i < (questions.Length - 1)){
                    qaData += ", ";
                }
            }
            qaData += "]";
            return qaData;
        }
    }

    [Serializable]
    public class StaticQA : ActivityQA{
        public QA[] questions;
        public Component[] options;
        public List<AdditionalComponent> additionalFields;
        string qaData;
        
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
                option.UpdateAssets();
            }
        }

        public override void Update(){
            if(questions == null || questions.Length == 0){
                InstantiateObject();
            }else{
                UpdateAsset();
            }
        }

        public string GetQAData(){
            qaData = "[";
            for(int i=0; i<questions.Length; i++){
                qaData += questions[i].GetData();
                if(i < (questions.Length - 1)){
                    qaData += ", ";
                }
            }
            qaData += "]";
            return qaData;
        }

        public string GetOptionData(){
            qaData = "[";
            for(int i=0; i<options.Length; i++){
                qaData += options[i].GetComponentStringfyData();
                if(i < (options.Length - 1)){
                    qaData += ", ";
                }
            }
            qaData += "]";
            return qaData;
        }

        public string GetAdditionalData(){
            qaData = "[";
            if(additionalFields.Count > 0){
                for(int i=0; i<additionalFields.Count; i++){
                    qaData += additionalFields[i].GetData();
                    if(i < (options.Length - 1)){
                       qaData += ", ";
                    }
                }
            }
            qaData += "]";
            return qaData;
        }
    }
#endregion

[Serializable]
public class QA{
    public Component question;
    public Component[] answers;
    string strData;
    
    public void Update(){
        question.UpdateAssets();
        foreach(var answer in answers){
            answer.UpdateAssets();
        }
    }

    string GetArrayData(Component[] components){
        string data = "[";
        for (int i=0; i < components.Length; i++)
        {
            data += components[i].GetComponentStringfyData();
            if(i < (components.Length -1)){
                data += ",";
            }
        }
        data += "]";
        return data;
    }

    public string GetData(){
        strData = "{";
        strData += $"\"question\":{question.GetComponentStringfyData()}, \"answer\":";
        strData += GetArrayData(answers) + "}";
        return strData;
    }
}

[Serializable]
public class QAO{
    public Component question;
    // public Component[] answers;
    public OptionComponent[] options;
    string strData;

    public void Update(){
        question.UpdateAssets();
        // foreach(var answer in answers){
        //     answer.UpdateAssets();
        // }

        foreach(var option in options){
            option.UpdateAssets();
        }
    }

    public OptionComponent[] GetAnswers(){
        List<OptionComponent> answers = new List<OptionComponent>();
        foreach (var option in options)
        {
            if(option.isAnswer)
                answers.Add(option);
        }
        return answers.ToArray();
    }

    string GetArrayData(Component[] components){
        string data = "[";
        for (int i=0; i < components.Length; i++)
        {
            data += components[i].GetComponentStringfyData();
            if(i < (components.Length -1)){
                data += ",";
            }
        }
        data += "]";
        return data;
    }

    public string GetData(){
        strData = "{";
        strData += $"\"question\":{question.GetComponentStringfyData()}, \"answer\":";
        strData += GetArrayData(GetAnswers()) + ", \"option\":";
        strData += GetArrayData(options) + "}";
        return strData;
    }
}

[Serializable]
public class AdditionalComponent{
    public string key;
    public QuestionType dataType;
    public Component value;
    string activityContent;

    public string GetData(){
        activityContent = "{";
        activityContent += $"\"{key}\"";
        activityContent += " : {";
        activityContent += $"\"DataType\" : \"{dataType.ToString()}\",";
        activityContent += $"\"Value\" : {value.GetComponentStringfyData()}";
        activityContent += "}}";
        return activityContent;
    }
}

[Serializable]
public class ActivityContent{
    public int slideNo;
    public string activityName;
    public QuestionType questionType;
    public StaticQA staticQA = new StaticQA();
    public DynamicQA dynamicQA = new DynamicQA();
    string activityData;

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

    public string GetData(){
        activityData = "{";
        activityData += $"\"ActivityName\":\"{activityName}\", ";
        activityData += $"\"SlideNo.\":\"{slideNo.ToString()}\", ";
        activityData += $"\"QAType\":\"{questionType.ToString()}\"";
        switch(questionType){
            case QuestionType.Static:
                activityData += ", ";
                activityData += $"\"Question Type\":\"{staticQA.questionType.ToString()}\", ";
                activityData += $"\"Option Type\":\"{staticQA.optionType.ToString()}\", ";
                activityData += $"\"QA\":"+staticQA.GetQAData()+", ";
                activityData += $"\"Option\":"+staticQA.GetOptionData();
                break;
            case QuestionType.Dynamic:
                activityData += ", ";
                activityData += $"\"Question Type\":\"{dynamicQA.questionType.ToString()}\", ";
                activityData += $"\"Option Type\":\"{dynamicQA.optionType.ToString()}\", ";
                activityData += $"\"QA\":"+dynamicQA.GetQAData();
                break;
        }
        activityData += "}";
        return activityData;
    }
}