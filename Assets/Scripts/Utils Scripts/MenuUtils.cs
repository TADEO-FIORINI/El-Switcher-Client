using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUtils : MonoBehaviour
{
    // Utils
    [SerializeField] TransformUtils transformUtils;

    // Screen
    [SerializeField] GameObject background;

    // Menus
    public GameObject registerMenu;
    public GameObject signUpMenu;
    public GameObject loginMenu;
    public GameObject initialMenu;
    public GameObject joinRoomMenu;
    public GameObject createRoomMenu;
    public GameObject roomMenu;
    public GameObject gameMenu;

    // Intern Logic
    List<GameObject> menus;

    void Start()
    {
        menus = new List<GameObject>();
        menus.Add(registerMenu);
        menus.Add(signUpMenu);
        menus.Add(loginMenu);
        menus.Add(initialMenu);
        menus.Add(joinRoomMenu);
        menus.Add(createRoomMenu);
        menus.Add(roomMenu);
        menus.Add(gameMenu);
        LoadMenus(registerMenu);
    }

    public void ChangeMenu(GameObject targetMenu, Side side)
    {
        float seconds = GetSecondsToChangeMenu(side);
        foreach (GameObject menu in menus)
        {
            if (menu.activeSelf)
            {
                transformUtils.MoveOriginalToTarget(
                    transform: menu.transform,
                    targetPosition: GetScreenSidePos(side).opositeScreenPos,
                    seconds: seconds,
                    handleActive: false
                );
            }
        }
        targetMenu.SetActive(true);
        transformUtils.MoveStartToScreen(
            transform: targetMenu.transform,
            startPosition: GetScreenSidePos(side).sideScreenPos,
            seconds: seconds,
            handleActive: true
        );
    }

    public enum Side
    {
        Up, Right, Down, Left
    }
    
    public float GetSecondsToChangeMenu(Side side)
    {
        float seconds = side == Side.Left || side == Side.Right ? 0.5f : 0.75f;
        return seconds;
    }

    public (Vector2 sideScreenPos, Vector2 opositeScreenPos) GetScreenSidePos(Side side)
    {
        Vector2 sideScreenPos = Vector2.zero;
        Vector2 opositeScreenPos = Vector2.zero;

        float backgroundWidth = background.GetComponent<RectTransform>().sizeDelta.x;
        float backgroundHeigth = background.GetComponent<RectTransform>().sizeDelta.y;
        
        switch (side)
        {
            case Side.Up:
                sideScreenPos = new Vector2(0, backgroundHeigth);
                opositeScreenPos = new Vector2(0, -backgroundHeigth);
                break;
            case Side.Right:
                sideScreenPos = new Vector2(backgroundWidth, 0);
                opositeScreenPos = new Vector2(-backgroundWidth, 0);
                break;
            case Side.Down:
                sideScreenPos = new Vector2(0, -backgroundHeigth);
                opositeScreenPos = new Vector2(0, backgroundHeigth);
                break;
            case Side.Left:
                sideScreenPos = new Vector2(-backgroundWidth, 0);
                opositeScreenPos = new Vector2(backgroundWidth, 0);
                break;
        }
        return (sideScreenPos, opositeScreenPos);
    }

    public void LoadMenus(GameObject targetMenu)
    {
        //StartCoroutine(LoadMenusAndActiveOneCoroutine(targetMenu));
        foreach (GameObject menu in menus)
            menu.SetActive(false);
        targetMenu.SetActive(true);
    }

    void SetActiveMenus(bool active)
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(active);
        }
    }

    IEnumerator LoadMenusAndActiveOneCoroutine(GameObject targetMenu)
    {
        SetActiveMenus(true);
        yield return new WaitForSeconds(0.5f);
        SetActiveMenus(false);
        targetMenu.SetActive(true);
    }

}
