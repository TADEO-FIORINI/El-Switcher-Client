using System.Collections.Generic;
using UnityEngine;

public class GamePublic : MonoBehaviour
{
    // Scene Components
    public AudioController audioController;
    public TransformUtils transformUtils;

    // Other Prefabs
    [SerializeField] Board boardPrefab;
    [SerializeField] Player playerPrefab;
    [SerializeField] PublicPlayer publicPlayerPrefab;
    [SerializeField] BlockedColor blockedColorPrefab;
    [SerializeField] Timer timerPrefab;

    // Prefab Settings
    public GamePublicData publicGameData { get; private set; }

    // Public Game Objects
    public Board board;
    public Player myPlayer;
    public List<PublicPlayer> otherPlayers = new List<PublicPlayer>();
    public BlockedColor blockedColor;
    public Timer timer;

    // other players positions
    public float otherPlayerPosX { get; private set; } = 0;
    public float firstOtherPlayerPosY { get; private set; } = 610;
    public float otherPlayerOffsetY { get; private set; } = 440;

    public void CreateGame(GamePublicData newPublicGameData)
    {
        publicGameData = newPublicGameData;
        CreateBoard();
        CreatePlayer();
        CreatePublicPlayers();
        CreateBlockedColor();
        CreateTimer();
    }

    void CreateBoard()
    {
        board = Instantiate(boardPrefab, transform);
        board.audioController = audioController;
        board.transformUtils = transformUtils;
        board.boardData = publicGameData.board;
        board.CreateBoard();
    }

    void CreateBlockedColor()
    {
        blockedColor = Instantiate(blockedColorPrefab, transform);
        blockedColor.tileColor = board.boardData.blocked_color;
        blockedColor.ApplyChanges();
    }

    void CreateTimer()
    {
        timer = Instantiate(timerPrefab, transform);
        UpdateTimer(60);
    }

    public void UpdateTimer(int time)
    {
        timer.timeValue = time;
        if (publicGameData.my_player.in_turn)
        {
            timer.playernameValue = publicGameData.my_player.username;
            timer.playerColor = publicGameData.my_player.player_color;
        }
        else
        {
            PublicPlayerData publicPlayerData = publicGameData.other_players.Find(publicPlayer => publicPlayer.in_turn);
            timer.playernameValue = publicPlayerData.username;
            timer.playerColor = publicPlayerData.player_color;
        }
        timer.ApplySettings();
    }


    void CreatePlayer()
    {
        myPlayer = Instantiate(playerPrefab, transform);
        myPlayer.audioController = audioController;
        myPlayer.transformUtils = transformUtils;
        myPlayer.playerData = publicGameData.my_player;
        myPlayer.CreatePlayer();
    }

    void CreatePublicPlayers()
    {
        for (int i = 0; i < publicGameData.other_players.Count; i++)
        {
            PublicPlayerData publicPlayerData = publicGameData.other_players[i];
            PublicPlayer publicPlayer = Instantiate(publicPlayerPrefab, transform);
            publicPlayer.audioController = audioController;
            publicPlayer.transformUtils = transformUtils;
            publicPlayer.publicPlayerData = publicPlayerData;
            publicPlayer.CreatePublicPlayer();
            publicPlayer.gameObject.transform.localPosition = new Vector2(otherPlayerPosX, firstOtherPlayerPosY + otherPlayerOffsetY * i);
            otherPlayers.Add(publicPlayer);
        }
    }

    public void DestroyLeftPlayer(string username)
    {
        bool isOtherPlayer = publicGameData.my_player.username != username;
        PublicPlayer leftPlayer = otherPlayers.Find(otherPlayer => otherPlayer.publicPlayerData.username == username);
        if (leftPlayer)
        {
            otherPlayers.Remove(leftPlayer);
            Destroy(leftPlayer.gameObject);
        }
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            transformUtils.MoveOriginalToTarget(
                transform: otherPlayers[i].transform,
                targetPosition: new Vector2(otherPlayerPosX, firstOtherPlayerPosY + otherPlayerOffsetY * i),
                seconds: 0.25f,
                handleActive: true);
        }
    }

    public void UpdateGameData(GamePublicData newPublicGameData)
    {
        publicGameData = newPublicGameData;
        board.UpdateBoardData(newPublicGameData.board);
        myPlayer.UpdatePlayerData(newPublicGameData.my_player);
        for (int i = 0; i < otherPlayers.Count; i++) 
        {
            PublicPlayer otherPlayer = otherPlayers[i];
            otherPlayer.UpdatePublicPlayerdata(newPublicGameData.other_players[i]);
        }
    }

    public void UpdateSwitch(int movCardIndex, int tile1_x, int tile1_y, int tile2_x, int tile2_y)
    {
        board.UpdateTileMatrix(tile1_x, tile1_y, tile2_x, tile2_y);
        if (myPlayer.playerData.in_turn)
        {
            MovCard usedMovCard = myPlayer.movCards[movCardIndex];
            usedMovCard.gameObject.SetActive(false);
            myPlayer.MoveMovCardsAtStart();
        }
    }

    public void UpdateDiscardFigure(int figCardIndex)
    {
        if (myPlayer.playerData.in_turn)
        {
            FigCard usedFigCard = myPlayer.figCards[figCardIndex];
            usedFigCard.gameObject.SetActive(false);
            myPlayer.MoveFigCardsAtStart();

        }
        else
        {
            PublicPlayer publicPlayer = otherPlayers.Find(player => player.publicPlayerData.in_turn);
            FigCard usedFigCard = publicPlayer.figCards[figCardIndex];
            usedFigCard.gameObject.SetActive(false);
            publicPlayer.MoveFigCardsAtStart();
        }
    }

    public void UpdateBlockFigure()
    {
        myPlayer.UpdateFigCards();
        foreach (PublicPlayer otherPlayer in otherPlayers)
        {
            otherPlayer.UpdateFigCards();
        }
    }

    public void UpdateBlockedColor()
    {
        blockedColor.tileColor = board.boardData.blocked_color;
        blockedColor.ApplyChanges();
    }

    public void UpdateCardsLeft()
    {
        if (myPlayer.playerData.in_turn)
        {
            myPlayer.SetCardsLeft();
        }
        else
        {
            PublicPlayer publicPlayer = otherPlayers.Find(player => player.publicPlayerData.in_turn);
            publicPlayer.SetCardsLeft();
        }
    }

    public void DestroyUsedCards()
    {
        myPlayer.DestroyUsedCards();
        foreach (PublicPlayer publicPlayer in otherPlayers)
        {
            publicPlayer.DestroyUsedCards();
        }
    }

    public void GetMoreCards()
    {
        myPlayer.GetMoreCards();
        foreach(PublicPlayer publicPlayer in otherPlayers)
        {
            publicPlayer.GetMoreCards();
        }
    }
}
