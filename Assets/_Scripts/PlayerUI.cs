using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{   
    [SerializeField]
    private PlayerAttributes playerAttributes;
    
    private int maxHealth, currentHealth;


    [SerializeField] private Image healthBar;
    
    [SerializeField] private Image[] manaBar;

    [SerializeField] private Player player;

    private int health;
    private int playerMana; 
    void Start()
    {
     player = player.GetComponent<Player>();
     maxHealth = player.PlayerLife;
     healthBar.fillAmount = 1;
    }

    public float GetHealthAsFraction() {
        return Mathf.Clamp01((float) health / maxHealth);
    }

    void Update()
    {
        health = player.PlayerLife;
        playerMana = player.PlayerMana;
        healthBar.fillAmount = GetHealthAsFraction();
        
        if(playerMana == 3){
            foreach (var item in manaBar)
            {
                item.enabled = true;
            }
        }else if(playerMana == 2){
            manaBar[0].enabled = false;
            manaBar[1].enabled = true;
            manaBar[2].enabled = true;
        }else if(playerMana == 1){
            manaBar[0].enabled = false;
            manaBar[1].enabled = false;
            manaBar[2].enabled = true;
        }else if(playerMana == 0){
            foreach (var item in manaBar)
            {
                item.enabled = false;
            }
        }
    }
}
