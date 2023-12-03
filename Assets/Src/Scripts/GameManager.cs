using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Instance.
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Instance가 존재시 처리.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        // Scene이 변경되었을 때, 유지되도록.
        DontDestroyOnLoad(this);
    }
    // 기능 작성 예정
}
