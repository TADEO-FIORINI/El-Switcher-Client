using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameState : MonoBehaviour
{
    // Scene components
    [SerializeField] GameNetworkManager gameNetworkManager;
    
    // Game
    public GamePublic publicGame;

    // Game state data
    public State state { get; private set; }
    public int selectedMovCardIndex { get; private set; }
    public string selectedFigCardPlayerName { get; private set; }
    public int selectedFigCardIndex { get; private set; }
    public int selectedTile1_X { get; private set; }
    public int selectedTile1_Y { get; private set; }
    public int selectedTile2_X { get; private set; }
    public int selectedTile2_Y { get; private set; }

    private void Awake()
    {
        ResetData();
    }


    void SelectMovCard(int movCardIndex)
    {
        ResetGameState();
        SetState(State.SelectTiles);
        selectedMovCardIndex = movCardIndex;
        publicGame.myPlayer.movCards[selectedMovCardIndex].SetSelected(true);
    }

    void SelectFigCard(int figCardIndex, string figCardPlayername)
    {
        ResetGameState();
        SetState(State.SelectTiles);
        selectedFigCardIndex = figCardIndex;
        selectedFigCardPlayerName = figCardPlayername;

        if (selectedFigCardPlayerName == publicGame.publicGameData.my_player.username)
        {
            publicGame.myPlayer.figCards[selectedFigCardIndex].SetSelected(true);
        }
        else
        {
            publicGame.otherPlayers
                .Find(publicPlayer => publicPlayer.publicPlayerData.username == selectedFigCardPlayerName)
                .figCards[selectedFigCardIndex].SetSelected(true);
        }

    }

    void SelectTile(int tile_X, int tile_Y)
    {
        if (selectedTile1_X == -1 && selectedTile1_Y == -1)
        {
            selectedTile1_X = tile_X;
            selectedTile1_Y = tile_Y;
            if (selectedFigCardIndex != -1)
            {
                if (selectedFigCardPlayerName == publicGame.publicGameData.my_player.username)
                {
                    gameNetworkManager.FigureDiscard(NetworkAuthentication.userPrivateData.user_id, publicGame.publicGameData.room.room_id, selectedFigCardIndex, selectedTile1_X, selectedTile1_Y);
                }
                else
                {
                    gameNetworkManager.FigureBlock(NetworkAuthentication.userPrivateData.user_id, publicGame.publicGameData.room.room_id, selectedFigCardPlayerName,selectedFigCardIndex, selectedTile1_X, selectedTile1_Y);
                }
                SetState(State.SelectCard);
            }
            else
            {
                publicGame.board.SetTargetTiles(publicGame.myPlayer.movCards[selectedMovCardIndex].movCardData.mov_type, selectedTile1_X, selectedTile1_Y);
            }
        }
        else if (selectedTile1_X == tile_X && selectedTile1_Y == tile_Y)
        {
            ResetBoardGameState();
        }
        else
        {
            selectedTile2_X = tile_X;
            selectedTile2_Y = tile_Y;
            gameNetworkManager.Switch(NetworkAuthentication.userPrivateData.user_id, publicGame.publicGameData.room.room_id, selectedMovCardIndex, selectedTile1_X, selectedTile1_Y, selectedTile2_X, selectedTile2_Y);
            SetState(State.SelectCard);
        }
    }

    public void SetButtonsListeners()
    {
        SetMovCardsListeners();
        SetTilesListeners();
        SetFigCardsListeners();
    }

    void SetMovCardsListeners()
    {
        for (int i = 0; i < publicGame.myPlayer.movCards.Count; i++)
        {
            int localIndex = i;
            MovCard movCard = publicGame.myPlayer.movCards[localIndex];
            Button button = movCard.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => movCard.SetSelected(!movCard.isSelected));
            button.onClick.AddListener(() => SelectMovCard(localIndex));
        }
    }

    void SetTilesListeners()
    {
        int boardDim = (int)Math.Sqrt(publicGame.publicGameData.board.tiles.Count);
        for (int y = 0; y < boardDim; y++)
        {
            for (int x = 0; x < boardDim; x++)
            {
                int localY = y;
                int localX = x;
                Tile tile = publicGame.board.tileMatrix[localY][localX];
                Button button = tile.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => tile.SetSelected(!tile.isSelected));
                button.onClick.AddListener(() => SelectTile(localX, localY));
            }
        }
    }

    void SetFigCardsListeners()
    {
        for (int i = 0; i < publicGame.myPlayer.figCards.Count; i++)
        {
            int localIndex = i;
            FigCard figCard = publicGame.myPlayer.figCards[localIndex];
            Button button = figCard.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => figCard.SetSelected(!figCard.isSelected));
            button.onClick.AddListener(() => SelectFigCard(localIndex, publicGame.publicGameData.my_player.username));
        }
        foreach (PublicPlayer publicPlayer in publicGame.otherPlayers)
        {
            for (int i = 0; i < publicPlayer.figCards.Count; i++)
            {
                FigCard figCard = publicPlayer.figCards[i];
                Button button = figCard.GetComponent<Button>();
                int localIndex = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SelectFigCard(localIndex, publicPlayer.publicPlayerData.username));
            }
        }
    }


    public void ResetData()
    {
        selectedFigCardPlayerName = string.Empty;
        selectedMovCardIndex = -1;
        selectedFigCardIndex = -1;
        selectedTile1_X = -1;
        selectedTile1_Y = -1;
        selectedTile2_X = -1;
        selectedTile2_Y = -1;
    }

    private void ResetGameState()
    {
        ResetData();
        ResetMovCardsGameState();
        ResetFigCardsGameState();
        ResetBoardGameState();
    }

    void ResetMovCardsGameState()
    {
        selectedMovCardIndex = -1;
        foreach (MovCard movCard in publicGame.myPlayer.movCards)
        {
            movCard.SetSelected(false);
        }
    }

    void ResetFigCardsGameState()
    {
        selectedFigCardIndex = -1;
        foreach (FigCard figCard in publicGame.myPlayer.figCards)
        {
            figCard.SetSelected(false);
        }
        foreach (PublicPlayer publicPlayer in publicGame.otherPlayers)
        {
            foreach (FigCard figCard in publicPlayer.figCards)
            {
                figCard.SetSelected(false);
            }
        }
    }

    void ResetBoardGameState()
    {
        selectedTile1_X = -1;
        selectedTile1_Y = -1;
        selectedTile2_X = -1;
        selectedTile2_Y = -1;
        int boardDim = (int)Math.Sqrt(publicGame.publicGameData.board.tiles.Count);
        for (int y = 0; y < boardDim; y++)
        {
            for (int x = 0; x < boardDim; x++)
            {
                Tile tile = publicGame.board.tileMatrix[y][x];
                Button button = tile.GetComponent<Button>();
                tile.SetSelected(false);
            }
        }
    }

    public enum State
    {
        None,                   /* no game actions */
        SelectCard,             /* select mov card or fig card */
        SelectTiles,            /* select tiles */
    }


    public void SetState(State newState)
    {
        switch (newState)
        {
            case State.None:
                SetNoneState();
                break;
            case State.SelectCard:
                SetSelectCardState();
                break;
            case State.SelectTiles:
                SetSelectTilesState();
                break;
        }
    }

    void SetNoneState()
    {
        ResetGameState();
        SetActiveAllTiles(false);
        SetActiveAllFigCards(false);
        SetActiveAllMovCards(false);
    }

    void SetSelectCardState()
    {
        ResetGameState();
        state = State.SelectCard;
        SetActiveAllTiles(false);
        SetActiveAllFigCards(true);
        SetActiveAllMovCards(true);
    }

    void SetSelectTilesState()
    {
        state = State.SelectTiles;
        SetActiveAllTiles(true);
        SetActiveAllFigCards(true);
        SetActiveAllMovCards(true);
    }

    void SetActiveAllTiles(bool setActive)
    {
        int boardDim = (int)Math.Sqrt(publicGame.publicGameData.board.tiles.Count);
        for (int y = 0; y < boardDim; y++)
        {
            for (int x = 0; x < boardDim; x++)
            {
                Tile tile = publicGame.board.tileMatrix[y][x];
                tile.SetActiveButton(setActive);
            }
        }
    }

    void SetActiveAllFigCards(bool setActive)
    {
        foreach (FigCard figCard in publicGame.myPlayer.figCards)
        {
            figCard.SetActiveButton(setActive);
        }
        foreach (PublicPlayer publicPlayer in publicGame.otherPlayers)
        {
            foreach (FigCard figCard in publicPlayer.figCards)
            {
                figCard.SetActiveButton(setActive);
            }
        }
    }

    void SetActiveAllMovCards(bool setActive)
    {
        foreach (MovCard movCard in publicGame.myPlayer.movCards)
        {
            movCard.SetActiveButton(setActive);
        }
    }
}
