﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RangeTower : Tower
{

    public int[] AttackDamage;
    private float _loadingTime;
    private HashSet<Enemy> _enemies;


    void Start()
    {
        CurrentLevel = 0;
        DestroyByEnemy = false;
        _loadingTime = AttackSpeed[CurrentLevel];
        CurrentHp = HitPoint[CurrentLevel];
        _enemies = new HashSet<Enemy>();

        LevelManager = GameManager.Instance.CurrentLevelManager;
        NotificationPanel = NotificationPanel.Instance;
        TowerController = TowerController.Instance;
        TowerAnimator = GetComponent<Animator>();
    }


    void LateUpdate()
    {
        if (DestroyByEnemy) return;
        //Debug.Log("RT: Tower 1 searching");
        if (0 != _enemies.Count)
        {
            float attackS = (float)AttackSpeed[CurrentLevel] - 0.95f;
            if (_loadingTime >= attackS)
            {
                // todo check if target enemy is still alive from controller
                AttackEnemy(_enemies.First()); // make this simple, just attack the first one
                _loadingTime = 0.0f;
                _enemies.Clear();
            }
            else
            {
                _loadingTime += Time.deltaTime;
            }
        }
    }


    public override void AddEnemy(Enemy t)
    {
        if (DestroyByEnemy) return;
        //Debug.Log("RT: Enemy added");
        _enemies.Add(t);
        TowerController.AddEnemy(t);
    }


    public new void AttackEnemy(Enemy t)
    {
        if (DestroyByEnemy) return;
        //Debug.Log("RT: AttackEnemy~~~~~~~~~~~~~~~~~");
        if (TowerController.CheckIfEnemyAlive(t))
        {
            Vector3 start = transform.position;
            GameObject target = t.gameObject;
            Transform endTransform = target.transform;
            Vector3 end = endTransform.position;
            Color result = new Color(1, 0, 0, 1.0f);
            DrawLine(start, end, result);
            t.GetDamaged(AttackDamage[CurrentLevel]);
            //Debug.Log("RT: Attacks");
        }
        else
        {
            //Debug.Log("RT: Targeted enemy died");
        }
    }


    public new int GetAttackRange()
    {
        return AttackRange;
    }


    public new void GetTowerInfo(int[] info)
    {
        info[0] = AttackRange;
        info[1] = (int)Type;
        info[2] = CurrentLevel;
        info[3] = CurrentHp;
        info[4] = HitPoint[CurrentLevel];
        info[5] = AttackDamage[CurrentLevel];
        info[6] = AttackSpeed[CurrentLevel];
        info[7] = UpgradeCost;
        info[8] = RepairCost[CurrentLevel];
        info[9] = SellGain[CurrentLevel];
        info[10] = BuildCost;
    }
}
