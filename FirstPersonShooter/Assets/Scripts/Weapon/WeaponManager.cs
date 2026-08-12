using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Slots")]
    [SerializeField] private List<WeaponBase> weaponSlots = new List<WeaponBase>();

    [Header("Settings")]
    [SerializeField] private Transform weaponHolder; // پرنت همه اسلحه‌ها زیر دوربین
    [SerializeField] private float switchDelay = 0.4f; // زمان انیمیشن equip
    [SerializeField] InputActionReference changeWeapon;

    private int currentWeaponIndex = -1;    
    private bool isSwitching = false;

    public WeaponBase CurrentWeapon =>
        currentWeaponIndex >= 0 ? weaponSlots[currentWeaponIndex] : null;


    void OnEnable()
    {
        changeWeapon.action.Enable();
    }
    void OnDisable()
    {
        changeWeapon.action.Disable();
    }

    private void Start()    
    {
        // در ابتدا همه اسلحه‌ها رو خاموش کن
        foreach (var weapon in weaponSlots)
        {
            weapon.gameObject.SetActive(false);
        }

        if (weaponSlots.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    private void Update()
    {
        HandleSwitchInput();
    }

    private void HandleSwitchInput()
    {
        // سوییچ با عدد ۱ تا ۹
        for (int i = 0; i < weaponSlots.Count && i < 9; i++)
        {
            if (changeWeapon.action.IsPressed())
            {
                EquipWeapon(i);
            }
        }

        // سوییچ با اسکرول ماوس
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) SwitchNext();
        else if (scroll < 0f) SwitchPrevious();
    }

    public void SwitchNext()
    {
        if (weaponSlots.Count <= 1) return;
        int nextIndex = (currentWeaponIndex + 1) % weaponSlots.Count;
        EquipWeapon(nextIndex);
    }

    public void SwitchPrevious()
    {
        if (weaponSlots.Count <= 1) return;
        int prevIndex = (currentWeaponIndex - 1 + weaponSlots.Count) % weaponSlots.Count;
        EquipWeapon(prevIndex);
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weaponSlots.Count) return;
        if (index == currentWeaponIndex) return;
        if (isSwitching) return;

        StartCoroutine(SwitchWeaponRoutine(index));
    }

    private System.Collections.IEnumerator SwitchWeaponRoutine(int newIndex)
    {
        isSwitching = true;

        // مرحله ۱: مخفی کردن اسلحه فعلی (اگه وجود داشته باشه)
        if (currentWeaponIndex >= 0)
        {
            WeaponBase oldWeapon = weaponSlots[currentWeaponIndex];
            oldWeapon.OnUnequip();
            oldWeapon.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(switchDelay * 0.5f);

        // مرحله ۲: فعال کردن اسلحه جدید
        currentWeaponIndex = newIndex;
        WeaponBase newWeapon = weaponSlots[currentWeaponIndex];
        newWeapon.gameObject.SetActive(true);
        newWeapon.OnEquip();

        yield return new WaitForSeconds(switchDelay * 0.5f);

        isSwitching = false;
    }

    public void AddWeapon(WeaponBase weapon)
    {
        if (!weaponSlots.Contains(weapon))
        {
            weaponSlots.Add(weapon);
            weapon.gameObject.SetActive(false);
        }
    }

    public bool CanShoot()
    {
        return !isSwitching && CurrentWeapon != null;
    }
}