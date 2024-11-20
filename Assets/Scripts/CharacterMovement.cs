using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Animator animator; 
    private Rigidbody rb;
    public float rollForce = 10f;
    private PlayerAttack attackScript;
    [SerializeField] private bool isRolling;
    private float horizontalInput;
    private float verticalInput;
    public bool IsRolling
    {
        get { return isRolling; }
        set { isRolling = value; }
    }

    private bool isAnimationRunning;

    void Start()
    {
        attackScript = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.LeftShift) && !attackScript.IsAttacking)
        {
            // Roll in direction
            Vector3 rollDirection = new Vector3(horizontalInput, 0f, verticalInput);
            rollDirection.Normalize();

            animator.SetTrigger("Roll");
            isRolling = true;
            if (rollDirection != Vector3.zero)
            {
                rb.velocity = rollDirection * rollForce;
            }
            // Roll to right
            else
            {
                float rollXDirection = transform.localScale.x > 0 ? 1 : -1;
                rb.velocity = new Vector3(rollXDirection * rollForce, 0f, 0f);
            }
            StartCoroutine(DisableRoll());
        }
    }

    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (!isAnimationRunning && !attackScript.IsAttacking)
        {
            
            

            Vector3 currentScale = transform.localScale;
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(transform.position + movement);
            // Flip sprite renderer
            if (horizontalInput < 0)
                currentScale.x = Mathf.Abs(currentScale.x) * -1;
            else if (horizontalInput > 0)
                currentScale.x = Mathf.Abs(currentScale.x);

            if(!attackScript.IsAttacking && !isRolling) transform.localScale = currentScale;
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
    }

    IEnumerator DisableRoll()
    {
        yield return new WaitForSeconds(.85f);

        isRolling = false;
    }


}