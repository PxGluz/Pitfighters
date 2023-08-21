using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDefaultBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private RotateToPoint rotateToPoint;
    [SerializeField] private WeaponManager weaponManager;

    [Header("Stats")] 
    [SerializeField] private float idleRange;
    [SerializeField] private float revolutionRate;

    private float originalSpeed;
    
    private void Start()
    {
        originalSpeed = weaponManager.currentWeapon.speed / 5;
    }

    private void Idle()
    {
        weaponManager.blocking = false;
        agent.speed = originalSpeed / 2;
        Vector3 direction = (transform.position - Player.m.rb.transform.position).normalized;
        if (Mathf.Abs(Vector3.Distance(transform.position, Player.m.rb.transform.position) - idleRange) < 0.1f)
            agent.destination = transform.position + revolutionRate * rotateToPoint.transform.right;
        else
            agent.destination = Player.m.rb.transform.position + idleRange * direction;
    }
    
    private void Defensive()
    {
        agent.speed = originalSpeed / 2;
        if (!weaponManager.blockOnCooldown)
            StartCoroutine(weaponManager.Block(rotateToPoint.transform));
        weaponManager.blocking = true;
        Vector3 direction = (transform.position - Player.m.rb.transform.position).normalized;
        if (Mathf.Abs(Vector3.Distance(transform.position, Player.m.rb.transform.position) - idleRange) < 0.1f)
            agent.destination = transform.position + revolutionRate * rotateToPoint.transform.right;
        else
            agent.destination = Player.m.rb.transform.position + idleRange * direction;
    }

    private void Aggressive()
    {
        
    }

    private void Faking()
    {
        
    }

    private void RotateEnemy()
    {
        rotateToPoint.rotationSpeed = RotateToPoint.originalRotationSpeed / (weaponManager.blocking ? 40f : 20f);
        float atan = (Player.m.rb.transform.position.x - transform.position.x) /
                     (Player.m.rb.transform.position.z - transform.position.z);
        rotateToPoint.angle = Mathf.Atan(atan) * Mathf.Rad2Deg + (Player.m.rb.transform.position.z < transform.position.z ? 180 : 0);
    }
    
    private void Update()
    {
        if (Player.m)
        {
            Defensive();
            RotateEnemy();
        }
    }
}
