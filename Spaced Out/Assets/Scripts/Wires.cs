using UnityEngine;
using System.Collections;

public class Wires : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerController>())
        {
            LevelManager._instance.Restart();
        }
    }
}