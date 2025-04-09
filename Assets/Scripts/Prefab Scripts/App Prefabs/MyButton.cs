using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Prefab settings
    public AudioController audioController;
    public bool interactable;

    Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        interactable = true;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactable)
        {
            transform.localScale = originalScale * 0.9f;
            audioController.PlaySfx(audioController.pressKeyClip);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (interactable)
        {
            transform.localScale = originalScale;
        }
    }
}
