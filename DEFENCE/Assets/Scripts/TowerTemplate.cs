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
        public string towerIdentity;
        public string towerSynergy;
        public Sprite sprite;
        public float critical;
        public float minDamage;
        public float maxDamage;
        public float slow;
        public float buff;
        public float rate;
        public float range;
        public int cost;
        public int sell;
    }
}
