using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject targetObject {get;set;}

    private float moveSpeed = 4f;
    
    public void SetTarget(GameObject enemyObject)
    {
        targetObject = enemyObject;
        transform.LookAt(targetObject.transform);
    }

    private void Update()
    {
        if(targetObject != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, moveSpeed * Time.deltaTime);

            if(transform.position == targetObject.transform.position)
            {
                Destroy(gameObject);
            }
        }
    }
}
