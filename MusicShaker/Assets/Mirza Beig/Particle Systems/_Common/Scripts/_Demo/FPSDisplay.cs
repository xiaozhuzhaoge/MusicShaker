
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.UI;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace ParticleSystems
    {

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================
            
            public class FPSDisplay : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                float timer;
                public float updateTime = 1.0f;

                int frameCount;
                float fpsAccum;

                // Display.

                Text fpsText;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {

                }

                // ...

                void Start()
                {
                    fpsText = GetComponent<Text>();

                }

                // ...

                void Update()
                {
                    frameCount++;
                    timer += Time.deltaTime;

                    fpsAccum += 1.0f / Time.deltaTime;

                    if (timer >= updateTime)
                    {
                        timer = 0.0f;

                        int fps = Mathf.RoundToInt(fpsAccum / frameCount);

                        fpsText.text = "Average FPS: " + fps;

                        frameCount = 0;
                        fpsAccum = 0.0f;
                    }
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

}

// =================================	
// --END-- //
// =================================

