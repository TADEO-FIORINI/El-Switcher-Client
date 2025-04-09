using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class Timer : MonoBehaviour
{
    // Prefab Components
    [SerializeField] TextMeshProUGUI timeLeft;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI playername;

    // Prefab Settings
    public int timeValue;
    public string playernameValue;
    public GameColor playerColor;

    // Intern Logic
    Color timerColor;

    public void ApplySettings()
    {
        UpdateTime();
        UpdatePlayername();
        UpdateColor();
    }

    void UpdateTime()
    {
        time.text = timeValue.ToString();
    }

    void UpdatePlayername()
    {
        playername.text = playernameValue;
    }

    void UpdateColor()
    {
        playername.color = GameColorUtils.GetColor(playerColor);
        timerColor = GetTimerColor();
        time.color = timerColor;
        timeLeft.color = timerColor;
    }

    Color GetTimerColor()
    {
        Color color = Color.white;
        if (timeValue <= 20)
        {
            color = Color.red;
        }
        else if (timeValue <= 40)
        {
            color = Color.yellow;
        }
        else if (timeValue <= 60)
        {
            color = Color.green;
        }
        return color;
    }
}
