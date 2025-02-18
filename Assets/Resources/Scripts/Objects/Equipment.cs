using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Equipment {

    Dictionary<string, EquipmentItem> equipment;
    PlayerScript playerScript;

    public Equipment() {
        equipment = new Dictionary<string, EquipmentItem>();
        equipment.Add("Head", null);
        equipment.Add("Chest", null);
        equipment.Add("Legs", null);
        equipment.Add("Feet", null);
        equipment.Add("Weapon", null);
    }

    public Equipment(Dictionary<string, EquipmentItem> equipment) {
        this.equipment = equipment;
    }

    public bool equipItem(PlayerScript player, EquipmentItem item) {
        if(null == player) {
            player = getPlayer();
        }
        //if(player.meetsRequirements(item)) {
            if(equipment[item.getSlot()] != null) {
                EquipmentItem existingItem = equipment[item.getSlot()];
                if(!player.getInventory().addItem(existingItem)) { 
                    Debug.Log("Inventory full");
                    return false;
                }
            }
            equipment[item.getSlot()] = item;
            updateCharacterSheetEquipment(player);
            return true;
        //}
        //else { return false; }
    }

    public bool unequipItem(PlayerScript player, EquipmentItem item)
    {
        if(null == player) {
            player = getPlayer();
        }
        if (null != item && equipment[item.getSlot()] != null && equipment[item.getSlot()].Equals(item))
        {
            if (!player.getInventory().addItem(item))
            {
                Debug.LogError("Cannot unequip, inventory full");
                return false;
            }
            else
            {
                equipment[item.getSlot()] = null;
                updateCharacterSheetEquipment(player);
                return true;
            }
        }
        else
        {
            Debug.Log("This armor isn't equipped, can't unequip it");
        }
        return false;
    }

    public void updateCharacterSheetEquipment(PlayerScript player) {
        Dictionary<string, Transform> characterSheetEquipmentMap = UiManager.characterSheetEquipmentMap;

        foreach (KeyValuePair<string, EquipmentItem> entry in equipment) {
            EquipmentItem equippedItem = entry.Value;
            Transform equipmentSlot = characterSheetEquipmentMap[entry.Key];
            UiManager.clearExistingSlotsAndButtons(equipmentSlot);
            loadEquipmentSlot(equippedItem, entry, equipmentSlot);
        }
        player.updatePlayerSkillsSection();
    }

    async void loadEquipmentSlot(EquipmentItem equippedItem, KeyValuePair<string, EquipmentItem> entry, Transform equipmentSlot)
    {
        //Waiting while existing buttons get cleared
        await Task.Delay(UiManager.buttonClearDelayMillis); 

        if(null == equippedItem) {
            String resourceName = "Images/" + entry.Key + "Slot";
            Texture2D emptySlotTexture = (Texture2D)Resources.Load(resourceName);

            UiManager.Instance.CreateButtonInSlot(equipmentSlot, UiButton.ButtonType.PlayerMenuOption, "", Item.Rarity.None, 
                            emptySlotTexture, () => Debug.Log("Nothing equipped"), false, null);
        }
        else {
            UiManager.Instance.CreateButtonInSlot(equipmentSlot, UiButton.ButtonType.PlayerMenuOption, "", equippedItem.getRarity(), 
                            equippedItem.getIcon(), () => unequipItem(null, equippedItem), false, null);
        }
    }

    public List<Item> getItems() {
        List<Item> items = new List<Item>();
        foreach (Item item in equipment.Values) {
            items.Add(item);
        }
        return items;
    }

    public Dictionary<string, EquipmentItem> getItemMap() {
        return equipment;
    }

    protected PlayerScript getPlayer() {
        if(playerScript == null && null != GameObject.FindGameObjectWithTag("Player")) {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }
        return playerScript;
    }

    public float getTotalArmor() {
        float totalArmor = 0f;
        foreach(EquipmentItem equipment in equipment.Values)
        {
            if (null != equipment && equipment is Armor)
            {
                totalArmor += ((Armor)equipment).getArmorPower();
            }
        }
        return totalArmor;
    }
}
