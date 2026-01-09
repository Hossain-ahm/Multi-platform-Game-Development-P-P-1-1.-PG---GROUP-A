using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogScript : MonoBehaviour
{
    [SerializeField] List<DialogInstance> scene = new List<DialogInstance>();

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartScene(scene);
    }
}

[Serializable]
public class DialogInstance
{
    public string dialog;
    public characters character;
    public enum characters { nonad, player, Queen_Reina};

    public emotions tone;
    public enum emotions { normal, angry, excited};

    public UnityEvent action;
}
