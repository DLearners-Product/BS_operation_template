using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Video;

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

        // string GetArrayData(QAO[] qas){
        //     string data = "";
        //     for (int i=0; i < qas.Length; i++)
        //     {
        //         data += qas[i].GetData();
        //         if(i < (qas.Length -1)){
        //             data += ",";
        //         }
        //     }
        //     return data;
        // }

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

    [Serializable]
    public class StaticQAWithSubQues{
        public List<StaticQASubQues> qaWithSubQuestion = new List<StaticQASubQues>();

        public string GetData(){
            string activityContent = "[";

            for (int i = 0; i < qaWithSubQuestion.Count; i++)
            {
                activityContent += qaWithSubQuestion[i].GetData();
                if(i < (qaWithSubQuestion.Count - 1)){
                    activityContent += ",";
                }
            }

            activityContent += "]";

            return activityContent;
        }
    }
#endregion

#region DIFFERENT_TYPE_QA
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
        public OptionComponent[] options;
        string strData;

        public void Update(){
            question.UpdateAssets();

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
    public class StaticQASubQues{
        public FileType questionType;
        public Component mainQues;
        public StaticQA staticSubQA;
        string strData;

        public string GetData(){
            strData = "{";
            strData += $"\"questionType\": \"{questionType}\", ";
            strData += $"\"mainQuestion\": {mainQues.GetComponentStringfyData()}, ";
            strData += $"\"subQuestion\": {staticSubQA.GetQAData()}, ";
            strData += $"\"Option\": {staticSubQA.GetOptionData()}"; 
            strData += "}";
            return strData;
        }
    }
#endregion

[Serializable]
public class AdditionalComponent{
    public string key;
    public FileType dataType;
    public Component value;
    string activityContent;

    public string GetData(){
        activityContent = "{";
        activityContent += $"\"{key}\"";
        activityContent += " : {";
        activityContent += $"\"DataType\" : \"{dataType}\",";
        activityContent += $"\"Value\" : {value.GetComponentStringfyData()}";
        activityContent += "}}";
        return activityContent;
    }
}

[Serializable]
public class ActivityContent{
    public int slideNo;
    public string activityName;
    public bool hasSubquestion;
    public QuestionType questionType;
    public StaticQA staticQA = new StaticQA();
    public DynamicQA dynamicQA = new DynamicQA();
    public StaticQAWithSubQues staticQAWithSQ = new StaticQAWithSubQues();
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
        activityData += $"\"SlideIndex\":\"{slideNo}\", ";
        activityData += $"\"IsManualActivity\":\"{MainBlendedData.instance.slideDatas[slideNo].IsManualActivity()}\", ";
        activityData += $"\"IsExceptionalActivity\":\"{MainBlendedData.instance.slideDatas[slideNo].IsExceptionalActivity()}\", ";
        activityData += $"\"HasSubQuestion\":\"{hasSubquestion}\", ";
        activityData += $"\"QAType\":\"{questionType}\"";
        switch(questionType){
            case QuestionType.Static:
                activityData += ", ";
                activityData += $"\"QuestionType\":\"{staticQA.questionType}\", ";
                activityData += $"\"OptionType\":\"{staticQA.optionType}\", ";
                if(!hasSubquestion){
                    Debug.Log($"{activityName} --> Not HAS Sub question");
                    activityData += $"\"QA\":"+staticQA.GetQAData()+", "; 
                    activityData += $"\"Option\":"+staticQA.GetOptionData();
                }else{
                    Debug.Log($"{activityName} --> HAS Sub question");
                    activityData += $"\"QA\":{staticQAWithSQ.GetData()}";
                }
                break;
            case QuestionType.Dynamic:
                activityData += ", ";
                activityData += $"\"QuestionType\":\"{dynamicQA.questionType}\", ";
                activityData += $"\"OptionType\":\"{dynamicQA.optionType}\", ";
                activityData += $"\"QA\":"+dynamicQA.GetQAData();
                break;
        }
        activityData += "}";
        return activityData;
    }
}