using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private GameObject goalObject;
    private Vector3 targetPos;
    private TreeManager treeScript;
    private SpriteRenderer healthBarSpriteRenderer;

    private float _moveSpeed = 0.7f;
    private float _attackAmount = 2f;
    private float _attackSpeed = 0.7f;
    private float _maxHealth = 100f;
    private bool isAttacking = false;
    private bool isFainted = false; //CC 걸렸나?
    
    private Sprite[] healthBarSprite;
    private float currentHealth;
    private float faintSecs;

    protected void InitStats(float moveSpeed, float attackAmount, float attackSpeed, float maxHealth)
    {
        _moveSpeed = moveSpeed;
        _attackAmount = attackAmount;
        _attackSpeed = attackSpeed;
        _maxHealth = maxHealth;
    }

    public void Faint(float time)
    {
        faintSecs = time;
        StartCoroutine("FaintForSec");
    }

    public void ChangeHealth(float value)
    {
        currentHealth = (float)Mathf.Clamp(currentHealth + value, 0, 100);

        float currentHealthPercent = currentHealth / _maxHealth * 100;
        healthBarSpriteRenderer.sprite = currentHealthPercent != 100f ? healthBarSprite[(int)currentHealthPercent/10] : healthBarSprite[10];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "RoseSpecialAttack")
        {
            Faint(1.5f);
            ChangeHealth(-(col.gameObject.transform.parent.gameObject.GetComponent<Rose>().attackAmount) * 1.5f);
        }
    }

    protected void Start()
    {
        currentHealth = _maxHealth;
        goalObject = GameObject.FindWithTag("GoalObject"); //중간에 있는 나무
        targetPos = goalObject.transform.position;
        treeScript = goalObject.GetComponent<TreeManager>();

        healthBarSprite = new Sprite[11];
        healthBarSprite = Resources.LoadAll<Sprite>("Sprite/tempObject/SunlightBar");
        healthBarSpriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        ChangeHealth(0);
    }

    protected void Update()
    {
        if(currentHealth <= 0f)
        {
            Destroy(gameObject);
        }

        Move(targetPos);
        
        if(!isAttacking && transform.position == targetPos)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }

    private void ChangeState(bool newState)
    {
        isFainted = newState;
    }

    protected void Move(Vector3 target)
    {
        if(!isFainted)
        {
            if(target.x > transform.position.x) transform.localRotation = Quaternion.Euler(0, 180, 0);
            else transform.localRotation = Quaternion.Euler(0, 0, 0);

            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator Attack() 
    {
        while(true)
        {
            if(!isFainted)
            {
                treeScript.ChangeHealth(-_attackAmount);
            }
            yield return new WaitForSeconds(1f/_attackSpeed);
        }
    }
    
    IEnumerator FaintForSec()
    {
        ChangeState(true); 
        gameObject.GetComponent<SpriteRenderer>().color = new Color(100, 134, 255, 1); //임시 기절표시
        yield return new WaitForSeconds(faintSecs);
        ChangeState(false);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        yield break;
    }
}
