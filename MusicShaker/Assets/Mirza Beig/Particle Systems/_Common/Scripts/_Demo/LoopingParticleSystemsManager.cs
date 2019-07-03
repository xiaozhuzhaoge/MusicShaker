
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

        namespace Demos
        {

            // =================================	
            // Classes.
            // =================================
            
            public class LoopingParticleSystemsManager : ParticleManager
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

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

                    // ...
                    
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(true);
                }

                // ...

                public override void Next()
                {
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(false);

                    base.Next();
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(true);
                }
                public override void Previous()
                {
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(false);

                    base.Previous();
                    particlePrefabs[currentParticlePrefabIndex].gameObject.SetActive(true);
                }

                // ...

                protected override void Update()
                {
                    base.Update();
                }

                // ...

                public override int GetParticleCount()
                {
                    // Return particle count from active prefab.

                    return particlePrefabs[currentParticlePrefabIndex].getParticleCount();
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

// =================================	
// --END-- //
// =================================
