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

    void Start()
    {
        SetupDeck();
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

        BattleController.instance.AdvanceTurn();
    }
}
