using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class GetTextFromSpeech : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    const string LANG_CODE = "ja-JP";
    // public TextMeshProUGUI inputText;
    public TMP_InputField inputText;
    void Start()
    {
        SpeechToText.Instance.isShowPopupAndroid = true;
        SpeechToText.Instance.Setting(LANG_CODE);
        SpeechToText.Instance.onResultCallback = OnResultSpeech;
        CheckPermission();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SpeechToText.Instance.StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopRecording();
    }
    

    public void StopRecording()
    {
        #if UNITY_EDITOR
            OnResultSpeech("Not support in editor.");
        #else
            SpeechToText.Instance.StopRecording();
        #endif
        #if UNITY_IOS
            // loading.SetActive(true);
        #endif
    }

    void OnResultSpeech(string _data)
    {
        inputText.text = _data;
        #if UNITY_IOS
            loading.SetActive(false);
        #endif
    }

    void CheckPermission()
    {
        #if UNITY_ANDROID
            // if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            // {
                Permission.RequestUserPermission(Permission.Microphone);
            // }
        #endif
    }
}
