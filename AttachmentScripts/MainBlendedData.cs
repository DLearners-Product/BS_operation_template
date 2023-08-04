using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[ExecuteInEditMode]
public class MainBlendedData : MonoBehaviour
{
    public List<Slide> slideDatas;
    List<int> slideDataCounts;
    public static MainBlendedData instance;
    List<GameObject> textObjects;
    List<Slide> oldSlideData;
    int currentSlideIndex = 0;

    private void Awake() {
        if(instance == null){
            instance = this;
        }

        textObjects = new List<GameObject>();
        oldSlideData = new List<Slide>();
    }

    void Start()
    {
        slideDataCounts = new List<int>();

        if(slideDatas == null || slideDatas.Count <= 0) return;

        for(int i=0; i<slideDatas.Count; i++){
            oldSlideData.Add(slideDatas[i]);
        }
        Main_Blended.OBJ_main_blended.MAX_SLIDES = slideDatas.Count;
    }

    void Update()
    {
        if(Application.isEditor && !Application.isPlaying){
            UpdateInspector();
        }
    }

    public void UpdateInspector(bool buttonClicked=false){
        if(slideDatas == null || slideDatas.Count <= 0) return;

        for(; currentSlideIndex < slideDatas.Count; currentSlideIndex++){
            bool isNewActivity = ((oldSlideData.Count - 1) < currentSlideIndex && slideDatas[currentSlideIndex].slideObject != null);
            bool isOldActivityChanged = ((oldSlideData.Count) > currentSlideIndex && oldSlideData[currentSlideIndex].slideObject != slideDatas[currentSlideIndex].slideObject);

            if(isNewActivity || isOldActivityChanged || buttonClicked){
                if(isNewActivity){
                    oldSlideData.Add(slideDatas[currentSlideIndex]);
                }
                PopulateTextField();
                UpdateOldSlideData();
            }
        }

        buttonClicked = false;
        currentSlideIndex = 0;
    }

    public void PopulateTextField(){
        if(slideDatas[currentSlideIndex].slideObject != null){
            Debug.Log($"Populating text field");

            slideDatas[currentSlideIndex].textComponents = new List<TextComponent>();
            
            GetAllTextComponent(slideDatas[currentSlideIndex].slideObject);
            slideDatas[currentSlideIndex].name = slideDatas[currentSlideIndex].slideObject.name;
        }else{
            Debug.Log("Cam to else");
            slideDatas[currentSlideIndex].name = "";
            slideDatas[currentSlideIndex].textComponents = null;
        }
    }

    void UpdateOldSlideData(){
        oldSlideData[currentSlideIndex].name = slideDatas[currentSlideIndex].name;
        oldSlideData[currentSlideIndex].slideObject = slideDatas[currentSlideIndex].slideObject;

        oldSlideData[currentSlideIndex].textComponents.Clear();

        for(int i=0; slideDatas[currentSlideIndex].textComponents != null && i<slideDatas[currentSlideIndex].textComponents.Count; i++){
            oldSlideData[currentSlideIndex].textComponents.Add(slideDatas[currentSlideIndex].textComponents[i]);
        }
    }

    // public void AssignData(int index){
    //     for(int i=0; i<slideData.Length; i++){
    //         slideData[index].textComponents[0].component.GetComponent<Text>().text="Text changed";
    //     }
    // }

    void GetAllTextComponent(GameObject rootObject){
        if(rootObject.GetComponent<Text>() != null || rootObject.GetComponent<TMP_Text>() != null){
            string textFieldID = "G_"+(slideDatas[currentSlideIndex].textComponents.Count + 1).ToString();
            // if( !rootObject.name.Contains(textFieldID) ){
            //     rootObject.name = rootObject.name;
            // }

            TextComponent textComponentData = new TextComponent(textFieldID, rootObject);
            slideDatas[currentSlideIndex].textComponents.Add(textComponentData);
            return;
        }
        if(rootObject.transform.childCount > 0){
            for(int j=0; j<rootObject.transform.childCount; j++){
                GetAllTextComponent(rootObject.transform.GetChild(j).gameObject);
            }
        }
    }

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        if(!Application.isPlaying){
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
#endif
    }

    public string[] GetStringData(string dataName){
        List<string> stringData = new List<string>();
        for(int i=0; i<slideDatas.Count; i++){
            switch(dataName){
                case "SLIDE_NAMES":
                    stringData.Add(slideDatas[i].slideName);
                    break;
                case "TEACHER_INSTRUCTION":
                    stringData.Add(slideDatas[i].teacherInstruction);
                    break;
            }
        }
        return stringData.ToArray();
    }

    public bool[] GetBoolData(string dataName){
        List<bool> boolData = new List<bool>();
        for(int i=0; i<slideDatas.Count; i++){
            switch(dataName){
                case "HAS_VIDEO":
                    boolData.Add(slideDatas[i].HAS_VIDEO);
                    break;
                case "HAS_WORKSHEET":
                    boolData.Add(slideDatas[i].HAS_WORKSHEET);
                    break;
                case "HAS_SYLLABLE":
                    boolData.Add(slideDatas[i].HAS_SYLLABLE);
                    break;
                case "HAS_GRAMMER":
                    boolData.Add(slideDatas[i].HAS_GRAMMER);
                    break;
                case "HAS_ACTIVITY":
                    boolData.Add(slideDatas[i].HAS_ACTIVITY);
                    break;
                case "IS_MANUAL_ACTIVITY":
                    boolData.Add(slideDatas[i].IS_MANUAL_ACTIVITY);
                    break;
            }
        }
        return boolData.ToArray();
    }

    private void OnValidate() {
        
    }

}
