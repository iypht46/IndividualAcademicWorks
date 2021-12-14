using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] TPSMovement m_movement;
    [SerializeField] LayerMask m_EnemyLayer;

    CombatSystem m_combatSystem;

    Animator m_animator;

    bool m_isContinueAttack = false;

    int m_Combo = 0;

    [SerializeField] Collider m_shieldCollider;

    void Start()
    {
        m_animator = gameObject.GetComponent<Animator>();
        m_combatSystem = gameObject.GetComponent<CombatSystem>();
    }

    void Update()
    {
        m_movement.m_isAttacking = m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_2");
        m_movement.m_isBlocking = m_animator.GetCurrentAnimatorStateInfo(0).IsName("ShieldHold");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!m_movement.m_isAttacking && context.performed)
        {
            m_movement.LookAtCameraAngle();
            m_animator.SetTrigger("Attack_1");
            m_combatSystem.m_AcceptingInput = false;
            m_isContinueAttack = false;
            m_Combo = 1;
        }
        else if (m_combatSystem.m_AcceptingInput && context.performed && !m_isContinueAttack)
        {
            //movement.LookAtCameraAngle();

            switch (m_Combo)
            {
                case 0: m_animator.SetTrigger("Attack_1"); break;
                case 1: m_animator.SetTrigger("Attack_2"); break;
                default: break;
            }

            m_Combo++;

            if (m_Combo > 1)
            {
                m_Combo = 0;
            }

            m_isContinueAttack = true;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_movement.LookAtCameraAngle();

            m_animator.SetTrigger("Block");
            m_animator.SetBool("isBlocking", true);

            m_combatSystem.isBlock = true;

            m_shieldCollider.enabled = true;
        }
        else if (context.canceled)
        {
            m_animator.SetBool("isBlocking", false);

            m_combatSystem.isBlock = false;
            m_shieldCollider.enabled = false;
        }
    }
}
