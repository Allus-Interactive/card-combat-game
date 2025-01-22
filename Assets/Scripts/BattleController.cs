using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public int enemyMana;
    private int currentPlayerMaxMana;
    private int currentEnemyMaxMana;

    public int startingCardsAmount = 5;
    public int cardsToDrawPerTurn = 1;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActive, enemyCardAttacks }
    public TurnOrder currentPhase;

    public Transform discardPoint;

    public int playerHealth;
    public int enemyHealth;

    public bool battleEnded;

    public float resultsScreenDelay = 3f;

    void Start()
    {
        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;
        FillPlayerMana();
        FillEnemyMana();

        DeckController.instance.DrawMultipleCards(startingCardsAmount);

        UIController.instance.SetPlayerHealthText(playerHealth);
        UIController.instance.SetEnemyHealthText(enemyHealth);
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
        playerMana -= amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillPlayerMana()
    {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana -= amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }

        UIController.instance.SetEnemyManaText(enemyMana);
    }

    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemyManaText(enemyMana);
    }

    public void AdvanceTurn()
    {
        if (!battleEnded) 
        { 
            currentPhase++;

            if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
            {
                currentPhase = 0;
            }

            switch (currentPhase)
            {
                case TurnOrder.playerActive:
                    // UIController.instance.endTurnButton.SetActive(true);
                    // UIController.instance.drawCardButton.SetActive(true);
                    UIController.instance.endTurnButton.GetComponent<Button>().interactable = true;
                    UIController.instance.drawCardButton.GetComponent<Button>().interactable = true;

                    if (currentPlayerMaxMana < maxMana)
                    {
                        currentPlayerMaxMana++;
                    }

                    FillPlayerMana();

                    if (cardsToDrawPerTurn > 1)
                    {
                        // Draw multiple cards at the beginning of the turn
                        DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);
                    }
                    else
                    {
                        // Draw one card per turn
                        DeckController.instance.DrawCardToHand();
                    }

                    break;
                case TurnOrder.playerCardAttacks:
                    CardPointsController.instance.PlayerAttack();
                    break;
                case TurnOrder.enemyActive:
                    if (currentEnemyMaxMana < maxMana)
                    {
                        currentEnemyMaxMana++;
                    }

                    FillEnemyMana();

                    EnemyController.instance.StartAction();
                    break;
                case TurnOrder.enemyCardAttacks:
                    CardPointsController.instance.EnemyAttack();
                    break;
            }
        }
    }

    public void EndPlayerTurn()
    {
        // UIController.instance.endTurnButton.SetActive(false);
        // UIController.instance.drawCardButton.SetActive(false);
        UIController.instance.endTurnButton.GetComponent<Button>().interactable = false;
        UIController.instance.drawCardButton.GetComponent<Button>().interactable = false;
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount)
    {
        if (playerHealth > 0 || !battleEnded)
        {
            playerHealth -= damageAmount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;

                // End the game
                EndBattle();
            }
        }

        UIController.instance.SetPlayerHealthText(playerHealth);

        UIDamageIndicator damageClone = Instantiate(UIController.instance.playerDamageIndicator, UIController.instance.playerDamageIndicator.transform.parent);
        damageClone.damageText.text = damageAmount.ToString();
        damageClone.gameObject.SetActive(true);
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0 || !battleEnded)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;

                // End the game
                EndBattle();
            }
        }

        UIController.instance.SetEnemyHealthText(enemyHealth);

        UIDamageIndicator damageClone = Instantiate(UIController.instance.enemyDamageIndicator, UIController.instance.enemyDamageIndicator.transform.parent);
        damageClone.damageText.text = damageAmount.ToString();
        damageClone.gameObject.SetActive(true);
    }

    void EndBattle()
    {
        battleEnded = true;

        HandController.instance.EmptyHandAndEndGame();
    }
}
