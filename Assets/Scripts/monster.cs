// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class monster : MonoBehaviour
// {
//     public float speed = 2.0f;           // 이동 속도 (기본값 2.0)
//     public Rigidbody2D target;           // 목표 타겟
//     Rigidbody2D rigid;                   // Rigidbody2D 컴포넌트
//     bool isLive = true;                  // 몬스터의 생존 여부
//     Animator animator;                   // 애니메이터 컴포넌트

//     // 이동할 좌표(waypoints)
//     Vector2[] waypoints = new Vector2[4] 
//     {
//         new Vector2(-7, -3.5f),   // 첫 번째 지점
//         new Vector2(-7, 2.5f),    // 두 번째 지점
//         new Vector2(7, 2.5f),     // 세 번째 지점
//         new Vector2(7, -3.5f)     // 네 번째 지점
//     };

//     int currentWaypointIndex = 0;        // 현재 목표 좌표 인덱스

//     void Awake()
//     {
//         rigid = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//     }

//     void FixedUpdate() // 타겟 이동
//     {
//         if (isLive)
//         {
//             animator.SetTrigger("1_Move");
//             // 현재 목표 지점으로 이동
//             Vector2 targetPosition = waypoints[currentWaypointIndex]; // 타켓 위치
//             Vector2 currentPosition = rigid.position; // 내 위치
//             Vector2 direction = (targetPosition - currentPosition).normalized; // 벡터화
//             Vector2 nextVec = direction * speed * Time.fixedDeltaTime; // 시간따라서 이동

//             // 이동
//             rigid.MovePosition(currentPosition + nextVec);

//             // 목표 지점에 도달했는지 체크 (거리가 매우 짧은지 확인)
//             if (Vector2.Distance(currentPosition, targetPosition) < 0.05f)
//             {
//                 currentWaypointIndex++;  // 다음 목표로 이동
//                 if (currentWaypointIndex >= waypoints.Length)
//                 {
//                     currentWaypointIndex = 0;  // 다시 처음 지점으로 돌아감
//                 }
//             }
//         }
//         else
//         {
//             // 몬스터가 죽으면 사망 애니메이션 재생
//             animator.SetTrigger("4_Death");
//         }
//     }

//     void LateUpdate() // 방향 전환을 위한 코드
//     {
//         Vector2 targetPosition = waypoints[currentWaypointIndex];

//         // 목표 지점의 X좌표에 따라 몬스터의 방향을 전환
//         if (transform.position.x < -6 && transform.position.y < 0)
//             transform.localScale = new Vector3(-1, 1, 1);  // 오른쪽
//         else if(transform.position.x > 6 && transform.position.y > 2)
//             transform.localScale = new Vector3(1, 1, 1); // 왼쪽
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster : MonoBehaviour
{
    public float speed = 2.0f;           // 이동 속도 (기본값 2.0)
    public float maxHealth = 100f;       // 몬스터의 최대 체력
    private float currentHealth;         // 현재 체력
    public Rigidbody2D target;           // 목표 타겟
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
        currentHealth = maxHealth;        // 처음에는 체력이 최대치
    }

    void FixedUpdate() // 타겟 이동
    {
        if (isLive)
        {
            animator.SetTrigger("1_Move");
            // 현재 목표 지점으로 이동
            Vector2 targetPosition = waypoints[currentWaypointIndex]; // 타켓 위치
            Vector2 currentPosition = rigid.position; // 내 위치
            Vector2 direction = (targetPosition - currentPosition).normalized; // 벡터화
            Vector2 nextVec = direction * speed * Time.fixedDeltaTime; // 시간따라서 이동

            // 이동
            rigid.MovePosition(currentPosition + nextVec);

            // 목표 지점에 도달했는지 체크 (거리가 매우 짧은지 확인)
            if (Vector2.Distance(currentPosition, targetPosition) < 0.05f)
            {
                currentWaypointIndex++;  // 다음 목표로 이동
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;  // 다시 처음 지점으로 돌아감
                }
            }
        }
        else
        {
            // 몬스터가 죽으면 사망 애니메이션 재생
            animator.SetTrigger("4_Death");
        }
    }

    // 영웅의 공격을 받으면 체력이 줄어드는 함수
    public void TakeDamage(float damageAmount)
    {
        if (!isLive) return;

        currentHealth -= damageAmount;    // 데미지만큼 체력 감소
        Debug.Log("남은 체력: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();                        // 체력이 0 이하가 되면 죽음 처리
        }
    }

    // 몬스터와 충돌하거나 트리거로 영웅의 공격을 받았을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hero")) // 태그로 영웅인지 확인
        {
            // 여기에 데미지를 받는 로직을 넣을 수 있음.
            TakeDamage(10f); // 예시로 10만큼의 데미지를 받음
        }
    }

    void Die()
    {
        isLive = false;
        animator.SetTrigger("4_Death");   // 사망 애니메이션 트리거
        
    }

    void LateUpdate() // 방향 전환을 위한 코드
    {
        Vector2 targetPosition = waypoints[currentWaypointIndex];

        // 목표 지점의 X좌표에 따라 몬스터의 방향을 전환
        if (transform.position.x < -6 && transform.position.y < 0)
            transform.localScale = new Vector3(-1, 1, 1);  // 오른쪽
        else if (transform.position.x > 6 && transform.position.y > 2)
            transform.localScale = new Vector3(1, 1, 1); // 왼쪽
    }


}
