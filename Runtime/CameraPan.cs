using UnityEngine;
using Unity.Cinemachine;

namespace Meangpu
{
    public class CameraPan : MonoBehaviour
    {
        /// <summary>
        /// work with cinemachine use this as camera target,
        /// set cinemachine to "follow" and "look at" this object target
        /// to make it 45 degree change in "body offset" of cinemachine
        /// </summary>
        ///
        [Header("Cinemachine target for zoom")]
        [SerializeField]
        CinemachineCamera _cinemachineCam;

        [Header("Setting")]
        [SerializeField] Transform _targetTransform;
        [SerializeField] bool _useEdgeScroll = true;
        [SerializeField] bool _useDragPan = true;
        [SerializeField] bool _useKeyboard = true;
        [SerializeField] bool _allowRotation = true;
        [SerializeField] bool _allowMove = true;
        [SerializeField] bool _allowZoom = true;
        [Header("Speed")]
        [SerializeField] float _edgeSize = 20f;
        [SerializeField] float _panSpeed = 10f;
        [SerializeField] float _rotationSpeed = 180f;
        [SerializeField] float _dragSpeed = 10;

        [Header("Zoom")]
        [SerializeField] ZoomType _zoomType = ZoomType.MOVE_ClOSER;
        [Header("ZoomFOV")]
        [Tooltip("start zoom value")]
        [SerializeField] float _targetFOV = 60;
        [SerializeField] float _fovZoomSpeed = 5;
        [SerializeField] float _fovZoomSmoothFactor = 10;
        [SerializeField] float _fovZoomMax = 60;
        [SerializeField] float _fovZoomMin = 10;
        [Header("Zoom move")]
        [SerializeField] float _zoomMoveSpeed = 5;
        [SerializeField] float _zoomMoveMax = 50;
        [SerializeField] float _zoomMoveMin = 5;
        [SerializeField] float _ZoomMoveSmoothFactor = 10;
        [Header("Zoom lower y")]
        [SerializeField] float _zoomLowerYSpeed = 2;
        [SerializeField] float _zoomLowerYMax = 50;
        [SerializeField] float _zoomLowerYMin = 5;
        [SerializeField] float _ZoomLowerYSmoothFactor = 10;

        bool _isDragging;
        float _rotateDirection;
        Vector3 _zoomMoveDirection = new();
        Vector3 _zoomFollowOffset = new();

        Vector2 _lastMousePos;
        Vector2 _mousePosDelta;
        Vector3 _rotateVector = new();
        Vector3 _moveDirection = new();
        Vector3 _inputDirection = new();
        CinemachineFollow _cinemachineTranspose;

        private void Start()
        {
            if (_targetTransform == null) _targetTransform = transform;
            _cinemachineTranspose = _cinemachineCam.GetComponent<CinemachineFollow>();
            _zoomFollowOffset = _cinemachineTranspose.FollowOffset;
        }

        private void Update()
        {
            if (_allowMove) HandleCamMovement();
            if (_allowRotation) HandleCamRotate();
            if (_allowZoom) HandleZoom();
        }

        private void HandleZoom()
        {
            switch (_zoomType)
            {
                case ZoomType.FOV:
                    HandleCameraZoom_FOV();
                    break;
                case ZoomType.MOVE_ClOSER:
                    HandleCameraZoom_MoveForward();
                    break;
                case ZoomType.LOWER_Y_VALUE:
                    HandleCameraZoom_MoveYLower();
                    break;
                case ZoomType.ORTHO_ZOOM2D:
                    HandleCameraZoom_ORTHO();
                    break;
            }
        }

        private void HandleCameraZoom_MoveYLower()
        {
            // _zoomFollowOffset = _zoomFollowOffset.normalized;
            if (Input.mouseScrollDelta.y < 0) _zoomFollowOffset.y += _zoomLowerYSpeed * _zoomLowerYSpeed;
            if (Input.mouseScrollDelta.y > 0) _zoomFollowOffset.y -= _zoomLowerYSpeed * _zoomLowerYSpeed;

            _zoomFollowOffset.y = Mathf.Clamp(_zoomFollowOffset.y, _zoomLowerYMin, _zoomLowerYMax);

            _cinemachineTranspose.FollowOffset = Vector3.Lerp(_cinemachineTranspose.FollowOffset, _zoomFollowOffset, Time.deltaTime * _ZoomLowerYSmoothFactor);
        }

        private void HandleCameraZoom_MoveForward()
        {
            _zoomMoveDirection = _zoomFollowOffset.normalized;
            if (Input.mouseScrollDelta.y < 0) _zoomFollowOffset += _zoomMoveDirection * _zoomMoveSpeed; // mouse up
            if (Input.mouseScrollDelta.y > 0) _zoomFollowOffset -= _zoomMoveDirection * _zoomMoveSpeed; // mouse down

            if (_zoomFollowOffset.magnitude < _zoomMoveMin) _zoomFollowOffset = _zoomMoveDirection * _zoomMoveMin;
            if (_zoomFollowOffset.magnitude > _zoomMoveMax) _zoomFollowOffset = _zoomMoveDirection * _zoomMoveMax;

            _cinemachineTranspose.FollowOffset = Vector3.Lerp(_cinemachineTranspose.FollowOffset, _zoomFollowOffset, Time.deltaTime * _ZoomMoveSmoothFactor);
        }

        private void HandleCameraZoom_FOV()
        {
            if (Input.mouseScrollDelta.y < 0) _targetFOV += _fovZoomSpeed;// mouse up
            if (Input.mouseScrollDelta.y > 0) _targetFOV -= _fovZoomSpeed;// mouse down

            _targetFOV = Mathf.Clamp(_targetFOV, _fovZoomMin, _fovZoomMax);

            _cinemachineCam.Lens.FieldOfView = Mathf.Lerp(_cinemachineCam.Lens.FieldOfView, _targetFOV, Time.deltaTime * _fovZoomSmoothFactor);
        }

        private void HandleCameraZoom_ORTHO()
        {
            if (Input.mouseScrollDelta.y < 0) _targetFOV += _fovZoomSpeed;// mouse up
            if (Input.mouseScrollDelta.y > 0) _targetFOV -= _fovZoomSpeed;// mouse down

            _targetFOV = Mathf.Clamp(_targetFOV, _fovZoomMin, _fovZoomMax);

            _cinemachineCam.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCam.Lens.OrthographicSize, _targetFOV, Time.deltaTime * _fovZoomSmoothFactor);
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

            if (_useKeyboard) HandleKeyboardInput();
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