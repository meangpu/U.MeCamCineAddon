using System;
using UnityEngine;

namespace Meangpu.Util
{
    public class CameraPan : MonoBehaviour
    {
        /// <summary>
        /// work with cinemachine use this as camera target,
        /// set cinemachine to "follow" and "look at" this object target
        /// to make it 45 degree change in "body offset" of cinemachine
        /// </summary>
        ///
        [SerializeField] Transform _targetTransform;
        [SerializeField] bool _useEdgeScroll = true;
        [SerializeField] bool _useDragPan = true;
        [SerializeField] float _edgeSize = 20f;
        [SerializeField] float _panSpeed = 10f;
        [SerializeField] float _rotationSpeed = 90f;
        [SerializeField] float _dragSpeed = 2;

        bool _isDragging;
        float _rotateDirection;
        Vector2 _lastMousePos;
        Vector2 _mousePosDelta;
        Vector3 _rotateVector = new();
        Vector3 _moveDirection = new();
        Vector3 _inputDirection = new();

        private void Start()
        {
            if (_targetTransform == null) _targetTransform = transform;
        }

        private void Update()
        {
            HandleCamMovement();
            HandleCamRotate();
            HandleCameraZoom();
        }

        private void HandleCameraZoom()
        {
            // if (Input.mouseScrollDelta.y > 0) _offset += _scrollZoomSpeed;// mouse up
            // if (Input.mouseScrollDelta.y < 0) _offset -= _scrollZoomSpeed;// mouse down
        }

        private void HandleCamRotate()
        {
            _rotateDirection = 0;
            if (Input.GetKey(KeyCode.Q)) _rotateDirection = 1;
            if (Input.GetKey(KeyCode.E)) _rotateDirection = -1;

            _rotateVector.Set(0, _rotateDirection * _rotationSpeed * Time.deltaTime, 0);
            _targetTransform.eulerAngles += _rotateVector;
        }

        private void HandleCamMovement()
        {
            _inputDirection.Set(0, 0, 0);
            HandleKeyboardInput();

            if (_useEdgeScroll) HandleEdgeScroll();
            if (_useDragPan) HandleDragMove();

            _moveDirection = (_targetTransform.forward * _inputDirection.z) + (_targetTransform.right * _inputDirection.x);
            _targetTransform.transform.position += _moveDirection * _panSpeed * Time.deltaTime;
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKey(KeyCode.W)) _inputDirection.z = 1;
            if (Input.GetKey(KeyCode.S)) _inputDirection.z = -1;
            if (Input.GetKey(KeyCode.A)) _inputDirection.x = -1;
            if (Input.GetKey(KeyCode.D)) _inputDirection.x = 1;
        }

        private void HandleDragMove()
        {
            // mouse 1 is right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
                _lastMousePos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1)) _isDragging = false;

            if (_isDragging)
            {
                _mousePosDelta = (Vector2)Input.mousePosition - _lastMousePos;
                _inputDirection.x = _mousePosDelta.x * _dragSpeed;
                _inputDirection.z = _mousePosDelta.y * _dragSpeed;
                _lastMousePos = Input.mousePosition;
            }
        }

        private void HandleEdgeScroll()
        {
            if (Input.mousePosition.x < _edgeSize) _inputDirection.x = -1;
            if (Input.mousePosition.y < _edgeSize) _inputDirection.z = -1;
            if (Input.mousePosition.x > Screen.width - _edgeSize) _inputDirection.x = 1;
            if (Input.mousePosition.y > Screen.height - _edgeSize) _inputDirection.z = 1;
        }
    }
}