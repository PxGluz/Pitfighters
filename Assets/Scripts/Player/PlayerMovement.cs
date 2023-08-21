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
        float finalSpeed = Player.m.weaponManager.currentWeapon.speed * (Player.m.weaponManager.blocking ? 0.5f : 1);
        Player.m.rb.AddForce(finalMovement * finalSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        Player.m.rb.velocity = (dashOnCursor || finalMovement.magnitude == 0 ? Player.m.playerGraphics.forward : finalMovement) * Player.m.weaponManager.currentWeapon.dashRange;
        yield return new WaitForSeconds(Player.m.weaponManager.currentWeapon.dashCooldown);
        canDash = true;
    }
    
    void DashLogic()
    {
        if (canDash && Input.GetKey(KeyCode.Space) && !Player.m.weaponManager.blocking)
            StartCoroutine(Dash());
    }
    
    private void RotatePlayer()
    {
        Ray ray = Player.m.playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.right, hit.point - Player.m.playerGraphics.position);
            bool flip = hit.point.z > Player.m.playerGraphics.position.z;
            Player.m.rotateToPoint.rotationSpeed = RotateToPoint.originalRotationSpeed / (Player.m.weaponManager.blocking ? 20f : 1f);
            Player.m.rotateToPoint.angle = angle * (flip ? -1f : 1f) + 90f;
        }
    }
    
    void FixedUpdate()
    {
        if (!Player.m.weaponManager.attacking)
        {
            MovePlayer();
            DashLogic();
            RotatePlayer();
        }
    }
}
