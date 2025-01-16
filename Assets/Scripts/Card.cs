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

    public bool inHand;
    public int handPosition;

    private HandController handController;

    private bool isSelected;
    private Collider col;

    public LayerMask whatIsDesktop;
    public LayerMask whatIsPlacement;

    private bool justPressed;

    public CardPlacePoint assignedPlace;

    void Start()
    {
        SetUpCard();
        handController = FindFirstObjectByType<HandController>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100.0f, whatIsPlacement))
                {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;

                            MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                            inHand = false;
                            isSelected = false;

                            handController.RemoveCardFromHand(this);

                            BattleController.instance.SpendPlayerMana(manaCost);
                        } else
                        {
                            ReturnToHand();
                        }
                    } else
                    {
                        ReturnToHand();
                    }
                } else
                {
                    ReturnToHand();
                }
            }
        }

        justPressed = false;
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

    public void ReturnToHand()
    {
        isSelected = false;
        col.enabled = true;

        MoveToPoint(handController.cardPositions[handPosition], handController.minPosition.rotation);
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

    private void OnMouseDown()
    {
        if (inHand)
        {
            isSelected = true;
            col.enabled = false;

            justPressed = true;
        }
    }
}
