using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class Sphere : MonoBehaviour
{
    public delegate void SphereEventHandler();
    public static event SphereEventHandler onSoundElectricitySphere;
    public static event SphereEventHandler onEndSoundElectricitySphere;
    [SerializeField] private GameObject objRechargeMana;
    [SerializeField] private VisualEffect vfxEletricRecharge;
    private bool canRecharge = true;
    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
                objRechargeMana.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Mouse1) && canRecharge){
                if(other.gameObject.GetComponent<Player>().RechargeMana != 3){
                    StartCoroutine(EffectRecharge());
                    other.gameObject.GetComponent<Player>().RechargeMana = 3;
                }
            }
        }
    }

    IEnumerator EffectRecharge(){
        canRecharge = false;
        onSoundElectricitySphere?.Invoke();
        vfxEletricRecharge.Play();
        yield return new WaitForSeconds(1f);
        vfxEletricRecharge.Stop();
        onEndSoundElectricitySphere?.Invoke();
        canRecharge = true;
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
                objRechargeMana.SetActive(false);
        }
    }
}
