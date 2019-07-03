
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
        
        public class TransformNoise : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public PerlinNoiseXYZ positionNoise;
            public PerlinNoiseXYZ rotationNoise;

            public bool unscaledTime;

            float time;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Start()
            {
                positionNoise.init();
                rotationNoise.init();
            }

            // ...

            void Update()
            {
                time = !unscaledTime ? Time.time : Time.unscaledTime;

                // I use Time.deltaTime vs. Time.time so that it starts off centered.
                // LEL, makes no difference.

                //time += !unscaledTime ? Time.deltaTime : Time.unscaledDeltaTime;

                transform.localPosition = positionNoise.GetXYZ(time);
                transform.localEulerAngles = rotationNoise.GetXYZ(time);
            }

            // =================================	
            // End functions.
            // =================================

        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
