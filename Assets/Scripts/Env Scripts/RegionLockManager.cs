using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Region
{
    public string name;
    public Collider regionCollider;
    public AudioClip ambientClip;
}

public class RegionLockManager : MonoBehaviour
{
    [SerializeField] private List<Region> regions = new();
    [SerializeField] private AudioSource playerAudio; // assign player's AudioSource here
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private DialogScript regionDialogue;

    private int unlockedRegions = 0;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        unlockedRegions = PlayerPrefs.GetInt("unlockedRegions", 0);

        for (int i = 0; i < regions.Count; i++)
        {
            //regions[i].regionCollider.enabled = i > unlockedRegions;
        }
    }

    public void UnlockRegion(int regionIndex)
    {
        if (regionIndex < 0 || regionIndex >= regions.Count) return;

        int currentUnlocked = PlayerPrefs.GetInt("unlockedRegions", 0);
        if (regionIndex > currentUnlocked)
        {
            PlayerPrefs.SetInt("unlockedRegions", regionIndex);
            PlayerPrefs.Save();
        }
    }

    public void RegionEntered(int regionIndex)
    {
        if (regionIndex < 0 || regionIndex >= regions.Count) return;
        if(regionIndex > PlayerPrefs.GetInt("unlockedRegions", 0))
        {
            regionDialogue.TriggerDialogue();
        }

        AudioClip targetClip = regions[regionIndex].ambientClip;
        if (playerAudio.clip == targetClip) return; // already playing

        // Start fading to the new audio
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeToClip(targetClip, fadeDuration));
    }

    private IEnumerator FadeToClip(AudioClip newClip, float duration)
    {
        float startVolume = playerAudio.volume;

        // Fade out
        float t = 0f;
        while (t < duration / 2f)
        {
            t += Time.deltaTime;
            playerAudio.volume = Mathf.Lerp(startVolume, 0f, t / (duration / 2f));
            yield return null;
        }

        // Switch clip
        playerAudio.clip = newClip;
        playerAudio.Play();

        // Fade in
        t = 0f;
        while (t < duration / 2f)
        {
            t += Time.deltaTime;
            playerAudio.volume = Mathf.Lerp(0f, startVolume, t / (duration / 2f));
            yield return null;
        }

        playerAudio.volume = startVolume;
        fadeCoroutine = null;
    }
}
