using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public Text text, qOption, eOption;
    public Dialog_SO dialog;
    public PlayerController pc=null;
    Dictionary<int, Branch> branches;
    Branch branch = null;
    int dialogIndex = 0;
    public AudioSource audioSource;
    private Dictionary<int, VoiceOver> voiceOver;

    void Start()
    {
        //Quick trick becuse jam, still allowing to put there though
        text = transform.GetChild(0).gameObject.GetComponent<Text>();
        eOption = transform.GetChild(1).gameObject.GetComponent<Text>();
        qOption = transform.GetChild(2).gameObject.GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        if(dialog)
        SetDialog(dialog);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (branch == null || branch.eBranch == -1)
            {
                //If we dont know where to go next, we go next :)
                if (dialogIndex < dialog.subDialogs.Count)
                {
                    
                    SetDialogIndex(dialogIndex);
                }
                else
                {
                    gameObject.SetActive(false); 
                    pc.canTakeActions = true; //if we don't have what to say we say nothing :))))))
                }
                if (branch != null) {
                    ApplyEffect(branch.eEffect);
                }
            }
            else {

                ApplyEffect(branch.eEffect);
                SetDialogIndex(branch.eBranch);
            }
        }    
        if (Input.GetKeyDown(KeyCode.Q) && branch!=null && branch.qBranch!=-1) { // Thinking ahead here, if you use an effect that closes the dialog, it shouldn't matter which number as long as its not -1
            ApplyEffect(branch.qEffect);
            SetDialogIndex(branch.qBranch);
        }    
    }

    private void ApplyEffect(DialogEffect effect)
    {
        switch (effect)
        {
            case DialogEffect.Nothing:
                break;
            case DialogEffect.EndTheLoopAfterBoss:
                Debug.Log("Todo end loop");
                break;
            case DialogEffect.SaveMore:
                Debug.Log("Todo save more");
                break;
            case DialogEffect.EndConv:
                gameObject.SetActive(false);
                pc.canTakeActions = true;
                break;
            case DialogEffect.StartBossFight:
                SoundManager.instance.TransitionBackgroundMusic(SoundManager.BackgroundMusic.BossMusic);
                break;
        }
    }

    public void SetDialog(Dialog_SO d) {
        pc.canTakeActions = false;
        dialog = d;
        branches = new Dictionary<int, Branch>();
        foreach (Branch branch in dialog.branches)
        {
            branches.Add(branch.forDialog, branch);
        }

        voiceOver = new Dictionary<int, VoiceOver>();
        foreach (VoiceOver vo in dialog.voiceOvers)
        {
            voiceOver.Add(vo.forDialog, vo);
        }
        SetDialogIndex(0);
    }

    private void SetDialogIndex(int v)
    {

        text.text =  dialog.subDialogs[v];
        eOption.text = "[ E ] Next";
        qOption.text = "";
        if (voiceOver.ContainsKey(v)) {
            audioSource.clip = voiceOver[v].clip;
            audioSource.Play();
        }
        if (branches.ContainsKey(v))
        {
            branch = branches[v];
            if (branch.qBranchText.Length > 0)
                qOption.text = "[ Q ] " + branch.qBranchText;
            else branch.qBranch = -1;
            if (branch.eBranchText.Length > 0)
                eOption.text = "[ E ] " + branch.eBranchText;
            else branch.eBranch = -1;
        }
        else 
        {
            branch = null;
        }
        dialogIndex=v+1;
    }
}
