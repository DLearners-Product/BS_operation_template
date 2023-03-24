using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

[Serializable]
public class CommonScript
{
    public int id;
    public string strVal;

    public CommonScript(int id, string strVal){
        this.id = id;
        this.strVal = strVal;
    }
}

[Serializable]
public class Slide{
    public string name;
    public GameObject slideObject;
    public List<TextComponent> textComponents;
}

[Serializable]
public class TextComponent{
    public string componentID;
    public GameObject textObject;

    public TextComponent(string componentID, GameObject component){
        this.componentID = componentID;
        this.textObject = component;
    }
}

[Serializable]
public class SlideData{
    public string slideName;
    public string slideTexts;
}

[Serializable]
public class TextComponentData{
    public string key;
    public string value;
    public TextComponentData(string id, string value){
        this.key = id;
        this.value = value;
    }
}

[Serializable]
public class SlideActivityData{
    public string question;
    public int questionNo;
    public int tries = 0;
    public int failures = 0;
    public int score = 0;

    public SlideActivityData(int qNo){
        this.questionNo = qNo;
    }

    public SlideActivityData(int qNo, string question){
        this.questionNo = qNo;
        this.question = question;
    }

    public string getParsedJsonData(){
        return JsonUtility.ToJson(this);
    }

    // return true if object is empty
    public bool isEmpty(){
        return this.score <= 0 && this.failures <= 0 && this.tries <= 0;
    }
}

[Serializable]
public class BlendedSlideActivityData{
    public SlideActivityData[] slideActivities;
}