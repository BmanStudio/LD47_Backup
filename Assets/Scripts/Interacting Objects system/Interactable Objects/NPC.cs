using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject
{
    public Dialog_SO dialog;
    public override string HintText => "Press F to talk";

    public override bool Interactable => true;

    public override void Interact()
    {
        DialogOpener.instance.StartDialog(dialog);
    }
}
