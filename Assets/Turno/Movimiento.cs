using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;

    Rigidbody2D rigidbodyTopDown;
    Animator animator;

    bool isAttacking;

    private void Awake()
    {
        transform.position = DataInstance.Instance.playerPosition;
    }
    private void Start()
    {
        rigidbodyTopDown = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rigidbodyTopDown.linearVelocity = direction * speed;
    }

    private void Update()
    {
        Movement();
        animaciones();
    }

    private void Movement()
    {
        if (isAttacking) return;
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if( Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Attack");
            isAttacking = true;
            direction = Vector2.zero;


        }
    }

    private void animaciones()
    {
        if (isAttacking) return; 


        if (direction.magnitude != 0)
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
            animator.Play("Run");
        }
        else animator.Play("Idle tree");
    }
    private void EndAttacking()
    {
        isAttacking = false;
    }
}
