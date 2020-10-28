using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeZoneTrigger : MonoBehaviour
{
    public TriggerEnterEvent enterUpgradeRange;
    public TriggerEnterEvent exitUpgradeRange;

    public class TriggerEnterEvent : UnityEvent<Collider> { } 

    private void OnTriggerEnter(Collider other)
    {
        enterUpgradeRange.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        exitUpgradeRange.Invoke(other);
    }
}
