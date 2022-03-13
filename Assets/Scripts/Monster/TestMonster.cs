using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Monster
{
    void Start()
    {
        base.InitStats(0.7f, 2f, 0.7f, 100f);
        base.Start();
    }
    void Update()
    {
        base.Update();
        if(!isAttacking && Vector3.Distance(transform.position, targetPos) < 0.2f)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }

    protected void ChangeState(bool newState)
    {
        base.ChangeState(newState);
    }

    public void ChangeHealth(float value)
    {
        base.ChangeHealth(value);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        base.OnTriggerStay2D(col);
    }

    IEnumerator Attack() 
    {
        while(true)
        {
            Debug.Log("tree health");
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
