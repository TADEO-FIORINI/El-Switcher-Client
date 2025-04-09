using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabUtils : MonoBehaviour
{
    // Prefabs
    [SerializeField] MyText myTextPrefab;
    [SerializeField] InputField inputFieldPrefab;
    [SerializeField] TextButton textButtonPrefab;
    [SerializeField] GameObject inputBackgroundPrefab;
    [SerializeField] Room roomPrefab;
    [SerializeField] Title titlePrefab;
    [SerializeField] SettingsButton settingsButtonPrefab;
    [SerializeField] GameObject leaveGameButtonPrefab;
    [SerializeField] GamePublic gamePrefab;
    [SerializeField] WinnerSign winnerSignPrefab;
    [SerializeField] LoserSign loserSignPrefab;
    [SerializeField] GameObject nextTurnButtonPrefab;

    // Scene components
    [SerializeField] WebSocketClient webSocketClient;
    [SerializeField] AudioController audioController;
    [SerializeField] TransformUtils transformUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;

    public MyText InstantiateMyText(string description, Color color, float fontSize, Transform parent)
    {
        MyText myText = Instantiate(myTextPrefab, parent);
        myText.description = description;
        myText.color = color;
        myText.fontSize = fontSize;
        myText.ApplySettings();
        return myText;
    }

    public InputField InstantiateInputField(string placeholderTmpDescription, bool isHide, Transform parent)
    {
        InputField inputField = Instantiate(inputFieldPrefab, parent);
        inputField.audioController = audioController;
        inputField.placeholderTmpDescription = placeholderTmpDescription;
        inputField.isHide = isHide;
        inputField.ApplySettings();
        return inputField;
    }

    public TextButton InstantiateTextButton(TextButton.ButtonColor buttonColor, string description, Color textColor, Color pressedTextColor, int fontSize, Transform parent)
    {
        TextButton textButton = Instantiate(textButtonPrefab, parent);
        textButton.audioController = audioController;
        textButton.buttonColor = buttonColor;
        textButton.description = description;
        textButton.textColor = textColor;
        textButton.pressedTextColor = pressedTextColor;
        textButton.fontSize = fontSize;
        textButton.ApplySettings();
        return textButton;
    }

    public GameObject InstantiateInputBackground(Transform parent)
    {
        GameObject inputBackgroundGO = Instantiate(inputBackgroundPrefab.gameObject, parent);
        return inputBackgroundGO;
    }

    public Room InstantiateRoom(string roomName, List<UserPublicData> usernames, Color color, Transform parent)
    {
        Room room = Instantiate(roomPrefab, parent);
        room.audioController = audioController;
        room.roomNameText = roomName;
        room.usernames = usernames;
        room.color = color;
        room.joinButton = InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.None,
            description: "JOIN",
            textColor: Color.white,
            pressedTextColor: new Color32(56, 184, 255, 255),
            fontSize: 200,
            parent: room.transform
        );
        room.joinButton.transform.localPosition = new Vector2(350, -150);
        room.ApplyChanges();
        return room;
    }

    public Title InstantiateTitle(Transform parent)
    {
        return Instantiate(titlePrefab, parent);
    }

    public SettingsButton InstantiateSettingsButton(Transform parent)
    {
        SettingsButton settingsButton = Instantiate(settingsButtonPrefab, parent);
        settingsButton.audioController = audioController;
        settingsButton.transformUtils = transformUtils;
        settingsButton.menuUtils = menuUtils;
        settingsButton.webSocketClient = webSocketClient;
        settingsButton.msg = msg;
        settingsButton.InitButton();

        return settingsButton;
    }

    public GameObject InstantiateLeaveGameButton(Transform parent)
    {
        GameObject leaveButtonGO = Instantiate(leaveGameButtonPrefab, parent);
        MyButton leaveButton = leaveButtonGO.GetComponent<MyButton>();
        leaveButton.audioController = audioController;
        return leaveButtonGO;
    }

    public GamePublic InstantiateGame(Transform parent)
    {
        GamePublic publicGame = Instantiate(gamePrefab, parent);
        publicGame.audioController = audioController;
        publicGame.transformUtils = transformUtils;
        return publicGame;
    }

    public WinnerSign InstantiateWinnerSign(string reasonWin, Transform parent)
    {
        WinnerSign winnerSign = Instantiate(winnerSignPrefab, parent);
        winnerSign.reasonWin = reasonWin;
        winnerSign.ApplySettings();
        return winnerSign;
    }

    public LoserSign InstantiateLoserSign(string winnerPlayername, Transform parent)
    {
        LoserSign loserSign = Instantiate(loserSignPrefab, parent);
        loserSign.winnerPlayername = winnerPlayername;
        loserSign.ApplySettings();
        return loserSign;
    }

    public GameObject InstantiateNextTurnButton(Transform parent)
    {
        GameObject nextTurnButtonGO = Instantiate(nextTurnButtonPrefab, parent);
        MyButton nextTurnButton = nextTurnButtonGO.GetComponent<MyButton>();
        nextTurnButton.audioController = audioController;
        return nextTurnButtonGO;
    }
}
