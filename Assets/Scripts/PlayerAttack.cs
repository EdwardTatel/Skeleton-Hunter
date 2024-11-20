using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator; 
    [SerializeField]  private bool isAttacking;
    private CharacterMovement moveScript;
    [SerializeField]  private bool canAttackAgain = true;
    [SerializeField] private int hp = 5;
    [SerializeField] GameOver gameOver;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set { IsAttacking = value; }
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        moveScript = GetComponentInChildren<CharacterMovement>();
    }
    void Update()
    {
        TextManager.hp = hp;
        if (hp <= 0)
        {
            GameOver.gameOver = true;
            hp = 5;
        }
        if (Input.GetKey(KeyCode.Space) && !isAttacking && !moveScript.IsRolling && canAttackAgain)
        {
            isAttacking = true;
            canAttackAgain = false;
            animator.SetTrigger("Attack");
            StartCoroutine(DisableAttack());
        }
    }
    IEnumerator DisableAttack()
    {
        yield return new WaitForSeconds(.60f);

        isAttacking = false;
        yield return new WaitForSeconds(1f);
        canAttackAgain = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {

            if (other.transform.parent.CompareTag("Enemy") && other is BoxCollider)
            {
                animator.SetTrigger("Hit");
                hp--;
                KnockbackAndJumpAway(other.transform.parent.position);
            }
        }
    }

    void KnockbackAndJumpAway(Vector3 playerPosition)
    {
        Vector3 knockbackDirection = transform.position - playerPosition;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(knockbackDirection.normalized * 4f, ForceMode.Impulse);

    }

}
