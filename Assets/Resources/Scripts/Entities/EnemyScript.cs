using UnityEngine;
using System.Collections;
using System;

public class EnemyScript : NpcScript
{
    protected bool aggrod, canShoot;
    protected float range, moveSpeed, rotationSpeed, nextTime, interval;
    public int maxHealth, currentHealth, damage, expWorth, goldWorth;
    protected float damageNumberY;
    protected int damageAmountTaken;
    protected DateTime damageTakenStartTime = DateTime.MinValue;
    
    
    public EnemyScript(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        this.texture = texture;
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
        maxHealth = 100;
        currentHealth = 100;
        damage = 10;
        expWorth = 20;
        goldWorth = 10;
        range = 10f;
        moveSpeed = 5f;
        rotationSpeed = 5f;
        aggrod = false;
        canShoot = true;
    }

    protected void getHit(Weapon weapon) {
        aggrod = true;
        loseHealth(weapon.getDamage());
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

}
