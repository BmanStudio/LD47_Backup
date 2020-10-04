using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableRaycaster : MonoBehaviour
{
    public void Update()
    {
        var allHits = ShootRayCast();

        var ad = Interact(allHits);
        ChangePointerColor(allHits, ad);
    }

    private RaycastHit[] ShootRayCast()
    {
        var screenCenter = new Vector2(Screen.width, Screen.height) / 2;
        var ray = Camera.main.ScreenPointToRay(screenCenter);

        return Physics.RaycastAll(transform.position, transform.forward, 10);
    }

    private bool Interact(RaycastHit[] allHits)
    {
        foreach (var hit in allHits)
        {
            if (hit.transform.CompareTag("Player")) continue;
            if (!hit.transform.gameObject.GetComponent<InteractableObject>()) break;
            
            var intObject = hit.transform.gameObject.GetComponent<InteractableObject>();

            Debug.Log(intObject.HintText);
            // TODO: Show hintText on UI

            //TODO: replace with the reference to the Interact key
            if (Input.GetKeyDown(KeyCode.F)) intObject.Interact();

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
