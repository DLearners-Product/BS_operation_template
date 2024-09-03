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

    private void Start() {
        InitializeLessonActivityData(MainBlendedData.instance.slideDatas.Count);
    }

    void InitializeLessonActivityData(int arrLength){
        lessonGameActivityDatas = new BlendedSlideActivityData[arrLength];
        for(int i=0; i<lessonGameActivityDatas.Length; i++){
            if(lessonGameActivityDatas[i] == null){
                lessonGameActivityDatas[i] = new BlendedSlideActivityData();
            }
        }
    }

    public void THI_InitialiseGameActivity(int QIndex){
        if ((lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Count - 1) < QIndex){
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Add(new SlideActivityData(QIndex));
        }
    }

    /*
        Function return score data recorded so far in JSON format.
        Data with empty data (ie., when Tries, Failures, Score is 0) will not be returned.
    */
    public string GetActivityData(int levelno = -1){
        if(levelno == -1)
            levelno = Main_Blended.OBJ_main_blended.levelno;

        activityData = "[";

        if(lessonGameActivityDatas[levelno].slideActivities != null){

            for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Count; i++){
                if(lessonGameActivityDatas[levelno].slideActivities[i].IsEmpty()) continue;

                activityData += lessonGameActivityDatas[levelno].slideActivities[i].GetParsedJsonData();
                if((i+1) < lessonGameActivityDatas[levelno].slideActivities.Count && !lessonGameActivityDatas[levelno].slideActivities[i+1].IsEmpty()){
                    activityData += ",";
                }
            }
        }

        activityData += "]";
        return activityData;
    }

    /*
        Function return score data recorded so far in JSON format. 
        Included Empty Data for debuggin purpose.
    */
    public string GetActivityDataForDebug(int levelno = -1){
        if(levelno == -1)
            levelno = Main_Blended.OBJ_main_blended.levelno;
        activityData = "[";
        if(lessonGameActivityDatas[levelno].slideActivities != null){
            for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Count; i++){
                activityData += lessonGameActivityDatas[levelno].slideActivities[i].GetParsedJsonData();

                if((i+1) < lessonGameActivityDatas[levelno].slideActivities.Count){
                    activityData += ",";
                }
            }
        }
        activityData += "]";
        return activityData;
    }

    /*
        Function reset score data that recorded so far to 0 leaving arrays created. 
    */
    public void ResetActivityData(int levelno = -1){
        if(levelno == -1)
            levelno = Main_Blended.OBJ_main_blended.levelno;

        if(lessonGameActivityDatas[levelno] == null || lessonGameActivityDatas[levelno].slideActivities == null) return;

        for(int i=0; i < lessonGameActivityDatas[levelno].slideActivities.Count; i++){
            lessonGameActivityDatas[levelno].slideActivities[i] = new SlideActivityData(i);
        }
    }

#region PLAYER_ATTEMPTS_RECORDING_FUNCS

    public void PlayerTried(int questionIndex){
        if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities.Count > questionIndex){
            THI_InitialiseGameActivity(questionIndex);
            Debug.Log($"SlideData : {lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex]}");

            if(lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex] == null)
                lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex] = new SlideActivityData(questionIndex);

            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        }
    }

    public void RightAnswer(int questionIndex, int scorevalue = 1, int questionID = -1, int answerID = -1, string answer = ""){
        THI_InitialiseGameActivity(questionIndex);

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].score += scorevalue;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answerAnalysis = true;
        if(questionID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].questionID = questionID;
        if(answerID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answerID = answerID;
        else if(answer != "")
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answer = answer;

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].AppendLog();
    }

    public void WrongAnswer(int questionIndex, int scorevalue = 1, int questionID = -1, int answerID = -1, string answer = ""){
        THI_InitialiseGameActivity(questionIndex);

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].tries++;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].failures += scorevalue;
        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answerAnalysis = false;
        if(questionID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].questionID = questionID;
        if(answerID != -1)
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answerID = answerID;
        else if(answer != "")
            lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].answer = answer;

        lessonGameActivityDatas[Main_Blended.OBJ_main_blended.levelno].slideActivities[questionIndex].AppendLog();
    }

#endregion

}
