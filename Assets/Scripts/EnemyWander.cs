using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    public float wanderRadius = 5f;
    public float wanderTimer = 3f;
    public float chaseRadius = 10f;
    public float attackRadius = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private float timer;
    private bool isFacingRight = true;
    private bool canAttackAgain;
    private int hp = 1;
    private GameObject gameSceneManager;

    void Start()
    {
        gameSceneManager = GameObject.Find("GameSceneManager");
        gameSceneManager.GetComponent<SpawnEnemies>().EnemyCount++;
        canAttackAgain = true;
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;

        animator = GetComponentInChildren<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        timer = wanderTimer;
    }

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        if (!animator.GetBool("Attack"))
        {
            WaitForAttackAgain();
        }
        if (agent == null || !agent.isOnNavMesh)
            return;

        /// Check player if in radius
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius && !animator.GetBool("Attack") && canAttackAgain && !animator.GetBool("isHurt"))
        {
            
            if (player.position.x < transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if (player.position.x > transform.position.x && !isFacingRight)
            {
                Flip();
            }
            animator.SetBool("Attack", true);
            StartCoroutine(WaitForAttackAgain());
            StartCoroutine(DisableAttack());
            return;
        }

        if (!animator.GetBool("Attack") && !animator.GetBool("isHurt"))
        {
            // Check player if in radius
            if (distanceToPlayer <= chaseRadius)
            {

                // Chase player
                agent.SetDestination(player.position);
                animator.SetBool("IsWalking", true);

                if (player.position.x < transform.position.x && isFacingRight)
                {
                    Flip();
                }
                else if (player.position.x > transform.position.x && !isFacingRight)
                {
                    Flip();
                }
            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    Vector3 randomPoint = RandomNavSphere(transform.position, wanderRadius);
                    agent.SetDestination(randomPoint);
                    timer = 0;

                   
                    animator.SetBool("IsWalking", true);

                   
                    if (agent.velocity.x < 0 && isFacingRight)
                    {
                        Flip();
                    }
                    else if (agent.velocity.x > 0 && !isFacingRight)
                    {
                        Flip();
                    }
                }
                else
                {
                    if (agent.velocity.magnitude > 0)
                    {
                        animator.SetBool("IsWalking", true);

                        if (agent.velocity.x < 0 && isFacingRight)
                        {
                            Flip();
                        }
                        else if (agent.velocity.x > 0 && !isFacingRight)
                        {
                            Flip();
                        }
                    }
                    else
                    {
                        animator.SetBool("IsWalking", false);
                    }
                }
            }

        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas))
        {
            return navHit.position;
        }
        
        return origin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null) {

            if (other.transform.parent.CompareTag("Player") && other is BoxCollider)
            {
                TextManager.score++;
                hp--;
                gameSceneManager.GetComponent<SpawnEnemies>().EnemyCount--;
                animator.SetBool("isHurt", true);
                animator.SetBool("Attack", false);
                StartCoroutine(DisableHurt());
                StartCoroutine(WaitForAttackAgain());

                KnockbackAndJumpAway(other.transform.parent.position);
            }
        }
        
    }

    IEnumerator DisableHurt()
    {
        yield return new WaitForSeconds(0.28f);

        animator.SetBool("isHurt", false);
    }
    IEnumerator DisableAttack()
    {
        
        yield return new WaitForSeconds(2f);
        animator.SetBool("Attack", false);
    }

    IEnumerator WaitForAttackAgain()
    {

        yield return new WaitForSeconds(1f);
        canAttackAgain = true;
    }

    void KnockbackAndJumpAway(Vector3 playerPosition)
    {
        Vector3 knockbackDirection = transform.position - playerPosition;


        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(knockbackDirection.normalized * 4f, ForceMode.Impulse);

    }
}