mergeInto(LibraryManager.library, {

    SetBlendedData : function(jsonData){
        SetBlendedData_(Pointer_stringify(jsonData));
    },

    TeacherInst : function (htmlJson) {
        var json = Pointer_stringify(htmlJson)
	    localStorage.setItem('htmlJson', json);
    },

    Game : function(name){
        var myGameName = Pointer_stringify(name)
        localStorage.setItem('gameName', myGameName);
    },

    CallSyllabyfyText : function(textToSyllabify) {
        PerformSyllabifycation(UTF8ToString(textToSyllabify));
    },

    PassActivityScoreData : function(scoreData){
        SetActivityScoreData(UTF8ToString(scoreData));
    },

    PassBlendedContentDataToDB : function(blendedContentData){
        send_blended_data(blendedContentData);
    },

    MarkActivityCompleted : function(activityScoreData){
        blendedActivityIsCompleted(UTF8ToString(activityScoreData));
    }

});