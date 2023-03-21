using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class Star : MonoBehaviour, IGravityObject
    {
        public Action OnShipEnteredDangerZone;

        [SerializeField]
        private float Mass = 0;
        [SerializeField]
        private Color StarColor = new Color(0.8f, 0.65f, 0.3f, 1f);
        [SerializeField]
        private Color CoronaColor = new Color(0.8f, 0.35f, 0.1f, 1f);
        [SerializeField]
        private float Radius = 1f;
        [SerializeField]
        private Renderer TargetZone = null;
        [SerializeField]
        private Renderer DangerZone = null;
        [SerializeField]
        private Renderer TargetZoneProgress = null;
        [SerializeField]
        private float TimeToComplete = 0f;
        [SerializeField]
        private float TargetZoneMinRadius = 0f;
        [SerializeField]
        private float TargetZoneMaxRadius = 0f;
        [SerializeField]
        private float DangerZoneMinRadius = 0f;
        [SerializeField]
        private float DangerZoneMaxRadius = 0f;
        [SerializeField]
        private TimeController _timeController = null;
        [SerializeField]
        private Ship _playerShip = null;

        private Material _material = null;
        private float _size = 1f;
        private bool _visited = false;
        private float _visitTimer = 0f;

        public Vector3 Position { get { return gameObject.transform.position; } }

        private void Awake()
        {
            _size = gameObject.transform.localScale.x;
        }

        // Start is called before the first frame update
        private void Start()
        {
            _material = GetComponent<Renderer>().material;

            _material.SetColor("_StarColor", StarColor);
            _material.SetColor("_CoronaColor", CoronaColor);
            _material.SetFloat("_Size", gameObject.transform.localScale.x);
            _material.SetFloat("_Radius", Radius);

            TargetZone.gameObject.transform.localScale = new Vector3(TargetZoneMaxRadius * 2, TargetZoneMaxRadius * 2, TargetZoneMaxRadius * 2);
            TargetZone.material.SetFloat("_PlaneSize", TargetZoneMaxRadius * 2);
            TargetZone.material.SetFloat("_MinRadius", TargetZoneMinRadius);
            TargetZone.material.SetFloat("_MaxRadius", TargetZoneMaxRadius);

            DangerZone.gameObject.transform.localScale = new Vector3(DangerZoneMaxRadius * 2, DangerZoneMaxRadius * 2, DangerZoneMaxRadius * 2);
            DangerZone.material.SetFloat("_PlaneSize", DangerZoneMaxRadius * 2);
            DangerZone.material.SetFloat("_MinRadius", DangerZoneMinRadius);
            DangerZone.material.SetFloat("_MaxRadius", DangerZoneMaxRadius);

            TargetZoneProgress.gameObject.transform.localScale = new Vector3(TargetZoneMaxRadius * 2, TargetZoneMaxRadius * 2, TargetZoneMaxRadius * 2);
            TargetZoneProgress.material.SetFloat("_PlaneSize", TargetZoneMaxRadius * 2);
            TargetZoneProgress.material.SetFloat("_MinRadius", TargetZoneMinRadius);
            TargetZoneProgress.material.SetFloat("_MaxRadius", TargetZoneMinRadius);

            _visitTimer = 0f;
        }

        // Update is called once per frame
        private void Update()
        {
            var shipDist = (_playerShip.gameObject.transform.position - Position).magnitude;

            if (shipDist <= DangerZoneMaxRadius)
            {
                OnShipEnteredDangerZone?.Invoke();
            }

            if (!_visited && shipDist >= TargetZoneMinRadius && shipDist <= TargetZoneMaxRadius)
            {
                _visitTimer += _timeController.GetGameTimerDt();

                if (_visitTimer >= TimeToComplete)
                {
                    _visited = true;
                    _visitTimer = TimeToComplete;
                }

                TargetZoneProgress.material.SetFloat("_MaxRadius", TargetZoneMinRadius + (TargetZoneMaxRadius - TargetZoneMinRadius) * _visitTimer / TimeToComplete);
            }
        }

        public float GetMass()
        {
            return Mass;
        }

        public float GetRadius()
        {
            return Radius;
        }

        public float GetSize()
        {
            return _size;
        }

        public bool Visited()
        {
            return _visited;
        }
    }
}