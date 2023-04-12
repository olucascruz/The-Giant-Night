using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;


public class Giant : MonoBehaviour
{
    public enum GiantState {RUNNING, CHASEPLAYER, ATTACKING};
    public GiantState giantState = GiantState.RUNNING;



    [Header("Movement")]
    private NavMeshAgent agent;
    private GameObject[] target;
    private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    private Vector3 walkPoint; 
    private bool walkPointSet = false;
    [SerializeField] private float walkPointRange;
    [Header("Attack")]
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked = false;
    [SerializeField] float sighRange, attackRange;
    [SerializeField] VisualEffect attackVFX;
    [Header("State")]
    [SerializeField] bool playerInSighRange, playerInAttackRange;
    [SerializeField] private Animator anim;
    private Collider otherCollider;
    private bool attackCollidingPlayer;
    public delegate void EventHandler();
    public static event EventHandler onAttackPlayer;
    public static event EventHandler onStepGiant;
    public static event EventHandler onHeavyImpact;
    private int targetSelected;
    private float attackArea;

    private GameController gc;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackVFX.Stop();
    }

    private void Start() {
        gc = GameController.gc;
        otherCollider =  GameObject.FindGameObjectWithTag("FrontWall").GetComponent<Collider>();
        Physics.IgnoreCollision(GetComponent<Collider>(), otherCollider);
        player =  GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.FindGameObjectsWithTag("EnemyTarget");
        targetSelected = Random.Range(0, 3);
        agent.stoppingDistance = 1.2f;
    }

    void Update()
    {
        if(gc){
            if(gc.gameState == GameController.GameState.GAMEOVER) return;
        }
       playerInSighRange = Physics.CheckSphere(transform.position, sighRange, whatIsPlayer);
       playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

       if(attackCollidingPlayer){
            onAttackPlayer?.Invoke();
        }

       if(!playerInSighRange && !playerInAttackRange) Running();
       else if(playerInSighRange && !playerInAttackRange) ChasePlayer();
       else if(playerInSighRange && playerInAttackRange) AttackPlayer();
    }

    private void Running(){
        giantState = GiantState.RUNNING;
        if(!walkPointSet){
            SearchWalkPoint();
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        if(walkPointSet && distanceToWalkPoint.magnitude > 2f ){            
            agent.isStopped = false;
            agent.SetDestination(walkPoint);
            anim.SetBool("walking", true);
        }


        if(distanceToWalkPoint.magnitude <= 2f){
            agent.isStopped = false;
            anim.SetBool("walking", false);
            AttackPlayer();
        }

    }

    private void SearchWalkPoint()
    {
        
        if(target.Length > 1){
            walkPoint = target[targetSelected].transform.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer(){
        giantState = GiantState.CHASEPLAYER;

        if(agent.remainingDistance <= agent.stoppingDistance){
            agent.isStopped = true;
            anim.SetBool("walking", false);
        }else{
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("walking", true);
        }
    }

    private void AttackPlayer(){
        giantState = GiantState.ATTACKING;
        
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            if(agent.remainingDistance <= agent.stoppingDistance){
                agent.isStopped = true;
                anim.SetBool("walking", false);
            }   
            anim.SetTrigger("attack");
            StartCoroutine(Attack());
            
        }
    }

    IEnumerator ResetAttack()
    {   
        float origialAttackRange = attackRange;
        attackRange = 0.1f;
        yield return new WaitForSeconds(timeBetweenAttacks);
        attackRange = origialAttackRange;
        alreadyAttacked = false;
    }

    IEnumerator Attack(){
        attackArea = 0.1f;
        yield return new WaitForSeconds(0.2f);
        while(attackArea < attackRange){
            yield return new WaitForSeconds(0.2f);
            SetAttackSize(attackArea);
            attackArea += 0.4f;
        }
        
        if(attackArea >= attackRange){
            attackVFX.Stop();
            attackArea = 0.1f;
            SetAttackSize(attackArea);
            giantState = GiantState.CHASEPLAYER;
            StartCoroutine(ResetAttack());
        }
    }

    void SetAttackSize(float attackSize){
        attackVFX.SetFloat("SizeArea", attackSize);
        attackCollidingPlayer = Physics.CheckSphere(transform.position, attackSize, whatIsPlayer);
    }

    //Eventos ativados por animação
    public void SpawnAttack(){
        attackVFX.Play();
    }
    public void Step()
    {
        onStepGiant?.Invoke();
    }
    public void HeavyImpact()
    {
        onHeavyImpact?.Invoke();   
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sighRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackArea);

    }
}
