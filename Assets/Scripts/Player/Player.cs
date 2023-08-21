using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum AttackType
    {
        Circle,
        Cone,
        Rectangle,
    }
    
    [Serializable]
    public class Attack
    {
        public AttackType attackType;
        public float duration;
        public float numberOfHits;
        public float size;
        public float range;
        public float damage;
        public Vector2 offset;
        public float pushForce;
        
        /// <summary>
        /// Temp Function
        /// </summary>
        public void DrawAttack(Transform direction, float intentDuration = 0.1f)
        {
            Vector3 offset3 = new Vector3(offset.x, 0, offset.y);
            switch (attackType)
            {
                case AttackType.Circle:
                    Debug.DrawRay(direction.position + offset3 + direction.right * size / 2, -direction.right * size, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    Debug.DrawRay(direction.position + offset3 + direction.forward * size / 2, -direction.forward * size, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    Debug.DrawRay(direction.position + offset3 + direction.right * size / 2 + direction.forward * size / 2, -(direction.right + direction.forward) * size, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    Debug.DrawRay(direction.position + offset3 + direction.right * size / 2 - direction.forward * size / 2, -(direction.right - direction.forward) * size, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    break;
                case AttackType.Rectangle:
                    Debug.DrawRay(direction.position + offset3 + direction.right * size / 2, direction.forward * range, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    Debug.DrawRay(direction.position + offset3 - direction.right * size / 2, direction.forward * range, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    break;
                case AttackType.Cone:
                    Debug.DrawRay(direction.position + offset3, Quaternion.Euler(0f, size / 2, 0f) * direction.forward * range, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    Debug.DrawRay(direction.position + offset3, Quaternion.Euler(0f, -size / 2, 0f) * direction.forward * range, intentDuration != 0.1f ? Color.black : Color.red, intentDuration, false);
                    break;
            }
        }
        
        public Attack(AttackType attackType, float duration, float numberOfHits, float size, float range, float damage, Vector2 offset, float pushForce)
        {
            this.attackType = attackType;
            this.duration = duration;
            this.numberOfHits = numberOfHits;
            this.size = size;
            this.range = range;
            this.damage = damage;
            this.offset = offset;
            this.pushForce = pushForce;
        }
    }
    
    [Serializable]
    public class Weapon
    {
        [Header("Movement")]
        public float speed;
        public float dashRange;
        public float dashCooldown;
        [Header("Block")] 
        public float blockAngle;
        public float parryWindow;
        [Header("Attacks")] 
        public Attack[] attacks;

        public Weapon()
        {
            speed = 50f;
            dashRange = 50f;
            dashCooldown = 1f;

            blockAngle = 90f;
            parryWindow = 0.25f;
            
            attacks = new Attack[] {
                new (AttackType.Rectangle, 0.3f, 2f, 2f, 5f, 1f, Vector2.zero, 10f), 
                new (AttackType.Cone, 0.3f, 2f, 90f, 5f, 1f, Vector2.zero, 10f), 
                new (AttackType.Cone, 0.5f, 8f, 60f, 8f, 1f, Vector2.zero, 50f)
            };
        }
    }

    [Header("Scripts")] 
    public PlayerCam playerCam;
    public PlayerMovement playerMovement;
    public RotateToPoint rotateToPoint;
    public WeaponManager weaponManager;
    public PlayerCombat playerCombat;

    [Header("Components")] 
    public Rigidbody rb;
    public Transform playerGraphics;
    public Camera playerCamera;
    
    public static Player m;

    void Start()
    {
        m = this;
    }
}
