﻿using UnityEngine;
using UnityEngine.UI;
public class TowerInfoPanel : MonoBehaviour {

    public static TowerInfoPanel Instance;
    public GameObject ThisPanel;
    public Text Type;
    public Text Level;
    public Text Hp;
    public Text DpmText;
    public Text DpmNum;
    public Text RTime;
    public Text RTimeNum;
    public Text UCost;
    public Text RCost;
    public Text SGain;
    public Text BCost;

    private int[] _upgradeTowerInfo;
    private int[] _towerInfo;
    private bool _displayUpgradedInfo;
    private int _upgradedHp;
    private bool _repairCase;

    private Tower.TowerType _type;
    private TankTower _tankTower;
    private RangeTower _rangeTower;
    private SlowTower _slowTower;
    private HealTower _healTower;
    private GoldTower _goldTower;
    private LevelManager _levelManager;
    private RepairButton _repairButton;

    void Awake()
    {
        Instance = this;
    }


    void Start () {
        ThisPanel.SetActive(false);
        _upgradeTowerInfo = new int[11];
        _towerInfo = new int[11];
        _levelManager = GameManager.Instance.CurrentLevelManager;
        _repairButton = RepairButton.Instance;
        _displayUpgradedInfo = false;
        _repairCase = false;
    }


    void LateUpdate()
    {
        // at beginning, no towerPtr is initialized, has to return directly
        if (GameBoard.GamePhase.BuildingPhase == _levelManager.CurrentGamePhase())
        {
            // if repair button is clicked in building phase, set current hp to color.green
            if (_repairCase)
            {
                Hp.text = "<color=#00ff00ff>" + _towerInfo[3] + "</color>/" + _towerInfo[4];
            }
            return;
        }
        // record a current selected tower, if no tower is selected at the beginning, _type is null
        int[] info = new int[11];
        switch (_type)
        {
            case Tower.TowerType.Tank:
                _tankTower.GetTowerInfo(info);
                break;
            case Tower.TowerType.Range:
                _rangeTower.GetTowerInfo(info);
                break;
            case Tower.TowerType.Slow:
                _slowTower.GetTowerInfo(info);
                break;
            case Tower.TowerType.Heal:
                _healTower.GetTowerInfo(info);
                break;
            case Tower.TowerType.Gold:
                _goldTower.GetTowerInfo(info);
                break;
        }
        // dynamically update current tower hp
        UpdateTowerCurrentHp(info[3], _towerInfo[4], info[2]);
        if (info[3] == info[4])
        {
            _repairButton.SetHpCheckFlag();
        }
        else
        {
            _repairButton.ResetHpCheckFlag();
        }
    }


    public void Appear()
    {
        ThisPanel.SetActive(true);
    }


    public void DisAppear()
    {
        ThisPanel.SetActive(false);
    }


    public void SetTankTower(TankTower tower)
    {
        _type = Tower.TowerType.Tank;
        _tankTower = tower;
    }


    public void SetRangeTower(RangeTower tower)
    {
        _type = Tower.TowerType.Range;
        _rangeTower = tower;
    }


    public void SetSlowTower(SlowTower tower)
    {
        _type = Tower.TowerType.Slow;
        _slowTower = tower;
    }


    public void SetHealTower(HealTower tower)
    {
        _type = Tower.TowerType.Heal;
        _healTower = tower;
    }


    public void SetGoldTower(GoldTower tower)
    {
        _type = Tower.TowerType.Gold;
        _goldTower = tower;
    }


    // when click on an existing tower, store this tower info and display the info
    public void SetTowerInfo(int[] info)
    {
        _repairCase = false;
        _displayUpgradedInfo = false;
        _towerInfo = info;
        switch (info[1])
        {
            case 0:
                Type.text = "Tank";
                DpmText.text = "Dpm";
                DpmNum.text = "" + (info[5] * 60 * 1.0 / info[6]);
                RTime.text = "Reload";
                RTimeNum.text = "" + info[6];
                break;
            case 3:
                Type.text = "Range";
                DpmText.text = "Dpm";
                DpmNum.text = "" + (info[5] * 60 * 1.0 / info[6]);
                RTime.text = "Reload";
                RTimeNum.text = "" + info[6];
                break;
            case 4:
                Type.text = "Slow";
                DpmText.text = "Slow";
                DpmNum.text = "" + info[5] + "%";
                RTime.text = "Reload";
                RTimeNum.text = "" + info[6];
                break;
            case 1:
                Type.text = "Heal";
                DpmText.text = "Hpm";
                DpmNum.text = "" + (info[5] * 60 * 1.0 / info[6]);
                RTime.text = "Reload";
                RTimeNum.text = "" + info[6];
                break;
            case 2:
                Type.text = "Gold";
                DpmText.text = "Gpm";
                DpmNum.text = "" + info[5];
                RTime.text = "Reload";
                RTimeNum.text = "" + info[6];
                break;
        }
        Level.text = "" + info[2];
        Hp.text = info[3] + "/" + info[4];
        UCost.text = "" + info[7] + "G";
        RCost.text = "" + info[8] + "G";
        SGain.text = "" + info[9] + "G";
        BCost.text = "" + info[10] + "G";
        _upgradedHp = _towerInfo[4];
    }


    // record a updated tower info
    public void SetUpgradedTowerInfo(int[] info)
    {
        _upgradeTowerInfo = info;
    }


    // when upgrade button is clicked, this function would be called to show updated info
    public void DisplayUpgradedInfo()
    {
        _repairCase = false;
        _displayUpgradedInfo = true;
        switch (_upgradeTowerInfo[1])
        {
            case 0:
                DpmText.text = "Dpm";
                DpmNum.text = "" + (_upgradeTowerInfo[5] * 60 * 1.0 / _upgradeTowerInfo[6]);
                break;
            case 3:
                DpmText.text = "Dpm";
                DpmNum.text = "" + (_upgradeTowerInfo[5] * 60 * 1.0 / _upgradeTowerInfo[6]);
                break;
            case 4:
                DpmText.text = "Slow";
                DpmNum.text = "" + _upgradeTowerInfo[5] + "%";
                break;
            case 1:
                DpmText.text = "Hpm";
                DpmNum.text = "" + (_upgradeTowerInfo[5] * 60 * 1.0 / _upgradeTowerInfo[6]);
                break;
            case 2:
                DpmText.text = "Gpm";
                DpmNum.text = "" + _upgradeTowerInfo[5];
                break;
        }
        Level.text = "" + _upgradeTowerInfo[2];
        int updatedCurHp = _upgradeTowerInfo[3] + _upgradeTowerInfo[4] / _upgradeTowerInfo[2] * (_upgradeTowerInfo[2] - 1);
        Hp.text = updatedCurHp + "/" + _upgradeTowerInfo[4];
        UCost.text = "" + _upgradeTowerInfo[7] + "G";
        RCost.text = "" + _upgradeTowerInfo[8] + "G";
        SGain.text = "" + _upgradeTowerInfo[9] + "G";
        _upgradedHp = _upgradeTowerInfo[4];
    }


    // when repair button and sell button are clicked, this function would be 
    // called  in case upgrade button early modify the displayed tower info 
    public void SetOriginalowerInfo()
    {
        _displayUpgradedInfo = false;
        switch (_towerInfo[1])
        {
            case 0:
                DpmText.text = "Dpm";
                DpmNum.text = "" + (_towerInfo[5] * 60 * 1.0 / _towerInfo[6]);
                break;
            case 3:
                DpmText.text = "Dpm";
                DpmNum.text = "" + (_towerInfo[5] * 60 * 1.0 / _towerInfo[6]);
                break;
            case 4:
                DpmText.text = "Slow";
                DpmNum.text = "" + _towerInfo[5] + "%";
                break;
            case 1:
                DpmText.text = "Hpm";
                DpmNum.text = "" + (_towerInfo[5] * 60 * 1.0 / _towerInfo[6]);
                break;
            case 2:
                DpmText.text = "Gpm";
                DpmNum.text = "" + _towerInfo[5];
                break;
        }
        Level.text = "" + _towerInfo[2];
        Hp.text = _towerInfo[3]  + "/" + _towerInfo[4];
        UCost.text = "" + _towerInfo[7] + "G";
        RCost.text = "" + _towerInfo[8] + "G";
        SGain.text = "" + _towerInfo[9] + "G";
    }


    // called every single frame when in battle phace to update current hp of a tower
    public void UpdateTowerCurrentHp(int curHp, int maxHp, int level)
    {
        if (_displayUpgradedInfo) // if upgrade button is clicked in battle phace
        {
            int updatedCurHp;
            if (1 == level)
            {
                updatedCurHp = curHp + maxHp;
            }
            else
            {
                updatedCurHp = curHp + maxHp / level * (level - 1);
            }
            Hp.text = updatedCurHp + "/" + _upgradedHp;
        }
        else if (_repairCase) // if repair button is clicked in battle phace
        {
            Hp.text = "<color=#00ff00ff>" + curHp + "</color>/" + _towerInfo[4];
        }
        else // sell button or no button is clicked in battoe phace
        {
            Hp.text = curHp + "/" + _towerInfo[4];
        }
    }


    // change upgrading tower info color to green
    public void SetUpgradingColor()
    {
        Level.color = new Color(0, 1, 0, 1);
        DpmNum.color = new Color(0, 1, 0, 1);
        Hp.color = new Color(0, 1, 0, 1);
        UCost.color = new Color(1, 0, 0, 1);
        RCost.color = new Color(1, 0, 0, 1);
        SGain.color = new Color(0, 1, 0, 1);
    }


    // change back all tower info color to white
    public void ResetTextColor()
    {
        Level.color = new Color(1, 1, 1, 1);
        DpmNum.color = new Color(1, 1, 1, 1);
        Hp.color = new Color(1, 1, 1, 1);
        UCost.color = new Color(1, 1, 1, 1);
        RCost.color = new Color(1, 1, 1, 1);
        SGain.color = new Color(1, 1, 1, 1);
    }


    public void RepairCase()
    {
        _repairCase = true;
    }

    // used for sellbutton when previous clicked button is repair button
    public void ResetRepairCase()
    {
        _repairCase = false;
    }
}
