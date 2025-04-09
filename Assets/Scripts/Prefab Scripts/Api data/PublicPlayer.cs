using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublicPlayer : MonoBehaviour
{
    // Scene Components
    public AudioController audioController;
    public TransformUtils transformUtils;

    // Other Prefabs
    [SerializeField] FigCard figCardPrefab;

    // Prefab Components
    [SerializeField] TextMeshProUGUI playername;
    [SerializeField] TextMeshProUGUI cardsLeft;

    // Prefab Settings
    public PublicPlayerData publicPlayerData;

    // Public Player
    public List<FigCard> figCards = new List<FigCard>();

    public void CreatePublicPlayer()
    {
        SetGameColor();
        SetName();
        SetCardsLeft();
        CreateFigCards();
    }

    void SetGameColor()
    {
        playername.color = GameColorUtils.GetColor(publicPlayerData.player_color);
        cardsLeft.color = GameColorUtils.GetColor(publicPlayerData.player_color);
    }

    void SetName()
    {
        playername.text = publicPlayerData.username;
    }

    public void SetCardsLeft()
    {
        cardsLeft.text = $"{publicPlayerData.fig_cards_left} cards";
    }

    void CreateFigCards()
    {
        for (int i = 0; i < publicPlayerData.fig_cards_in_hand.Count; i++)
        {
            FigCardData figCardData = publicPlayerData.fig_cards_in_hand[i];
            FigCard figCard = CreateFigCard(figCardData, i);
            figCards.Add(figCard);
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
        figCard.originalPosition = new Vector2(-272 + 272 * cardIndex, -70);
        figCard.transform.localPosition = figCard.originalPosition;
        figCard.ApplySettings();
        return figCard;
    }

    public void UpdatePublicPlayerdata(PublicPlayerData newPublicPlayerData)
    {
        publicPlayerData = newPublicPlayerData;
        for (int j = 0; j < figCards.Count; j++)
        {
            FigCard figCard = figCards[j];
            figCard.figCardData = newPublicPlayerData.fig_cards_in_hand[j];
        }
    }

    public void DestroyUsedCards()
    {
        DestroyUsedFigCards();
    }

    public void GetMoreCards()
    {
        GetMoreFigCards();
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

    public void MoveFigCardsAtStart()
    {
        int visibleIndex = 0;

        for (int i = 0; i < figCards.Count; i++)
        {
            FigCard figCard = figCards[i];

            if (!figCard.gameObject.activeSelf)
                continue;

            figCard.originalPosition = new Vector2(-272 + 272 * visibleIndex, -70);
            transformUtils.MoveOriginalToTarget(transform: figCard.transform,
                targetPosition: figCard.originalPosition,
                seconds: 0.25f,
                handleActive: true);

            visibleIndex++;
        }
    }

    void GetMoreFigCards()
    {
        int inHandCardsCount = publicPlayerData.fig_cards_in_hand.Count;
        for (int i = figCards.Count; i < inHandCardsCount; i++)
        {
            FigCard figCard = CreateFigCard(publicPlayerData.fig_cards_in_hand[i], i);
            figCard.originalPosition = new Vector2(-272 + 272 * i, -70);
            figCard.transform.localPosition = new Vector2(-272 + 272 * (inHandCardsCount - 1), -70);
            figCards.Add(figCard);
            transformUtils.MoveOriginalToTarget(transform: figCard.transform,
                targetPosition: figCard.originalPosition,
                seconds: 0.25f,
                handleActive: true);
        }
    }
}
