using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Serializable]
    public class ParralaxItem
    {
        public Transform Transform;
        public float Strength;
    }
    
    public Transform Target;

    [SerializeField] private ParralaxItem[] _parralaxItems;
    
    [SerializeField] private float smoothTime;
    
    [SerializeField] private float hardLeftX;
    [SerializeField] private float hardRightX;

    [SerializeField] private float viewportDeadzoneLeft;
    [SerializeField] private float viewportDeadzoneRight;

    private Camera _camera;
    private Vector2 _target;
    private Vector2 velocity = Vector2.zero;

    private float _targetZ;
    private float _targetY;
    private Vector3 _positionResult;
    
    private Vector3 _lastCameraPosition;
    private void Awake()
    {
        _targetZ = transform.position.z;
        _targetY = transform.position.y;
        _target = Target.position;
        _camera = GetComponent<Camera>();
    }
    
    private void FixedUpdate()
    {
        var viewportPosition = _camera.WorldToViewportPoint(Target.position);

        if (viewportPosition.x <= viewportDeadzoneLeft)
        {
            _target += Vector2.right * (viewportPosition.x - viewportDeadzoneLeft);
        } else if (viewportPosition.x >= viewportDeadzoneRight)
        {
            _target += Vector2.right * (viewportPosition.x - viewportDeadzoneRight);
        }

        if (_target.x <= hardLeftX)
        {
            _target = new Vector2(hardLeftX, 0);
        } else if (_target.x >= hardRightX)
        {
            _target = new Vector2(hardRightX, 0);
        }
        
        _positionResult = transform.position;
        _positionResult = Vector2.SmoothDamp(_positionResult, _target, ref velocity, smoothTime);
        _positionResult.z = _targetZ;
        _positionResult.y = _targetY;

        _lastCameraPosition = transform.position;
        transform.position = _positionResult;

        ApplyParralax();
    }

    private void ApplyParralax()
    {
        var delta = transform.position - _lastCameraPosition;
        
        foreach (var item in _parralaxItems)
        {
            item.Transform.position += delta * item.Strength;
        }
    }
}
