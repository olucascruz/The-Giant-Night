using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxEnemys;
    public List<GameObject> enemys;
    public static SpawnSystem ss;

    // Start is called before the first frame update
    void Awake(){
        if (ss == null){
            ss = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        enemys = new List<GameObject>();
        StartCoroutine(InitialInstanciete());
    }

    private IEnumerator InitialInstanciete(){
        for(int i = 0; i< maxEnemys; i++){
            GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
            enemy.SetActive(false);
            enemys.Add(enemy);
            yield return new WaitForSeconds(0.05f);
        }

    }

    public void Spawner(int max, float time){
        if(enemys.Count == maxEnemys){
            StartCoroutine(CoroutineSpawner(max, time));
        }
    } 

    private IEnumerator CoroutineSpawner(int max, float time){
        if(max >= enemys.Count) max = enemys.Count; 
        for(int i = 0; i < max; i++){
            enemys[i].SetActive(true);
            yield return new WaitForSeconds(time);
        }
}
    
}
