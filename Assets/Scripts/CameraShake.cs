using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;
    private float _shakeAmount = 0;

    void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    public void Shake(float _amount, float _length)
    {
        _shakeAmount = _amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
    }

    void BeginShake()
    {

    }

    void StopShake()
    {

    }

}
