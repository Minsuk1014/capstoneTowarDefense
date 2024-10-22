using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackRange = 3.0f;  // 공격 범위
    public float attackPower = 20f;   // 공격력
    public float attackCooldown = 2f; // 공격 쿨다운 시간
    private float lastAttackTime = 0f; // 마지막 공격 시간

    private GameObject targetMonster;  // 공격할 몬스터
    private bool isAttacking = false;  // 공격 중인지 여부
    private Animator animator;         // 애니메이션 컨트롤러 (있다면)

    void Awake()
    {
        animator = GetComponent<Animator>(); // 타워에 애니메이터가 있을 경우
    }

    void Update()
    {
        // 타겟 몬스터가 있고, 공격 쿨다운이 끝났으면 공격
        if (targetMonster != null && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackMonster();
        }
    }

    // 몬스터를 공격하는 함수
    void AttackMonster()
    {
        if (targetMonster == null) return;  // 타겟이 없으면 공격하지 않음

        isAttacking = true; // 공격 중 상태 설정
        if (animator != null)
        {
            animator.SetTrigger("Stand");
            animator.SetTrigger("Attack"); // 공격 애니메이션 실행
        }

        // 몬스터에 데미지를 줌
        Monster monsterScript = targetMonster.GetComponent<Monster>();
        if (monsterScript != null)
        {
            monsterScript.TakeDamage(attackPower);
            Debug.Log("Attacked monster: " + monsterScript.name + " for " + attackPower + " damage.");
        }

        lastAttackTime = Time.time; // 마지막 공격 시간 갱신
        isAttacking = false; // 공격 종료
    }

    // Trigger 범위 내로 몬스터가 들어왔을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("Monster entered tower range.");
            targetMonster = other.gameObject; // 타겟 몬스터 설정
        }
    }

    // Trigger 범위 밖으로 몬스터가 나갔을 때
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("Monster left tower range.");
            targetMonster = null; // 타겟을 비움
        }
    }
}
