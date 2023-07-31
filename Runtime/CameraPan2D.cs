using UnityEngine;
using Cinemachine;

namespace Meangpu
{
    public class CameraPan2D : MonoBehaviour
    {
        [Header("Cinemachine target for zoom")]
        [SerializeField]
        CinemachineVirtualCamera _cinemachineCam;

        [Header("Setting")]
        [SerializeField] Transform _targetTransform;
        [SerializeField] bool _useEdgeScroll = true;
        [SerializeField] bool _useDragPan = true;
        [SerializeField] bool _useKeyboard = true;
        [SerializeField] bool _allowMove = true;
        [SerializeField] bool _allowZoom = true;
        [Header("Speed")]
        [SerializeField] float _edgeSize = 20f;
        [SerializeField] float _panSpeed = 10f;
        [SerializeField] float _dragSpeed = 10;

        [Header("ZoomFOV")]
        [Tooltip("start zoom value")]
        [SerializeField] float _targetFOV = 10;
        [SerializeField] float _fovZoomSpeed = 5;
        [SerializeField] float _fovZoomSmoothFactor = 10;
        [SerializeField] float _fovZoomMax = 60;
        [SerializeField] float _fovZoomMin = 10;

        bool _isDragging;

        Vector2 _lastMousePos;
        Vector2 _mousePosDelta;
        Vector3 _moveDirection = new();
        Vector3 _inputDirection = new();

        private void Start()
        {
            if (_targetTransform == null) _targetTransform = transform;
        }

        private void Update()
        {
            if (_allowMove) HandleCamMovement();
            if (_allowZoom) HandleZoom();
        }

        private void HandleZoom()
        {
            if (Input.mouseScrollDelta.y < 0) _targetFOV += _fovZoomSpeed;// mouse up
            if (Input.mouseScrollDelta.y > 0) _targetFOV -= _fovZoomSpeed;// mouse down

            _targetFOV = Mathf.Clamp(_targetFOV, _fovZoomMin, _fovZoomMax);

            _cinemachineCam.m_Lens.OrthographicSize = Mathf.Lerp(_cinemachineCam.m_Lens.OrthographicSize, _targetFOV, Time.deltaTime * _fovZoomSmoothFactor);
        }

        private void HandleCamMovement()
        {
            _inputDirection.Set(0, 0, 0);

            if (_useKeyboard) HandleKeyboardInput();
            if (_useEdgeScroll) HandleEdgeScroll();
            if (_useDragPan) HandleDragMove();

            _moveDirection = (_targetTransform.up * _inputDirection.y) + (_targetTransform.right * _inputDirection.x);
            _targetTransform.transform.position += _moveDirection * _panSpeed * Time.deltaTime;
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKey(KeyCode.W)) _inputDirection.y = 1;
            if (Input.GetKey(KeyCode.S)) _inputDirection.y = -1;
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
                _inputDirection.y = _mousePosDelta.y * _dragSpeed;
                _lastMousePos = Input.mousePosition;
            }
        }

        private void HandleEdgeScroll()
        {
            if (Input.mousePosition.x < _edgeSize) _inputDirection.x = -1;
            if (Input.mousePosition.y < _edgeSize) _inputDirection.y = -1;
            if (Input.mousePosition.x > Screen.width - _edgeSize) _inputDirection.x = 1;
            if (Input.mousePosition.y > Screen.height - _edgeSize) _inputDirection.y = 1;
        }
    }
}
