using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text costText;

    public TMP_Text cardNameText;
    public TMP_Text actionDescriptionText;
    public TMP_Text cardLoreText;

    public Image characterArt;
    public Image backgroundArt;

    private int currentHealth;
    private int attackPower;
    private int manaCost;

    void Start()
    {
        setUpCard();
    }

    void Update()
    {
        
    }

    public void setUpCard()
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        manaCost = cardSO.manaCost;

        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = manaCost.ToString();

        cardNameText.text = cardSO.cardName;
        actionDescriptionText.text = cardSO.actionDescription;
        cardLoreText.text = cardSO.cardLore;

        characterArt.sprite = cardSO.characterSprite;
        backgroundArt.sprite = cardSO.backgroundSprite;
    }
}
