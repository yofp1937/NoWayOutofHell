using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : Interactable
{
    public UnityEvent OnInteract;

    protected override void Interact()
    {
        if(OnInteract != null)
        {
            Debug.Log("Interacted with " + gameObject.name);
            OnInteract.Invoke();
        }
    }
}
