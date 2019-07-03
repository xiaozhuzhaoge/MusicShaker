
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

        // =================================	
        // Classes.
        // =================================

        public class MouseRotateCamera : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public float maxRotation = 5.0f;
            public float speed = 2.0f;

            public bool unscaledTime;

            // =================================	
            // Functions.
            // =================================

            void Awake()
            {

            }

            // ...

            void Start()
            {

            }

            // ...

            void Update()
            {

            }

            // ...

            void LateUpdate()
            {
                Vector2 mousePosition = Input.mousePosition;

                float screenHalfWidth = Screen.width / 2.0f;
                float screenHalfHeight = Screen.height / 2.0f;

                float mouseNormalizedPositionHalfX = (mousePosition.x - screenHalfWidth) / screenHalfWidth;
                float mouseNormalizedPositionHalfY = (mousePosition.y - screenHalfHeight) / screenHalfHeight;

                Vector3 localEulerAngles = transform.localEulerAngles;

                localEulerAngles.y = mouseNormalizedPositionHalfX * -maxRotation;
                localEulerAngles.x = mouseNormalizedPositionHalfY * maxRotation;

                float deltaTime = (!unscaledTime ? Time.deltaTime : Time.unscaledDeltaTime) * speed;

                localEulerAngles.x = Mathf.LerpAngle(transform.localEulerAngles.x, localEulerAngles.x, deltaTime);
                localEulerAngles.y = Mathf.LerpAngle(transform.localEulerAngles.y, localEulerAngles.y, deltaTime);

                transform.localEulerAngles = localEulerAngles;
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
