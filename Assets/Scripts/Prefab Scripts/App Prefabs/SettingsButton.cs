using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    // Prefabs components
    public GameObject settingsBackground;
    [SerializeField] MyButton myButton;
    [SerializeField] Button button;
    [SerializeField] Image gear;
    [SerializeField] QuitButton quitButton;
    [SerializeField] SfxButton sfxButton;
    [SerializeField] MusicButton musicButton;

    // Scene components
    public WebSocketClient webSocketClient;
    public TransformUtils transformUtils;
    public MenuUtils menuUtils;
    public AudioController audioController;
    public Msg msg;

    // Intern logic
    bool isOpen;
    Vector2 pos1;
    Vector2 pos2;
    int degrees = 360;

    void Start()
    {
        isOpen = false;
        pos1 = new Vector2(760, 0);
        pos2 = menuUtils.GetScreenSidePos(MenuUtils.Side.Right).sideScreenPos; 
        settingsBackground.transform.localPosition = pos2;
        settingsBackground.SetActive(isOpen);
    }

    public void InitButton()
    {
        InitQuitButton();
        InitSfxButton();
        InitMusicButton();
    }

    void InitQuitButton()
    {
        myButton.audioController = audioController;
        button.onClick.AddListener(SettingHandler);
        quitButton.audioController = audioController;
        quitButton.menuUtils = menuUtils;
        quitButton.settingsButton = this;
        quitButton.msg = msg;
        quitButton.webSocketClient = webSocketClient;
        quitButton.InitButton();
    }

    void InitSfxButton()
    {
        sfxButton.audioController = audioController;
        sfxButton.InitButton();
    }

    void InitMusicButton()
    {
        musicButton.audioController = audioController;
        musicButton.InitButton();
    }


    void SettingHandler()
    {
        isOpen = !isOpen;
        settingsBackground.SetActive(true);
        RotateGear();
        OpenCloseHandler();
    }

    void RotateGear()
    {
        int degrees_ = isOpen ? degrees : -degrees;
        transformUtils.StartRotation(
        transform: gear.transform,
        rotationDegrees: new Vector3(0, 0, degrees_),
        seconds: 0.5f,
        handleActive: true);
    }

    void OpenCloseHandler()
    {
        Vector2 startPosition = !isOpen ? pos1 : pos2;
        Vector2 targetPosition = !isOpen ? pos2 : pos1;
        transformUtils.MoveStartToTarget(
                transform: settingsBackground.transform,
                startPosition: startPosition,
                targetPosition: targetPosition,
                seconds: 0.5f,
                handleActive: isOpen);
    }

    public void ForceClose()
    {
        isOpen = false;
        transformUtils.SetActive(settingsBackground.transform, isOpen, 0.5f);
        transformUtils.ChangePosition(settingsBackground.transform, pos2, 0.5f);
    }

}
