using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//임시 스크립트

public class SummonFlower : MonoBehaviour
{
    [SerializeField] private GameObject flowerObject;
    [SerializeField] private GameObject monsterObject;

    // Update is called once per frame
    void Update()
    {
        //임시
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 mousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(flowerObject, new Vector3(mousePose.x, mousePose.y, 0), Quaternion.identity);
        }
        if(Input.GetKeyDown("space"))
        {
            Vector3 mousePose = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(monsterObject, new Vector3(mousePose.x, mousePose.y, 0), Quaternion.identity);
        }
        //
    }
}
