
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections.Generic;

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

            [RequireComponent(typeof(ParticleSystem))]
            public class ParticleFlocking : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                public struct Voxel
                {
                    public Bounds bounds;
                    public int[] particles;

                    public int particleCount;
                }

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("N^2 Mode Settings")]

                public float maxDistance = 0.5f;

                [Header("Forces")]

                public float cohesion = 0.5f;
                public float separation = 0.25f;

                [Header("Voxel Mode Settings")]

                public bool useVoxels = true;
                public bool voxelLocalCenterFromBounds = true;

                public float voxelVolume = 8.0f;
                public int voxelsPerAxis = 5;

                int previousVoxelsPerAxisValue;

                Voxel[] voxels;
                new ParticleSystem particleSystem;

                ParticleSystem.Particle[] particles;
                Vector3[] particlePositions;

                ParticleSystem.MainModule particleSystemMainModule;
                
                [Header("General Performance Settings")]

                [Range(0.0f, 1.0f)]
                public float delay = 0.0f;

                float timer;

                public bool alwaysUpdate = false;
                bool visible;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Start()
                {
                    particleSystem = GetComponent<ParticleSystem>();
                    particleSystemMainModule = particleSystem.main;
                }

                // ...

                void OnBecameVisible()
                {
                    visible = true;
                }
                void OnBecameInvisible()
                {
                    visible = false;
                }

                // ...

                void buildVoxelGrid()
                {
                    int voxelCount =
                        voxelsPerAxis * voxelsPerAxis * voxelsPerAxis;

                    voxels = new Voxel[voxelCount];

                    float voxelSize = voxelVolume / voxelsPerAxis;

                    float voxelSizeHalf = voxelSize / 2.0f;
                    float voxelVolumeHalf = voxelVolume / 2.0f;

                    Vector3 positionBase = transform.position;

                    int i = 0;

                    for (int x = 0; x < voxelsPerAxis; x++)
                    {
                        float posX = (-voxelVolumeHalf + voxelSizeHalf) + (x * voxelSize);

                        for (int y = 0; y < voxelsPerAxis; y++)
                        {
                            float posY = (-voxelVolumeHalf + voxelSizeHalf) + (y * voxelSize);

                            for (int z = 0; z < voxelsPerAxis; z++)
                            {
                                float posZ = (-voxelVolumeHalf + voxelSizeHalf) + (z * voxelSize);

                                voxels[i].particleCount = 0;
                                voxels[i].bounds = new Bounds(positionBase + new Vector3(posX, posY, posZ), Vector3.one * voxelSize);

                                i++;
                            }
                        }
                    }
                }

                // ...

                void LateUpdate()
                {
                    if (alwaysUpdate || visible)
                    {
                        if (useVoxels)
                        {
                            int voxelCount =
                                voxelsPerAxis * voxelsPerAxis * voxelsPerAxis;

                            if (voxels == null || voxels.Length < voxelCount)
                            {
                                buildVoxelGrid();
                            }
                        }

                        int maxParticles = particleSystemMainModule.maxParticles;

                        if (particles == null || particles.Length < maxParticles)
                        {
                            particles = new ParticleSystem.Particle[maxParticles];
                            particlePositions = new Vector3[maxParticles];

                            if (useVoxels)
                            {
                                for (int i = 0; i < voxels.Length; i++)
                                {
                                    voxels[i].particles = new int[maxParticles];
                                }
                            }
                        }

                        timer += Time.deltaTime;

                        if (timer >= delay)
                        {
                            float deltaTime = timer;
                            timer = 0.0f;

                            particleSystem.GetParticles(particles);
                            int particleCount = particleSystem.particleCount;

                            float cohesionDeltaTime = cohesion * deltaTime;
                            float separationDeltaTime = separation * deltaTime;

                            for (int i = 0; i < particleCount; i++)
                            {
                                particlePositions[i] = particles[i].position;
                            }

                            if (useVoxels)
                            {
                                int voxelCount = voxels.Length;
                                float voxelSize = voxelVolume / voxelsPerAxis;

                                for (int i = 0; i < particleCount; i++)
                                {
                                    for (int j = 0; j < voxelCount; j++)
                                    {
                                        if (voxels[j].bounds.Contains(particlePositions[i]))
                                        {
                                            voxels[j].particles[voxels[j].particleCount] = i;
                                            voxels[j].particleCount++;

                                            break;
                                        }
                                    }
                                }

                                for (int i = 0; i < voxelCount; i++)
                                {
                                    if (voxels[i].particleCount > 1)
                                    {
                                        for (int j = 0; j < voxels[i].particleCount; j++)
                                        {
                                            Vector3 directionToLocalCenter;
                                            Vector3 localCenter = particlePositions[voxels[i].particles[j]];

                                            if (voxelLocalCenterFromBounds)
                                            {
                                                directionToLocalCenter = voxels[i].bounds.center - particlePositions[voxels[i].particles[j]];
                                            }
                                            else
                                            {
                                                for (int k = 0; k < voxels[i].particleCount; k++)
                                                {
                                                    if (k == j)
                                                    {
                                                        continue;
                                                    }

                                                    localCenter += particlePositions[voxels[i].particles[k]];
                                                }

                                                localCenter /= voxels[i].particleCount;

                                                directionToLocalCenter = localCenter - particlePositions[voxels[i].particles[j]];
                                            }

                                            float distanceToLocalCenterSqr = directionToLocalCenter.sqrMagnitude;

                                            directionToLocalCenter.Normalize();

                                            Vector3 force = Vector3.zero;

                                            force += directionToLocalCenter * cohesionDeltaTime;
                                            force -= directionToLocalCenter * ((1.0f - (distanceToLocalCenterSqr / voxelSize)) * separationDeltaTime);

                                            Vector3 particleVelocity = particles[voxels[i].particles[j]].velocity;

                                            particleVelocity.x += force.x;
                                            particleVelocity.y += force.y;
                                            particleVelocity.z += force.z;

                                            particles[voxels[i].particles[j]].velocity = particleVelocity;
                                        }

                                        voxels[i].particleCount = 0;
                                    }
                                }
                            }
                            else
                            {
                                float maxDistanceSqr = maxDistance * maxDistance;

                                Vector3 p1p2_difference;

                                for (int i = 0; i < particleCount; i++)
                                {
                                    int localCount = 1;
                                    Vector3 localCenter = particlePositions[i];

                                    for (int j = 0; j < particleCount; j++)
                                    {
                                        if (j == i)
                                        {
                                            continue;
                                        }

                                        p1p2_difference.x = particlePositions[i].x - particlePositions[j].x;
                                        p1p2_difference.y = particlePositions[i].y - particlePositions[j].y;
                                        p1p2_difference.z = particlePositions[i].z - particlePositions[j].z;

                                        float distanceSqr = Vector3.SqrMagnitude(p1p2_difference);

                                        // TODO: Expand vector arithmetic for performance.

                                        if (distanceSqr <= maxDistanceSqr)
                                        {
                                            localCount++;
                                            localCenter += particlePositions[j];
                                        }
                                    }

                                    if (localCount != 1)
                                    {
                                        localCenter /= localCount;

                                        Vector3 directionToLocalCenter = localCenter - particlePositions[i];
                                        float distanceToLocalCenterSqr = directionToLocalCenter.sqrMagnitude;

                                        directionToLocalCenter.Normalize();

                                        Vector3 force = Vector3.zero;

                                        force += directionToLocalCenter * cohesionDeltaTime;
                                        force -= directionToLocalCenter * ((1.0f - (distanceToLocalCenterSqr / maxDistanceSqr)) * separationDeltaTime);

                                        Vector3 particleVelocity = particles[i].velocity;

                                        particleVelocity.x += force.x;
                                        particleVelocity.y += force.y;
                                        particleVelocity.z += force.z;

                                        particles[i].velocity = particleVelocity;
                                    }

                                    //Vector3 velocity = particles[i].velocity;

                                    //if (velocity != Vector3.zero)
                                    //{
                                    //    particles[i].rotation3D = Quaternion.LookRotation(velocity, Vector3.up).eulerAngles;
                                    //}
                                }
                            }

                            particleSystem.SetParticles(particles, particleCount);
                        }
                    }
                }

                // ...

                void OnDrawGizmosSelected()
                {
                    //buildVoxelGrid();

                    //for (int i = 0; i < voxels.Length; i++)
                    //{
                    //    Gizmos.DrawWireCube(voxels[i].bounds.center, voxels[i].bounds.size);
                    //}

                    float size = voxelVolume / voxelsPerAxis;

                    float sizeHalf = size / 2.0f;
                    float totalSizeHalf = voxelVolume / 2.0f;

                    Vector3 positionBase = transform.position;

                    Gizmos.color = Color.red;

                    Gizmos.DrawWireCube(positionBase, Vector3.one * voxelVolume);

                    Gizmos.color = Color.white;

                    for (int x = 0; x < voxelsPerAxis; x++)
                    {
                        float posX = (-totalSizeHalf + sizeHalf) + (x * size);

                        for (int y = 0; y < voxelsPerAxis; y++)
                        {
                            float posY = (-totalSizeHalf + sizeHalf) + (y * size);

                            for (int z = 0; z < voxelsPerAxis; z++)
                            {
                                float posZ = (-totalSizeHalf + sizeHalf) + (z * size);

                                Gizmos.DrawWireCube(positionBase + new Vector3(posX, posY, posZ), Vector3.one * size);
                            }
                        }
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
