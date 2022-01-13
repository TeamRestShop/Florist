using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : Singleton<SpawnMonster> //몬스터들 랜덤으로 스폰해주는거
{
    [SerializeField] private GameObject[] monsterTypes;

    private const float xEdge = 6f; //몬스터가 스폰할수있는 공간 모서리 x 좌표
    private const float yEdge = 5f; //몬스터가 스폰할수있는 공간 모서리 y 좌표

    private int[] leftMonsters; //각 몬스터 타입마다 몇마리 남았는지; 레벨마다 나오는 몬스터 마릿수 다르기 때문에
    private float spawnRate = 0.5f; //1초에 몇마리 스폰하는지
    private int side = 0;
    private int type; //monsterTypes 의 index
    private Vector3 spawnPos;
    void Start()
    {
        leftMonsters = new int[monsterTypes.Length];

        for(int i = 0; i < monsterTypes.Length; i++)
        {
            leftMonsters[i] = 5;
        }

        spawnPos = new Vector3(0, 0, 0);
        StartCoroutine("RandomlySpawn");
    }

    void Update()
    {
        
    }

    private void changeLeftMonsters(int index)
    {
        leftMonsters[index]--;
    }

    IEnumerator RandomlySpawn()
    {
        while(true)
        {
            type = 0; //monsterTypes 의 type 번째 몬스터를 스폰
            side = Random.Range(0, 4); //왼쪽, 오른쪽, 위, 아래 모서리중 어디 스폰할껀지

            switch(side)
            {
                case 0:
                    spawnPos = new Vector3(-xEdge, Random.Range(-yEdge, yEdge), 0); //왼쪽
                    break;
                case 1:
                    spawnPos = new Vector3(xEdge, Random.Range(-yEdge, yEdge), 0); //오른쪽
                    break;
                case 2:
                    spawnPos = new Vector3(Random.Range(-xEdge, xEdge), yEdge, 0); //위쪽
                    break;
                case 3:
                    spawnPos = new Vector3(Random.Range(-xEdge, xEdge), -yEdge, 0); //아래쪽
                    break;
            }

            Instantiate(monsterTypes[type], spawnPos, Quaternion.identity);
            changeLeftMonsters(type);

            yield return new WaitForSeconds(1f/spawnRate);
        }
    }
}
