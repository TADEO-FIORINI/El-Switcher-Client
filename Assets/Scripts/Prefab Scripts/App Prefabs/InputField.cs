using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static Unity.Burst.Intrinsics.X86.Avx;

public class InputField: MonoBehaviour
{
    // Prefab components
    [SerializeField] TMP_InputField tmpInputField;
    [SerializeField] TextMeshProUGUI placeholderTmp;

    // Scene components
    public AudioController audioController;
    public string placeholderTmpDescription;
    public bool isHide;

    // Intern logic
    Vector2 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        tmpInputField.onValueChanged.AddListener(OnTextChanged);
    }

    public void ApplySettings()
    {
        placeholderTmp.text = placeholderTmpDescription;

        if (isHide)
        {
            tmpInputField.contentType = TMP_InputField.ContentType.Password;
        }
        else
        {
            tmpInputField.contentType = TMP_InputField.ContentType.Standard;
        }

        tmpInputField.ForceLabelUpdate();
    }

    void OnTextChanged(string newText)
    {
        tmpInputField.text = newText.ToUpper();
        if (newText != "")
            audioController.PlaySfx(audioController.pressKeyClip);
    }

    public string GetInput()
    {
        return tmpInputField.text;
    }

    public void ResetInput()
    {
        tmpInputField.text = "";
    }
}
