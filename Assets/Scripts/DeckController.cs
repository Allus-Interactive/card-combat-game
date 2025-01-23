using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;

    public int drawCardCost = 2;

    public float waitBetweenDrawingCards = 0.4f;

    private int cardHandLimit = 7;

    void Start()
    {
        SetupDeck();
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

    public void DrawCardToHand()
    {
        if (HandController.instance.heldCards.Count < cardHandLimit)
        {
            if (activeCards.Count == 0)
            {
                // TODO: do we want to refill the deck? Maybe player has to finish the game with no deck
                // or does an empty deck mean an immediate loss?
                SetupDeck();
            }

            Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);
            newCard.cardSO = activeCards[0];
            newCard.SetUpCard();

            activeCards.RemoveAt(0);
            HandController.instance.AddCardToHand(newCard);

            AudioManager.instance.PlaySFX(3);
        }
    }

    public void DrawCardForMana()
    {
        if (HandController.instance.heldCards.Count < cardHandLimit)
        {
            if (BattleController.instance.playerMana >= drawCardCost)
            {
                DrawCardToHand();
                BattleController.instance.SpendPlayerMana(drawCardCost);
            }
            else
            {
                UIController.instance.ShowManaWarning();
                // Set button to disabled rather than inactive
                // UIController.instance.drawCardButton.SetActive(false);
                UIController.instance.drawCardButton.GetComponent<Button>().interactable = false;
            }
        } else
        {
            UIController.instance.ShowHandLimitWarning();
        }
    }

    public void DrawMultipleCards(int amountToDraw)
    {
        StartCoroutine(DrawMultipleCoroutine(amountToDraw));
    }

    IEnumerator DrawMultipleCoroutine(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();
            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }
}
