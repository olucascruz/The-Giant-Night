using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Player");
        }
    }
}
