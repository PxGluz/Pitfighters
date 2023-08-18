using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
        
        Vector3 finalMovement = Quaternion.Euler(0, -45, 0) * moveVector * Player.m.weaponManager.currentWeapon.speed;
        Player.m.rb.AddForce(finalMovement);
    }

    void Update()
    {
        MovePlayer();
    }
}
