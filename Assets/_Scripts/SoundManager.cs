using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private AudioSource heavyImpact;
    [SerializeField] private AudioSource stepsGiant;
    [SerializeField] private AudioSource backgroundSound;
    [SerializeField] private AudioSource electricitySound;

    private GameController gc;

    void Start()
    {   
        gc = GameController.gc;
        SkinAnimation.onUseElectricity += ElectricityPlay;
        SkinAnimation.onEndUseElectricity += ElectricityStop;
        Sphere.onSoundElectricitySphere += ElectricityPlay;
        Sphere.onEndSoundElectricitySphere += ElectricityStop;
        Giant.onStepGiant += StepsPlay;
        Giant.onHeavyImpact += HeavyImpactPlay; 
        BackgroundSoundPlay();
    }

    void Update(){
        if (gc.gameState == GameController.GameState.PAUSE)
        {
            heavyImpact.Pause();
            stepsGiant.Pause();
            electricitySound.Pause();
            backgroundSound.Pause();
        }
        else
        {
            heavyImpact.UnPause();
            stepsGiant.UnPause();
            electricitySound.UnPause();
            backgroundSound.UnPause();
        }
    
    }

    public void HeavyImpactPlay(){
        PlaySoundEffect(heavyImpact);
    }

    public void StepsPlay(){
        PlaySoundEffect(stepsGiant);
    }
    
    public void BackgroundSoundPlay(){
        PlaySoundEffect(backgroundSound);
    }

    public void ElectricityPlay(){
        PlaySoundEffect(electricitySound);
    }

    public void ElectricityStop(){
        StopSoundEffect(electricitySound);
    }

    private void PlaySoundEffect(AudioSource SFX){
        
            SFX.Play();
    }

    private void StopSoundEffect(AudioSource SFX){
        SFX.Stop();
    }

    private void OnDestroy() {
        SkinAnimation.onUseElectricity -= ElectricityPlay;

        SkinAnimation.onEndUseElectricity -= ElectricityStop;

        Sphere.onSoundElectricitySphere -= ElectricityPlay;

        Sphere.onEndSoundElectricitySphere -= ElectricityStop;

        Giant.onStepGiant -= StepsPlay;
        
        Giant.onHeavyImpact -= HeavyImpactPlay;
    }
}
