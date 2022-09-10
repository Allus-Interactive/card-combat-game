using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public int currentHealth;
    public int attackPower;
    public int manaCost;

    public TMP_Text healthText;
    public TMP_Text attackText;
    public TMP_Text manaText;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        manaText.text = manaCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
