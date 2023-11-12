using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    // Instance.
    public static ResourcesManager Instance { get; private set; }

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

    // 게임 플레이에 영향을 주는 자원들 관리, 음식, 의약품, 오염도, 인구수, 지지도.
    private float foods;
    private float medicines;
    private float pollution;
    private float population;
    private float approvalRating;
    // 자원 값 리턴.
    public float _food { get { return foods; } }
    public float _medicines { get { return medicines; } }
    public float _pollution { get { return pollution; } }
    public float _population { get { return population; } }
    public float _approvalRating { get { return approvalRating; } }

    
}
