using UnityEngine;
using UnityEngine.UI;

public class SfxButton : MonoBehaviour
{
    // Button components
    [SerializeField] MyButton myButton;
    [SerializeField] Button button;
    [SerializeField] Image buttonActive;
    [SerializeField] Image buttonInactive;

    // Scene components
    public AudioController audioController;

    // Logic
    bool isActive;

    private void Start()
    {
        isActive = true;
    }

    public void InitButton()
    {
        myButton.audioController = audioController;
        button.onClick.AddListener(SfxHandler); 
    }

    void SfxHandler()
    {
        isActive = !isActive;
        buttonActive.gameObject.SetActive(isActive);
        buttonInactive.gameObject.SetActive(!isActive);
        audioController.sfxActive = isActive;
    }
}
