using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private void Awake()
    {
        instance = this;
    }

    public int startingMana = 4;
    public int maxMana = 12;
    public int playerMana;
    public int startingCardsAmount = 5;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActive, enemyCardAttacks }
    public TurnOrder currentPhase;

    void Start()
    {
        playerMana = startingMana;
        UIController.instance.SetPlayerManaText(playerMana);
        DeckController.instance.DrawMultipleCards(startingCardsAmount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana = playerMana - amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn()
    {
        currentPhase++;

        if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }

        switch(currentPhase)
        {
            case TurnOrder.playerActive:

                break;
            case TurnOrder.playerCardAttacks:
                Debug.Log("Skipping Player Attack");
                // AdvanceTurn();
                break;
            case TurnOrder.enemyActive:
                Debug.Log("Skipping Enemy Actions");
                AdvanceTurn();
                break;
            case TurnOrder.enemyCardAttacks:
                Debug.Log("Skipping Enemy Attack");
                AdvanceTurn();
                break;
        }
    }
}
