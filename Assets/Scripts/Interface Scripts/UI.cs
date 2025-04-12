using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] CanvasScaler scaler;

    void Start()
    {
        // para debuggear multiples clientes sin que explote la PC
        Application.targetFrameRate = 120; // Reduce el uso de CPU limitando FPS
    }

    void Update()
    {
        DeviceScreenSettings();
    }

    public void LeaveGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DeviceScreenSettings()
    {
#if UNITY_ANDROID || UNITY_IOS
            MobileScreenSettings();
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX
        PCScreenSettings();
#endif
    }

    void MobileScreenSettings()
    {
        scaler.referenceResolution = new Vector2(1100, 3500);
        if (IsGalaxyZFold())
        {
            scaler.referenceResolution = new Vector2(1100, 4600);
        }
    }


    void PCScreenSettings()
    {
        SetViewport();
    }

    bool IsGalaxyZFold()
    {
        if (Screen.width * 2.7f < Screen.height)
        {
            return true;
        }
        return false;
    }


    void SetViewport()
    {
        float targetWidth = 800f;
        float targetHeight = 2000f;
        float targetAspect = targetWidth / targetHeight;

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera mainCam = Camera.main;

        if (scaleHeight < 1.0f)
        {
            // La pantalla es más ancha que el target, ajusta el ancho del viewport
            Rect rect = mainCam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCam.rect = rect;
        }
        else
        {
            // La pantalla es más alta que el target, ajusta la altura del viewport
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCam.rect = rect;
        }
    }
}
