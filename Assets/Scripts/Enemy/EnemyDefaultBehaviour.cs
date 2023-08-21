using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class EnemyDefaultBehaviour : MonoBehaviour
{
    private enum States
    {
        Idle,
        Blocking,
        Aggressive,
        Faking,
    }
    
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private RotateToPoint rotateToPoint;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private Rigidbody rb;

    [Header("Stats")] 
    [SerializeField] private float idleRange;
    [SerializeField] private float revolutionRate;

    private float originalSpeed;
    private States currentState = States.Idle;
    
    private void Start()
    {
        StartCoroutine(ChangeState());
        originalSpeed = weaponManager.currentWeapon.speed / 5;
    }

    private IEnumerator ChangeState()
    {
        agent.enabled = true;
        weaponManager.blocking = false;
        weaponManager.currentAttack = 0;
        
        int rand = Random.Range(0,4);
        print(rand);
        switch (rand)
        {
            case 0:
                currentState = States.Idle;
                break;
            case 1:
                currentState = States.Blocking;
                break;
            case 2:
                currentState = States.Aggressive;
                break;
            case 3:
                currentState = States.Faking;
                break;
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(ChangeState());
    }
    
    private IEnumerator Idle()
    {
        agent.speed = originalSpeed / 2;
        Vector3 direction = (transform.position - Player.m.rb.transform.position).normalized;
        if (Mathf.Abs(Vector3.Distance(transform.position, Player.m.rb.transform.position) - idleRange) < 0.1f)
            agent.destination = transform.position + revolutionRate * rotateToPoint.transform.right;
        else
            agent.destination = Player.m.rb.transform.position + idleRange * direction;
        yield return 0;
        coroutineDone = true;
    }
    
    private IEnumerator Defensive()
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
        yield return 0;
        coroutineDone = true;
    }

    private bool finishedCombo;
    private IEnumerator Aggressive()
    {
        if (finishedCombo)
            yield break;
        if (Vector3.Distance(transform.position, Player.m.rb.transform.position) - weaponManager.currentWeapon.attacks[weaponManager.currentAttack].range / 2 <= 0f)
        {
            agent.enabled = false;
            weaponManager.shouldAttack = true;
            if (!weaponManager.attacking && !finishedCombo)
                StartCoroutine(weaponManager.Attack(rotateToPoint.transform, rb));
            if (weaponManager.currentAttack == weaponManager.currentWeapon.attacks.Length - 1)
            {
                finishedCombo = true;
                yield return new WaitForSeconds(2f);
                finishedCombo = false;
            }
        }
        else if (!weaponManager.attacking)
        {
            weaponManager.shouldAttack = false;
            agent.enabled = true;
            agent.destination = Player.m.rb.transform.position;
        }
        coroutineDone = true;
    }

    private bool canDash = true;
    private IEnumerator Faking()
    {
        if (!canDash)
            yield break;
        if (Vector3.Distance(transform.position, Player.m.rb.transform.position) - weaponManager.currentWeapon.attacks[0].range / 2 <= 0f)
        {
            agent.enabled = false;
            rb.velocity = rotateToPoint.transform.right * weaponManager.currentWeapon.dashRange;
            canDash = false;
            yield return new WaitForSeconds(weaponManager.currentWeapon.dashCooldown);
            canDash = true;
        }
        else
        {
            agent.enabled = true;
            agent.destination = Player.m.rb.transform.position;
        }
        yield return 0;
        coroutineDone = true;
    }

    private void RotateEnemy()
    {
        rotateToPoint.rotationSpeed = RotateToPoint.originalRotationSpeed / (weaponManager.blocking ? 40f : 1f);
        float atan = (Player.m.rb.transform.position.x - transform.position.x) /
                     (Player.m.rb.transform.position.z - transform.position.z);
        rotateToPoint.angle = Mathf.Atan(atan) * Mathf.Rad2Deg + (Player.m.rb.transform.position.z < transform.position.z ? 180 : 0);
    }

    private bool coroutineDone = true;
    private void FixedUpdate()
    {
        if (Player.m)
        {
            if (coroutineDone)
            {
                coroutineDone = false;
                switch (currentState)
                {
                    case States.Idle:
                        StartCoroutine(Idle());
                        break;
                    case States.Blocking:
                        StartCoroutine(Defensive());
                        break;
                    case States.Aggressive:
                        StartCoroutine(Aggressive());
                        break;
                    case States.Faking:
                        StartCoroutine(Faking());
                        break;
                }
            }
            if (!weaponManager.attacking)
                RotateEnemy();
        }
    }
}
