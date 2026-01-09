using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TPtoBoss : MonoBehaviour
{
    public DialogScript dialogue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("unlockedRegions", 0) == 2)
            {
                SceneManager.LoadScene(1);
            }
            else dialogue.TriggerDialogue();
        }
    }
}
