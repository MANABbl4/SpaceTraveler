using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class TrajectoryRenderer : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem Particles = null;

        [SerializeField]
        private float PointSize = 0.1f;

        [SerializeField]
        private Color PointColor = new Color(1f, 1f, 1f, 1f);

        private ParticleSystem.Particle[] Points = null;

        public void SetTrajectory(IEnumerable<Vector3> points, float size)
        {
            var pointsCount = points.Count();

            if (pointsCount > Points.Length)
            {
                Points = new ParticleSystem.Particle[pointsCount];
            }

            var i = 0;
            foreach (var point in points)
            {
                Points[i].position = point;
                Points[i].startSize = PointSize * size;
                Points[i].startColor = PointColor;
                ++i;
            }

            Particles.SetParticles(Points, pointsCount);
        }

        private void Awake()
        {
            Points = new ParticleSystem.Particle[1024];
        }

        public void Clear()
        {
            Particles.SetParticles(Points, 0);
        }
    }
}