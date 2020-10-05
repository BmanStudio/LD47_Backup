using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableRaycaster : MonoBehaviour
{
   [SerializeField] [Range(0,20)] float interactionDistance=2f;
    [SerializeField] Text GUIHintText = null;
    public void Update()
    {
        var allHits = ShootRayCast();

        var ad = Interact(allHits);
        if (!ad && GUIHintText) {
            GUIHintText.text = "";
        }
        ChangePointerColor(allHits, ad);
    }

    private RaycastHit[] ShootRayCast()
    {
        Debug.DrawRay(transform.position, transform.forward.normalized* interactionDistance, Color.green);
        
        return Physics.RaycastAll(transform.position, transform.forward.normalized, interactionDistance);
    }

    private bool Interact(RaycastHit[] allHits)
    {
        foreach (var hit in allHits)
        {
            if (hit.transform.CompareTag("Player")) continue;
            var intObject = hit.transform.gameObject.GetComponent<InteractableObject>();
            if (!intObject) continue;

            if(GUIHintText)
            GUIHintText.text = intObject.HintText;
            

            //TODO: replace with the reference to the Interact key
            if (Input.GetKeyDown(KeyCode.F))
            {
                intObject.Interact();

            }


            return intObject.Interactable;
        }

        return false;
    }

    private void ChangePointerColor(RaycastHit[] allHits, bool additional)
    {
        var foundTag = "";
        foreach (var hit in allHits)
        {
            if (hit.transform.CompareTag("Player")) continue;

            foundTag = hit.transform.tag;
            break;
        }

        switch (foundTag)
        {
            case "Enemy":
                CenterDot.ChangeColor(CenterDot.EnemyColor);
                break;
            case "NPC":
                CenterDot.ChangeColor(CenterDot.SpeakColor);
                break;
            case "InteractableObject":
                CenterDot.ChangeColor(additional ? CenterDot.InteractColor : CenterDot.DisabledColor);
                break;
            default:
                CenterDot.ChangeColor(CenterDot.NormalColor);
                break;
        }
    }
}
