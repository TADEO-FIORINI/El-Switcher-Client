using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class RoomMenu : MonoBehaviour
{
    // Menu Components
    [SerializeField] Image roomBackground;

    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;
    [SerializeField] RoomNetworkManager roomNetworkManager;
    [SerializeField] GameNetworkManager gameNetworkManager;

    // UI Components
    Button startButton;
    Button leaveButton;
    MyText formatedRoomName;
    List<MyText> usersList;
    TextButton textButtonLeave;

    // curr room
    public string roomName;

    void Awake()
    {
        roomBackground.gameObject.SetActive(true);
        InitializeUI();
    }

    public void SetOwnerRoomMenu(RoomPublicData publicRoomData)
    {
        SetupRoomMenu(publicRoomData.room_name, publicRoomData.room_users, publicRoomData.room_color);
        startButton.gameObject.SetActive(true);
        SetupLeaveButton("DELETE", () => roomNetworkManager.DeleteRoom(NetworkAuthentication.userPrivateData.user_id, publicRoomData.room_id));
    }

    public void SetJoinedRoomMenu(RoomPublicData publicRoomData)
    {
        SetupRoomMenu(publicRoomData.room_name, publicRoomData.room_users, publicRoomData.room_color);
        startButton.gameObject.SetActive(false);
        SetupLeaveButton("LEAVE", () => roomNetworkManager.LeaveRoom(NetworkAuthentication.userPrivateData.user_id, publicRoomData.room_id));
    }

    void SetupRoomMenu(string roomNameText, List<UserPublicData> usersData, GameColor roomColor)
    {
        roomName = roomNameText;
        roomBackground.color = GameColorUtils.GetColor(roomColor);
        formatedRoomName.description = FormatRoomName(roomNameText);
        formatedRoomName.ApplySettings();
        UpdateUsers(usersData);
    }

    public void UpdateUsers(List<UserPublicData> usersData)
    {
        for (int i = 0; i < usersList.Count; i++)
        {
            usersList[i].description = i < usersData.Count ? usersData[i].username : "";
            usersList[i].ApplySettings();
        }
    }

    void SetupLeaveButton(string buttonText, UnityEngine.Events.UnityAction onClickAction)
    {
        textButtonLeave.description = buttonText;
        textButtonLeave.ApplySettings();
        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(() => menuUtils.ChangeMenu(menuUtils.initialMenu, MenuUtils.Side.Left));
        leaveButton.onClick.AddListener(() => msg.Deactivate());
        leaveButton.onClick.AddListener(onClickAction);
    }

    void InitializeUI()
    {
        CreateRoomComponents();
        CreateButtons();
        AlignUIElements();
    }

    void CreateButtons()
    {
        CreateStartGameButton();
        CreateLeaveButton();
    }

    void CreateRoomComponents()
    {
        CreateRoomName();
        CreateUsersList();
    }

    void CreateStartGameButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Green,
            description: "START",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        startButton = textButton.gameObject.GetComponent<Button>();
        startButton.onClick.AddListener(() => gameNetworkManager.StartGame(
            NetworkAuthentication.userPrivateData.user_id,
            RoomNetworkManager.publicRoomData.room_id));

    }

    void CreateLeaveButton()
    {
        textButtonLeave = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Red,
            description: "LEAVE",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        leaveButton = textButtonLeave.gameObject.GetComponent<Button>();
    }

    void CreateRoomName()
    {
        formatedRoomName = prefabUtils.InstantiateMyText(
            description: "",
            color: Color.white,
            fontSize: 200,
            parent: roomBackground.transform
        );
        formatedRoomName.transform.localPosition = new Vector2(0, 800);
    }

    void CreateUsersList()
    {
        usersList = new List<MyText>();
        for (int i = 0; i < 4; i++)
        {
            MyText username = prefabUtils.InstantiateMyText(
                description: "",
                color: Color.white,
                fontSize: 150,
                parent: roomBackground.transform
            );
            username.tmp.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            usersList.Add(username);
        }
    }

    void AlignUIElements()
    {
        AlignButtons();
        AlignUsernames();
    }

    void AlignButtons()
    {
        List<Transform> buttons = new List<Transform>
        {
            startButton.transform,
            leaveButton.transform
        };
        TransformUtils.AlignGameObjects(buttons, new Vector2(0, -1000), new Vector2(0, -1300));
    }

    void AlignUsernames()
    {
        List<Transform> usernames = new List<Transform>();
        foreach (var user in usersList)
        {
            usernames.Add(user.transform);
        }
        TransformUtils.AlignGameObjects(usernames, new Vector2(0, 300), new Vector2(0, -600));
    }

    static string FormatRoomName(string input)
    {
        string[] words = input.Split(' ');
        string formattedInput = words[0];
        int charCount = words[0].Length;

        for (int i = 1; i < words.Length; i++)
        {
            if (charCount + words[i].Length + 1 > 11) // +1 por el espacio
            {
                formattedInput += "\n" + words[i];
                charCount = words[i].Length;
            }
            else
            {
                formattedInput += " " + words[i];
                charCount += words[i].Length + 1;
            }
        }
        return formattedInput;
    }
}
