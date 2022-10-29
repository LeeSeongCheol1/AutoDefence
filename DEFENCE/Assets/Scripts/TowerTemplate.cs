using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject followTowerPrefab;
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public string unitSynergy;  //유닛특성
        public string speciesIdentity;  //종족특성
        public string individualSynergy;    //개체특성
        public Sprite sprite;
        public float critical;
        public float minDamage;
        public float maxDamage;
        public float slow;
        public float buff;
        public float rate;
        public float range;
        public int HP;
        public int cost;
        public int sell;
    }
}
