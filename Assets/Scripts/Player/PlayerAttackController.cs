using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField]
    private LayerMask damagableObject;

    [SerializeField]
    private bool enableAttack = true;
    [SerializeField]
    private Transform attackBox;
    [SerializeField]
    private float attackBoxRadius = 0.8f;
    [SerializeField]
    private float attackDamage = 10;
    [SerializeField]
    private float immuneTime = 0.2f;

    private Animator anim;
    private PlayerController pc;

    private float lastImmuneTime = Mathf.NegativeInfinity;
    private bool attackRequest = false;
    private bool isAttacking = false;
    private bool isImmune = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        CheckAttackInput();
        PerformAttack();
        RemoveImmunity();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackBox.position, attackBoxRadius);
    }

    private void CheckAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (enableAttack) {
                attackRequest = true;
            }
        }
    }

    private void PerformAttack()
    {
        if (attackRequest && !isAttacking) {
            attackRequest = false;
            isAttacking = true;
            anim.SetBool("isAttacking", true);
        } 
    }

    private void FinishAttack()
    {
        isAttacking = false;
        attackRequest = false;
        anim.SetBool("isAttacking", false);
    }

    private void CheckAttackObjects()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackBox.position, attackBoxRadius, damagableObject);

        foreach (Collider2D hitObject in hitObjects) {
            hitObject.SendMessage("TakeDamage", attackDamage);
        }
    }

    private void TakeDamage(float damage)
    {
        if (!isImmune) {
            PlayerController pc = GetComponent<PlayerController>();
            pc.playerHealth -= damage;
            isImmune = true;
            lastImmuneTime = Time.time;
        }
    }

    private void RemoveImmunity()
    {
        if (isImmune) {
            if (Time.time > lastImmuneTime + immuneTime) {
                isImmune = false;
            }
        }
    }

}
