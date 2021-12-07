using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : Flower
{
    [SerializeField] private Sprite[] stageSprite; 
    [SerializeField] private GameObject attackObject; //총알? 임시

    private Animator ani;
    private Transform attackTransform; //자식 attack 의 transform
    
    private float attackRange = 5;
    private float attackSpeed = 0.7f;
    private float attackAmount = 10;
    private float specialAttack = 10;

    public GameObject targetEnemy {get; set;} //현재 타겟팅하고 있는 적

    private bool isAttacking = false;

    private void Start()
    {
        base.InitStats(attackRange, attackSpeed, attackAmount, specialAttack, stageSprite);
        base.Start();

        ani = gameObject.transform.FindChild("Attack").GetComponent<Animator>();
        attackTransform = gameObject.transform.FindChild("Attack").GetComponent<Transform>();
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

    private void changeIsAttacking(bool state)
    {
        isAttacking = state;
        if(state)
        {
            ani.SetTrigger("attack");
        }
        else
        {
            ani.ResetTrigger("attack");
        }
    }

    public void StartAttack()
    {
        if(!isAttacking && stage != FlowerStage.Seed)
        {
            Debug.Log("attack");
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }


    IEnumerator Attack() 
    {
        while(targetEnemy != null)
        {
            ani.ResetTrigger("attack");
            ani.SetTrigger("attack"); //덩굴 공격 애니메이션

            Quaternion rotation = Quaternion.LookRotation (targetEnemy.transform.position - transform.position, transform.TransformDirection(Vector3.up)); 
            attackTransform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            attackTransform.Rotate(0, 0, -90);
            //덩굴을 적 방향으로 돌리기

            yield return new WaitForSeconds(1f/_speed);
            //1/공격속도초 기다리기
        }

        yield break; 
    }
}
