using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] float velocity;
    [SerializeField] float jumpForce;
    [SerializeField] float groundDrag;

    [Header("Attack")]
    [SerializeField] GameObject attackObject;


    [Header("Attributes")]
    [SerializeField]
    private PlayerAttributes playerAttributes;

    public event Action onMoveStart;
    public event Action onMoveStop;
    public event Action onAttack;

    private Vector3 dir;

    private bool isGround;

    private int playerLife = 100;

    private bool canAttack = true;

    private GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameController.gc;
        playerAttributes.lifePlayer = playerLife;
        playerAttributes.manaPlayer = 3;
        Giant.onAttackPlayer += TakeDamage; 
        rb = this.GetComponent<Rigidbody>();
    }
    private void Update() {
        dir = this.transform.TransformVector(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical") ).normalized);
        SpeedControl();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gc.gameState != GameController.GameState.PAUSE)
        {
            Move();
            Jump();
            Attack();
        }
    }

    void Jump(){
        if(Input.GetKey(KeyCode.Space) && isGround){
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Move()
    {
        if(canAttack) rb.AddForce(dir.normalized * velocity * 10f, ForceMode.Force);
        if (!Vector3.Equals(dir, Vector3.zero))
        {
            onMoveStart?.Invoke();
            
        }
        else
        {
            onMoveStop?.Invoke();
            
        }
        
    }

    void SpeedControl(){
        if(isGround){
            rb.drag = groundDrag;
        }else{
            rb.drag = 0;
        }
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > velocity){
            Vector3 limitVel = flatVel.normalized * velocity;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    void Attack(){
        if(Input.GetMouseButtonDown(0) &&
            isGround &&
            canAttack &&
            playerAttributes.manaPlayer > 0){
            
            canAttack = false;
            UseMana();
            onAttack?.Invoke();
            StartCoroutine(ActiveAtackArea());
        }
    }

    IEnumerator ActiveAtackArea(){
        yield return new WaitForSeconds(1f);
        attackObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        attackObject.SetActive(false);
        canAttack = true;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
            isGround = true;
        }
    }
    private void OnCollisionStay(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
            isGround = false;
        }
    }

    private void TakeDamage(){
        if(isGround){
            rb.AddForce(-transform.forward * 5f, ForceMode.Impulse);
            rb.AddForce(transform.up * 1.5f, ForceMode.Impulse);
            StartCoroutine(DecrementLife());
        }
    }
    
    private IEnumerator DecrementLife(){
        if(playerAttributes.lifePlayer > 0)
            playerAttributes.lifePlayer--;
        yield return new WaitForSeconds(0.2f);
    }

    private void UseMana(){
        if(playerAttributes.manaPlayer > 0)
            playerAttributes.manaPlayer--;
    }
}
