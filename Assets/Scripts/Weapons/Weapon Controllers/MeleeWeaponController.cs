using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  // Initialize base class variables
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();  // Update cooldown timer

        // Check if the player presses the space bar and if the cooldown is finished
        if (Input.GetKeyDown(KeyCode.Space) && currentCooldown <= 0f)
        {
            Attack();  // Perform the attack
        }
    }

    // Override the attack function to handle the melee weapon behavior
    protected override void Attack()
    {
        base.Attack();  // Reset cooldown using weaponData.CooldownDuration

        // Spawn the melee weapon attack object (e.g., a hitbox or melee effect)
        GameObject spawnedMeleeWeapon = Instantiate(weaponData.Prefab);
        spawnedMeleeWeapon.transform.position = transform.position;
        spawnedMeleeWeapon.transform.parent = transform;

        // Optionally, add some logic to the melee weapon object (e.g., duration of hitbox, effects)
    }
}
