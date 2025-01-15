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

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 540.0f;

    void Start()
    {
        SetUpCard();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    public void SetUpCard()
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

    public void MoveToPoint(Vector3 pointToMoveto, Quaternion rotationToMatch)
    {
        targetPoint = pointToMoveto;
        targetRotation = rotationToMatch;
    }
}
