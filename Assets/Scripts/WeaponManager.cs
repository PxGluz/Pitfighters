using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Player.Weapon currentWeapon;
    public float blockingCooldown;
    public float comboWindow;
    public bool isEnemy;
    
    [HideInInspector] public int currentAttack;
    [HideInInspector] public bool parrying, blocking, blockOnCooldown;
    [HideInInspector] public bool attacking, shouldAttack;
    public IEnumerator Block(Transform direction)
    {
        blockOnCooldown = true;
        parrying = true;
        Debug.DrawRay(transform.position, Quaternion.Euler(0f, currentWeapon.blockAngle / 2, 0f) * direction.forward * 5f, Color.yellow, duration:currentWeapon.parryWindow);
        Debug.DrawRay(transform.position, Quaternion.Euler(0f, -currentWeapon.blockAngle / 2, 0f) * direction.forward * 5f, Color.yellow, duration:currentWeapon.parryWindow);
        yield return new WaitForSeconds(currentWeapon.parryWindow);
        parrying = false;
        while (blocking)
        {
            Debug.DrawRay(transform.position, Quaternion.Euler(0f, currentWeapon.blockAngle / 2, 0f) * direction.forward * 5f, Color.green);
            Debug.DrawRay(transform.position, Quaternion.Euler(0f, -currentWeapon.blockAngle / 2, 0f) * direction.forward * 5f, Color.green);
            yield return 0;
        }
        yield return new WaitForSeconds(blockingCooldown);
        blockOnCooldown = false;
    }

    public IEnumerator Attack(Transform direction, Rigidbody rb, MeshFilter attackMesh)
    {
        shouldAttack = false;
        attacking = true;
        Player.Attack attack = currentWeapon.attacks[currentAttack];

        if (isEnemy)
        {
            attack.DrawAttack(direction, 1f);
            yield return new WaitForSeconds(1f);
        }
        
        float attackIntervals = attack.duration / attack.numberOfHits;
        rb.velocity = direction.forward * attack.pushForce;
        for (int i = 0; i < attack.numberOfHits; i++)
        {
            // Temp function
            yield return new WaitForSeconds(attackIntervals);
            attack.DrawAttack(direction);
            StartCoroutine(attack.StartAttack(attackMesh));
        }
        if (isEnemy)
            yield return new WaitForSeconds(comboWindow);
        attacking = false;
        currentAttack++;
        if (currentAttack == currentWeapon.attacks.Length)
            currentAttack = 0;
        yield return new WaitForSeconds(comboWindow);
        if (!shouldAttack && !attacking)
            currentAttack = 0;
    }
    
    void Start()
    {
        currentWeapon = new Player.Weapon();
    }
}
