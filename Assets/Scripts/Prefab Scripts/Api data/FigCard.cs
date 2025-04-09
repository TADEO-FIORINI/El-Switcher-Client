using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigCard : MonoBehaviour
{
    // Scene components
    public AudioController audioController;
    public TransformUtils transformUtils;

    // Prefab components
    [SerializeField] List<GameObject> figTypesGO;
    [SerializeField] MyButton myButton;
    [SerializeField] Button button;
    [SerializeField] GameObject blocked;

    // Prefab Settings
    public FigCardData figCardData;

    // State
    public bool isSelected;

    // Intern logic
    Vector3 originalScale;
    public Vector3 originalPosition;

    void Awake()
    {
        isSelected = false;
        originalScale = transform.localScale;
    }

    public void ApplySettings()
    {
        myButton.audioController = audioController;
        blocked.SetActive(figCardData.is_blocked);
        SetFigType();
    }

    void SetFigType()
    {
        figTypesGO[(int)figCardData.fig_type].SetActive(true);
    }

    public void SetActiveButton(bool setActive)
    {
        button.interactable = setActive;
        //myButton.interactable = setActive;
    }

    public void SetSelected(bool selected)
    {
        if (!figCardData.is_used)
        {
            isSelected = selected;
            if (isSelected)
            {
                transform.localScale = originalScale * 1.25f;
                transform.localPosition = originalPosition + new Vector3(0, 50, 0);
            }
            else
            {
                transform.localScale = originalScale;
                transform.localPosition = originalPosition;
            }
        }
    }

}
