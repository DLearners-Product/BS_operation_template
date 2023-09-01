using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

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

    void AssignStaticQuestionsIds(JSONNode quesJSONData, JSONNode optionJSONData, StaticQA staticQA){
        for(int j=0; j<quesJSONData.Count; j++){
            int qIndex = Int32.Parse(quesJSONData[j]["question_flow_no"]) - 1;
            staticQA.questions[qIndex].question.id = quesJSONData[j]["question_id"];
            staticQA.questions[qIndex].question.image_url = quesJSONData[j]["question_image"];
            staticQA.questions[qIndex].question.audio_url = quesJSONData[j]["question_audio"];
        }

        for(int i=0; i<staticQA.options.Length; i++){
            for(int j=0; j<optionJSONData.Count; j++){
                if(staticQA.options[i].text == optionJSONData[j]["option_text"]){
                    staticQA.options[i].id = optionJSONData[j]["option_id"];
                    staticQA.options[i].image_url = optionJSONData[j]["question_image"];
                    staticQA.options[i].audio_url = optionJSONData[j]["question_audio"];
                }
            }
        }
    }

    void AssignDynamicOptionIds(JSONNode jsonData, OptionComponent[] options){
        for(int i=0; i<options.Length; i++){
            for(int j=0; j<jsonData.Count; j++){
                if(options[i].text == jsonData[j]["option_text"]){
                    options[i].id = jsonData[j]["option_id"];
                    options[i].image_url = jsonData[j]["question_image"];
                    options[i].audio_url = jsonData[j]["question_audio"];
                }
            }
        }
    }

    void AssignDynamicQuestionIds(JSONNode jsonData, DynamicQA dynamicQA){
        for(int i=0; i<jsonData.Count; i++){
            int qIndex = Int32.Parse(jsonData[i]["question_flow_no"]) - 1;
            dynamicQA.questions[qIndex].question.id = jsonData[i]["question_id"];
            dynamicQA.questions[qIndex].question.image_url = jsonData[i]["question_image"];
            dynamicQA.questions[qIndex].question.audio_url = jsonData[i]["question_audio"];
            AssignDynamicOptionIds(jsonData[i]["options"], dynamicQA.questions[qIndex].options);
        }
    }

#region EXTERNAL_JS_INVOKE_FUNCTIONS

    public void JS_CALL_SetBlendedData(string blendedData){
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

    public void JS_CALL_GetBlendedData(){
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

    public void JS_CALL_GetActivityScoreData(){
        string scoreData = ScoreManager.instance.GetActivityData();
        bridge.SendActivityScoreData(scoreData);
        ScoreManager.instance.ResetActivityData();
    }

    public void JS_CALL_GetActivityContentData(){
        // string filePath = "ActivityContent.txt";
        string activityOerallData = ActivityContentManager.instance.GetOverallData();
        // using(StreamWriter writer = new StreamWriter(filePath)){
        //     writer.WriteLine(activityOerallData);
        // }
        bridge.PassActivityOverallContent(activityOerallData);
    }

    public void JS_CALL_GetActivityQA(){
        // string activityData = "";
        List<ActivityContent> activityContents = QAManager.instance.GetCurrentActivityContents();
        // for(int i=0; i<activityContents.Count; i++){
        //     activityData += activityContents[i].GetData();
        // }
        string qaData = "";
        if(activityContents.Count > 0){
            qaData = activityContents[0].GetData();
            bridge.PassQAData(qaData);
        }else{
            bridge.PassQAData(qaData);
        }
        // return activityContents[0].GetData();
    }

    public void JS_CALL_SetQAActivity(string qaData){
        Debug.Log(qaData);
        JSONNode jsonData = JSON.Parse(qaData);
        ActivityContent[] activityContents = ActivityContentManager.instance.activityContents;
        foreach (ActivityContent activityContent in activityContents)
        {
            for (int i=0; i<jsonData.Count; i++)
            {
                int slideIndex = int.Parse(jsonData[i]["slide_number"]) - 1;
                // Debug.Log(activityContent.slideNo+" "+slideIndex+" || "+activityContent.questionType.ToString()+" "+jsonData[i]["qa_type"]);
                if(activityContent.slideNo == slideIndex && activityContent.questionType.ToString() == jsonData[i]["qa_type"]){
                    if(activityContent.questionType == QuestionType.Dynamic){
                        AssignDynamicQuestionIds(jsonData[i]["questions"], activityContent.dynamicQA);
                    }else if(activityContent.questionType == QuestionType.Static){
                        AssignStaticQuestionsIds(jsonData[i]["questions"], jsonData[i]["options"], activityContent.staticQA);
                    }
                    break;
                }
            }
        }
    }

    public void JS_CALL_ScoreDataDisplayAll()
    {
        Debug.Log($"{ScoreManager.instance.GetActivityDataForDebug()}");
    }
    public void JS_CALL_CheckFunc(){
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

        if(!MainBlendedData.instance.slideDatas[Main_Blended.OBJ_main_blended.levelno].HAS_SYLLABLE) return;

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
        // Debug.Log("SendDataToSylabify ...");
        // Debug.Log(dataToSyllabify);
        dataToSyllabify = Regex.Replace(dataToSyllabify, "<.*?>", "");
        bridge.SyllabyfyText(dataToSyllabify);
    }

#endregion

#region PATCH_WORKS

    // Function called from JS dont change the name it is used in blended session 
    public void BUT_reset(){
        ScoreManager.instance.ResetActivityData();
    }

    public void SetQAActivityDataID(string lesson_id){
        StartCoroutine(GET_Blended_ID(lesson_id));
    }

    IEnumerator GET_Blended_ID(string lesson_id){
        string URL ="https://dlearners.in/template_and_games/Blended_session_apis/get_blended_question_options.php";

        WWWForm form = new WWWForm();
        form.AddField("lesson_id", lesson_id);
        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        if (!www.isNetworkError){
            JS_CALL_SetQAActivity(www.downloadHandler.text);
        }
    }

#endregion
}
