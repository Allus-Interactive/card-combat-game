using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public int currentHealth;
    public int attackPower;
    public int manaCost;

    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text costText;

    void Start()
    {
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString(); 
        costText.text = manaCost.ToString();
    }

    void Update()
    {
        
    }
}
