using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterMenu : MonoBehaviour
{
    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] MenuUtils menuUtils;

    // Intern Logic
    Button goToSignUpButton;
    Button goToLoginButton;
    MyText myText;

    void Awake()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        CreateMyText();
        prefabUtils.InstantiateInputBackground(transform);
        CreateButtons();
        AlignUIElements();
    }

    void CreateMyText()
    {
        myText = prefabUtils.InstantiateMyText(
            description: "ACCOUNT",
            color: Color.yellow,
            fontSize: 350,
            parent: transform);
    }

    void CreateButtons()
    {
        CreateGoToSignUpButton();
        CreateGoToLoginButton();
    }

    void CreateGoToSignUpButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Green,
            description: "SIGN UP",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        goToSignUpButton = textButton.gameObject.GetComponent<Button>();

        goToSignUpButton.onClick.AddListener(() =>
            menuUtils.ChangeMenu(menuUtils.signUpMenu, MenuUtils.Side.Right)
        );
    }

    void CreateGoToLoginButton()
    {
        TextButton textButton = prefabUtils.InstantiateTextButton(
            buttonColor: TextButton.ButtonColor.Lightblue,
            description: "LOG IN",
            textColor: Color.white,
            pressedTextColor: Color.yellow,
            fontSize: 150,
            parent: transform
        );

        goToLoginButton = textButton.gameObject.GetComponent<Button>();

        goToLoginButton.onClick.AddListener(() =>
            menuUtils.ChangeMenu(menuUtils.loginMenu, MenuUtils.Side.Right)
        );
    }

    void AlignUIElements()
    {
        List<Transform> elements = new List<Transform>
        {
            goToSignUpButton.transform,
            goToLoginButton.transform
        };

        TransformUtils.AlignGameObjects(elements, new Vector2(0, 300), new Vector2(0, -300));
    }
}
