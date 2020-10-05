using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject
{
    public Dialog_SO dialog;
    public bool isBoss = false;
    public override string HintText => "Press F to talk";

    public override bool Interactable => true;

    public override void Interact()
    {
        DialogOpener.instance.StartDialog(dialog);
        if (!isBoss) return;
        StartCoroutine(BossWaitForEndOfDialog(FindObjectOfType<DialogPanel>()));
    }

    IEnumerator BossWaitForEndOfDialog(DialogPanel dp) {
        yield return new WaitUntil(()=>!dp.gameObject.activeSelf);
        GetComponent<EnemyAttacker>().enabled = true;
        GetComponent<FieldOfView>().enabled = true;
        GetComponent<HealthSystem>().enabled = true;
        GetComponent<EnemyMovement>().enabled = true;
        GetComponent<Assets.Scripts.Actors.Enemy.EnemyAIBrain>().enabled = true;

        Destroy(this,0);

    }
}
