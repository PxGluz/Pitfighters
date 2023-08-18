using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private WeaponManager weaponManager;
    
    IEnumerator Start()
    {
        yield return 0;
        weaponManager = Player.m.weaponManager;
    }

    private void BlockingLogic()
    {
        if (Input.GetMouseButton(1) && !weaponManager.blockOnCooldown && !weaponManager.attacking && !weaponManager.shouldAttack)
        {
            StartCoroutine(weaponManager.Block(Player.m.playerGraphics));
            weaponManager.blocking = true;
        }
        else if(!Input.GetMouseButton(1))
        {
            weaponManager.blocking = false;
        }
    }

    private void AttackingLogic()
    {
        if (Input.GetMouseButtonDown(0) && !weaponManager.blocking)
            weaponManager.shouldAttack = true;
        if (weaponManager.shouldAttack && !weaponManager.attacking)
            StartCoroutine(weaponManager.Attack(Player.m.playerGraphics, Player.m.rb));
    }
    
    void Update()
    {
        if (weaponManager)
        {
            AttackingLogic();
            BlockingLogic();
        }
    }
}
