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

    public float moveSpeed = 5f;
    public float rotateSpeed = 540f;

    public bool inHand;
    public int handPosition;

    private Vector3 targetPoint;
    private Quaternion targetRotation;
    private HandController handController;

    void Start()
    {
        InitializeCard();

        handController = FindObjectOfType<HandController>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, (moveSpeed * Time.deltaTime));
        transform.rotation = Quaternion.RotateTowards( transform.rotation, targetRotation, (rotateSpeed * Time.deltaTime));
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

    public void MoveToPoint(Vector3 destination, Quaternion rotation)
    {
        targetPoint = destination;
        targetRotation = rotation;
    }

    private void OnMouseOver() 
    {
        if (inHand)
        {
            MoveToPoint(handController.cardPositions[handPosition] + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(handController.cardPositions[handPosition], handController.minPosition.rotation);
        }    
    }
}
