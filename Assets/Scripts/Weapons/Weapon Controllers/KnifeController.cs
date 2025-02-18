using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController

{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position;
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionChecker(pm.lastMovedVector);
    }
}
