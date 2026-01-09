using UnityEngine;

public class BossArena : MonoBehaviour
{
    public BossAI boss;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (boss != null)
                boss.PlayerEnteredArena();
            Debug.Log("Player entered arena: Boss activated");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (boss != null)
                boss.PlayerExitedArena();
            Debug.Log("Player exited arena: Boss returning to idle");
        }
    }
}
