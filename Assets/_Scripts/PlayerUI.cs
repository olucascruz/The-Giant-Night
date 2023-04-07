using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{   
    [SerializeField]
    private PlayerAttributes playerAttributes;

    private int maxHealth;
    private int currentHealth;
    [SerializeField]
    private Image healthBar;
    
    [SerializeField]
    private Image[] manaBar;
    // Start is called before the first frame update

    
    

    void Start()
    {
     maxHealth = playerAttributes.lifePlayer;
    
     healthBar.fillAmount = 1;
    }

    public float GetHealthAsFraction() {
        return Mathf.Clamp01((float) playerAttributes.lifePlayer / maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = GetHealthAsFraction();
        if(playerAttributes.manaPlayer == 3){
            foreach (var item in manaBar)
            {
                item.enabled = true;
            }
        }else if(playerAttributes.manaPlayer == 2){
            manaBar[0].enabled = false;
            manaBar[1].enabled = true;
            manaBar[2].enabled = true;
        }else if(playerAttributes.manaPlayer == 1){
            manaBar[0].enabled = false;
            manaBar[1].enabled = false;
            manaBar[2].enabled = true;
        }else if(playerAttributes.manaPlayer == 0){
            foreach (var item in manaBar)
            {
                item.enabled = false;
            }
        }
    }
}
