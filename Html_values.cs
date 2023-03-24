using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Html_values
{
    public List<Html_List> myLists;

    public Html_values(string[] slideName, string[] teacherInst, bool[] videoSlides, bool[] worksheetSlides, bool[] syllableSlides, bool[] grammerSlide, bool[] hasActivity, bool[] isManualActivity)
    {
        myLists = new List<Html_List>();
        for (int i = 0; i < slideName.Length; i++)
        {
            myLists.Add(new Html_List(i + 1, slideName[i], teacherInst[i], videoSlides[i], worksheetSlides[i], syllableSlides[i], grammerSlide[i], hasActivity[i], isManualActivity[i]));
        };

        //Debug.Log(myLists);
    }
}

public class Html_List
{
    public int _slideNo;
    public string _slideName;
    public string _teacherInst;
    public bool _HasVideo;
    public bool _HasWorksheet;
    public bool _HasSyllable;
    public bool _HasGrammer;
    public bool _HasActivity;
    public bool _IsManualActivity;

    public Html_List(int slideNo, string slideName, string teacherInst, bool hasVideo, bool hasWorksheet, bool hasSyllable, bool hasGrammer, bool hasActivity, bool isManualActivity)
    {
        this._slideNo = slideNo;
        this._slideName = slideName;
        this._teacherInst = teacherInst;
        this._HasVideo = hasVideo;
        this._HasWorksheet = hasWorksheet;
        this._HasSyllable = hasSyllable;
        this._HasGrammer = hasGrammer;
        this._HasActivity = hasActivity;
        this._IsManualActivity = isManualActivity;
    }
}