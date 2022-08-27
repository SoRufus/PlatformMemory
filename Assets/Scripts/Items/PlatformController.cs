using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public bool Breakable { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (Breakable) Destroy(gameObject);
    }
}
