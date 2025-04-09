using TMPro;
using UnityEngine;

public class MyText : MonoBehaviour
{
    // PrefabComponents
    public TextMeshProUGUI tmp;

    // Settings
    public bool withSign;
    public string description;
    public Color color;
    public float fontSize;

    public void ApplySettings()
    {
        tmp.text = description;
        tmp.color = color;
        tmp.fontSize = fontSize;
    }
}
