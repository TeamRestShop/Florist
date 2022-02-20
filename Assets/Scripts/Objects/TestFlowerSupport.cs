using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlowerSupport : Flower
{
    [SerializeField] private float supportRange;
    [SerializeField] private float supportSpeed; //1초에 효과 몇번
    [SerializeField] private float supportAmount;
    private float specialAttack = 0; //지원형은 툭수 공격이 없어요

    [SerializeField] private Sprite[] stageSprite; 

    public List<GameObject> targetFlowers {get; set;} //현재 지원 효과를 주고있는 아군 꽃

    private bool isSupporting = false;
    private bool isFainted = false; //CC 먹어서 기절 상태일때

    private void Start()
    {
        base.InitStats(supportRange, supportSpeed, supportAmount, specialAttack, stageSprite);
        base.Start();
    }

    private void Update()
    {
        base.Update();
        targetFlowers = base.GetFlowersInRange(transform);

        if(!isSupporting && !isFainted)
        {
            StartSupport();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        base.OnTriggerStay2D(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        base.OnTriggerExit2D(col);
    }

    public void StartSupport()
    {
        StartCoroutine(Support());
        isSupporting = true;
        
    }

    IEnumerator Support() 
    {
        while(!isFainted) 
        {
            //지원하기

            yield return new WaitForSeconds(1f/_speed);
        }

        isSupporting = false;
        yield break;
    }
}
