using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SignUpMenu : MonoBehaviour
{
    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;
    [SerializeField] NetworkAuthentication networkAuthentication;

    // UI Components
    InputField usernameInputField;
    InputField passwordInputField;
    InputField confirmPasswordInputField;
    Button signUpButton;
    Button backButton;

    void Awake()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        if (usernameInputField != null && passwordInputField != null && confirmPasswordInputField != null)
        {
            usernameInputField.ResetInput();
            passwordInputField.ResetInput();
            confirmPasswordInputField.ResetInput();
        }
    }

    void InitializeUI()
    {
        CreateMyText();
        prefabUtils.InstantiateInputBackground(transform);
        usernameInputField = CreateInputField("YOUR USERNAME", false);
        passwordInputField = CreateInputField("YOUR PASSWORD", true);
        confirmPasswordInputField = CreateInputField("YOUR PASSWORD", true);
        CreateButtons();
        AlignUIElements();
    }

    void CreateMyText()
    {
        prefabUtils.InstantiateMyText(
            description: "ACCOUNT",
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
        CreateSignUpButton();
        CreateBackButton();
    }

    void CreateSignUpButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Green,
            description: "SIGN UP",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        signUpButton = textButton.gameObject.GetComponent<Button>();

        signUpButton.onClick.AddListener(() =>
            networkAuthentication.RegisterUser(
            usernameInputField.GetInput(),
            passwordInputField.GetInput(),
            confirmPasswordInputField.GetInput()
            )
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
            menuUtils.ChangeMenu(menuUtils.registerMenu, MenuUtils.Side.Left)
        );
        backButton.onClick.AddListener(() =>
            msg.Deactivate()
        );
    }

    void AlignUIElements()
    {
        List<Transform> inputs = new List<Transform>
        {
            usernameInputField.transform,
            passwordInputField.transform,
            confirmPasswordInputField.transform,
        };

        TransformUtils.AlignGameObjects(inputs, new Vector2(0, 500), new Vector2(0, -500));

        List<Transform> buttons = new List<Transform>
        {
            signUpButton.transform,
            backButton.transform
        };

        TransformUtils.AlignGameObjects(buttons, new Vector2(0, -1000), new Vector2(0, -1300));
    }
}
