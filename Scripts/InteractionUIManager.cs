using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager manager;

    public Image interact;
    public Graphic interactNotify;

    //Singleton
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(this);
        }
    }

    public void UpdateInteract(float amount, Vector3 position)
    {
        if (!interact.gameObject.activeInHierarchy)
        {
            interact.gameObject.SetActive(true);
        }

        interact.fillAmount = amount;

        RectTransform interactTransform = interact.GetComponent<RectTransform>();
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(position);
        interactTransform.anchorMin = viewportPos;
        interactTransform.anchorMax = viewportPos;
    }

    public void CloseInteract()
    {
        interact.gameObject.SetActive(false);
    }

    public void UpdateHint (Vector3 position)
    {
        if (!interactNotify.gameObject.activeInHierarchy)
        {
            interactNotify.gameObject.SetActive(true);
        }

        RectTransform interactTransform = interactNotify.GetComponent<RectTransform>();
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(position);
        interactTransform.anchorMin = viewportPos;
        interactTransform.anchorMax = viewportPos;
    }

    public void  CloseHint ()
    {
        interactNotify.gameObject.SetActive(false);
    }
}
