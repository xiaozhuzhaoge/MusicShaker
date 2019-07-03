
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Demos
    {

        namespace Wallpapers
        {

            // =================================	
            // Classes.
            // =================================

            public class GravityClockInteractivityUVFX : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                public GameObject forceAffectors;
                public GameObject forceAffectors2;

                public ParticleSystem gravityClockPrefab;

                ParticleSystem gravityClock;

                public bool enableGravityClockVisualEffects = true;
                public bool enableGravityClockAttractionForce = true;

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

                public void SetGravityClockVisualEffectsActive(bool value)
                {
                    if (value)
                    {
                        if (enableGravityClockVisualEffects)
                        {
                            gravityClock = Instantiate(gravityClockPrefab, transform);
                            gravityClock.transform.localPosition = Vector3.zero;
                        }
                    }
                    else
                    {
                        if (gravityClock)
                        {
                            gravityClock.Stop();
                            gravityClock.transform.SetParent(null, true);
                        }
                    }
                }
                public void SetGravityClockAttractionForceActive(bool value)
                {
                    if (value)
                    {
                        if (enableGravityClockAttractionForce)
                        {
                            forceAffectors.gameObject.SetActive(true);
                            forceAffectors2.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        forceAffectors.gameObject.SetActive(false);
                        forceAffectors2.gameObject.SetActive(false);                        
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
