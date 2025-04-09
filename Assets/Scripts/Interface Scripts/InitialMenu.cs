using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InitialMenu : MonoBehaviour
{
    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;
    [SerializeField] NetworkAuthentication networkAuthentication;

    // UI Components
    Button createButton;
    Button joinButton;

    // Intern Logic
    public Title title;
    public SettingsButton settingsButton;

    void Awake()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        prefabUtils.InstantiateTitle(transform);
        CreateButtons();
        AlignUIElements();
    }

    void CreateButtons()
    {
        settingsButton = prefabUtils.InstantiateSettingsButton(transform);
        CreateGameButton();
        CreateJoinGameButton();
    }


    void AlignUIElements()
    {
        List<Transform> buttons = new List<Transform>
        {
            createButton.transform,
            joinButton.transform
        };

        TransformUtils.AlignGameObjects(buttons, new Vector2(0, -1000), new Vector2(0, -1300));
    }

    void CreateGameButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Green,
            description: "CREATE",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        createButton = textButton.gameObject.GetComponent<Button>();

        createButton.onClick.AddListener(() =>
            menuUtils.ChangeMenu(menuUtils.createRoomMenu, MenuUtils.Side.Right)
        );
        createButton.onClick.AddListener(() =>
            msg.Deactivate()
        );
        createButton.onClick.AddListener(() =>
            settingsButton.ForceClose()
        );
        createButton.onClick.AddListener(() =>
            msg.Deactivate()
        );
    }

    void CreateJoinGameButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Lightblue,
            description: "JOIN",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        joinButton = textButton.gameObject.GetComponent<Button>();

        joinButton.onClick.AddListener(() =>
            menuUtils.ChangeMenu(menuUtils.joinRoomMenu, MenuUtils.Side.Right)
        ); 
        joinButton.onClick.AddListener(() =>
            menuUtils.joinRoomMenu.GetComponent<JoinMenu>().SearchRooms()
        );
        joinButton.onClick.AddListener(() => 
            settingsButton.ForceClose()
        );
        joinButton.onClick.AddListener(() =>
            msg.Deactivate()
        );
    }
}
