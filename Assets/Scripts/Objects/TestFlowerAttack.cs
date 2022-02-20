using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlowerAttack : Flower //임시로 만든 꽃 종류
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackAmount;
    [SerializeField] private float specialAttack;

    [SerializeField] private Sprite[] stageSprite; 
    [SerializeField] private GameObject attackObject; //총알? 임시

    public GameObject targetEnemy {get; set;} //현재 타겟팅하고 있는 적

    private bool isAttacking = false;
    private bool isFainted = false; //CC 먹어서 기절 상태일때

    private void Start()
    {
        base.InitStats(attackRange, attackSpeed, attackAmount, specialAttack, stageSprite);
        base.Start();
    }

    private void Update()
    {
        base.Update();
        targetEnemy = base.TargetEnemy(transform);

        if(targetEnemy != null && !isFainted)
        {
            StartAttack();
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

    public void StartAttack()
    {
        if(!isAttacking && stage != FlowerStage.Seed)
        {
            StartCoroutine(Attack());
            isAttacking = true;
        }
    }

    IEnumerator Attack() 
    {
        while(targetEnemy != null && !isFainted)
        {
            GameObject attackInstance = Instantiate(attackObject, transform.position, Quaternion.identity);
            (attackInstance.GetComponent<AttackManager>() as AttackManager).targetObject = targetEnemy;

            yield return new WaitForSeconds(1f/_speed);
        }

        isAttacking = false;
        yield break;
    }
}
