using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform wayPointA;
    public Transform wayPointB;
    public float movementSpeed = 2f;
    private Animator animator;
    private bool isWalking = false;
    public int enemyHealth = 50;
    public float attackInterval = 2f;

    private Transform currentTarget;
    private Rigidbody2D rb;
    private Vector3 scale;
    private Coroutine attackCoroutine;

    public float fadeDuration = 1f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = wayPointA;
        scale = transform.localScale;
        Debug.Log($"life do inimigo: {enemyHealth}");
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ZoneAttack"))
        {
            Debug.Log("Inimigo entrou na zona de ataque");
        }

        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerController>();
        }

        if (player != null)
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(AttackPlayer(player));
            }
        } else
        {
            Debug.LogWarning("PlayerController não encontrado no objeto com a tag ZoneAttack");
        }
    }

    private IEnumerator AttackPlayer(PlayerController player)
    {
        while (true)
        {
            player.TakeDamage(10);//Valor pode ser alterado conforme sua necessidade
            animator.SetTrigger("Attack");
            Debug.Log("Inimigo atacando...");

            yield return new WaitForSeconds(attackInterval);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ZoneAttack"))
        {
            Debug.Log("Inimigo saiu da zona de ataque");

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 curTargetHorizontal = new Vector2(currentTarget.position.x, transform.position.y);
        Vector2 direction = (curTargetHorizontal - transform.position).normalized;

        transform.position += (Vector3)direction * movementSpeed * Time.deltaTime;

        if (Vector2.Distance(curTargetHorizontal, transform.position) <= 0.2f)
        {
            SwitchTarget();
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        isWalking = (Vector2.Distance(transform.position, currentTarget.position) > 0.1f);
        animator.SetBool("isWalking", isWalking);
    }

    private void SwitchTarget()
    {
        if (currentTarget == wayPointA)
        {
            currentTarget = wayPointB;
            Flip();
        } else
        {
            currentTarget = wayPointA;
            transform.localScale = scale;
        }
    }

    private void Flip()
    {
        Vector3 flippedScale = scale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;
    }
}
