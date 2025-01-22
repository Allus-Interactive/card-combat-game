using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Card> heldCards = new List<Card>();

    public Transform minPosition;
    public Transform maxPosition;

    public List<Vector3> cardPositions = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetCardPositonsInHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCardPositonsInHand()
    {
        cardPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPosition.position - minPosition.position) / (heldCards.Count - 1);
        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPositions.Add(minPosition.position + (distanceBetweenPoints * i));

            // heldCards[i].transform.position = cardPositions[i];
            // heldCards[i].transform.rotation = minPosition.rotation;

            heldCards[i].MoveToPoint(cardPositions[i], minPosition.rotation);

            heldCards[i].inHand = true;
            heldCards[i].handPosition = i;
        }
    }

    public void RemoveCardFromHand(Card cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        } else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + " is not the card being removed from the hand");
        }

        SetCardPositonsInHand();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPositonsInHand();
    }

    public void EmptyHand()
    {
        /* foreach (Card heldCard in heldCards)
        {
            heldCard.inHand = false;
            // heldCard.MoveToPoint(BattleController.instance.discardPoint.position, heldCard.transform.rotation);
            heldCard.MoveToPoint(DeckController.instance.transform.position, DeckController.instance.transform.rotation);
        }

        heldCards.Clear(); */
        StartCoroutine(EmptyHandCoroutine());
    }

    IEnumerator EmptyHandCoroutine()
    {
        // reverse order of the hand
        heldCards.Reverse();

        yield return new WaitForSeconds(0.5f);

        // from right to left, return the cards to the deck
        foreach (Card heldCard in heldCards)
        {
            heldCard.inHand = false;
            heldCard.MoveToPoint(DeckController.instance.transform.position, DeckController.instance.transform.rotation);
            yield return new WaitForSeconds(0.25f);
        }

        // Clear the heldCards object
        heldCards.Clear();
    }
}
