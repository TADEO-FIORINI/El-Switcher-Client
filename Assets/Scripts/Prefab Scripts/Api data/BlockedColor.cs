using UnityEngine;
using UnityEngine.UI;

public class BlockedColor : MonoBehaviour
{
    // Prefab components
    [SerializeField] Image tile;

    // Prefab settings
    public TileColor tileColor;

    public void ApplyChanges()
    {
        SetTileColor();
    }

    void SetTileColor()
    {
        gameObject.SetActive(tileColor != TileColor.none);
        switch (tileColor)
        {
            case TileColor.red:
                tile.color = Color.red;
                break;
            case TileColor.yellow:
                tile.color = Color.yellow;
                break;
            case TileColor.green:
                tile.color = Color.green;
                break;
            case TileColor.blue:
                tile.color = Color.blue;
                break;
            default:
                break;
        }
    }
}
