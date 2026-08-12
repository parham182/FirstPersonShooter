using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public GameObject weaponPrefab;
    public Sprite icon;

    [Header("Shooting")]
    public float damage = 10f;
    public float fireRate = 0.1f; // فاصله بین شلیک‌ها
    public bool isAutomatic = true;
    public float range = 100f;
    public float spread = 0.02f; // پخش‌شدگی تیر

    [Header("Ammo")]
    public int magazineSize = 30;
    public int maxReserveAmmo = 90;
    public float reloadTime = 1.5f;

    [Header("Recoil")]
    public float recoilX = 2f;
    public float recoilY = 2f;

    [Header("Effects")]
    public GameObject muzzleFlashPrefab;
    public GameObject hitEffectPrefab;
    public AudioClip fireSound;
    public AudioClip reloadSound;
}