using UnityEngine;
using System.Collections;
using System;

public class EnemyScript : NpcScript
{
    protected bool aggrod, canShoot;
    protected float range, moveSpeed, rotationSpeed, nextTime, interval;
    public int maxHealth, currentHealth, damage, expWorth, goldWorth, numLoot;
    protected float damageNumberY;
    protected int damageAmountTaken;
    protected DateTime damageTakenStartTime = DateTime.MinValue;
    
    
    public EnemyScript(string npcName, Texture2D texture, int maxHealth, int damage, int expWorth, int goldWorth, int numLoot) : base(npcName, texture) {
        this.npcName = npcName;
        this.texture = texture;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.expWorth = expWorth;
        this.goldWorth = goldWorth;
        this.numLoot = numLoot;
        DoInits();

    }

    protected new void DoInits()
    {
        initStats();
        initValues();
        initAbilities();
        getGameScript();
    }

    protected void initAbilities()
    {
        
    }

    protected void initStats()
    {
        nextTime = 0;
        interval = 0.2f;
        currentHealth = maxHealth;
        range = 10f;
        moveSpeed = 5f;
        rotationSpeed = 5f;
        aggrod = false;
        canShoot = true;
    }

    protected void getHit(Weapon weapon) {
        aggrod = true;
        loseHealth((int)weapon.getDamage());
    }

    public void loseHealth(int x)
    {
        currentHealth -= x;
        if(currentHealth < 0) {
            currentHealth = 0;
        }
        damageAmountTaken = x;
        damageTakenStartTime = DateTime.Now;
    }

    public void resetDamageTaken() {
        damageTakenStartTime = DateTime.MinValue;
        damageAmountTaken = 0;
    }

    public int getCurrentHealth() {
        return currentHealth;
    }

    public int getMaxHealth() {
        return maxHealth;
    }

    public bool isDead() {
        return currentHealth <= 0;
    }

    public float getDamageNumberY() {
        return damageNumberY;
    }

    public int getDamageAmountTaken() {
        return damageAmountTaken;
    }

    public int getDamageTakenTimeElapsed() {
        return DateTime.Now.Subtract(damageTakenStartTime).Milliseconds/10;
    }

    public int getDamage() {
        return damage;
    }

    public int getGoldWorth() {
        return goldWorth;
    }

    public int getExpWorth() {
        return expWorth;
    }

    public int getNumLoot() {
        return numLoot;
    }

    public string getTooltip() {
        return npcName + ": Max health = " + getMaxHealth();
    }

}
