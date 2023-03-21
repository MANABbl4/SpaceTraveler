using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class StarsBackground2 : MonoBehaviour, IStarsBackground
    {
        [SerializeField]
        private Camera _camera = null;

        [SerializeField]
        private float StarSizeMultiplier = 1f;

        [SerializeField]
        private float StarCount = 100f;

        [SerializeField]
        private Vector2 Seed = new Vector2(32.9898f, 78.233f);

        [SerializeField]
        private Color MinColor = new Color(155f, 176f, 255f, 255f);

        [SerializeField]
        private Color MaxColor = new Color(255f, 204f, 111f, 255f);

        private Material _material;
        private Resolution _resolution;
        private float _cameraAspect;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _material.SetFloat("_StarCount", StarCount);
            _material.SetFloat("_StarSizeMultiplier", StarSizeMultiplier);
            _material.SetVector("_Seed", new Vector4(Seed.x, Seed.y, 0, 0));
            _material.SetColor("_StarColorMin", MinColor);
            _material.SetColor("_StarColorMax", MaxColor);
        }

        // Start is called before the first frame update
        private void Start()
        {
            _cameraAspect = _camera.aspect;
            _resolution = Screen.currentResolution;

            CameraOrtographicSizeChanged();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Mathf.Approximately(_cameraAspect, _camera.aspect) ||
                _resolution.height != Screen.currentResolution.height ||
                _resolution.width != Screen.currentResolution.width)
            {
                _cameraAspect = _camera.aspect;
                _resolution.width = Screen.width;
                _resolution.height = Screen.height;

                CameraOrtographicSizeChanged();
            }
        }

        public void CameraOrtographicSizeChanged()
        {
            var scale = Vector2.one * _camera.orthographicSize * 2f;
            scale.x *= _camera.aspect;
            transform.localScale = scale;
        }

        public void Tick()
        {
            var cameraPos = _camera.transform.position;

            _material.SetVector("_Move", new Vector4(-cameraPos.x, -cameraPos.y, 0, 0));
        }
    }
}