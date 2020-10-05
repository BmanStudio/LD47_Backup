using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    ///By Xist3nce
    /// Hurts. Thats it.

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem hs;
        if (other.gameObject.GetComponent<HealthSystem>())
        {
            hs = other.gameObject.GetComponent<HealthSystem>();
            hs.TakeDamage(100);
        }
    }
}
