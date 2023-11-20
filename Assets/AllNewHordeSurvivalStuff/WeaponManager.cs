using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class WeaponManager : MonoBehaviour
{
    public static UnityEvent<Weapon> OnWeaponShoot = new UnityEvent<Weapon>();

    [SerializeField] public Transform Player;
    [SerializeField] public LayerMask damageableLayerMask;
    [SerializeField] public Camera camera;
    [SerializeField] Weapon CurrentWeapon;
    [SerializeField] public Transform weaponKick;

    [SerializeField] float TurnSmooth = 2f;
    // Start is called before the first frame update
    void Start()
    {
        CurrentWeapon.weaponManager = this;
        OnWeaponShoot.AddListener(OnShoot);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentWeapon.OnUpdate();
    }

    void OnShoot(Weapon weapon)
    {
        Quaternion Target = camera.transform.rotation;

        Target.x = 0;
        Target.z = 0;

        Target = Quaternion.Lerp(Player.transform.rotation,Target, TurnSmooth * Time.deltaTime);

        Player.transform.rotation = Target;
    }

}
