using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    public class Weapon
    {
        public float speed;
        public float dashRange;
        public float dashCooldown;

        public Weapon()
        {
            speed = 20f;
            dashRange = 50f;
            dashCooldown = 1f;
        }
    }
    
    [Header("Scripts")] 
    public PlayerCam playerCam;
    public PlayerMovement playerMovement;
    public RotateToPoint rotateToPoint;
    public WeaponManager weaponManager;

    [Header("Components")] 
    public Rigidbody rb;
    public Transform playerGraphics;
    
    public static Player m;

    void Start()
    {
        m = this;
    }
}
