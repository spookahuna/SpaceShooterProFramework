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

    //Test to check if Camera Shake works. Delete after testing.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(0.1f, 0.2f);
        }
    }

    public void Shake(float _amount, float _length)
    {
        _shakeAmount = _amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", _length);
    }

    void BeginShake()
    {
        if (_shakeAmount > 0)
        {
            Vector3 _cameraPosition = _mainCamera.transform.position;
            
            float _axisOffsetAmountX = Random.value * _shakeAmount * 2 - _shakeAmount;
            float _axisOffsetAmountY = Random.value * _shakeAmount * 2 - _shakeAmount;
            _cameraPosition.x += _axisOffsetAmountX;
            _cameraPosition.y += _axisOffsetAmountY;

            _mainCamera.transform.position = _cameraPosition;
        }
    }

    void StopShake()
    {
        CancelInvoke("BeginShake");
        _mainCamera.transform.localPosition = Vector3.zero;
    }

}
