using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [ExecuteInEditMode]
public class ActivityContentManager : MonoBehaviour
{
    public ActivityContent[] activityContents;
    public static ActivityContentManager instance;
    string activityContentData;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void UpdateAsset(){
        foreach(var activityContent in activityContents){
            activityContent.UpdateAsset();
        }
    }

    public string GetOverallData(){
        activityContentData = "[";

        for (int i = 0; i < activityContents.Length; i++)
        {
            activityContentData += activityContents[i].GetData();
            if(i < (activityContents.Length - 1)){
                activityContentData += ", ";
            }
        }
        activityContentData += "]";

        return activityContentData;
    }

    public void Clear(){
        for(int i=0; i<activityContents.Length; i++){
            activityContents[i] = null;
        }
    }

    public void AssignData(string qaData){
        // activityContents = 
    }
}
