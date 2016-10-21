using UnityEngine;
using System.Collections;

public class SideviewController : MonoBehaviour
{
    public GameObject target;

    private void LateUpdate()
    {
        Vector3 direction = target.transform.position - transform.position;
        transform.Translate(0f, direction.y, direction.z, Space.World);
    }
}
