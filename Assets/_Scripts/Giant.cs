using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AI;


public class Giant : MonoBehaviour
{       
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

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackVFX.Stop();
    }

    private void Start() {
        otherCollider =  GameObject.FindGameObjectWithTag("FrontWall").GetComponent<Collider>();
        Physics.IgnoreCollision(GetComponent<Collider>(), otherCollider);
        player =  GameObject.FindGameObjectWithTag("Player").transform;
        target = GameObject.FindGameObjectsWithTag("EnemyTarget");
        targetSelected = Random.Range(0, 3);    
    }

    void Update()
    {
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
        if(!walkPointSet){
            SearchWalkPoint();
        }

        if(walkPointSet){            
            agent.isStopped = false;
            agent.SetDestination(walkPoint);
            anim.SetBool("walking", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 2f){
            AttackPlayer();
        }

    }

    private void SearchWalkPoint()
    {
        
        if(target.Length > 1){
            walkPoint = target[targetSelected].transform.position;
            print("SearchWalkPoint is something");
            Debug.Log(walkPoint);

            walkPointSet = true;
        }
    }

    private void ChasePlayer(){
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetBool("walking", true);

    }

    private void AttackPlayer(){
        Vector3 distanceToPlayer = transform.position - player.position; 
        
        if(!alreadyAttacked)
        {
            alreadyAttacked = true;
            if(distanceToPlayer.magnitude <  attackRange - 1f){
            agent.isStopped = true;
            }   
            anim.SetTrigger("attack");
            StartCoroutine(Attack());
            
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }

    IEnumerator Attack(){
        attackArea = 0.1f;
        while(attackArea < attackRange){
            yield return new WaitForSeconds(0.2f);
            SetAttackSize(attackArea);
            yield return new WaitForSeconds(0.05f);
            attackArea += 0.4f;
        }
        
        if(attackArea >= attackRange){
            attackVFX.Stop();
            attackArea = 0.1f;
            SetAttackSize(attackArea);
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
