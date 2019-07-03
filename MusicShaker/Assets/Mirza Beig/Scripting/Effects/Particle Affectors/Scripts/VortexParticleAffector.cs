
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Scripting
    {

        namespace Effects
        {

            // =================================	
            // Classes.
            // =================================

            public class VortexParticleAffector : ParticleAffector
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                Vector3 axisOfRotation;

                [Header("Affector Controls")]

                // Useful if particle affector and particle system
                // are on the same game object, and you need a seperate 
                // rotation for the system, and the affector, but don't 
                // want to make the two different game objects.

                public Vector3 axisOfRotationOffset = Vector3.zero;

                // =================================	
                // Functions.
                // =================================

                // ...

                protected override void Awake()
                {
                    base.Awake();
                }

                // ...

                protected override void Start()
                {
                    base.Start();
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                protected override void LateUpdate()
                {
                    base.LateUpdate();
                }

                // ...

                void UpdateAxisOfRotation()
                {
                    axisOfRotation = Quaternion.Euler(axisOfRotationOffset) * transform.up;
                }

                // ...

                protected override void PerParticleSystemSetup()
                {
                    UpdateAxisOfRotation();
                }

                // ...

                protected override Vector3 GetForce()
                {
                    // With no rotation, looking at the PS with the vortex down the Z axis, you may
                    // think it's spinning the wrong way because it's counter-clockwise. But it's actually correct...

                    // Because if you were to look up aligned with the up-axis of the vortex, you'd see it spinning
                    // clockwise. And if that up was inverted (you looked down at the vortex from above), then it would
                    // be spinning counter-clockwise since now the vector of rotation is point at you, not away from you. 

                    // I can't believe I almost mixed that up by adding a negative (-) in front of the return.

                    return Vector3.Normalize(Vector3.Cross(axisOfRotation, parameters.scaledDirectionToAffectorCenter));
                }

                // ...

                protected override void OnDrawGizmosSelected()
                {
                    if (enabled)
                    {
                        base.OnDrawGizmosSelected();

                        // ...

                        Gizmos.color = Color.red;

                        // When not playing, I don't have a reference to the specific particle system,
                        // so just use the default method of showing the axis of rotation (which may be wrong).

                        // There's no easy way around this since I may have several particle systems being updated
                        // with a single vortex. It's just a visual guide anyways, so no big deal, I suppose.

                        Vector3 axisOfRotation;

                        if (Application.isPlaying && enabled)
                        {
                            UpdateAxisOfRotation();
                            axisOfRotation = this.axisOfRotation;
                        }
                        else
                        {
                            axisOfRotation = Quaternion.Euler(axisOfRotationOffset) * transform.up;
                        }

                        Gizmos.DrawLine(transform.position + offset, (transform.position + offset) + (axisOfRotation * scaledRadius));
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
