using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LTTriggerEvent : LTMonoBehaviour
{
    public List<string> MatchTags = new List<string>();
    public UnityEvent<Collider> OnEnter = new UnityEvent<Collider>();
    public UnityEvent<Collider> OnExit = new UnityEvent<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (CanTrigger(other))
            OnEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (CanTrigger(other))
            OnExit.Invoke(other);
    }

    private bool CanTrigger(Collider other)
    {
        return MatchTags.Contains(other.tag);
    }

    private void OnValidate()
    {
        if (TryGetComponent<Collider>(out var collider))
            collider.isTrigger = true;
    }
}