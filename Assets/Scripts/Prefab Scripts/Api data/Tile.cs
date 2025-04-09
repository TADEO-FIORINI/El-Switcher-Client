using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    // Scene components
    public AudioController audioController;

    // Prefab Components
    [SerializeField] Image tileInside;
    [SerializeField] Image borderTile;
    [SerializeField] Image borderLine;
    [SerializeField] Image borderInside;
    [SerializeField] Image glow; 
    [SerializeField] Image glowInside; 
    [SerializeField] MyButton myButton;
    [SerializeField] Button button;

    // Prefab Settings
    public TileData tileData;
    
    // State
    public bool isSelected;
    Vector3 originalPosition;

    // Intern logic
    Color selectedColor;
    Color targetColor;

    void Awake()
    {
        isSelected = false;   
        originalPosition = transform.localPosition;
        selectedColor = new Color32(119, 255, 255, 255);
        targetColor = new Color32(154, 154, 154, 255);
    }

    public void ApplyChanges()
    {
        myButton.audioController = audioController;
        SetTileColor();
    }

    void SetTileColor()
    {
        switch (tileData.color)
        {
            case TileColor.red:
                tileInside.color = Color.red;
                break;
            case TileColor.yellow:
                tileInside.color = Color.yellow;
                break;
            case TileColor.green:
                tileInside.color = Color.green;
                break;
            case TileColor.blue:
                tileInside.color = Color.blue;
                break;
        }
        borderInside.color = tileInside.color;
        glowInside.color = tileInside.color;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        borderTile.gameObject.SetActive(isSelected);
        borderLine.color = selectedColor;
    }

    public void SetTarget()
    {
        borderTile.gameObject.SetActive(true);
        borderLine.color = targetColor;
    }

    public void SetActiveButton(bool setActive)
    {
        button.interactable = setActive;
        //myButton.interactable = setActive;
    }

    public void HandleHighLights()
    {
        glow.gameObject.SetActive(tileData.figure != FigType.none);
        glowInside.color = tileInside.color;
    }

    IEnumerator HandleHighlightsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        glow.gameObject.SetActive(tileData.figure != FigType.none);
        glowInside.color = tileInside.color;
    }
}
