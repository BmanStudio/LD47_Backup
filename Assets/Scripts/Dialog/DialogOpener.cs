using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOpener : MonoBehaviour
{
    public GameObject dp;
    public void StartDialog(Dialog_SO dialog) {
        dp.SetActive(true);
        dp.GetComponent<DialogPanel>().SetDialog(dialog);
    }
}
