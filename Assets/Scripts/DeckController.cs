using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        SetupDeck();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DrawCardToHand();
        }
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
    }
}
