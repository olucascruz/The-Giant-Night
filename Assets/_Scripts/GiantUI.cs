using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiantUI : MonoBehaviour
{

    [SerializeField]
    private GiantAttributes giantAttributes;
    
    private int maxHealth;
    private int currentHealth;
    
    [SerializeField] private Image healthBar;
    
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        maxHealth = giantAttributes.lifeGiant;    
        healthBar.fillAmount = 1;   
    }

    public float GetHealthAsFraction() {
        return Mathf.Clamp01((float) giantAttributes.lifeGiant / maxHealth);
    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        healthBar.fillAmount = GetHealthAsFraction();
    }
}
