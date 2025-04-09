using UnityEngine;

public class GameColorUtils : MonoBehaviour
{
    public static Color GetColor(GameColor gameColor)
    {
        Color color = Color.white;
        switch (gameColor)
        {
            case GameColor.color1:
                ColorUtility.TryParseHtmlString("#CAFF00", out color);
                break;
            case GameColor.color2:
                ColorUtility.TryParseHtmlString("#FF9900", out color);
                break;
            case GameColor.color3:
                ColorUtility.TryParseHtmlString("#FF5100", out color);
                break;
            case GameColor.color4:
                ColorUtility.TryParseHtmlString("#FF006D", out color);
                break;
            case GameColor.color5:
                ColorUtility.TryParseHtmlString("#FF00D6", out color);
                break;
            case GameColor.color6:
                ColorUtility.TryParseHtmlString("#9700FF", out color);
                break;
            case GameColor.color7:
                ColorUtility.TryParseHtmlString("#4E00FF", out color);
                break;
            case GameColor.color8:
                ColorUtility.TryParseHtmlString("#0078FF", out color);
                break;
            case GameColor.color9:
                ColorUtility.TryParseHtmlString("#00EEFF", out color);
                break;
            case GameColor.color10:
                ColorUtility.TryParseHtmlString("#00FF6F", out color);
                break;
        }
        return color;
    }

    public static Color ReduceIntensity(Color color, float factor)
    {
        Color newColor = color;
        newColor.a = factor;
        return newColor;
    }

}
