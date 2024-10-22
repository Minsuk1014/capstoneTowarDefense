using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject monsterPrefab;      // 몬스터 프리팹
    public Transform[] spawnPoints;       // 몬스터 소환 지점
    public int totalMonsters = 50;        // 총 소환할 몬스터 수
    public float spawnInterval = 2f;      // 몬스터 소환 간격
    public float gameTimeLimit = 120f;    // 게임 제한 시간 (초)
    private int monstersSpawned = 0;      // 소환된 몬스터 수
    private int monstersDefeated = 0;     // 처치된 몬스터 수
    private bool gameEnded = false;       // 게임 종료 여부
    private float startTime;

    void Start()
    {
        startTime = Time.time;            // 게임 시작 시간 기록
        StartCoroutine(SpawnMonsters());  // 몬스터 소환 시작
    }

    void Update()
    {
        // 게임이 끝났는지 확인
        if (gameEnded)
            return;

        // 제한 시간 초과 체크
        if (Time.time - startTime >= gameTimeLimit)
        {
            EndGame(false);  // 패배 처리
        }

        // 모든 몬스터를 처치했는지 체크
        if (monstersDefeated >= totalMonsters)
        {
            EndGame(true);   // 클리어 처리
        }
    }

    // 몬스터 소환을 위한 코루틴
    IEnumerator SpawnMonsters()
    {
        while (monstersSpawned < totalMonsters)
        {
            // 랜덤한 위치에서 몬스터 소환
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

            monstersSpawned++;  // 소환된 몬스터 수 증가

            // 소환 간격만큼 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 몬스터가 처치되었을 때 호출하는 함수
    public void MonsterDefeated()
    {
        monstersDefeated++;
    }

    // 게임 종료 처리 함수
    void EndGame(bool victory)
    {
        gameEnded = true;

        if (victory)
        {
            Debug.Log("Game Cleared! All monsters defeated.");
            // 클리어 시 처리 (예: 클리어 씬 이동)
        }
        else
        {
            Debug.Log("Game Over! Time's up.");
            // 패배 시 처리 (예: 패배 씬 이동)
        }

        // 결과에 따른 씬 이동 (예시)
        // SceneManager.LoadScene(victory ? "VictoryScene" : "GameOverScene");
    }
}
