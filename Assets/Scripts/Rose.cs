using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : Flower
{
    [SerializeField] private Sprite[] stageSprite; 
    [SerializeField] private GameObject attackObject; //총알? 임시
    
    private float attackRange = 5;
    private float attackSpeed = 2;
    private float attackAmount = 10;
    private float specialAttack = 10;

    public GameObject targetEnemy {get; set;} //현재 타겟팅하고 있는 적

    private bool isAttacking = false;

    private void Start()
    {
        base.InitStats(attackRange, attackSpeed, attackAmount, specialAttack, stageSprite);
        base.Start();
    }

    private void Update()
    {
        base.Update();
        targetEnemy = base.TargetEnemy(transform);

        if(targetEnemy != null)
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
        Debug.Log("attack");
        if(!isAttacking && stage != FlowerStage.Seed)
        {
            StartCoroutine(Attack());
            isAttacking = true;
        }
    }

    IEnumerator Attack() 
    {
        while(targetEnemy != null)
        {
            Debug.Log("attack");
            GameObject attackInstance = Instantiate(attackObject, transform.position, Quaternion.identity);
            (attackInstance.GetComponent<AttackManager>() as AttackManager).targetObject = targetEnemy;

            yield return new WaitForSeconds(1f/_speed);
        }

        yield break;
    }
}
