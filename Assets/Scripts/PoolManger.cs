using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManger : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // .. 풀 담당을 하는 리스트 
    List<GameObject>[] pools;
    // 1:1 관계 하나 당 하나 

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++){
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // .. 선택한 풀의 놀고 있는 게임오브젝트 접근
         // .. 발견하면 select 변수에 할당

        foreach (GameObject item in pools[index]) // 배열, 리스트를의 데이터를 순차적으로 접근하는 반복문
        {
            if (!item.activeSelf){
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // .. 못 찾았으면?
        if (!select){
            // .. 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
         
        return select;
    }
}
