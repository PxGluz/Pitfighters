using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Player.Weapon currentWeapon;

    void Start()
    {
        currentWeapon = new Player.Weapon();
    }
}
