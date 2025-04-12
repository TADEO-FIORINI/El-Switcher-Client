using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    // Scene components
    [SerializeField] PrefabUtils prefabUtils;
    [SerializeField] TransformUtils transformUtils;
    [SerializeField] MenuUtils menuUtils;
    [SerializeField] Msg msg;
    [SerializeField] GameNetworkManager gameNetworkManager;
    [SerializeField] GameState gameState;

    // UI Components
    MyText formatedRoomName;
    Button leaveButton;
    Button nextTurnButton;
    Color nextTurnButtonColor;
    Color disableNextTurnButtonColor;

    // Game Public
    public GamePublic publicGame;

    // Intern logic
    public bool isEnd;

    public void InitGameMenu(GamePublicData publicGameData)
    {
        isEnd = false;

        publicGame = prefabUtils.InstantiateGame(transform);
        publicGame.CreateGame(publicGameData);
        publicGame.UpdateGameData(publicGameData);
        gameState.publicGame = publicGame;

        CreateButtons();
        ResetState();
    }

    void CreateButtons()
    {
        CreateLeaveButton();
        CreateNextTurnButton();
    }

    void CreateLeaveButton()
    {
        leaveButton = prefabUtils.InstantiateLeaveGameButton(
            parent: transform
        ).GetComponent<Button>();
        leaveButton.onClick.AddListener(() => msg.Deactivate());
        leaveButton.onClick.AddListener(() => menuUtils.ChangeMenu(menuUtils.initialMenu, MenuUtils.Side.Up));
        leaveButton.onClick.AddListener(() => gameNetworkManager.LeaveGame(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id));
        leaveButton.onClick.AddListener(() => transformUtils.DestroyChildren(transform, menuUtils.GetSecondsToChangeMenu(MenuUtils.Side.Up) - 0.25f));
    }

    void CreateNextTurnButton()
    {
        nextTurnButton = prefabUtils.InstantiateNextTurnButton(
            parent: transform
        ).GetComponent<Button>();
        nextTurnButton.onClick.AddListener(() => gameNetworkManager.NextTurn(NetworkAuthentication.userPrivateData.user_id, RoomNetworkManager.publicRoomData.room_id));
        nextTurnButtonColor = nextTurnButton.GetComponent<Image>().color;
        disableNextTurnButtonColor = GameColorUtils.ReduceIntensity(nextTurnButtonColor, 0.3f);
    }


    public void ResetState()
    {
        gameState.SetButtonsListeners();
        if (publicGame.myPlayer.playerData.in_turn)
        {
            nextTurnButton.gameObject.GetComponent<Image>().color = nextTurnButtonColor;
            gameState.SetState(GameState.State.SelectCard);
        }
        else
        {
            nextTurnButton.gameObject.GetComponent<Image>().color = disableNextTurnButtonColor;
            gameState.SetState(GameState.State.None);
        }
    }

    public void HandleEndOfGameForDefault()
    {
        if (publicGame.publicGameData.other_players.Count == 0)
        {
            Win("FOR DEFAULT");
        }
    }

    public void HandleEndOfGame()
    {
        if (publicGame.myPlayer.playerData.fig_cards_left == 0)
        {
            Win("YOU'RE OUT OF CARDS");
        }
        else
        {
            PublicPlayerData winnerPlayerData = publicGame.publicGameData.other_players.Find(publicPlayerData => publicPlayerData.fig_cards_left == 0);
            if (winnerPlayerData != null)
            {
                Lose(winnerPlayerData.username);
            }
        }
    }

    void Win(string reasonWin)
    {
        msg.Deactivate();
        WinnerSign winnerSign = prefabUtils.InstantiateWinnerSign(reasonWin, transform);
        EndGame(winnerSign.transform);
    }

    void Lose(string winnerPlayername)
    {
        msg.Deactivate();
        LoserSign loserSign = prefabUtils.InstantiateLoserSign(winnerPlayername, transform);
        EndGame(loserSign.transform);
    }

    void EndGame(Transform endConditionSign)
    {
        isEnd = true;
        Destroy(nextTurnButton.gameObject);
        transformUtils.MoveStartToScreen(
        transform: endConditionSign,
        startPosition: menuUtils.GetScreenSidePos(MenuUtils.Side.Up).sideScreenPos,
        seconds: menuUtils.GetSecondsToChangeMenu(MenuUtils.Side.Up),
        handleActive: true
        );
        transformUtils.MoveOriginalToTarget(
        transform: publicGame.transform,
        targetPosition: menuUtils.GetScreenSidePos(MenuUtils.Side.Down).sideScreenPos,
        seconds: menuUtils.GetSecondsToChangeMenu(MenuUtils.Side.Down),
        handleActive: false
        );
    }

}
