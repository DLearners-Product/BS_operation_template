using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using TMPro;
using UnityEngine.UI;
using System.IO;
// using UnityEditor.Events;

public class BlendedOperations : MonoBehaviour
{
    public Bridge bridge;
    public static BlendedOperations instance;
    
    private void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    Transform FindGameObject(GameObject rootObject, string gameObjectName){
        if(gameObjectName == rootObject.name){
            return rootObject.transform;
        }
        for(int i=0; i<rootObject.transform.childCount; i++){
            Transform findObject = FindGameObject(rootObject.transform.GetChild(i).gameObject, gameObjectName);
            if(findObject != null) return findObject;
        }
        return null;
    }

    public void NotifyActivityCompleted(){
        string activityScore = ScoreManager.instance.GetActivityData();
        bridge.NotifyActivityIsCompleted(activityScore);
    }

#region EXTERNAL_JS_INVOKE_FUNCTIONS

    public void SetBlendedData(string blendedData){
        // Debug.Log("From Unity ");
        // Debug.Log(blendedData);
        // Debug.Log("------------------------------------------------------------");
        blendedData = blendedData.Replace("\"[", "[").Replace("]\"", "]").Replace("\\", "");
        JSONNode blendedParsedData = JSON.Parse(blendedData);

        for(int i=0; i<blendedParsedData.Count; i++){
            List<TextComponent> slideTextComponents = MainBlendedData.instance.slideDatas[Int32.Parse(blendedParsedData[i]["slide_flow_id"]) - 1].textComponents;
            
            // Debug.Log(MainBlendedData.instance.slideDatas[Int32.Parse(blendedParsedData[i]["slide_flow_id"])].slideName, MainBlendedData.instance.slideDatas[Int32.Parse(blendedParsedData[i]["slide_flow_id"])].slideObject);

            foreach(var slideTextComponent in slideTextComponents){
                // Debug.Log(slideTextComponent.componentID + " ----- " + blendedParsedData[i]["component_id"]);
                if(slideTextComponent.componentID == blendedParsedData[i]["component_id"]){
                    // Debug.Log("Came In chnage value to : "+blendedParsedData[i]["paragraph"]);
                    if(slideTextComponent.textObject.GetComponent<Text>() != null){
                        slideTextComponent.textObject.GetComponent<Text>().text = blendedParsedData[i]["paragraph"];
                    }else {
                        slideTextComponent.textObject.GetComponent<TMP_Text>().text = blendedParsedData[i]["paragraph"];
                    }
                }
            }
        }

        Main_Blended.OBJ_main_blended.THI_cloneLevels();
    }

    public void GetBlendedData(){
        string blendedData = "[";
        List<Slide> slideDataContainer = MainBlendedData.instance.slideDatas;

        for(int i = 0; i < slideDataContainer.Count; i++){
            SlideData slideData = new SlideData();
            slideData.slideName = slideDataContainer[i].name;
            List<string> slideTexts = new List<string>();
            for(int j=0; j<slideDataContainer[i].textComponents.Count; j++){
                slideTexts.Add(JsonUtility.ToJson(
                    new TextComponentData(
                        slideDataContainer[i].textComponents[j].componentID, 
                        (slideDataContainer[i].textComponents[j].textObject.GetComponent<Text>() != null) ? slideDataContainer[i].textComponents[j].textObject.GetComponent<Text>().text : slideDataContainer[i].textComponents[j].textObject.GetComponent<TMP_Text>().text
                    )
                ));
            }
            slideData.slideTexts = "["+string.Join(", ", slideTexts)+"]";

            // for JSON formating
            if( i > 0){
                blendedData += ", ";
            }
            blendedData += JsonUtility.ToJson(slideData);
        }
        blendedData += "]";

        bridge.SendBlendedContentData(blendedData);
    }

    // Called from external JS
    public void GetActivityScoreData(){
        Debug.Log($"Came to GetActivityScoreData");
        string scoreData = ScoreManager.instance.GetActivityData();
        bridge.SendActivityScoreData(scoreData);
        ScoreManager.instance.ResetActivityData();
    }

    public void GetActivityContentData(){
        string filePath = "ActivityContent.txt";
        string activityOerallData = ActivityContentManager.instance.GetOverallData();
        using(StreamWriter writer = new StreamWriter(filePath)){
            writer.WriteLine(activityOerallData);
        }
    }

    public void GetActivityQA(){
        Debug.Log("GetActivityQA");
        // string activityData = "";
        List<ActivityContent> activityContents = QAManager.instance.GetCurrentActivityContents();
        // for(int i=0; i<activityContents.Count; i++){
        //     activityData += activityContents[i].GetData();
        // }
        string qaData = "";
        if(activityContents.Count > 0){
            Debug.Log("if part");
            qaData = activityContents[0].GetData();
            Debug.Log(qaData);
            bridge.PassQAData(qaData);
        }else{
            Debug.Log("else part");
            bridge.PassQAData(qaData);
        }
        // return activityContents[0].GetData();

    }


    public void CheckFunc(){
        Debug.Log($"In BlendedOperations CheckFunc");
    }
#endregion

#region ADD_SYLLABIFYING_SETUP

    public void ChangeSyllabifyTCName(){
        // Debug.Log($"came to ChangeSyllabifyTCName");
        List<TextComponent> textComponents = MainBlendedData.instance.slideDatas[Main_Blended.OBJ_main_blended.levelno].textComponents;

        for(int i=0; i<textComponents.Count; i++){
            if(!textComponents[i].textObject.name.Contains(textComponents[i].componentID))
                textComponents[i].textObject.name = textComponents[i].componentID + textComponents[i].textObject.name;
        }
    }

    bool CheckFunctionInPersistentListener(Button buttonComp, string functionName){
        Debug.Log("In check function " + buttonComp.name, buttonComp.gameObject);
        int persistentEventCount = buttonComp.onClick.GetPersistentEventCount();
        for(int i=0; i<persistentEventCount; i++){
            if(buttonComp.onClick.GetPersistentMethodName(i) == functionName){
                return true;
            }
        }
        return false;
    }

    public void AddButtonToSyllabifyingTC(){

        if(!Main_Blended.OBJ_main_blended.HAS_SYLLABLE[Main_Blended.OBJ_main_blended.levelno]) return;

        List<TextComponent> textComponentData = MainBlendedData.instance.slideDatas[Main_Blended.OBJ_main_blended.levelno].textComponents;

        GameObject textField;

        for(int i=0; i<textComponentData.Count; i++){
            textField = textComponentData[i].textObject;
            string textCompValue = (textField.GetComponent<Text>()) ? textField.GetComponent<Text>().text : textField.GetComponent<TMP_Text>().text;

            // Debug.Log($"{textField.GetComponent<Text>().text} - {textField.GetComponent<Text>().raycastTarget}", textField);
            if(textField.GetComponent<Text>() && !textField.GetComponent<Text>().raycastTarget){
                textField.GetComponent<Text>().raycastTarget = true;
            }

            Button textBtn;

            if(textComponentData[i].textObject.TryGetComponent<Button>(out textBtn) && textComponentData[i].textObject.GetComponent<SyllabifyListenerScript>() != null) continue;

            if(textBtn == null){
                textComponentData[i].textObject.AddComponent<Button>();
                textComponentData[i].textObject.AddComponent<SyllabifyListenerScript>();
            }
        }
    }

    public void SendDataToSylabify(string dataToSyllabify){
        // string dataToSyllabify = EventSystem.current.currentSelectedGameObject.gameObject.name;
        Debug.Log("SendDataToSylabify ...");
        Debug.Log(dataToSyllabify);
        bridge.SyllabyfyText(dataToSyllabify);
    }

#endregion

}
