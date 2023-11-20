using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static Weapon;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponState CurrentState = WeaponState.idle;
    [SerializeField] public ProjectileType projectile;
    [SerializeField] public FiremodeType Firemode;
    [SerializeField] WeaponData weaponData;
    [SerializeField] public WeaponManager weaponManager;
    [SerializeField] public KickData weaponKickData;
    [SerializeField] public TrailRenderer BulletTrail;
    public void OnEnable()
    {
        weaponData.OnStart(this);
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        weaponData.HandleKick();
        switch (CurrentState)
        {
            case WeaponState.idle:
                CurrentState = weaponData.OnIdle();
                break;
            case WeaponState.firing:
                CurrentState = weaponData.OnFire(projectile,Firemode);
                break;
            case WeaponState.reloading:
                if (!weaponData.IsReloading && canReload()) StartCoroutine(Reload());
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void OnShot(Vector3 WeaponTip, Vector3 Direction, RaycastHit hit, bool didHit)
    {
        //Spawn Bullet Trail
        TrailRenderer trail = TrailRenderer.Instantiate(BulletTrail, WeaponTip, Quaternion.identity);

        if (didHit)
        {
            StartCoroutine(SpawnTrailWithHit(trail, hit));
        }
        else
        {
            StartCoroutine(SpawnTrailWithoutHit(trail,Direction));
        }
    }

    IEnumerator SpawnTrailWithHit(TrailRenderer trail, RaycastHit hit)
    {
        float t = 0;
        Vector3 Startpos = trail.transform.position;

        Vector3 Direction = (hit.point - Startpos).normalized;

        while (t < 5)
        {
            trail.transform.position = Vector3.MoveTowards(trail.transform.position, Startpos + Direction * hit.distance, weaponData.BulletVelocity * Time.deltaTime * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject,trail.time);
    }

    IEnumerator SpawnTrailWithoutHit(TrailRenderer trail,Vector3 Direction)
    {
        float t = 0;
        Vector3 Startpos = trail.transform.position;

        while (t < 5)
        {
            trail.transform.position = Vector3.MoveTowards(trail.transform.position, Startpos + Direction * weaponData.Range, weaponData.BulletVelocity * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        trail.transform.position = Startpos + Direction * weaponData.Range;
        Destroy(trail.gameObject, trail.time);
    }

    public void OnValidate()
    {
        weaponData.isBurst = Firemode == FiremodeType.Burst;
    }

    bool canReload() => weaponData.CurrentAmmo < weaponData.MaxAmmo;

    IEnumerator Reload()
    {
        weaponData.IsReloading = true;
        yield return new WaitForSeconds(weaponData.ReloadTime);
        weaponData.CurrentAmmo = weaponData.MaxAmmo;
        weaponData.IsReloading = false;
        CurrentState = WeaponState.idle;
    }

    [Serializable]
    public enum ProjectileType
    {
        Hitscan,
        AOEHitscan,
        Physics
    }
    [Serializable]
    public enum FiremodeType
    {
        Single,
        Fullauto,
        Burst
    }

    [Serializable]
    public enum WeaponState
    {
        idle,firing,reloading
    }

    [Serializable]
    public struct KickData
    {
        public float X;
        public float Y;
        public float Z;
        public Vector3 MaxKick;
        public float returnSpeed;
        public float snapiness;
    }

}

[Serializable]
public class WeaponData
{
    public float FireRate = 192;
    public float TimeSinceLastShot = 0f; 
    public float Damage;
    public float Range;
    public float ReloadTime;
    public int CurrentAmmo;
    public int MaxAmmo;
    [ShowIf("isBurst",true),SerializeField]public int BurstCount = 3;
    public int projectilesPerShot = 1;
    public Transform BarrelTip;
    public Vector2 WeaponInaccuracy;
    public float BulletVelocity = 25f;

    public Weapon thisWeapon;

    float CurrentInaccuracyModifier;
    [SerializeField] float BaseInaccuracy = 1f;
    [SerializeField] float ScopedInaccuracy = .4f;
    [HideInInspector] public bool IsReloading = false;
    [HideInInspector] public bool isBurst = false;
    public Weapon.WeaponState OnIdle()
    {
        if (TimeSinceLastShot > 0)TimeSinceLastShot -= Time.deltaTime;
        if (CurrentAmmo <= 0) return WeaponState.reloading;
        if (InputManager.isAttack1.performed || InputManager.isAttack1.started && TimeSinceLastShot <= 0) return WeaponState.firing;

        if (InputManager.isReloading.started || InputManager.isReloading.performed) return WeaponState.reloading;
        return WeaponState.idle;
    }

    public void OnStart(Weapon weapon)
    {
        thisWeapon = weapon;
    }

    public WeaponState OnFire(ProjectileType projectile,FiremodeType firemode)
    {
        switch (firemode)
        {
            case FiremodeType.Single:
                if (InputManager.IsAttack1Clicked)
                {
                    InputManager.IsAttack1Clicked = false;
                    TimeSinceLastShot = 60f / FireRate;
                    return OnFire(projectile);
                }
                break;
            case FiremodeType.Fullauto:
                TimeSinceLastShot = 60f / FireRate;
                return OnFire(projectile);
                break;
            case FiremodeType.Burst:
                TimeSinceLastShot = (60f / FireRate) * BurstCount;
                return OnBurst(0,projectile);
                break;
            default:
                return WeaponState.idle;
        }
        return WeaponState.idle;
    }

    public Weapon.WeaponState OnFire(ProjectileType projectile)
    {
        WeaponManager.OnWeaponShoot?.Invoke(thisWeapon);
        DoKick();
        switch (projectile)
        {
            case ProjectileType.Hitscan:
                return OnHitscan();
                break;
            case ProjectileType.AOEHitscan:
                return OnHitscanAOE();
                break;
            case ProjectileType.Physics:
                return OnPhysics();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return WeaponState.idle;
    }

    public WeaponState OnHitscan()
    {
        CurrentAmmo--;

        Ray ray = thisWeapon.weaponManager.camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));


        CurrentInaccuracyModifier = InputManager.isAttack2 ? ScopedInaccuracy : BaseInaccuracy;
        bool rayHit = Physics.Raycast(ray.origin, ray.direction + new Vector3(Random.Range(-WeaponInaccuracy.x * CurrentInaccuracyModifier, WeaponInaccuracy.x * CurrentInaccuracyModifier), Random.Range(-WeaponInaccuracy.y * CurrentInaccuracyModifier, WeaponInaccuracy.y * CurrentInaccuracyModifier), 0), out RaycastHit hit, Range, thisWeapon.weaponManager.damageableLayerMask);

        if (rayHit)
        {
            if (hit.transform.TryGetComponent(out IDamagable hitDamagable))
            {
                hitDamagable.OnDamage(hit.point,Damage,thisWeapon.weaponManager.Player.gameObject);
            }
        }
        thisWeapon.OnShot(BarrelTip.position,ray.direction,hit,rayHit);
        return WeaponState.idle;
    }
    public WeaponState OnHitscanAOE()
    {
        CurrentAmmo--;
        return WeaponState.idle;
    }

    public WeaponState OnPhysics()
    {
        CurrentAmmo--;
        return WeaponState.idle;
    }

    public WeaponState OnBurst(int currentShotCount,ProjectileType type)
    {
        if(CurrentAmmo <= 0) return WeaponState.idle;
        currentShotCount++;
        OnFire(type);
        if (currentShotCount == BurstCount) return WeaponState.idle;
        return OnBurst(currentShotCount,type);
    }

    //Position Vectors
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    public void HandleKick()
    {
        targetPosition = Vector3.Lerp(targetPosition,Vector3.zero, thisWeapon.weaponKickData.returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition,targetPosition, thisWeapon.weaponKickData.snapiness * Time.deltaTime);

        thisWeapon.weaponManager.weaponKick.localPosition = currentPosition;
    }

    public void DoKick()
    {
        KickData data = thisWeapon.weaponKickData;
        targetPosition -= new Vector3(Random.Range(0, data.X), Random.Range(0, data.Y), Random.Range(0, data.Z));
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, -data.MaxKick.x, data.MaxKick.x), Mathf.Clamp(targetPosition.y, -data.MaxKick.y, data.MaxKick.y), Mathf.Clamp(targetPosition.z, -data.MaxKick.z, data.MaxKick.z));
    }

}