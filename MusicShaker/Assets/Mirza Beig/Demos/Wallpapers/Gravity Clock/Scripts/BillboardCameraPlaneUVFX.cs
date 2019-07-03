
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

    namespace Demos
    {

        namespace ParticlePlayground
        {

            // =================================	
            // Classes.
            // =================================
            
            public class BillboardCameraPlaneUVFX : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                Transform cameraTransform;

                // =================================	
                // Functions.
                // =================================

                void Awake()
                {

                }

                // ...

                void Start()
                {
                    cameraTransform = Camera.main.transform;
                }

                // ...

                void Update()
                {


                }

                // ...

                void LateUpdate()
                {
                    transform.forward = -cameraTransform.forward;
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
