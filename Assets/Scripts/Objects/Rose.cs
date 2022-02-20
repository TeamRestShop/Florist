using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Rose : Flower
{
    [SerializeField] private Sprite[] stageSprite;
    [SerializeField] private GameObject attackObject; //총알? 임시
    [SerializeField] private GameObject specialAttackObject; //바닥에서 나오는 가시

    private Animator ani;
    private Transform attackTransform; //자식 attack 의 transform

    private float attackRange = 5f;
    private float attackSpeed = 0.7f;
    public float attackAmount = 10f;
    private float specialAttack = 10f;

    private float currentSpecialAttack = 0f; //공격할때마다 차는 특수공격 게이지

    public GameObject targetEnemy {get; set;} //현재 타겟팅하고 있는 적

    private bool isAttacking = false;

    private void Start()
    {
        base.InitStats(attackRange, attackSpeed, attackAmount, specialAttack, stageSprite);
        base.Start();

        ani = gameObject.transform.Find("Attack").GetComponent<Animator>();
        attackTransform = gameObject.transform.Find("Attack").GetComponent<Transform>();
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
        // if(stage == FlowerStage.Sprout)
        // {
        //     targetEnemy.GetComponent<Monster>().ChangeHealth(-attackAmount * 0.6f * 1.5f); 
        // }
        // else if(stage == FlowerStage.Flower)
        // {
        //     targetEnemy.GetComponent<Monster>().ChangeHealth(-attackAmount * 1.5f); 
        // }
        // targetEnemy.GetComponent<Monster>().Faint(3f); 

        StartCoroutine("SpecialAttackLaunch");
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
                //덩굴 공격 애니메이션은 3개 중 랜덤하게 골라진다

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
                    targetEnemy.GetComponent<Monster>().ChangeHealth(-attackAmount * 0.6f); 
                }
                else if(stage == FlowerStage.Flower)
                {
                    targetEnemy.GetComponent<Monster>().ChangeHealth(-attackAmount); 
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

    IEnumerator SpecialAttackLaunch()
    {
        Debug.Log("special attack launch");
        //rose 의 좌표와 targetEnemy 의 좌표를 지나는 직선으로 가시를 보냄
        int thornNum = 5;
        float range = 2f;
        Vector3 endPos = transform.position + (targetEnemy.transform.position - transform.position).normalized * range;
        Vector3 position;

        for(int i = 0; i < thornNum; i++)
        {
            position = new Vector3(transform.position.x + (endPos.x - transform.position.x)/thornNum*i, transform.position.y + (endPos.y - transform.position.y)/thornNum*i, 0);
            Debug.Log(position);
            GameObject thorn = Instantiate(specialAttackObject, position, Quaternion.identity);
            Destroy(thorn, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
}
