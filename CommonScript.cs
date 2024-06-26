﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using System.IO;
using UnityEditor;

[Serializable]
public class CommonScript
{
    public int id;
    public string strVal;

    public CommonScript(int id, string strVal){
        this.id = id;
        this.strVal = strVal;
    }
}

[Serializable]
public class Slide{
    public string name;
    public GameObject slideObject;
    public List<TextComponent> textComponents;
    public string slideName;
    public string teacherInstruction;
    public string activityInstruction;
    public ActivityType activityType;
    public bool HAS_VIDEO,
                HAS_WORKSHEET,
                HAS_SYLLABLE,
                HAS_GRAMMER,
                HAS_ACTIVITY,
                IS_MANUAL_ACTIVITY;

    public bool IsManualActivity(){
        if(IS_MANUAL_ACTIVITY) return true;
        return activityType == ActivityType.ManualActivity;
    }

    public bool IsExceptionalActivity(){
        return activityType == ActivityType.ExceptionalActivity;
    }
}

[Serializable]
public class TextComponent{
    public string componentID;
    public GameObject textObject;

    public TextComponent(string componentID, GameObject component){
        this.componentID = componentID;
        this.textObject = component;
    }
}

[Serializable]
public class SlideData{
    public string slideName;
    public string slideTexts;
}

[Serializable]
public class TextComponentData{
    public string key;
    public string value;
    public TextComponentData(string id, string value){
        this.key = id;
        this.value = value;
    }
}

[Serializable]
public class SlideActivityData{
    public string question;
    public int questionID = 0;
    public int answerID = 0;
    public int tries = 0;
    public int failures = 0;
    public int score = 0;
    public string answer;
    public bool answerAnalysis;
    List<SlideActivityData> logs;

    public SlideActivityData(int qNo){
        this.answerAnalysis = false;
        this.questionID = qNo;
        logs = new List<SlideActivityData>();
    }

    public SlideActivityData(int qNo, string question){
        this.questionID = qNo;
        this.question = question;
        logs = new List<SlideActivityData>();
    }

    SlideActivityData(string questionSTR, int qID, int answerID, int tries, int failures, int score, string answer, bool analysis){
        this.question = questionSTR;
        this.questionID = qID;
        this.answerID = answerID;
        this.tries = tries;
        this.failures = failures;
        this.score = score;
        this.answer = answer;
        this.answerAnalysis = analysis;
    }

    public string GetParsedJsonData(){
        string slideActivityData = "{";

        slideActivityData += $"\"question\": \"{question}\", \"questionID\": {questionID}, \"answerID\": {answerID}, \"tries\": {tries}, \"failures\": {failures}, \"score\": {score}, \"answer\": \"{answer}\", \"isCorrect\": ";

        slideActivityData += (answerAnalysis) ? "true" : "false";
        slideActivityData += ", \"isValid\": true";

        if(logs != null && logs.Count > 0){
            slideActivityData += ", \"logs\": [";
            for (int i = 0; i < logs.Count; i++)
            {
                slideActivityData += logs[i].GetParsedJsonData();
                if((i+1) < logs.Count){
                    slideActivityData += ", ";
                }
            }
            slideActivityData += "]";
        }

        slideActivityData += "}";

        return slideActivityData;
    }

    // return true if object is empty (ie., object is considered empty when score, failures and tries are valued 0 and below)
    public bool IsEmpty(){
        return this.score <= 0 && this.failures <= 0 && this.tries <= 0;
    }

    public void AppendLog(){
        if(this.IsEmpty()) return;

        logs.Add(new SlideActivityData(this.question, this.questionID, this.answerID, this.tries, this.failures, this.score, this.answer, this.answerAnalysis));
    }
}

[Serializable]
public class BlendedSlideActivityData{
    public List<SlideActivityData> slideActivities = new List<SlideActivityData>();
}

public enum FileType{
    None,
    Text,
    Image,
    Audio
}

public enum QuestionType{
    None,
    Static,
    Dynamic
}

public enum ActivityType{
    None,
    ManualActivity,
    TimerActivity,
    ExceptionalActivity
}

#region GROUP_IMAGE

[Serializable]
public class Component{
    public string text;
    public int id=0;
    public Sprite _sprite;
    public Texture2D texture2D;
    public AudioClip audioClip;
    public string image_url, audio_url;
    int width, height;
    float[] audioData;
    int _aduioSample, _audioChannel, _audioFrequency;
    string textureBS64Text;

    public void UpdateAssets(){
        UpdateTextureData();

        // UpdateAudioData();
    }

    Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        try{
            // if(sprite.rect.width != sprite.texture.width){
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] colors = newText.GetPixels();
                Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                    (int)System.Math.Ceiling(sprite.textureRect.y),
                    (int)System.Math.Ceiling(sprite.textureRect.width),
                    (int)System.Math.Ceiling(sprite.textureRect.height));
                Debug.Log(colors.Length+"_"+ newColors.Length);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            // }else{
            //     Debug.Log("In else part");
            //     return sprite.texture;
            // }
        }catch{
            Debug.Log("In catch");
            return sprite.texture;
        }
    }

    void Make2DTextureReadWriteEnabled(bool makeReadable=true){
#if UNITY_EDITOR
        if(texture2D != null && !texture2D.isReadable){
            string assetPath = AssetDatabase.GetAssetPath( texture2D );
            var tImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;

            if(makeReadable)
                tImporter.isReadable = true;
            else
                tImporter.isReadable = false;

            AssetDatabase.ImportAsset( assetPath );
            AssetDatabase.Refresh();
        }
#endif
    }

    void UpdateTextureData(){
#if UNITY_EDITOR
        if(EditorApplication.isPlaying) return;
        if(texture2D == null) {
            width = 0;
            height = 0;
            return;
        }

        width = (int)(texture2D.width);
        height = (int)(texture2D.height);

        if(textureBS64Text == null){
            Make2DTextureReadWriteEnabled();
            textureBS64Text = GetTextureBS64();
            Make2DTextureReadWriteEnabled(false);
        }
#endif
    }

    void UpdateAudioData(){
        if(audioClip == null) {
            audioData = null;
            return;
        }

        _aduioSample = audioClip.samples;
        _audioChannel = audioClip.channels;
        _audioFrequency = audioClip.frequency;
        audioData = new float[_aduioSample * _audioChannel];
        audioClip.GetData(audioData, 0);
    }

    string GetAudioBS64(){
        if(audioClip == null || audioData == null) return "";

        byte[] bytes = new byte[audioData.Length * 2];
        int index = 0;
        foreach (float sample in audioData) {
            short convertedSample = (short) (sample * short.MaxValue);
            BitConverter.GetBytes(convertedSample).CopyTo(bytes, index);
            index += 2;
        }

        using (MemoryStream stream = new MemoryStream()) {
            using (BinaryWriter writer = new BinaryWriter(stream)) {
                writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
                writer.Write(36 + bytes.Length);
                writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
                writer.Write(new char[4] { 'f', 'm', 't', ' ' });
                writer.Write(16);
                writer.Write((ushort) 1);
                writer.Write((ushort) _audioChannel);
                writer.Write(_audioFrequency);
                writer.Write(_audioFrequency * _audioChannel * 2);
                writer.Write((ushort) (_audioChannel * 2));
                writer.Write((ushort) 16);
                writer.Write(new char[4] { 'd', 'a', 't', 'a' });
                writer.Write(bytes.Length);
                writer.Write(bytes);
            }
            byte[] wavBytes = stream.ToArray();
            return Convert.ToBase64String(wavBytes);
        }
    }

    string GetTextureBS64(){
        if(texture2D == null) return "";
        // Debug.Log("came to GetTextureBS64................");
        byte[] bytes = null;
        // Texture2D texture = ConvertSpriteToTexture(_sprite);
        try{
            bytes = texture2D.EncodeToPNG();
        }catch{
            if(bytes == null){
                Texture2D newText = new Texture2D(width, height, TextureFormat.RGBA32, false);
                try{
                    newText.SetPixels(texture2D.GetPixels());
                    newText.Apply();
                    bytes = newText.EncodeToPNG();
                }catch{
                    newText.SetPixels32(texture2D.GetPixels32());
                    newText.Apply();
                    bytes = newText.EncodeToPNG();
                }
            }
        }

        string imgBase64 = "";
        if(bytes != null){
            imgBase64 = Convert.ToBase64String(bytes);
        }
        return imgBase64;
    }

    public string GetComponentStringfyData(){
        string responseData = "{";
        if(image_url == "" && audio_url == "")
            responseData += $"\"id\":{id}, \"text\":\"{text}\", \"image\":\"{textureBS64Text}\", \"image-width\":\"{width}\", \"image-height\":\"{height}\", \"audio\":\"{GetAudioBS64()}\"";
        else
            responseData += $"\"id\":{id}, \"text\":\"{text}\", \"image\":\"{image_url}\", \"image-width\":\"{width}\", \"image-height\":\"{height}\", \"audio\":\"{audio_url}\"";
        responseData +="}";
        return responseData;
    }
}

[Serializable]
public class OptionComponent : Component{
    public bool isAnswer;
}

#endregion

