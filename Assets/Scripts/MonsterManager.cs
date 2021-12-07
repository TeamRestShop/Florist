using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameObject goalObject;
    private Vector3 targetPos;
    private TreeManager treeScript;

    private float moveSpeed = 0.7f;
    private float attackAmount = 2f;
    private float attackSpeed = 0.7f;
    private bool isAttacking = false;
    private bool isFainted = false; //CC 걸렸나?

    private void Start()
    {
        goalObject = GameObject.FindWithTag("GoalObject"); //중간에 있는 나무
        targetPos = goalObject.transform.position;
        treeScript = goalObject.GetComponent<TreeManager>();
    }

    private void Update()
    {
        Move(targetPos);
        
        if(!isAttacking && transform.position == targetPos)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }

    private void Move(Vector3 target)
    {
        if(target.x > transform.position.x) transform.localRotation = Quaternion.Euler(0, 180, 0);
        else transform.localRotation = Quaternion.Euler(0, 0, 0);

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    IEnumerator Attack() 
    {
        while(true)
        {
            if(!isFainted)
            {
                treeScript.ChangeHealth(-attackAmount);
            }
            yield return new WaitForSeconds(1f/attackSpeed);
        }
    }
}
