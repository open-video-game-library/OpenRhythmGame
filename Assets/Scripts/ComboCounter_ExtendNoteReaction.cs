using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HolmonUtility;
using MusicGameEngine;
using System;
using TMPro;

public class ComboCounter_ExtendNoteReaction : ExtendNoteReactionBase
{
    [Serializable]
    public class ComboText
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI comboText;

        public void SetCombo(int combo)
        {
            comboText.text = combo.ToString();
            animator.SetTrigger("CountUp");
        }

        public void ResetCombo()
        {
            animator.SetTrigger("Hide");
        }
    }

    public int currentCombo { get; private set; } = 0;
    public int maxCombo { get; private set; } = 0;

    [SerializeField] private ComboText comboText;

    public override void ExtendJudgeCallBack(int judge, ScoreData.Note note, float diff)
    {
        if (!(judge == 0 || judge == 1))
        {
            currentCombo++;
            if (currentCombo > maxCombo)
            {
                maxCombo = currentCombo;
            }

            comboText.SetCombo(currentCombo);
        }
        else
        {
            currentCombo = 0;
            comboText.ResetCombo();
        }
    }

    //Žg‚í‚È‚¢
    public override void ExtendlInputOffProgress(ScoreData.Note note)
    {

    }

    //Žg‚í‚È‚¢
    public override void ExtendlInputOnProgress(ScoreData.Note note)
    {

    }
}
