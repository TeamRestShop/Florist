using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected GameObject goalObject;
    protected GameObject imageObject;
    protected Vector3 targetPos;
    protected TreeManager treeScript;
    protected SpriteRenderer healthBarSpriteRenderer;

    private float _moveSpeed = 0.7f;
    protected float _attackAmount = 2f;
    protected float _attackSpeed = 0.7f;
    private float _maxHealth = 100f;
    protected bool isAttacking = false;
    protected bool isFainted = false; //CC 걸렸나?
    
    protected Sprite[] healthBarSprite;
    protected float currentHealth;
    protected float faintSecs;

    protected void InitStats(float moveSpeed, float attackAmount, float attackSpeed, float maxHealth)
    {
        _moveSpeed = moveSpeed;
        _attackAmount = attackAmount;
        _attackSpeed = attackSpeed;
        _maxHealth = maxHealth;
    }

    public void Faint(float time)
    {
        
    }

    public void ChangeHealth(float value)
    {
        currentHealth = (float)Mathf.Clamp(currentHealth + value, 0, 100);

        float currentHealthPercent = currentHealth / _maxHealth * 100;
        healthBarSpriteRenderer.sprite = currentHealthPercent != 100f ? healthBarSprite[(int)currentHealthPercent/10] : healthBarSprite[10];
    }

    protected void OnTriggerEnter2D(Collider2D col)
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

        imageObject = gameObject.transform.GetChild(1).gameObject;

        ChangeHealth(0);
    }

    protected void Update()
    {
        if(currentHealth <= 0f)
        {
            Destroy(gameObject);
        }

        Move(targetPos);
        
    }

    protected void ChangeState(bool newState)
    {
        isFainted = newState;
    }

    protected void Move(Vector3 target)
    {
        if(!isFainted)
        {
            if(target.x > transform.position.x) imageObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            else imageObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

            //transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);
        }
    }
}
