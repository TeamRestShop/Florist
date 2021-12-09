using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Rose : Flower
{
    [SerializeField] private Sprite[] stageSprite;
    [SerializeField] private GameObject attackObject; //총알? 임시

    private Animator ani;
    private Transform attackTransform; //자식 attack 의 transform

    private float attackRange = 5f;
    private float attackSpeed = 0.7f;
    private float attackAmount = 10f;
    private float specialAttack = 10f;

    private float currentSpecialAttack = 0f; //공격할때마다 차는 특수공격 게이지

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

    private void SpecialAttack()
    {

    }

    IEnumerator Attack()
    {
        while(targetEnemy != null)
        {
            if(currentSpecialAttack <= specialAttack) //특수 공격 게이지가 안채워졌다면
            {
                Debug.Log("attack");
                int attackNum = Random.Range(0, 3);

                switch(attackNum)
                {
                case 0:
                    ani.SetTrigger("attack");
                    break;
                case 1:
                    ani.SetTrigger("attack2");
                    break;
                case 2:
                    ani.SetTrigger("attack3");
                    break;
                }
                //덩굴 공격 애니메이션은 3개 중 랜덤하게 골라져요

                Vector2 direction = new Vector2(
                transform.position.x - targetEnemy.transform.position.x,
                transform.position.y - targetEnemy.transform.position.y
                );

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                Quaternion rotation = Quaternion.Slerp(attackTransform.rotation, angleAxis, 100f);
                attackTransform.rotation = rotation;
                //덩굴을 적 방향으로 돌리기

                yield return new WaitForSeconds(0.09f); //덩굴이 뻗는 에니메이션이 재생된 다음에 적 피 닳게 하기

                if(stage == FlowerStage.Sprout)
                {
                targetEnemy.GetComponent<MonsterManager>().ChangeHealth(-attackAmount * 0.6f); 
                }
                else if(stage == FlowerStage.Flower)
                {
                targetEnemy.GetComponent<MonsterManager>().ChangeHealth(-attackAmount); 
                }

                currentSpecialAttack += 5f;

                yield return new WaitForSeconds(0.06f);//애니메이션 끝날때까지 기다리기

                yield return new WaitForSeconds(1f/_speed);
                //1/공격속도초 기다리기

                targetEnemy = base.TargetEnemy(transform);
            }
            else
            {
                Debug.Log("special attack");
                SpecialAttack();
                currentSpecialAttack = 0f;
            }
        }

        changeIsAttacking(false);
        yield break;
    }
}
