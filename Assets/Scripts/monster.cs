using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed = 2.0f;           // 이동 속도
    public float maxHealth = 100f;       // 몬스터의 최대 체력
    private float currentHealth;         // 몬스터의 현재 체력
    public Rigidbody2D target;           // 목표 타겟 (사용 중이 아니므로 제거 가능)
    Rigidbody2D rigid;                   // Rigidbody2D 컴포넌트
    bool isLive = true;                  // 몬스터의 생존 여부
    Animator animator;                   // 애니메이터 컴포넌트

    // 이동할 좌표(waypoints)
    Vector2[] waypoints = new Vector2[4] 
    {
        new Vector2(-7, -3.5f),   // 첫 번째 지점
        new Vector2(-7, 2.5f),    // 두 번째 지점
        new Vector2(7, 2.5f),     // 세 번째 지점
        new Vector2(7, -3.5f)     // 네 번째 지점
    };

    int currentWaypointIndex = 0;        // 현재 목표 좌표 인덱스

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;       // 초기 체력을 최대 체력으로 설정
    }

    void FixedUpdate() // 타겟 이동
    {
        if (isLive)
        {
            MoveMonster();  // 이동 처리
        }
    }

    // 몬스터 이동 처리 함수
    void MoveMonster()
    {
        animator.SetTrigger("1_Move");

        // 현재 목표 지점으로 이동
        Vector2 targetPosition = waypoints[currentWaypointIndex]; // 타겟 위치
        Vector2 currentPosition = rigid.position; // 몬스터의 현재 위치
        Vector2 direction = (targetPosition - currentPosition).normalized; // 벡터화
        Vector2 nextVec = direction * speed * Time.fixedDeltaTime; // 속도에 따른 이동

        // 이동
        rigid.MovePosition(currentPosition + nextVec);

        // 목표 지점에 도달했는지 확인 (거리가 매우 짧은지 확인)
        if (Vector2.Distance(currentPosition, targetPosition) < 0.05f)
        {
            currentWaypointIndex++;  // 다음 목표로 이동
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;  // 처음으로 돌아감
            }
        }
    }

    // LateUpdate는 방향 전환용으로 사용
    void LateUpdate() 
    {
        UpdateDirection();  // 방향 전환 처리
    }

    // 방향 전환 처리 함수
    void UpdateDirection()
    {
        Vector2 targetPosition = waypoints[currentWaypointIndex];
        
        if (transform.position.x < -6 && transform.position.y < 0)
            transform.localScale = new Vector3(-1, 1, 1);  // 왼쪽으로 방향 전환
        else if (transform.position.x > 6 && transform.position.y > 2)
            transform.localScale = new Vector3(1, 1, 1);  // 오른쪽으로 방향 전환
    }

    // 몬스터가 데미지를 받는 함수
    public void TakeDamage(float damage)
    {
        if (!isLive) return;  // 이미 죽은 몬스터는 데미지를 받지 않음

        currentHealth -= damage; // 체력 감소
        Debug.Log("Monster took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하일 경우 사망 처리
        }
    }

    // 몬스터 사망 처리 함수
    void Die()
    {
        if (!isLive) return;

        isLive = false;  // 생존 상태를 false로 설정
        animator.SetTrigger("4_Death");  // 사망 애니메이션 실행
        Debug.Log("Monster has died.");
        
        // 몬스터 비활성화 처리는 애니메이션이 끝난 후 실행
        Invoke("DisableMonster", 1.5f);  // 애니메이션 시간만큼 딜레이 후 비활성화
    }

    // 몬스터 비활성화 함수
    void DisableMonster()
    {
        gameObject.SetActive(false);  // 오브젝트 비활성화
    }
}
