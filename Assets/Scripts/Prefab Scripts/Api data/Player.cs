using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Scene Components
    public AudioController audioController;
    public TransformUtils transformUtils;

    // Other Prefabs
    [SerializeField] MovCard movCardPrefab;
    [SerializeField] FigCard figCardPrefab;

    // Prefab Components
    [SerializeField] TextMeshProUGUI playername; 
    [SerializeField] TextMeshProUGUI cardsLeft; 

    // Prefab Settings
    public PlayerData playerData;

    // Public Player
    public List<FigCard> figCards = new List<FigCard>();
    public List<MovCard> movCards = new List<MovCard>();

    public void CreatePlayer()
    {
        SetGameColor();
        SetName();
        SetCardsLeft();
        CreateFigCards();
        CreateMovCards();
    }

    void SetGameColor()
    {
        playername.color = GameColorUtils.GetColor(playerData.player_color);
        cardsLeft.color = GameColorUtils.GetColor(playerData.player_color);
    }

    void SetName()
    {
        playername.text = playerData.username;
    }

    public void SetCardsLeft()
    {
        cardsLeft.text = $"{playerData.fig_cards_left} cards";
    }

    void CreateFigCards()
    {
        for (int i = 0; i < playerData.player_deck.Count; i++)
        {
            FigCardData figCardData = playerData.player_deck[i];
            if (figCardData.in_hand)
            {
                FigCard figCard = CreateFigCard(figCardData, i);
                figCards.Add(figCard);
            }
        }
    }

    public void UpdateFigCards()
    {
        foreach (FigCard figCard in figCards)
        {
            figCard.ApplySettings();
        }
    }

    FigCard CreateFigCard(FigCardData figCardData, int cardIndex)
    {
        FigCard figCard = Instantiate(figCardPrefab, transform);
        figCard.audioController = audioController;
        figCard.transformUtils = transformUtils;
        figCard.figCardData = figCardData;
        figCard.originalPosition = new Vector2(-680 + 272 * cardIndex, -80);
        figCard.transform.localPosition = figCard.originalPosition;
        figCard.ApplySettings();
        return figCard;
    } 

    void CreateMovCards()
    {
        for (int i = 0; i < playerData.mov_cards.Count; i++)
        {
            MovCardData movCardData = playerData.mov_cards[i];
            MovCard movCard = CreateMovCard(movCardData, i);

            movCards.Add(movCard);
        }
    }

    MovCard CreateMovCard(MovCardData movCardData, int cardIndex)
    {
        GameObject movCardGO = Instantiate(movCardPrefab.gameObject, transform);
        MovCard movCard = movCardGO.GetComponent<MovCard>();
        movCard.audioController = audioController;
        movCard.transformUtils = transformUtils;
        movCard.movCardData = movCardData;
        movCard.originalPosition = new Vector2(136 + 272 * cardIndex, -40);
        movCard.transform.localPosition = movCard.originalPosition; 
        movCard.ApplySettings();
        return movCard;
    }


    public void UpdatePlayerData(PlayerData newPlayerData)
    {
        playerData = newPlayerData;
        for (int j = 0; j < figCards.FindAll(figCard => figCard.figCardData.in_hand).Count; j++)
        {
            FigCard figCard = figCards[j];
            figCard.figCardData = newPlayerData.player_deck[j];
        }
        for (int j = 0; j < movCards.Count; j++)
        {
            MovCard movCard = movCards[j];
            movCard.movCardData = newPlayerData.mov_cards[j];
        }
    }

    public void DestroyUsedCards()
    {
        DestroyUsedMovCards();
        DestroyUsedFigCards();
    }

    public void GetMoreCards()
    {
        GetMoreMovCards();
        GetMoreFigCards();
    }

    void DestroyUsedMovCards()
    {
        foreach (MovCard movCard in movCards)
        {
            if (movCard.movCardData.is_used)
                Destroy(movCard.gameObject);
        }
        movCards.RemoveAll(movCard => movCard.movCardData.is_used);
    }

    void DestroyUsedFigCards()
    {
        foreach (FigCard figCard in figCards)
        {
            if (figCard.figCardData.is_used)
                Destroy(figCard.gameObject);
        }
        figCards.RemoveAll(figCard => figCard.figCardData.is_used);
    }

    public void MoveMovCardsAtStart()
    {
        int visibleIndex = 0;

        for (int i = 0; i < movCards.Count; i++)
        {
            MovCard movCard = movCards[i];

            if (!movCard.gameObject.activeSelf)
                continue;

            movCard.originalPosition = new Vector2(136 + 272 * visibleIndex, -40);
            transformUtils.MoveOriginalToTarget(transform: movCard.transform,
                targetPosition: movCard.originalPosition,
                seconds: 0.25f,
                handleActive: true);

            visibleIndex++;
        }
    }

    void GetMoreMovCards()
    {
        for (int i = movCards.Count; i < playerData.mov_cards.Count; i++)
        {
            MovCard movCard = CreateMovCard(playerData.mov_cards[i], i);
            movCard.originalPosition = new Vector2(136 + 272 * i, -40);
            movCard.transform.localPosition = new Vector2(136 + 272 * (playerData.mov_cards.Count - 1), -40);
            movCards.Add(movCard);
            transformUtils.MoveOriginalToTarget(transform: movCard.transform,
                targetPosition: movCard.originalPosition,
                seconds: 0.25f,
                handleActive: true);
            }
    }

    public void MoveFigCardsAtStart()
    {
        int visibleIndex = 0;

        for (int i = 0; i < figCards.Count; i++)
        {
            FigCard figCard = figCards[i];

            if (!figCard.gameObject.activeSelf)
                continue;

            figCard.originalPosition = new Vector2(-680 + 272 * visibleIndex, -80);
            transformUtils.MoveOriginalToTarget(transform: figCard.transform,
                targetPosition: figCard.originalPosition,
                seconds: 0.25f,
                handleActive: true);

            visibleIndex++;
        }
    }

    void GetMoreFigCards()
    {
        int inHandCardsCount = playerData.player_deck.Where(figCard => figCard.in_hand).ToList().Count;
        for (int i = figCards.Count; i < inHandCardsCount; i++)
        {
            FigCard figCard = CreateFigCard(playerData.player_deck[i], i);
            figCard.originalPosition = new Vector2(-680 + 272 * i, -80);
            figCard.transform.localPosition = new Vector2(-680 + 272 * (inHandCardsCount - 1), -80);
            figCards.Add(figCard);
            transformUtils.MoveOriginalToTarget(transform: figCard.transform,
                targetPosition: figCard.originalPosition,
                seconds: 0.25f,
                handleActive: true);
        }
    }

}
