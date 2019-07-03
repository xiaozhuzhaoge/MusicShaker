
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.UI;

//using System.Collections;

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

            //[ExecuteInEditMode]
            [System.Serializable]

            public class DemoManager_XPTitles : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                LoopingParticleSystemsManager list;

                public Text particleCountText;
                public Text currentParticleSystemText;

                Rotator cameraRotator;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {
                    (list = GetComponent<LoopingParticleSystemsManager>()).Init();
                }

                // ...

                void Start()
                {
                    cameraRotator = Camera.main.GetComponentInParent<Rotator>();
                    updateCurrentParticleSystemNameText();
                }

                // ...

                public void ToggleRotation()
                {
                    cameraRotator.enabled = (!cameraRotator.enabled);
                }
                public void ResetRotation()
                {
                    cameraRotator.transform.eulerAngles = Vector3.zero;
                }

                // ...

                void Update()
                {
                    // Scroll through systems.

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        Next();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        previous();
                    }

                    //// Activate/deactive camera rotation.

                    //if (Input.GetKeyDown(KeyCode.Space))
                    //{
                    //    ToggleRotation();
                    //}

                    //// Reset camera rotator transform rotation.

                    //if (Input.GetKeyDown(KeyCode.R))
                    //{
                    //    ResetRotation();
                    //}
                }

                // ...

                void LateUpdate()
                {
                    if (particleCountText)
                    {
                        // Update particle count display.

                        particleCountText.text = "PARTICLE COUNT: ";
                        particleCountText.text += list.GetParticleCount().ToString();
                    }
                }

                // ...

                public void Next()
                {
                    list.Next();
                    updateCurrentParticleSystemNameText();
                }
                public void previous()
                {
                    list.Previous();
                    updateCurrentParticleSystemNameText();
                }

                // ...

                void updateCurrentParticleSystemNameText()
                {
                    if (currentParticleSystemText)
                    {
                        currentParticleSystemText.text = list.GetCurrentPrefabName(true);
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
