using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class JoinMenu : MonoBehaviour
{
    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] TransformUtils transformUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;
    [SerializeField] ListRooms listRooms;

    // UI Components
    Button backButton;
    List<Room> rooms;

    void Awake()
    {
        InitializeUI();
    }

    public void SearchRooms()
    {
        rooms = listRooms.SearchRooms();
    }

    void InitializeUI()
    {
        CreateMyText();
        CreateButtons();
    }

    void CreateMyText()
    {
        MyText title = prefabUtils.InstantiateMyText(
            description: "JOIN\nROOM",
            color: Color.yellow,
            fontSize: 350,
            parent: transform);
    }

    void CreateButtons()
    {
        CreateBackButton();
    }

    void CreateBackButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Red,
            description: "BACK",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );
        textButton.transform.localPosition = new Vector2(0, -1300);

        backButton = textButton.gameObject.GetComponent<Button>();

        backButton.onClick.AddListener(() =>
            menuUtils.ChangeMenu(menuUtils.initialMenu, MenuUtils.Side.Left)
        );
        backButton.onClick.AddListener(() =>
            msg.Deactivate()
        );
    }

}
