using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject CardAsset;

    public int currentHealth;
    public int attackPower;
    public int manaCost;

    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text manaText;
    public TMP_Text nameText;
    public TMP_Text actionText;
    public TMP_Text descriptionText;

    public Image characterArt;
    public Image backgroundArt;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeCard()
    {
        currentHealth = CardAsset.currentHealth;
        attackPower = CardAsset.attackPower;
        manaCost = CardAsset.manaCost;

        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        manaText.text = manaCost.ToString();

        nameText.text = CardAsset.cardName;
        actionText.text = CardAsset.actionDescription;
        descriptionText.text = CardAsset.cardDescription;

        characterArt.sprite = CardAsset.cardSprite;
        backgroundArt.sprite = CardAsset.backgroundSprite;
    }
}
