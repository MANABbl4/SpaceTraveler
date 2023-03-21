using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class StarsBackground : MonoBehaviour, IStarsBackground
    {
        [System.Serializable]
        public class StarPlane
        {
            [SerializeField]
            public Material material;

            [SerializeField]
            public float speed;
        }

        [SerializeField]
        private List<StarPlane> _planes = null;

        [SerializeField]
        private Camera _camera = null;

        [SerializeField]
        private float _cameraOrtographicSizeMultiplier = 2f;

        private Vector3 _cameraPreviousPosition;

        // Start is called before the first frame update
        private void Start()
        {
            _cameraPreviousPosition = _camera.transform.position;

            CameraOrtographicSizeChanged();

            foreach (var plane in _planes)
            {
                plane.material.mainTextureOffset = Vector2.zero;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            var cameraPos = _camera.transform.position;
            var cameraDelta = cameraPos - _cameraPreviousPosition;
            transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);

            foreach (var plane in _planes)
            {
                var offset = plane.material.mainTextureOffset + new Vector2(cameraDelta.x * plane.speed, cameraDelta.y * plane.speed);
                offset.x -= Mathf.Floor(offset.x);
                offset.y -= Mathf.Floor(offset.y);
                plane.material.mainTextureOffset = offset;
            }

            _cameraPreviousPosition = cameraPos;

        }

        public void CameraOrtographicSizeChanged()
        {
            transform.localScale = Vector2.one * _camera.orthographicSize * _cameraOrtographicSizeMultiplier;
        }
    }
}