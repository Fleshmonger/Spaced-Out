using UnityEngine;
using System.Collections;

public class Wires : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        LevelManager._instance.Restart();
    }
}