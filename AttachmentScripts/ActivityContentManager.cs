using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [ExecuteInEditMode]
public class ActivityContentManager : MonoBehaviour
{
    public ActivityContent[] activityContents;
    public static ActivityContentManager instance;
    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void OnValidate()
    {

    }
}
