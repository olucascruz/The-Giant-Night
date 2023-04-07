using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class SkinAnimation : MonoBehaviour
{
    private Player player;
    [SerializeField] private Animator anim;
    [SerializeField] private VisualEffect VfxEletricAttack;

    public delegate void SkinPlayerEventHandler();
    public static event SkinPlayerEventHandler onUseElectricity;
    public static event SkinPlayerEventHandler onEndUseElectricity;


    void Start()
    {
        player = FindObjectOfType<Player>();
        player.onMoveStart += OnPlayerMoveStart;
        player.onMoveStop += OnPlayerMoveStop;
        player.onAttack += OnPlayerAttack;
        VfxEletricAttack.Stop();
    }

    void OnPlayerMoveStart()
    {
        // o jogador começou a se mover
        anim.SetBool("isMove", true);
    }

    void OnPlayerMoveStop()
    {
        // o jogador parou de se mover
        anim.SetBool("isMove", false);

    }

    void OnPlayerAttack(){
        anim.SetTrigger("attack");
        StartCoroutine(ShowPower());
    }

    IEnumerator ShowPower(){
        yield return new WaitForSeconds(1f);
        onUseElectricity?.Invoke();
        VfxEletricAttack.Play();
        yield return new WaitForSeconds(2f);
        onEndUseElectricity?.Invoke();
        VfxEletricAttack.Stop();
    }

    void OnDestroy()
    {
        player.onMoveStart -= OnPlayerMoveStart;
        player.onMoveStop -= OnPlayerMoveStop;
        player.onAttack -= OnPlayerAttack;

    }
}
