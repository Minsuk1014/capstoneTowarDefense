// using System.Collections;
// using System.Collections.Generic;
// using System.ComponentModel.Design.Serialization;
// using UnityEngine;
// using UnityEngine.Assertions.Must;
// using UnityEngine.InputSystem;

// public class palyer : MonoBehaviour
// {
//     private Animator animator;
//     public Vector2 inputVec; // 속도와 방향을 제어
//     public float speed = 2;
//     Rigidbody2D rigid; // 터치제어
    
//     void Awake(){
//         animator = GetComponent<Animator>();
//         rigid = GetComponent<Rigidbody2D>(); // 터치 

//     }
    
    
//     void FixedUpdate() // 물리 연산 frame 마다 호출 실행 / 고정된 시간 간격으로 호출
//     {
//         Vector2 nextVec = inputVec*speed*Time.fixedDeltaTime; // 위치 조절 프레임 맞춰서 움직이기 
//         rigid.MovePosition(rigid.position + nextVec);
//     }

//     void OnMove(InputValue value) //사용하는 함수 이름 적으면됨 On붙이고 적으면 됨 OnMove OnLook.. 이런느낌
//     { 
//         inputVec = value.Get<Vector2>();  // Control type 사용
//         // 방향 전환
//         if(inputVec.x < 0)
//             transform.localScale = new Vector3(1,1,1);
//         else if(inputVec.x > 0)
//             transform.localScale = new Vector3(-1,1,1);
//     }


//     // Start is called before the first frame update
//     void LateUpdate()
//     {
//         if(inputVec.magnitude != 0)
//         {
//             animator.SetFloat("Speed",2);
//         }
//         else
//         {
//             animator.SetFloat("Speed",0);
//         }
//         if(Input.GetKeyDown(KeyCode.Z)) // 차후 GetTouch
//         {
//             animator.speed = 1.5f;
//             animator.SetTrigger("Stand"); // 정지된 상태에서 공격하게 만듦
//             animator.SetTrigger("Attack");
//             animator.speed = 1.0f;
//         }

//     }
// }


using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    private Animator animator;
    public Vector2 inputVec; // 속도와 방향을 제어
    public float speed = 2;
    public float attackDamage = 10f; // 공격 데미지
    public float attackRange = 1.5f; // 공격 범위
    Rigidbody2D rigid; // 터치제어

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>(); // 터치
    }

    void FixedUpdate() // 물리 연산 frame 마다 호출 실행 / 고정된 시간 간격으로 호출
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime; // 위치 조절 프레임 맞춰서 움직이기 
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value) // OnMove로 사용자의 입력을 받아 이동 처리
    {
        inputVec = value.Get<Vector2>();  // Control type 사용
        // 방향 전환
        if (inputVec.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (inputVec.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void LateUpdate()
    {
        if (inputVec.magnitude != 0)
        {
            animator.SetFloat("Speed", 2);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.Z)) // 공격 키 (Z)를 누르면 공격
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Stand");
        animator.SetTrigger("Attack");
        
        // 공격 범위 내에 있는 몬스터를 찾아 데미지를 줌
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("monster"))
            {
                // 몬스터의 TakeDamage 함수 호출
                monster monsterScript = hit.collider.GetComponent<monster>();
                if (monsterScript != null)
                {
                    monsterScript.TakeDamage(attackDamage); // 몬스터에 데미지 적용
                }
            }
        }
    }
}
