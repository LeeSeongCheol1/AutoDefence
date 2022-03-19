using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private void Update() {
        transform.position += Vector3.up*0.3f;
    }

    public void DestoryEvent() {
        Destroy(gameObject);
    }
}
