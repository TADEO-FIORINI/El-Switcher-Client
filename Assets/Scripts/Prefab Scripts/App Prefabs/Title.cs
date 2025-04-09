using System.Collections;
using TMPro;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public float cycleDuration = 8f;
    [Range(0f, 1f)] public float saturation = 0.5f;
    [Range(0f, 1f)] public float value = 1f;

    private void OnEnable()
    {
        StartCoroutine(CycleTextColor());
    }

    IEnumerator CycleTextColor()
    {
        float t = 0f;
        while (true)
        {
            float hue = Mathf.Repeat(t / cycleDuration, 1f);
            Color color = Color.HSVToRGB(hue, saturation, value);
            text.color = color;
            t += Time.deltaTime;
            yield return null;
        }
    }
}
