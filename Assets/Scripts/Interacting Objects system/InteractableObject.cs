using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract string HintText { get; }
    public abstract bool Interactable { get; }
    public abstract void Interact();
}
