using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{
    public WeaponData data;

    private int currentAmmo;
    private int reserveAmmo;
    private float nextFireTime;
    private bool isReloading;
    private bool isFiring;

    [SerializeField] private Transform firePoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator weaponAnimator;

    [Header("Input")]
    [SerializeField] private InputActionReference fireAction;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private Animator gun;
    private WeaponManager weaponManager;

    private void Awake()
    {
        currentAmmo = data.magazineSize;
        reserveAmmo = data.maxReserveAmmo;
        weaponManager = GetComponentInParent<WeaponManager>();
    }

    private void OnEnable()
    {
        fireAction.action.Enable();
        reloadAction.action.Enable();

        fireAction.action.started += OnFireStarted;
        fireAction.action.canceled += OnFireCanceled;
        reloadAction.action.performed += OnReloadPerformed;
    }

    private void OnDisable()
    {
        fireAction.action.started -= OnFireStarted;
        fireAction.action.canceled -= OnFireCanceled;
        reloadAction.action.performed -= OnReloadPerformed;
    }

    private void OnFireStarted(InputAction.CallbackContext ctx)
    {
        isFiring = true;
        if (!data.isAutomatic)
            TryShoot(); // برای سلاح نیمه‌اتومات فقط یک بار شلیک
    }

    private void OnFireCanceled(InputAction.CallbackContext ctx)
    {
        isFiring = false;
    }

    private void OnReloadPerformed(InputAction.CallbackContext ctx)
    {
        if (currentAmmo < data.magazineSize && reserveAmmo > 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void Update()
    {
        if (weaponManager != null && !weaponManager.CanShoot()) return;

        // برای سلاح‌های اتوماتیک، هر فریم چک می‌کنیم
        if (data.isAutomatic && isFiring && !isReloading)
        {
            TryShoot();
        }
    }

    public void OnEquip()
    {
        isReloading = false;
        StopAllCoroutines();
        if (weaponAnimator != null)
            weaponAnimator.SetTrigger("Equip");
    }

    public void OnUnequip()
    {
        StopAllCoroutines();
        isReloading = false;
        isFiring = false;
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0 || isReloading) return;

        nextFireTime = Time.time + data.fireRate;
        currentAmmo--;
        Shoot();
    }

    private void Shoot()
    {
        gun.SetTrigger("isShoot");
        Vector3 direction = playerCamera.transform.forward;
        direction += playerCamera.transform.TransformDirection(
            Random.Range(-data.spread, data.spread),
            Random.Range(-data.spread, data.spread),
            0);

        if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit, data.range))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(data.damage);

            if (data.hitEffectPrefab != null)
                Instantiate(data.hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }

        if (data.muzzleFlashPrefab != null && firePoint != null)
            Instantiate(data.muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);

        if (weaponAnimator != null)
            weaponAnimator.SetTrigger("Fire");
    }

    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        if (weaponAnimator != null)
            weaponAnimator.SetTrigger("Reload");

        yield return new WaitForSeconds(data.reloadTime);

        int neededAmmo = data.magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;
        isReloading = false;
    }

    public int GetCurrentAmmo() => currentAmmo;
    public int GetReserveAmmo() => reserveAmmo;
    public string GetWeaponName() => data.weaponName;
}