using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;
    public Transform cardSpawnPoint;

    public enum AiType { placeFromDeck, handRandomPlace, handDefensive, handOffensive }
    public AiType enemyAiType;

    private List<CardScriptableObject> cardsInHand = new List<CardScriptableObject>();
    public int startHandSize;

    void Start()
    {
        SetupDeck();
        
        if (enemyAiType != AiType.placeFromDeck)
        {
            SetupHand();
        }
    }

    void Update()
    {
        
    }

    public void SetupDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        while (tempDeck.Count > 0)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);
        }
    }

    public void StartAction()
    {
        StartCoroutine(EnemyActionCoroutine());
    }

    IEnumerator EnemyActionCoroutine()
    {
        if (activeCards.Count == 0)
        {
            // TODO: do we want to refill the deck? Maybe enemy has to finish the game with no deck
            // or does an empty deck mean an immediate loss?
            SetupDeck();
        }

        yield return new WaitForSeconds(0.5f);

        if (enemyAiType != AiType.placeFromDeck)
        {
            for (int i = 0; i < BattleController.instance.cardsToDrawPerTurn; i++)
            {
                cardsInHand.Add(activeCards[0]);
                activeCards.RemoveAt(0);

                if (activeCards.Count == 0)
                {
                    // TODO: do we want to refill the deck or break out of the loop as there are no more cards
                    // break;
                    SetupDeck();
                }
            }
        }

        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        if (enemyAiType == AiType.placeFromDeck || enemyAiType == AiType.handRandomPlace)
        {
            cardPoints.Remove(selectedPoint);

            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }

        CardScriptableObject selectedCard = null;
        List<CardPlacePoint> preferredPoints = new List<CardPlacePoint>();
        List<CardPlacePoint> secondaryPoints = new List<CardPlacePoint>();

        switch (enemyAiType)
        {
            case AiType.placeFromDeck:
                if (selectedPoint.activeCard == null)
                {
                    Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                    newCard.cardSO = activeCards[0];
                    activeCards.RemoveAt(0);
                    newCard.SetUpCard();
                    newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);

                    selectedPoint.activeCard = newCard;
                    newCard.assignedPlace = selectedPoint;
                }
                break;
            case AiType.handRandomPlace:
                selectedCard = SelectedCardToPlay();

                while (selectedCard != null && selectedPoint.activeCard == null)
                {
                    PlayCard(selectedCard, selectedPoint);

                    // check if the enemy can play another card
                    selectedCard = SelectedCardToPlay();

                    yield return new WaitForSeconds(CardPointsController.instance.timeBetweenAttacks);

                    while (selectedPoint.activeCard != null && cardPoints.Count > 0)
                    {
                        randomPoint = Random.Range(0, cardPoints.Count);
                        selectedPoint = cardPoints[randomPoint];
                        cardPoints.RemoveAt(randomPoint);
                    }
                }
                break;
            case AiType.handDefensive:
                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointsController.instance.playerCardPoints[i].activeCard != null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                        } else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }

                while (selectedCard != null && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    // Pick a card point to use
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    } else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    // Check if the enemy should try play another card
                    selectedCard = SelectedCardToPlay();

                    yield return new WaitForSeconds(CardPointsController.instance.timeBetweenAttacks);
                }

                break;
            case AiType.handOffensive:
                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointsController.instance.playerCardPoints[i].activeCard == null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }

                while (selectedCard != null && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    // Pick a card point to use
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    // Check if the enemy should try play another card
                    selectedCard = SelectedCardToPlay();

                    yield return new WaitForSeconds(CardPointsController.instance.timeBetweenAttacks);
                }
                break;
        }

        yield return new WaitForSeconds(0.5f);

        BattleController.instance.AdvanceTurn();
    }

    void SetupHand()
    {
        for (int i = 0; i < startHandSize; i++)
        {
            if (activeCards.Count == 0)
            {
                SetupDeck();
            }

            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        } 
    }

    public void PlayCard(CardScriptableObject cardSO, CardPlacePoint placePoint)
    {
        if (cardSO == null)
        {
            // No card has been selected, skip placement action
            return;
        }

        Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
        newCard.cardSO = cardSO;
        newCard.SetUpCard();
        newCard.MoveToPoint(placePoint.transform.position, placePoint.transform.rotation);

        placePoint.activeCard = newCard;
        newCard.assignedPlace = placePoint;

        cardsInHand.Remove(cardSO);

        BattleController.instance.SpendEnemyMana(cardSO.manaCost);
    }

    CardScriptableObject SelectedCardToPlay()
    {
        CardScriptableObject cardToPlay = null;

        List<CardScriptableObject> cardsToPlay = new List<CardScriptableObject>();
        foreach (CardScriptableObject card in cardsInHand)
        {
            if (card.manaCost <= BattleController.instance.enemyMana)
            {
                cardsToPlay.Add(card);
            }
        }

        if (cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0, cardsToPlay.Count);

            cardToPlay = cardsToPlay[selected];
        }

        return cardToPlay;
    }
}
