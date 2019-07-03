
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

            public class TurbulenceParticleAffector : ParticleAffector
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                public enum NoiseType
                {
                    PseudoPerlin,

                    Perlin,

                    Simplex,

                    OctavePerlin,
                    OctaveSimplex
                }

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("Affector Controls")]

                public float speed = 1.0f;

                [Range(0.0f, 8.0f)]
                public float frequency = 1.0f;

                public NoiseType noiseType = NoiseType.Perlin;

                // ...

                [Header("Octave Variant-Only Controls")]

                [Range(1, 8)]
                public int octaves = 1;

                [Range(0.0f, 4.0f)]
                public float lacunarity = 2.0f;

                [Range(0.0f, 1.0f)]
                public float persistence = 0.5f;

                float time;

                // Noise start offsets.

                float randomX;
                float randomY;
                float randomZ;

                // Final offset.

                float offsetX;
                float offsetY;
                float offsetZ;

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

                    randomX = Random.Range(-32.0f, 32.0f);
                    randomY = Random.Range(-32.0f, 32.0f);
                    randomZ = Random.Range(-32.0f, 32.0f);
                }

                // ...

                protected override void Update()
                {
                    time = Time.time;

                    // ...

                    base.Update();
                }

                // ...

                protected override void LateUpdate()
                {
                    offsetX = (time * speed) + randomX;
                    offsetY = (time * speed) + randomY;
                    offsetZ = (time * speed) + randomZ;

                    // ...

                    base.LateUpdate();
                }

                // ...

                protected override Vector3 GetForce()
                {
                    // I could also pre-multiply the frequency, but
                    // all the octave variants also use frequency
                    // within themselves, so it would cause redundant
                    // multiplication.

                    float xX = parameters.particlePosition.x + offsetX;
                    float yX = parameters.particlePosition.y + offsetX;
                    float zX = parameters.particlePosition.z + offsetX;

                    float xY = parameters.particlePosition.x + offsetY;
                    float yY = parameters.particlePosition.y + offsetY;
                    float zY = parameters.particlePosition.z + offsetY;

                    float xZ = parameters.particlePosition.x + offsetZ;
                    float yZ = parameters.particlePosition.y + offsetZ;
                    float zZ = parameters.particlePosition.z + offsetZ;

                    Vector3 force;

                    switch (noiseType)
                    {
                        case NoiseType.PseudoPerlin:
                            {
                                // This isn't really right, but... it gives believable-enough results. 
                                // It's also much faster than real perlin noise.

                                // It works well where you don't have to animate a large field where
                                // the repeating pattern would otherwise be easily seen. 

                                // Examples of good uses: smoke trail particle turbulence.
                                // Example of bad uses: particle box simulating waves or something...

                                float noiseX = Mathf.PerlinNoise(xX * frequency, yY * frequency);
                                float noiseY = Mathf.PerlinNoise(xX * frequency, zY * frequency);
                                float noiseZ = Mathf.PerlinNoise(xX * frequency, xY * frequency);

                                noiseX = Mathf.Lerp(-1.0f, 1.0f, noiseX);
                                noiseY = Mathf.Lerp(-1.0f, 1.0f, noiseY);
                                noiseZ = Mathf.Lerp(-1.0f, 1.0f, noiseZ);

                                Vector3 forceX = (Vector3.right * noiseX);
                                Vector3 forceY = (Vector3.up * noiseY);
                                Vector3 forceZ = (Vector3.forward * noiseZ);

                                force = forceX + forceY + forceZ;

                                break;
                            }

                        // ...

                        default:
                        case NoiseType.Perlin:
                            {
                                force.x = Noise.perlin(xX * frequency, yX * frequency, zX * frequency);
                                force.y = Noise.perlin(xY * frequency, yY * frequency, zY * frequency);
                                force.z = Noise.perlin(xZ * frequency, yZ * frequency, zZ * frequency);

                                return force;
                            }

                        // ...

                        case NoiseType.Simplex:
                            {
                                force.x = Noise.simplex(xX * frequency, yX * frequency, zX * frequency);
                                force.y = Noise.simplex(xY * frequency, yY * frequency, zY * frequency);
                                force.z = Noise.simplex(xZ * frequency, yZ * frequency, zZ * frequency);

                                break;
                            }

                        // ...

                        case NoiseType.OctavePerlin:
                            {
                                force.x = Noise.octavePerlin(xX, yX, zX, frequency, octaves, lacunarity, persistence);
                                force.y = Noise.octavePerlin(xY, yY, zY, frequency, octaves, lacunarity, persistence);
                                force.z = Noise.octavePerlin(xZ, yZ, zZ, frequency, octaves, lacunarity, persistence);

                                break;
                            }

                        case NoiseType.OctaveSimplex:
                            {
                                force.x = Noise.octaveSimplex(xX, yX, zX, frequency, octaves, lacunarity, persistence);
                                force.y = Noise.octaveSimplex(xY, yY, zY, frequency, octaves, lacunarity, persistence);
                                force.z = Noise.octaveSimplex(xZ, yZ, zZ, frequency, octaves, lacunarity, persistence);

                                break;
                            }
                    }

                    return force;
                }

                // ...

                protected override void OnDrawGizmosSelected()
                {
                    if (enabled)
                    {
                        base.OnDrawGizmosSelected();
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
