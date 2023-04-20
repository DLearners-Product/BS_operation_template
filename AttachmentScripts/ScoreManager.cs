using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    string activityData;
    public static ScoreManager instance;

    // Contains ScoreData for all Slides lessonGameActivityDatas.Count = Slide Count
    [SerializeField] BlendedSlideActivityData[] lessonGameActivityDatas;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void OnEnable() {
        InitializeLessonActivityData(MainBlendedData.instance.slideDatas.Count);
        // InitializeLessonActivityData(Main_Blended.OBJ_main_blended.GA_levelsIG.Length);
    }

    void InitializeLessonActivityData(int arrLength){
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
        Debug.Log("QIndex : "+QIndex);
        Debug.Log(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno]);
        if ((lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Count - 1) < QIndex){
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Add(new SlideActivityData(QIndex));
        }
    }

    public string GetActivityData(int levelno = -1){
        if(levelno == -1)
            levelno = Main_Blended.OBJ_main_blended.levelno;

        activityData = "[";

        if(lessonGameActivityDatas[levelno].slideActivities != null){
            for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Count; i++){
                if(lessonGameActivityDatas[levelno].slideActivities[i].isEmpty()) continue;

                activityData += lessonGameActivityDatas[levelno].slideActivities[i].getParsedJsonData();
                if((i+1) < lessonGameActivityDatas[levelno].slideActivities.Count){
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

        for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Count; i++){
            lessonGameActivityDatas[levelno].slideActivities[i] = new SlideActivityData(i);
        }
    }

    public void PlayerTried(int questionIndex){
        if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Count > questionIndex){
            THI_InitialiseGameActivity(questionIndex);
            Debug.Log($"SlideData : {lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex]}");

            if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex] == null)
                lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex] = new SlideActivityData(questionIndex);

            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        }
    }

    public void RightAnswer(int questionIndex, int scorevalue = 1, int questionID = -1, int answerID = -1){
        THI_InitialiseGameActivity(questionIndex);

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].score += scorevalue;
        if(questionID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].questionID = questionID;
        if(answerID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answerID = answerID;
    }

    public void WrongAnswer(int questionIndex, int scorevalue = 1, int questionID = -1, int answerID = -1){
        THI_InitialiseGameActivity(questionIndex);

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].failures += scorevalue;
        if(questionID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].questionID = questionID;
        if(answerID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answerID = answerID;
    }
}
