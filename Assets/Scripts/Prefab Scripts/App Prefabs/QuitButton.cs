using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    // Button components
    [SerializeField] MyButton myButton;
    [SerializeField] Button button;

    // Scene components
    public WebSocketClient webSocketClient;
    public MenuUtils menuUtils;
    public AudioController audioController;
    public SettingsButton settingsButton;
    public Msg msg;

    public void InitButton()
    {
        myButton.audioController = audioController;
        button.onClick.AddListener(QuitHandler);
    }

    void QuitHandler()
    {
        settingsButton.ForceClose();
        menuUtils.ChangeMenu(menuUtils.registerMenu, MenuUtils.Side.Up);
        webSocketClient.CloseConnection();
        msg.Deactivate();
    }

}
