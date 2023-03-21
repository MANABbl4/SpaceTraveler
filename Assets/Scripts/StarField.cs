using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class StarField : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem Particles = null;

        [SerializeField]
        private float StarSizeMin = 0.05f;

        [SerializeField]
        private float StarSizeMax = 0.2f;

        [SerializeField]
        private int StarsCount = 1000;

        [SerializeField]
        private Vector2 From = new Vector2(-5f, -5f);

        [SerializeField]
        private Vector2 To = new Vector2(5f, 5f);

        [SerializeField]
        private Color MaxColorDelta = new Color(0.1f, 0.1f, 0.1f, 0.5f);

        private ParticleSystem.Particle[] Points = null;


        private void Awake()
        {
            Points = new ParticleSystem.Particle[StarsCount];
        }

        // Start is called before the first frame update
        private void Start()
        {
            for (int i = 0; i < StarsCount; ++i)
            {
                Points[i].position = new Vector3(Random.Range(From.x, To.x), Random.Range(From.y, To.y), 0f);
                Points[i].startSize = StarSizeMin + Random.Range(0f, 1f) * StarSizeMax;
                Points[i].startColor = new Color(1 - Random.Range(0f, 1f) * MaxColorDelta.r, 1 - Random.Range(0f, 1f) * MaxColorDelta.g, 1 - Random.Range(0f, 1f) * MaxColorDelta.b, 1 - Random.Range(0f, 1f) * MaxColorDelta.a);
            }

            Particles.SetParticles(Points, StarsCount);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}