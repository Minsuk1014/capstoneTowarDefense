using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트
    List<GameObject>[] pools;

    // 스폰 위치들 (추가)
    public Transform[] spawnPoints;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    // 몬스터를 특정 인덱스에서 스폰하는 메서드
    public GameObject SpawnAt(int index, Vector3 position)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 있는 게임오브젝트 접근
        foreach (GameObject item in pools[index]) // 배열, 리스트를의 데이터를 순차적으로 접근하는 반복문
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.transform.position = position; // 위치 설정
                select.SetActive(true);
                break;
            }
        }

        // 풀에서 사용 가능한 오브젝트가 없을 때
        if (select == null)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], position, Quaternion.identity, transform);
            pools[index].Add(select);
        }

        return select;
    }

    // 랜덤한 스폰 지점에서 몬스터 스폰 (추가)
    public GameObject SpawnAtRandomPoint(int index)
    {
        // 랜덤한 스폰 지점을 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        return SpawnAt(index, spawnPoint.position);
    }
}
