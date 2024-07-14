using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    private EnemyAnimation enemyAnimation;
    [SerializeField] protected float healthPoint = 10f;
    [SerializeField]protected int bounty = 50;
    protected virtual void Start()
    {
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
    }

    public void TakeDamage(float damage)
    {
        healthPoint -= damage;
        Debug.Log("Damage Taken");
        enemyAnimation.TriggerHit();
        if(healthPoint <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        WaveManager.DecreaseEnemyCount();
        Debug.Log("Enemy Killed");
        Resources.AddMoney(bounty);
    }
}
