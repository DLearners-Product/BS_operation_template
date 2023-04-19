using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using System.IO;

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
    public int questionNo;
    public int tries = 0;
    public int failures = 0;
    public int score = 0;
    public string answer;

    public SlideActivityData(int qNo){
        this.questionNo = qNo;
    }

    public SlideActivityData(int qNo, string question){
        this.questionNo = qNo;
        this.question = question;
    }

    public string getParsedJsonData(){
        return JsonUtility.ToJson(this);
    }

    // return true if object is empty
    public bool isEmpty(){
        return this.score <= 0 && this.failures <= 0 && this.tries <= 0;
    }
}

[Serializable]
public class BlendedSlideActivityData{
    public SlideActivityData[] slideActivities;
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

#region GROUP_IMAGE

[Serializable]
public class Component{
    public int id=0;
    public string text;
    public Sprite _sprite { get; private set; }
    public Texture2D texture2D;
    public AudioClip audioClip;
    public int width, height;
    float[] audioData;
    int _aduioSample, _audioChannel, _audioFrequency;

    public void UpdateAssets(){
        UpdateTextureData();

        UpdateAudioData();
    }

    void UpdateTextureData(){
        if(texture2D == null) {
            width = 0;
            height = 0;
            return;
        }

        width = (int)(texture2D.width);
        height = (int)(texture2D.height);
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
        if(audioClip == null) return "";

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
        
        byte[] bytes;
        // Texture2D texture = ConvertSpriteToTexture(_sprite);
        bytes = texture2D.EncodeToPNG();

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

        string imgBase64 = Convert.ToBase64String(bytes);
        return imgBase64;
    }

    public string GetComponentStringfyData(){
        string responseData = "{";
        responseData += $"\"id\":{id}, \"text\":\"{text}\", \"image\":\"{GetTextureBS64()}\", \"image-width\":\"{width}\", \"image-height\":\"{height}\", \"audio\":\"{GetAudioBS64()}\"";
        responseData +="}";
        return responseData;
    }
}

#endregion