using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

    protected GameObject weaponGameObject;

    int baseDamage, addedDamage;

    public Weapon(string baseName, string type, Texture2D icon, int cost, Rarity rarity) 
    : base(baseName, type, icon, cost, rarity) {
        basicInits();
    }

    public void basicInits() {
        loadWeaponInfo(baseName);
    }
    
    void loadWeaponInfo(string baseName) {
        baseDamage = 10;
        addedDamage = 0;
    }

    public int getDamage() {
        return baseDamage + addedDamage;
    }

    public void setBaseDamage(int baseDamage) {
        this.baseDamage = baseDamage;
    }

    public void setAddedDamage(int addedDamage) {
        this.addedDamage = addedDamage;
    }
}
