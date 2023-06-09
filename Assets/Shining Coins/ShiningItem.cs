using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShiningItem : MonoBehaviour {

	public float shiningTime = 1f;
	public float width = 0.2f;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventDataㄹㄹ;
    EventSystem m_EventSystem;
    Image image;
    bool isShining = false;
	SpriteRenderer sr;


	// Use this for initialization
	void Start () {
        if(this.GetComponent<SpriteRenderer>() != null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        if(this.GetComponent<Image>() != null)
        {
            image = GetComponent<Image>();
        }

        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
	}

    public void StartShine()
    {
        StartCoroutine(Shine());
    }

	IEnumerator Shine () {
		float currentTime = 0;
		float speed = 1f / shiningTime;

        if(sr != null)
        {
            sr.material.SetFloat("_Width", width);
        }
        if(image != null)
        {
            image.material.SetFloat("_Width", width);
        }
		while (currentTime <= shiningTime) {
			currentTime += Time.deltaTime;
			float value = Mathf.Lerp (0, 1, speed * currentTime);
            if(sr != null)
            {
                sr.material.SetFloat("_TimeController", value);
            }
            if(image != null)
            {
                image.material.SetFloat("_TimeController", value);
            }
			yield return null;
		}
		yield return new WaitForSeconds (1f);
        if(sr != null)
        {
            sr.material.SetFloat("_Width", 0);
        }
        if(image != null)
        {
            image.material.SetFloat("_Width", 0);
        }
		isShining = false;
	}

    private void OnDestroy()
    {
        if(sr != null)
        {
            sr.material.SetFloat("Width", 0.2f);
            sr.material.SetFloat("_TimeController", 0);
        }
        if(image != null)
        {
            image.material.SetFloat("Width", 0.2f);
            image.material.SetFloat("_TimeController", 0);
        }

    }
}
