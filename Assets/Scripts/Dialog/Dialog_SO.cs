using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog/New Dialog", order = 1)]
public class Dialog_SO : ScriptableObject
{
    public List<string> subDialogs;//please remember the index
    public List<Branch> branches;//The dialogs that branch in "forDialog", then use index for where it branchs to. please remember that it goes to the next one if no branch

}

[Serializable]
public class Branch {
    public int forDialog;
    public int qBranch;
    public int eBranch;
    public string qBranchText;
    public string eBranchText;
    public DialogEffect qEffect;
    public DialogEffect eEffect;

}

public enum DialogEffect {
    Nothing,EndTheLoopAfterBoss, SaveMore,EndConv
}