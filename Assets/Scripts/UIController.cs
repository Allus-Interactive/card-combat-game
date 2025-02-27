using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject handLimitWarning;
    public float handLimitWarningTime;
    private float handLimitWarningCounter;

    public GameObject drawCardButton;
    public GameObject endTurnButton;

    public UIDamageIndicator playerDamageIndicator;
    public UIDamageIndicator enemyDamageIndicator;

    public GameObject BattleEndScreen;
    public TMP_Text battleResultText;

    private string mainMenuScene = "Main Menu";
    private string gameSelectionScene = "Game Selection";

    public GameObject pauseScreen;

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

        if (handLimitWarningCounter > 0)
        {
            handLimitWarningCounter -= Time.deltaTime;

            if (handLimitWarningCounter <= 0)
            {
                handLimitWarning.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
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

    public void ShowHandLimitWarning()
    {
        handLimitWarning.SetActive(true);
        handLimitWarningCounter = handLimitWarningTime;
    }

    public void DrawCard()
    {
        DeckController.instance.DrawCardForMana();

        AudioManager.instance.PlaySFX(0);
    }

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();

        AudioManager.instance.PlaySFX(0);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;

        AudioManager.instance.PlaySFX(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;

        AudioManager.instance.PlaySFX(0);
    }

    public void ChooseNewGame()
    {
        SceneManager.LoadScene(gameSelectionScene);

        Time.timeScale = 1f;

        AudioManager.instance.PlaySFX(0);
    }

    public void PauseGame()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        } else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

        AudioManager.instance.PlaySFX(0);
    }
}
