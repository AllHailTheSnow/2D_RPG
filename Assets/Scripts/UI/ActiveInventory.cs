using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    private void Awake()
    {
        //sets the player controls to a new player controls object
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        //subscribes to the inventory keyboard event and calls the ToggleActiveSlot function with the value of the context
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        //enables the player controls
        playerControls.Enable();
    }

    private void ToggleActiveSlot(int numValue)
    {
        //toggles the active highlight based on the value of the number passed in minus 1
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        //sets the active slot index number to the index number passed in
        activeSlotIndexNum = indexNum;

        //loops through all the children of the inventory and sets the child at the index number to active and the rest to inactive
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        //sets the child at the index number to active
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        //Debug.Log(transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab.name);

        //if the active weapon is not null, destroy the active weapon
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        //if the child at the active slot index number does not have an inventory slot component, set the active weapon to null
        if (transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().GetWeaponInfo() == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        ////creates a new weapon based on the weapon prefab of the child at the active slot index number
        //GameObject weaponSpawn = transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab;

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        //instantiates the weapon spawn at the active weapon position
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

        //sets the rotation of the new weapon to 0, 0, 0
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);

        //sets the parent of the new weapon to the active weapon
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        //sets the new weapon to the current active weapon
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
