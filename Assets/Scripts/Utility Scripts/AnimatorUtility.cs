using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUtility : MonoBehaviour
{
    public GameObject[] objPool;
    public void setObjActive(int index)
    {
        objPool[index].SetActive(true);
    }
    public void setObjInctive(int index)
    {
        objPool[index].SetActive(false);
    }
}
