using System.Collections;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController instance;

    private void Awake()
    {
        instance = this;
    }

    public CardPlacePoint[] playerCardPoints;
    public CardPlacePoint[] enemyCardPoints;

    public float timeBetweenAttacks = 0.25f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCoroutine());
    }

    IEnumerator PlayerAttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (enemyCardPoints[i].activeCard != null)
                {
                    // Attack the enemy card
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);
                } else
                {
                    // Attack the enemy directly
                    BattleController.instance.DamageEnemy(playerCardPoints[i].activeCard.attackPower);
                }

                playerCardPoints[i].activeCard.animator.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCards();

        BattleController.instance.AdvanceTurn();
    }

    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCoroutine());
    }

    IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    // Attack the player card
                    playerCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);
                }
                else
                {
                    // Attack the player directly
                    BattleController.instance.DamagePlayer(enemyCardPoints[i].activeCard.attackPower);
                }

                enemyCardPoints[i].activeCard.animator.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCards();

        BattleController.instance.AdvanceTurn();
    }

    public void CheckAssignedCards()
    {
        foreach (CardPlacePoint point in enemyCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.currentHealth <= 0)
                {
                    point.activeCard = null;
                }
            }
        }

        foreach (CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.currentHealth <= 0)
                {
                    point.activeCard = null;
                }
            }
        }
    }
}
