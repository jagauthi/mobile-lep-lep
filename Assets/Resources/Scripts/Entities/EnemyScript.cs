using UnityEngine;
using System.Collections;

public class EnemyScript : NpcScript
{
    protected bool aggrod, canShoot;
    protected float range, moveSpeed, rotationSpeed, nextTime, interval;
    public int maxHealth, currentHealth, damage, expWorth, goldWorth;
    
    
    public EnemyScript(string npcName, Texture2D texture) : base(npcName, texture) {
        this.npcName = npcName;
        this.texture = texture;
        DoInits();
    }

    protected new void DoInits()
    {
        basicInits();
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

    protected new void Update()
    {
        
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
    }

    public bool isDead() {
        return currentHealth <= 0;
    }

}
