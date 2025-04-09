using TMPro;
using UnityEngine;

public class WinnerSign : MonoBehaviour
{
    // Prefab Components
    [SerializeField] TextMeshProUGUI reasonWinText;

    // Prefab Settings
    public string reasonWin;

    public void ApplySettings()
    {
        reasonWinText.text = reasonWin;
    }
}
