using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public bool isPlayer;

    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text costText;

    public TMP_Text cardNameText;
    public TMP_Text actionDescriptionText;
    public TMP_Text cardLoreText;

    public Image characterArt;
    public Image backgroundArt;

    public int currentHealth;
    public int attackPower;
    public int manaCost;

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

    public Animator animator;

    public Vector3 playedLocation;
    public Quaternion playedRotation;
    public bool isPlayed;
    public bool isZoomed;
    public Transform zoomPoint;

    void Start()
    {
        if (targetPoint == Vector3.zero)
        {
            targetPoint = transform.position;
            targetRotation = transform.rotation;
        }

        SetUpCard();

        handController = FindFirstObjectByType<HandController>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (isSelected && BattleController.instance.battleEnded == false && Time.timeScale != 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1) && BattleController.instance.battleEnded == false) 
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false && BattleController.instance.battleEnded == false)
            {
                if (Physics.Raycast(ray, out hit, 100.0f, whatIsPlacement) && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive)
                {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            // Play the card
                            isPlayed = true;
                            col.enabled = true;

                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;

                            // Move the card to the selected slot
                            MoveToPoint(selectedPoint.transform.position, Quaternion.identity);
                            // Store this location for inspecting the card
                            playedLocation = selectedPoint.transform.position;
                            playedRotation = Quaternion.identity;

                            inHand = false;
                            isSelected = false;

                            handController.RemoveCardFromHand(this);

                            BattleController.instance.SpendPlayerMana(manaCost);

                            AudioManager.instance.PlaySFX(4);
                        } else
                        {
                            ReturnToHand();

                            UIController.instance.ShowManaWarning();
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

        UpdateCardDisplay();

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

    public void DamageCard(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            assignedPlace.activeCard = null;

            MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);

            animator.SetTrigger("Jump");

            Destroy(gameObject, 5f);

            AudioManager.instance.PlaySFX(2);
        } else
        {
            AudioManager.instance.PlaySFX(1);
        }

        animator.SetTrigger("Hurt");

        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = manaCost.ToString();
    }

    public void InspectCard()
    {
        if (!isZoomed && !BattleController.instance.isCardInFocus)
        {
            MoveToPoint(new Vector3(0f, 3f, 0f), Quaternion.identity);
            isZoomed = true;
            BattleController.instance.isCardInFocus = true;
        } else if (isZoomed)
        {
            MoveToPoint(playedLocation, playedRotation);
            isZoomed = false;
            BattleController.instance.isCardInFocus = false;
        }
    }

    private void OnMouseOver()
    {
        if (inHand && isPlayer && BattleController.instance.battleEnded == false)
        {
            MoveToPoint(handController.cardPositions[handPosition] + new Vector3(0f, 1f, 0.5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand && isPlayer && BattleController.instance.battleEnded == false)
        {
            MoveToPoint(handController.cardPositions[handPosition], handController.minPosition.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive && isPlayer && BattleController.instance.battleEnded == false && Time.timeScale != 0f)
        {
            isSelected = true;
            col.enabled = false;

            justPressed = true;
        }

        if (isPlayed)
        {
            InspectCard();
        }
    }
}
