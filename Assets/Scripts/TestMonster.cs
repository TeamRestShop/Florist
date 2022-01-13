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
    }
}
