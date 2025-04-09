using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovCard : MonoBehaviour
{
    // Scene components
    public AudioController audioController;
    public TransformUtils transformUtils;

    // Prefab components
    [SerializeField] List<GameObject> movTypesGO;
    [SerializeField] MyButton myButton;
    [SerializeField] Button button;


    // Prefab Settings
    public MovCardData movCardData;

    // State
    public bool isSelected;

    // Intern logic
    Vector3 originalScale;
    public Vector3 originalPosition;

    void Awake()
    {
        isSelected = false;
    }

    public void ApplySettings()
    {
        myButton.audioController = audioController;
        originalScale = transform.localScale;
        SetMovType();
    }

    void SetMovType()
    {
        movTypesGO[(int)movCardData.mov_type].SetActive(true);
    }

    public void SetSelected(bool selected)
    {
        if (!movCardData.is_used)
        {
            isSelected = selected;
            if (isSelected )
            {
                transform.localScale = originalScale * 1.1f;
                transform.localPosition = originalPosition + new Vector3(0, 50, 0);
            }
            else
            {
                transform.localScale = originalScale;
                transform.localPosition = originalPosition;
            }
        }
    }

    public void SetActiveButton(bool setActive)
    {
        button.interactable = setActive;
        //myButton.interactable = setActive;
    }   

    static public List<Vector2> GetMovTypeTarget(MovType movType, int x, int y)
    {
            List<Vector2> targetTilesPos = new List<Vector2>();
            switch (movType)
            {
                case MovType.mov1:
                    targetTilesPos = new List<Vector2>
                { new Vector2(x + 2, y + 2), new Vector2(x + 2, y - 2), new Vector2(x - 2, y + 2), new Vector2(x - 2, y - 2) };
                    break;
                case MovType.mov2:
                    targetTilesPos = new List<Vector2>
                { new Vector2(x + 2, y), new Vector2(x - 2, y), new Vector2(x, y + 2), new Vector2(x, y - 2) };
                    break;
                case MovType.mov3:
                    targetTilesPos = new List<Vector2>
                { new Vector2(x + 1, y), new Vector2(x - 1, y), new Vector2(x, y + 1), new Vector2(x, y - 1) };
                    break;
                case MovType.mov4:
                    targetTilesPos = new List<Vector2>
                { new Vector2(x + 1, y + 1), new Vector2(x + 1, y - 1), new Vector2(x - 1, y + 1), new Vector2(x - 1, y - 1) };
                    break;
                case MovType.mov5:
                    targetTilesPos = new List<Vector2>
                { new Vector2(x + 1, y - 2), new Vector2(x - 1, y + 2), new Vector2(x - 2, y - 1), new Vector2(x + 2, y + 1) };
                    break;
                case MovType.mov6:
                    targetTilesPos = new List<Vector2>
                { new Vector2(x + 1, y + 2), new Vector2(x - 1, y - 2), new Vector2(x + 2, y - 1), new Vector2(x - 2, y + 1) };
                    break;
                case MovType.mov7:
                    targetTilesPos = new List<Vector2>
                { new Vector2(0, y), new Vector2(5, y), new Vector2(x, 0), new Vector2(x, 5) };
                    break;
            }
            return targetTilesPos;
        }
}
