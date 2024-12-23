using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private float moveInput;
    private Animator animator;
    private bool isRunning = false;
    private bool isJumping = false;
    public int playerHealth = 100;

    public Slider lifeSlider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("inDamage", false);
        Debug.Log("Life do Player: " + playerHealth);
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            isRunning = true;
            animator.SetBool("isJumping", false);
        }
        else
        {
            isRunning = false;
        }

        animator.SetBool("isRunning", isRunning);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Jump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }

        lifeSlider.value = playerHealth * 0.01f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            Debug.Log("PowerUp");
            {
                Debug.Log("Player apanhou power up");
                if (playerHealth < 100)
                {
                    playerHealth += 10;
                } else
                {
                    Debug.Log("Voc� j� tem o n�mero m�ximo de Health");
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (Mathf.Abs(rb.velocity.y) > 0.02f)
        {
            isJumping = true;
            animator.SetBool("isJumping", true);
        }
        else
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit.collider != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        animator.SetBool("inDamage", true);
        Debug.Log("Player tomou " + damage + " de dano. Sa�de restante: " + playerHealth);

        StartCoroutine(ResetDamageAnimation());

        if (playerHealth <= 0)
        {
            Debug.Log("Player Morreu!");
            SceneManager.LoadScene(2);
        }
    }

    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(1F);
        animator.SetBool("inDamage", false);
    }
}
