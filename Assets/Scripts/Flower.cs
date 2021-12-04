using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerStage
{
    Seed = 0,
    Sprout,
    Flower
}

public class Flower : Singleton<Flower>
{
    public FlowerStage stage {get; set;} //씨앗, 새싹, 꽃

    public float _attackRange{get;set;} //공격범위
    public float _attackSpeed{get;set;} //1초에 몇번 공격할 수 있는지; 공격속도
    public float _attackAmount{get;set;} //공격력
    public float _specialAttack{get;set;} //특수 공격 게이지

    private Sprite[] _stageSprite; //씨앗, 새싹, 꽃의 스프라이트
    private Sprite[] sunBarSprite;  //총 11개의 햇빛게이지 표시 이미지 (0% 일때 + 10%마다 증가); 햇빛이 찰수록 게이지가 차게

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer sunBarSpriteRenderer;
    private GameObject sunBarObject;

    private bool inSunlight = false;

    private float sunlightPercent = 0f; // 햇빛 게이지 0%~100%
    private float sunlightChangeRate = 10f; //햇빞에 있거나 없으면 1초에 햇빛 게이지 몇퍼센트 변하는지

    protected void InitStats(float attackRange, float attackSpeed, float attackAmount, float specialAttack, Sprite[] stageSprite)
    {
        _attackRange = attackRange;
        _attackSpeed = attackSpeed;
        _attackAmount = attackAmount;
        _specialAttack = specialAttack;

        _stageSprite = stageSprite;
    }

    protected void Start()
    {
        sunBarSprite = new Sprite[11];

        sunBarSprite = Resources.LoadAll<Sprite>("Sprite/tempObject/SunlightBar");
        Debug.Log(sunBarSprite.Length);

        // sunBarSprite[0] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar0");
        // sunBarSprite[1] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar1");
        // sunBarSprite[2] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar2");
        // sunBarSprite[3] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar3");
        // sunBarSprite[4] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar4");
        // sunBarSprite[5] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar5");
        // sunBarSprite[6] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar6");
        // sunBarSprite[7] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar7");
        // sunBarSprite[8] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar8");
        // sunBarSprite[9] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar9");
        // sunBarSprite[10] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar10");
        // sunBarSprite[11] = Resources.Load<Sprite>("Sprite/tempObject/SunlightBar/sunbar11");

        spriteRenderer = GetComponent<SpriteRenderer>();
        sunBarObject = transform.GetChild(0).gameObject;
        sunBarSpriteRenderer = sunBarObject.GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        ChangeSunlightPercent();

        //임시
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePose.x, mousePose.y, 0);
        }
        //
    }

    protected List<GameObject> GetEnemiesInRange(Transform objectTransform) //원 범위 내에 있는 모든 적들 찾기
    {
        GameObject[] enemies;
        List<GameObject> enemiesInRange = new List<GameObject>();
        enemies = GameObject.FindGameObjectsWithTag("Monster");
        
        foreach(GameObject enemy in enemies)
        {
            if(Vector3.Distance(enemy.transform.position, objectTransform.position) <= _attackRange) //지금은 원이지만 나중에 타원 공식으로 바꿀겁니다
            {
                enemiesInRange.Add(enemy);
            }
        }

        return enemiesInRange;
    }

    protected GameObject TargetEnemy(Transform objectTransform) //타겟할 적 정하기
    {
        GameObject closestEnemy = null; //목표 오브젝트와 가장 가까운 적을 타겟팅합니다
        List<GameObject> enemiesInRange = GetEnemiesInRange(objectTransform);

        if(enemiesInRange.Count != 0)
        {   
            float minDistance = Vector3.Distance(enemiesInRange[0].transform.position, objectTransform.position);
            closestEnemy = enemiesInRange[0];

            foreach(GameObject enemy in enemiesInRange) //범위 내에 있는 적 중에 목표 오브젝트와 가장 가까운 적 찾기
            {
                GameObject goalObject = GameObject.FindGameObjectWithTag("GoalObject");
                float distance = Vector3.Distance(enemy.transform.position, goalObject.transform.position);

                if(minDistance > distance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    protected void ChangeSunlightPercent() 
    {
        if(inSunlight) //꽃이 햇빛에 있냐 없냐 체크 --> 햇빛 게이지 바꾸기
        {
            sunlightPercent = (float)Mathf.Clamp(sunlightPercent + sunlightChangeRate*Time.deltaTime, 0, 100);
        }
        else
        {
            sunlightPercent = (float)Mathf.Clamp(sunlightPercent - sunlightChangeRate*Time.deltaTime, 0, 100);
        }

        switch(sunlightPercent) 
        {
            case float n when n < 30f:
                stage = FlowerStage.Seed;
                spriteRenderer.sprite = _stageSprite[0];
                break;
            case float n when n < 60f:
                stage = FlowerStage.Sprout;
                spriteRenderer.sprite = _stageSprite[1];
                break;
            default:
                stage = FlowerStage.Flower;
                spriteRenderer.sprite = _stageSprite[2];
                break;
        } //햇빛 게이지 따라서 상태 (씨앗, 새싹, 꽃) 바꾸기

        sunBarSpriteRenderer.sprite = sunlightPercent != 100f ? sunBarSprite[(int)sunlightPercent/10] : sunBarSprite[10];
    }

    protected void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Sunlight")
        {
            inSunlight = true;
        }
    }
    protected void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Sunlight")
        {
            inSunlight = false;
        }
    }
}
