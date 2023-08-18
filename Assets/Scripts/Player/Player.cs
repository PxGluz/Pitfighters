using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Serializable]
    public class Weapon
    {
        public float speed;

        public Weapon()
        {
            speed = 20f;
        }
    }
    
    [Header("Scripts")] 
    public PlayerCam playerCam;
    public PlayerMovement playerMovement;
    public RotateToPoint rotateToPoint;
    public WeaponManager weaponManager;

    [Header("Components")] 
    public Rigidbody rb;
    
    public static Player m;

    void Start()
    {
        m = this;
    }
}
