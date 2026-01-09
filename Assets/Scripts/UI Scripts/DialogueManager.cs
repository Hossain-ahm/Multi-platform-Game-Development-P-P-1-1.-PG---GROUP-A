using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText, nameText;
    [SerializeField] UnityEngine.UI.Image characterImage;
    [SerializeField] List<CharacterImageInstance> availableCharacters = new List<CharacterImageInstance>();
    [SerializeField] GameObject DialogueCanvas;
    [SerializeField] AudioSource typingSrc;

    List<DialogInstance> currentScene = new();
    int dialoguePointer = 0;
    bool textAnimating = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextLine();
        }
    }
    public void StartScene(List<DialogInstance> scene)
    {
        StopAllCoroutines();

        DialogueCanvas.SetActive(true);

        currentScene.Clear();
        currentScene.AddRange(scene);

        dialoguePointer = -1;
        textAnimating = false;
        typingSrc.Stop();

        NextLine();
    }
    public void EndScene()
    {
        DialogueCanvas.SetActive(false);
        dialoguePointer = -1;
    }

    public void NextLine()
    {
        if (textAnimating)
        {
            textAnimating = false;
            typingSrc.Stop();
        }
        else
        {
            if (dialoguePointer >= currentScene.Count - 1)
            {
                Debug.Log(dialoguePointer + ">=" + currentScene.Count);
                Debug.Log("ENDING SCENE");
                EndScene();
                return;
            }
            else
            {
                dialoguePointer++;
                DialogInstance dialogue = currentScene[dialoguePointer];
                //setting the UI image to the correct char
                foreach (CharacterImageInstance character in availableCharacters)
                {
                    if (character.name.ToLower() == dialogue.character.ToString().ToLower())
                        characterImage.sprite = character.pfp;
                }
                //typing the text
                nameText.text = dialogue.character.ToString();
                StopAllCoroutines();
                dialogueText.text = dialogue.dialog;
                dialogueText.ForceMeshUpdate();
                StartCoroutine(ScaleTextIn(dialogueText, dialogue));

                dialogue.action.Invoke();
            }
        }
    }

    IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    IEnumerator ScaleTextIn(TMP_Text text, DialogInstance dialogue)
    {
        textAnimating = true;
        typingSrc.Play();
        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;

        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        int charCount = textInfo.characterCount;

        float charDelay = 0.02f;
        float scaleTime = 0.05f;

        float[] startTimes = new float[charCount];
        for (int i = 0; i < charCount; i++)
            startTimes[i] = -1f;

        // Collapse all characters
        for (int i = 0; i < charCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;

            Vector3[] src = cachedMeshInfo[matIndex].vertices;
            Vector3[] dst = textInfo.meshInfo[matIndex].vertices;

            Vector3 center = (src[vertIndex] + src[vertIndex + 2]) * 0.5f;

            for (int v = 0; v < 4; v++)
                dst[vertIndex + v] = center;
        }

        PushMesh(text, textInfo);

        int visibleCount = 0;
        float nextCharTime = Time.unscaledTime;

        while (visibleCount < charCount || AnyCharStillAnimating(startTimes, scaleTime))
        {
            // Reveal next character
            if (visibleCount < charCount && Time.unscaledTime >= nextCharTime)
            {
                startTimes[visibleCount] = Time.unscaledTime;
                visibleCount++;
                nextCharTime += charDelay;
            }

            // Animate visible characters
            for (int i = 0; i < visibleCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int matIndex = charInfo.materialReferenceIndex;
                int vertIndex = charInfo.vertexIndex;

                Vector3[] src = cachedMeshInfo[matIndex].vertices;
                Vector3[] dst = textInfo.meshInfo[matIndex].vertices;

                float t = (Time.unscaledTime - startTimes[i]) / scaleTime;
                float scale = Mathf.Clamp01(t);

                Vector3 center = (src[vertIndex] + src[vertIndex + 2]) * 0.5f;

                Vector3 shakeOffset = Vector3.zero;
                if (dialogue.tone == DialogInstance.emotions.angry)
                {
                    float shakeAmount = 1f;
                    shakeOffset = new Vector3(UnityEngine.Random.Range(-shakeAmount, shakeAmount),
                                              UnityEngine.Random.Range(-shakeAmount, shakeAmount), 0);
                }
                for (int v = 0; v < 4; v++)
                    dst[vertIndex + v] =
                        (src[vertIndex + v] - center) * scale + center + shakeOffset;
            }

            if (dialogue.tone == DialogInstance.emotions.angry && visibleCount == charCount - 1)
            {
                StartCoroutine(ShakeText(text, cachedMeshInfo));
            }
            if (!textAnimating)
            {
                typingSrc.Stop();

                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    Vector3[] dst = textInfo.meshInfo[i].vertices;
                    Vector3[] src = cachedMeshInfo[i].vertices;
                    for (int v = 0; v < dst.Length; v++)
                        dst[v] = src[v];
                    //handling skip
                    if (dialogue.tone == DialogInstance.emotions.angry)
                    {
                        Debug.Log("SKIPSHAKE");
                        StartCoroutine(ShakeText(text, cachedMeshInfo));
                    }
                }
                PushMesh(text, textInfo);
                yield break;
            }
            PushMesh(text, textInfo);
            yield return null;
        }
        textAnimating = false;
        typingSrc.Stop();
        /*while (visibleCount == charCount)
        {
            if (dialogue.tone != DialogInstance.emotions.angry) break;

            for (int i = 0; i < visibleCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int matIndex = charInfo.materialReferenceIndex;
                int vertIndex = charInfo.vertexIndex;

                Vector3[] src = cachedMeshInfo[matIndex].vertices;
                Vector3[] dst = textInfo.meshInfo[matIndex].vertices;

                Vector3 shakeOffset = Vector3.zero;
                Vector3 center = (src[vertIndex] + src[vertIndex + 2]) * 0.5f;

                if (dialogue.tone == DialogInstance.emotions.angry)
                {
                    float shakeAmount = 1f;
                    shakeOffset = new Vector3(UnityEngine.Random.Range(-shakeAmount, shakeAmount),
                                              UnityEngine.Random.Range(-shakeAmount, shakeAmount), 0);
                }
                for (int v = 0; v < 4; v++)
                    dst[vertIndex + v] =
                        (src[vertIndex + v] - center)  + center + shakeOffset;
            }
        }*/
    }

    IEnumerator ShakeText(TMP_Text text, TMP_MeshInfo[] cachedMeshInfo)
    {
        TMP_TextInfo textInfo = text.textInfo;
        int charCount = textInfo.characterCount;

        while (true)
        {
            for (int i = 0; i < charCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int matIndex = charInfo.materialReferenceIndex;
                int vertIndex = charInfo.vertexIndex;

                Vector3[] src = cachedMeshInfo[matIndex].vertices;
                Vector3[] dst = textInfo.meshInfo[matIndex].vertices;

                Vector3 center = (src[vertIndex] + src[vertIndex + 2]) * 0.5f;

                float shakeAmount = 1f;
                Vector3 shakeOffset = new Vector3(
                    UnityEngine.Random.Range(-shakeAmount, shakeAmount),
                    UnityEngine.Random.Range(-shakeAmount, shakeAmount),
                    0);

                for (int v = 0; v < 4; v++)
                    dst[vertIndex + v] = src[vertIndex + v] + shakeOffset;
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    bool AnyCharStillAnimating(float[] startTimes, float scaleTime)
    {
        float now = Time.unscaledTime;
        for (int i = 0; i < startTimes.Length; i++)
            if (startTimes[i] >= 0 && now - startTimes[i] < scaleTime)
                return true;
        textAnimating = false;
        return false;
    }

    void PushMesh(TMP_Text text, TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}

[Serializable]
public class CharacterImageInstance
{
    public Sprite pfp;
    public string name;
}
