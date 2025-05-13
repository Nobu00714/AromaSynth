using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
public class PressMicButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SpeechRecog speechRecog;
    public void OnPointerDown(PointerEventData eventData)
    {
        speechRecog.StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        speechRecog.StopRecording();
    }


}
