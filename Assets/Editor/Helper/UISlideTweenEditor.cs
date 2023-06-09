using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UISlideTween))]
public class SlideTweenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();     

        UISlideTween slideTween = (UISlideTween)target;
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Play"))
        {
            slideTween.EnterTween();
        }
        if (GUILayout.Button("Exit"))
        {
            slideTween.ExitTween();
        }
        if (GUILayout.Button("INITIAL State"))
        {
            Debug.Log(slideTween.transform.lossyScale);
            // slideTween.enterPosition = slideTween.transform.localPosition;
            slideTween.enterPosition = slideTween.GetComponent<RectTransform>().anchoredPosition;
            slideTween.enterWidth = slideTween.GetComponent<RectTransform>().rect.width;
            slideTween.enterHeight = slideTween.GetComponent<RectTransform>().rect.height;
            slideTween.enterScale = new Vector3(1, 1, 1);
        }
        
        if (GUILayout.Button("Set TARGET State")) {
            // slideTween.targetPosition = slideTween.transform.localPosition;
            slideTween.targetPosition = slideTween.GetComponent<RectTransform>().anchoredPosition;
            slideTween.targetWidth = slideTween.GetComponent<RectTransform>().rect.width / slideTween.enterWidth;
            slideTween.targetHeight = slideTween.GetComponent<RectTransform>().rect.height / slideTween.enterHeight;
            slideTween.targetScale = new Vector3(slideTween.targetWidth, slideTween.targetHeight, 1);
        }
        
        if (GUILayout.Button("Set FINAL State")) {
            slideTween.exitPosition = slideTween.transform.localPosition;
            slideTween.exitWidth = slideTween.GetComponent<RectTransform>().rect.width / slideTween.enterWidth;
            slideTween.exitHeight = slideTween.GetComponent<RectTransform>().rect.height / slideTween.enterHeight;
            slideTween.exitScale = new Vector3(slideTween.exitWidth, slideTween.exitHeight, 1);
        }
                
        GUILayout.EndHorizontal();
    }
}