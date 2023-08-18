using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool dashOnCursor;
    
    private bool canDash = true;
    private Vector3 finalMovement;
    void MovePlayer()
    {
        Vector3 moveVector = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            moveVector += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            moveVector += Vector3.right;
        if (Input.GetKey(KeyCode.W))
            moveVector += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            moveVector += Vector3.back;

        if (moveVector.magnitude > 1f)
            moveVector *= Mathf.Sqrt(2) / 2;
        
        finalMovement = Quaternion.Euler(0, -45, 0) * moveVector;
        Player.m.rb.AddForce(finalMovement * Player.m.weaponManager.currentWeapon.speed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        print(finalMovement);
        Player.m.rb.velocity = (dashOnCursor || finalMovement.magnitude == 0 ? Player.m.playerGraphics.forward : finalMovement) * Player.m.weaponManager.currentWeapon.dashRange;
        yield return new WaitForSeconds(Player.m.weaponManager.currentWeapon.dashCooldown);
        canDash = true;
    }
    
    void DashLogic()
    {
        if (canDash && Input.GetKey(KeyCode.Space))
            StartCoroutine(Dash());
    }

    void Update()
    {
        MovePlayer();
        DashLogic();
    }
}
