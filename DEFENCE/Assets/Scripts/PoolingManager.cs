using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance; // 어디서든 접근 가능하게 싱글톤 설정

    // 프리팹 이름별로 큐(Queue)를 만들어 풀 관리
    private Dictionary<string, Queue<GameObject>> poolDict = new Dictionary<string, Queue<GameObject>>();

    private void Awake() => Instance = this;

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        string key = prefab.name;

        if (!poolDict.ContainsKey(key))
            poolDict.Add(key, new Queue<GameObject>());

        if (poolDict[key].Count > 0)
        {
            // 풀에 있으면 꺼내서 활성화
            GameObject obj = poolDict[key].Dequeue();
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 풀이 비었으면 새로 생성
            GameObject obj = Instantiate(prefab, pos, rot);
            obj.name = key; // 이름을 키값과 일치시킴
            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false); // 비활성화 후 풀에 저장
        poolDict[obj.name].Enqueue(obj);
    }
}