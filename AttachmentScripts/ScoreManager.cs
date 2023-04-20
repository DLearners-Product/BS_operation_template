using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    string activityData;
    public static ScoreManager instance;
    [SerializeField] BlendedSlideActivityData[] lessonGameActivityDatas;
    // [SerializeField] GameActivityData[] GAA_activityData;

    private void Awake()
    {
        instance = this;
    }

    private void Start() {
        InitializeLessonActivityData(MainBlendedData.instance.slideDatas.Count);
    }

    public void InitializeLessonActivityData(int arrLength){
        lessonGameActivityDatas = new BlendedSlideActivityData[arrLength];
        for(int i=0; i<lessonGameActivityDatas.Length; i++){
            if(lessonGameActivityDatas[i] == null){
                lessonGameActivityDatas[i] = new BlendedSlideActivityData();
            }
        }
    }

    /* 
        - This fucntion is removed dure to BlendedSlideActivityData.slideActivities is converted from Array to List
        - Array is Changed to list due to question with multiple answer is to be recorded
    */
    // public void InstantiateScore(int arrSize){
    //     // Debug.Log($"came to InstantiateScore "+arrSize);
    //     // Debug.Log($"Level No : "+Main_Blended.OBJ_main_blended.levelno);
    //     // Debug.Log(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities);

    //     if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities == null || lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Length <= 0){
    //         Debug.Log($"Slide activity initialized");
    //         lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities = new SlideActivityData[arrSize];
    //         for(int i=0; i<arrSize; i++){
    //             THI_InitialiseGameActivity(i);
    //         }
    //     }else{
    //         Debug.Log($"Slide activity not initialized");
    //     }
    // }

    public void THI_InitialiseGameActivity(int QIndex){
        if (lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[QIndex] == null){
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[QIndex] = new SlideActivityData(QIndex);
        }
    }

    public string GetActivityData(int levelno = -1){
        if(levelno == -1)
            levelno = Main_Blended.OBJ_main_blended.levelno;

        activityData = "[";

        if(lessonGameActivityDatas[levelno].slideActivities != null){
            for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Length; i++){
                if(lessonGameActivityDatas[levelno].slideActivities[i].isEmpty()) continue;

                activityData += lessonGameActivityDatas[levelno].slideActivities[i].getParsedJsonData();
                if((i+1) < lessonGameActivityDatas[levelno].slideActivities.Length){
                    activityData += ",";
                }
            }
        }

        activityData += "]";
        return activityData;
    }

    public void ResetActivityData(int levelno = -1){
        if(levelno == -1)
            levelno = Main_Blended.OBJ_main_blended.levelno;

        if(lessonGameActivityDatas[levelno] == null || lessonGameActivityDatas[levelno].slideActivities == null) return;

        for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Length; i++){
            lessonGameActivityDatas[levelno].slideActivities[i] = new SlideActivityData(i);
        }
    }

    // public string GetAllActivityData(){
    // }

    public void PlayerTried(int questionIndex){
        if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Length > questionIndex){
            THI_InitialiseGameActivity(questionIndex);
            Debug.Log($"SlideData : {lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex]}");
            if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex] == null)
                lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex] = new SlideActivityData(questionIndex);
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        }
    }

    public void RightAnswer(int questionIndex, int scorevalue = 1, string questionValue=null, string answerValue=null, bool appendAnswer=false){
        THI_InitialiseGameActivity(questionIndex);
        if(questionValue != null)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].question = questionValue;
        if(answerValue != null)
        {
            bool checkIsAnswerEmpty = lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answer != null && lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answer.Trim() != "";
            if (appendAnswer && checkIsAnswerEmpty)
            {
                lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answer += " | "+answerValue;
            }
            else
                lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answer = answerValue;
        }

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].score += scorevalue;
    }

    public void WrongAnswer(int questionIndex, int scorevalue = 1, string questionValue=null){
        THI_InitialiseGameActivity(questionIndex);
        if(questionValue != null)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].question = questionValue;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].failures += scorevalue;
    }
}
