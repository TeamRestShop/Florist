using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private GameObject goalObject;
    private Vector3 targetPos;

    private float moveSpeed = 0.7f;

    void Start()
    {
        goalObject = GameObject.FindWithTag("GoalObject"); //중간에 있는 나무
        targetPos = goalObject.transform.position;
    }

    void Update()
    {
        Move(targetPos);
    }

    void Move(Vector3 target)
    {
        if(target.x > transform.position.x) transform.localRotation = Quaternion.Euler(0, 180, 0);
        else transform.localRotation = Quaternion.Euler(0, 0, 0);
    
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }
}
