using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Prefabs components
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI tmp;

    // Prefab settings
    public AudioController audioController;
    public ButtonColor buttonColor;
    public string description;
    public Color textColor;
    public Color pressedTextColor;
    public float fontSize;
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void ApplySettings()
    {
        SetButtonColor();
        tmp.text = description;
        tmp.color = textColor;
        tmp.fontSize = fontSize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * 0.9f;
        tmp.color = pressedTextColor;
        audioController.PlaySfx(audioController.pressKeyClip);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        tmp.color = textColor;
    }

    void SetButtonColor()
    {
        switch (buttonColor)
        {
            case ButtonColor.Green:
                image.gameObject.SetActive(true);
                image.color = new Color32(42, 255, 87, 255);
                break;
            case ButtonColor.Lightblue:
                image.gameObject.SetActive(true);
                image.color = new Color32(56, 184, 255, 255);
                break;
            case ButtonColor.Red:
                image.gameObject.SetActive(true);
                image.color = new Color32(255, 42, 53, 255);
                break;
            case ButtonColor.None:
                image.gameObject.SetActive(false);
                break;
        }
    }


    public enum ButtonColor
    {
        Green,
        Lightblue,
        Red,
        None
    }
}
