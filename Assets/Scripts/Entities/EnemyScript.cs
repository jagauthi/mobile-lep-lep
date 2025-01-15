using UnityEngine;
using System.Collections;

public class EnemyScript : NpcScript
{
    protected bool aggrod, canShoot;
    protected float range, moveSpeed, rotationSpeed, nextTime, interval;
    public int maxHealth, currentHealth, damage, expWorth, goldWorth;
    protected Rigidbody fireball;

    protected const float maxInvuln = 0.3f;
    
    protected new void Start()
    {
        npcName = "Enemy";
        basicInits();
        initStats();
        initValues();
        initAbilities();
        getGameScript();
    }

    protected void initAbilities()
    {
        GameObject holder = (GameObject)Resources.Load("Prefabs/Fireball");
        fireball = holder.GetComponent<Rigidbody>();
    }

    protected void initStats()
    {
        nextTime = 0;
        interval = 0.2f;
        maxHealth = 100;
        currentHealth = 100;
        damage = 10;
        expWorth = 10;
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

    protected void loseHealth(int x)
    {
        currentHealth -= x;
    }

}
