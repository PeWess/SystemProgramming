using SolarSystemKritskiy_Main;
using SolarSystemKritskiy_Mechanics;
using SolarSystemKritskiy_Network;
using SolarSystemKritskiy_UI;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace SolarSystemKritskiy_Characters
{
    public class ShipController : NetworkMovableObject
    {
        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        protected override float _speed => _shipSpeed;

        [SyncVar] private string _playerName;
        [SerializeField] private Transform _cameraAttach;
        private CameraOrbit _cameraOrbit;
        private PlayerLabel _playerLabel;
        private float _shipSpeed;
        private Rigidbody _rb;

        private void OnGUI()
        {
            if (_cameraOrbit == null)
                return;
            
            _cameraOrbit.ShowPlayerLabels(_playerLabel);
        }

        public override void OnStartAuthority()
        {
            _rb = GetComponent<Rigidbody>();

            if (_rb == null)
                return;

            gameObject.name = _playerName;
            _cameraOrbit = ServiceLocator.Instance.CameraOrbit;
            _cameraOrbit.Initiate(_cameraAttach == null ? transform : _cameraAttach);
            _playerLabel = GetComponentInChildren<PlayerLabel>();
            base.OnStartAuthority();
        }

        private void Update()
        {
            Movement();
        }

        protected override void HasAuthorityMovement()
        {
            var spaceShipSettings = SettingsContainer.Instance?.SpaceShipSettings;

            if (spaceShipSettings == null)
                return;

            var isFaster = Input.GetKey(KeyCode.LeftShift);
            var speed = spaceShipSettings.ShipSpeed;
            var faster = isFaster ? spaceShipSettings.Faster : 1.0f;
            _shipSpeed = Mathf.Lerp(_shipSpeed, speed * faster,
                SettingsContainer.Instance.SpaceShipSettings.Acceleration);
            var currentFov = isFaster
                ? SettingsContainer.Instance.SpaceShipSettings.FasterFov
                : SettingsContainer.Instance.SpaceShipSettings.NormalFov;
            _cameraOrbit.SetFov(currentFov, SettingsContainer.Instance.SpaceShipSettings.ChangeFovSpeed);
            var velocity = _cameraOrbit.transform.TransformDirection(Vector3.forward) * _shipSpeed;
            _rb.velocity = velocity * Time.deltaTime;

            if (!Input.GetKey(KeyCode.C))
            {
                var targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(_cameraOrbit.LookAngle,
                    transform.right) * velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }
        }
        
        protected override void FromServerUpdate(){}
        protected override void SendToServer(){}

        [ClientCallback]
        private void LateUpdate()
        {
            _cameraOrbit?.CameraMovement();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name != "Crystal(Clone)")
                Death();
            else
                CrystalCollect(other);
        }

        private void Death()
        {
            var parts = GetComponentsInChildren<MeshRenderer>(true);
            Random rnd = new Random();
            
            foreach (var part in parts)
            {
                part.enabled = false;
            }
            
            transform.position =
                ServiceLocator.Instance.SpawnPoints[rnd.Next(0, ServiceLocator.Instance.SpawnPoints.Length)].position;
            
            foreach (var part in parts)
            {
                part.enabled = true;
            }
        }

        private void CrystalCollect(Collision other)
        {
            ServiceLocator.Instance.CrystalSpawns.Remove(other.gameObject.transform.position);
            other.gameObject.SetActive(false);
            if (ServiceLocator.Instance.CrystalSpawns.Count == 0)
            {
                Debug.Log("You win!");
            }
        }
    }
}