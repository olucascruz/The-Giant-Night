using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [ SerializeField ] private int enemyLife;
    public int EnemyLife { get { return enemyLife; } }
    [ SerializeField ] private Renderer enemyRenderer;
    private bool canTakeDamage = true;

    private GameController gc;
    [Header("Attributes")]
    [SerializeField]
    private GiantAttributes giantAttributes;

    private Vector3 startPos; 
    
   
    void Start(){
        Material originalMaterial = enemyRenderer.material;
        Material myMaterial = Instantiate(originalMaterial);
        enemyRenderer.material = myMaterial;

        gc = GameController.gc;
        enemyRenderer.material.SetFloat("_DissolveAmount", 0f);
        enemyLife = giantAttributes.lifeGiant;
        startPos = this.gameObject.transform.position;

    }

    private void OnEnable() {
        enemyLife = giantAttributes.lifeGiant;
        canTakeDamage = true;
    }


   void OnTriggerEnter(Collider other) 
    {
        
        if (other.gameObject.tag == "PlayerAttack")
        {
            if(canTakeDamage){
                StartCoroutine(TakeDamage());
            }
        }
    }

    void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "PlayerAttack")
        {
            if(canTakeDamage)
            {
                StartCoroutine(TakeDamage());
            }
        }
    }

    private IEnumerator Disappear(){
            float materialDisappear = 0f;
            while(materialDisappear < 1f){
                enemyRenderer.material.SetFloat("_DissolveAmount", materialDisappear);
                yield return new WaitForSeconds(0.05f);
                materialDisappear += 0.1f;
            }

            yield return new WaitForSeconds(0.5f);
            Death();
    }

    private IEnumerator TakeDamage(){
        canTakeDamage = false;
        if(enemyLife > 0)
            enemyLife--;

        if(enemyLife == 0)
            StartCoroutine(Disappear());

        yield return new WaitForSeconds(0.2f);
        canTakeDamage = true;
    }

    void Death(){
        gc.enemiesDied++;
        gameObject.SetActive(false);
        enemyRenderer.material.SetFloat("_DissolveAmount", 0f);
        transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
    }   
}
