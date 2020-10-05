using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOpener : MonoBehaviour
{
    public GameObject dp;
    public static DialogOpener instance; // Assuming the last one will always be the one we need, not using a constructor 

    void Awake()
    {
        instance = this;
    }


    public void StartDialog(Dialog_SO dialog) {
        dp.SetActive(true);
        dp.GetComponent<DialogPanel>().SetDialog(dialog);
    }
}
