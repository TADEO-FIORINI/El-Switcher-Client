using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateMenu : MonoBehaviour
{
    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;
    [SerializeField] RoomNetworkManager roomNetworkManager;

    // UI Components
    Button backButton;
    Button createRoomButton;
    InputField roomNameInputField;


    void Awake()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        if (roomNameInputField != null)
            roomNameInputField.ResetInput();
    }

    void InitializeUI()
    {
        CreateMyText();
        prefabUtils.InstantiateInputBackground(transform);
        roomNameInputField = CreateInputField("ROOM NAME", false);
        CreateButtons();
        AlignUIElements();
    }

    void CreateMyText()
    {
        MyText title = prefabUtils.InstantiateMyText(
            description: "CREATE\nROOM",
            color: Color.yellow,
            fontSize: 350,
            parent: transform);
    }

    InputField CreateInputField(string placeholder, bool isPassword)
    {
        InputField inputField = prefabUtils.InstantiateInputField(
            placeholderTmpDescription: placeholder,
            isHide: isPassword,
            parent: transform
        );
        return inputField;
    }

    void CreateButtons()
    {
        CreateRoomButton();
        CreateBackButton();
    }

    void CreateRoomButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Green,
            description: "CREATE",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        createRoomButton = textButton.gameObject.GetComponent<Button>();

        createRoomButton.onClick.AddListener(() =>
            roomNetworkManager.CreateRoom(NetworkAuthentication.userPrivateData.user_id, roomNameInputField.GetInput(),
            (RoomPublicData roomData) =>
            {
                msg.Deactivate();
                menuUtils.roomMenu.SetActive(true);
                menuUtils.ChangeMenu(menuUtils.roomMenu, MenuUtils.Side.Right);
                menuUtils.roomMenu.GetComponent<RoomMenu>().SetOwnerRoomMenu(roomData);
            })
        );
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

        backButton = textButton.gameObject.GetComponent<Button>();

        backButton.onClick.AddListener(() =>
            menuUtils.ChangeMenu(menuUtils.initialMenu, MenuUtils.Side.Left)
        );
        backButton.onClick.AddListener(() =>
            msg.Deactivate()
        );
    }

    void AlignUIElements()
    {
        List<Transform> buttons = new List<Transform>
        {
            createRoomButton.transform,
            backButton.transform
        };

        TransformUtils.AlignGameObjects(buttons, new Vector2(0, -1000), new Vector2(0, -1300));
    }
}
