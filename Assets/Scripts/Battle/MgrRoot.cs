using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrRoot<T> : MonoBehaviour where T:MonoBehaviour
{
    public static T Instance;

    protected void Awake()
    {
        Instance = this as T;
    }
}
