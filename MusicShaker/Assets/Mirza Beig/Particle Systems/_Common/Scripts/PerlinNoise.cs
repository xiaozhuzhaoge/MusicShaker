
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        // =================================	
        // Classes.
        // =================================

        [System.Serializable]
        public class PerlinNoise
        {
            public void init()
            {
                // Don't make the range values too large, else floating point precision will result in jitter.

                offset.x = Random.Range(-32.0f, 32.0f);
                offset.y = Random.Range(-32.0f, 32.0f);
            }
            //public PerlinNoise()
            //{
            //    offset.x = Random.Range(0.0f, 99999.0f);
            //    offset.y = Random.Range(0.0f, 99999.0f);
            //}

            public float GetValue(float time)
            {
                float noiseTime = time * frequency;
                return (Mathf.PerlinNoise(noiseTime + offset.x, noiseTime + offset.y) - 0.5f) * amplitude;
            }

            Vector2 offset;

            public float amplitude = 1.0f;
            public float frequency = 1.0f;

            public bool unscaledTime;
        }

        // ...

        [System.Serializable]
        public class PerlinNoiseXYZ
        {
            public void init()
            {
                x.init();
                y.init();
                z.init();
            }

            public Vector3 GetXYZ(float time)
            {
                float frequencyScaledTime = time * frequencyScale;
                return new Vector3(x.GetValue(frequencyScaledTime), y.GetValue(frequencyScaledTime), z.GetValue(frequencyScaledTime)) * amplitudeScale;
            }

            public PerlinNoise x;
            public PerlinNoise y;
            public PerlinNoise z;

            public bool unscaledTime;

            public float amplitudeScale = 1.0f;
            public float frequencyScale = 1.0f;
        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
