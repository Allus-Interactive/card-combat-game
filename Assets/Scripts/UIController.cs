using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }

    public TMP_Text playerManaText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyManaText;
    public TMP_Text enemyHealthText;

    public GameObject manaWarning;
    public float manaWarningTime;
    private float manaWarningCounter;

    public GameObject drawCardButton;
    public GameObject endTurnButton;

    public UIDamageIndicator playerDamageIndicator;
    public UIDamageIndicator enemyDamageIndicator;

    public GameObject BattleEndScreen;
    public TMP_Text battleResultText;

    void Start()
    {

    }

    void Update()
    {
        if (manaWarningCounter > 0)
        {
            manaWarningCounter -= Time.deltaTime;

            if (manaWarningCounter <= 0)
            {
                manaWarning.SetActive(false);
            }
        }
    }

    public void SetPlayerManaText(int manaAmount)
    {
        playerManaText.text = "Mana: " + manaAmount;
    }

    public void SetPlayerHealthText(int healthAmount)
    {
        playerHealthText.text = "Player Health: " + healthAmount;
    }

    public void SetEnemyManaText(int manaAmount)
    {
        enemyManaText.text = "Mana: " + manaAmount;
    }

    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = "Enemy Health: " + healthAmount;
    }

    public void ShowManaWarning()
    {
        manaWarning.SetActive(true);
        manaWarningCounter = manaWarningTime;
    }

    public void DrawCard()
    {
        DeckController.instance.DrawCardForMana();
    }

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();
    }

    public void MainMenu()
    {
        Debug.Log("Go to Main Menu");
    }

    public void RestartGame()
    {
        Debug.Log("Restart the game");
    }

    public void ChooseNewGame()
    {
        Debug.Log("Choose a new game");
    }
}
