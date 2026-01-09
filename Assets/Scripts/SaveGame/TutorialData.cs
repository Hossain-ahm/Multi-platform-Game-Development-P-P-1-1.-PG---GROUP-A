using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialData : MonoBehaviour
{
    private const string TutorialKey = "TutorialPlayed";
    [SerializeField] DialogScript tutorialDialogue;
    private void Start()
    {
        if (!PlayerPrefs.HasKey(TutorialKey))
        {
            tutorialDialogue.TriggerDialogue();
            PlayerPrefs.SetInt(TutorialKey, 1);
            PlayerPrefs.Save();
        }
    }
}
