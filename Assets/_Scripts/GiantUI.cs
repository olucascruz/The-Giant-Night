using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiantUI : MonoBehaviour
{

    [SerializeField] private GiantAttributes giantAttributes;
    
    private int maxHealth;
    private int currentHealth;
    
    [SerializeField] private Image healthBar;
    
    private GameObject player;
    [SerializeField] private GameObject giant;

    private Enemy enemy;

    private int life;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = giant.GetComponent<Enemy>();
        player = GameObject.FindWithTag("Player");
        maxHealth = giantAttributes.lifeGiant;    
        healthBar.fillAmount = 1;   
    }

    public float GetHealthAsFraction() {
        return Mathf.Clamp01((float) life / maxHealth);
    }


    // Update is called once per frame
    void Update()
    {   
        life = enemy.EnemyLife;
        transform.LookAt(player.transform);
        healthBar.fillAmount = GetHealthAsFraction();
    }
}
