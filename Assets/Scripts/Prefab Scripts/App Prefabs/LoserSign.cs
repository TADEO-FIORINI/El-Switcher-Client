using TMPro;
using UnityEngine;

public class LoserSign : MonoBehaviour
{
    // Prefab Components
    [SerializeField] TextMeshProUGUI playerWinText;

    // Prefab Settings
    public string winnerPlayername;

    public void ApplySettings()
    {
        playerWinText.text = $"{winnerPlayername} WIN";
    }
}
