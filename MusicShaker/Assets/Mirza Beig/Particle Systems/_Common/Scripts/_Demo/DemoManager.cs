
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

            public class DemoManager : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                public enum ParticleMode
                {
                    looping,
                    oneshot,
                }

                public enum Level
                {
                    none,
                    basic,
                }

                // =================================	
                // Variables.
                // =================================

                public Transform cameraRotationTransform;
                public Transform cameraTranslationTransform;

                public Vector3 cameraLookAtPosition = new Vector3(0.0f, 3.0f, 0.0f);

                public FollowMouse mouse;

                Vector3 targetCameraPosition;
                Vector3 targetCameraRotation;

                Vector3 cameraPositionStart;
                Vector3 cameraRotationStart;

                Vector2 input;

                // Because Euler angles wrap around 360, I use
                // a separate value to store the full rotation.

                Vector3 cameraRotation;

                public float cameraMoveAmount = 2.0f;
                public float cameraRotateAmount = 2.0f;

                public float cameraMoveSpeed = 12.0f;
                public float cameraRotationSpeed = 12.0f;

                public Vector2 cameraAngleLimits = new Vector2(-8.0f, 60.0f);

                public GameObject[] levels;
                public Level currentLevel = Level.basic;

                public ParticleMode particleMode = ParticleMode.looping;

                public bool advancedRendering = true;

                public Toggle loopingParticleModeToggle;
                public Toggle oneshotParticleModeToggle;

                public Toggle advancedRenderingToggle;

                Toggle[] levelToggles;
                public ToggleGroup levelTogglesContainer;

                LoopingParticleSystemsManager loopingParticleSystems;
                OneshotParticleSystemsManager oneshotParticleSystems;

                public Text particleCountText;
                public Text currentParticleSystemText;

                public Text particleSpawnInstructionText;

                public Slider timeScaleSlider;
                public Text timeScaleSliderValueText;

                public Camera mainCamera;

                public MonoBehaviour[] mainCameraPostEffects;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {
                    loopingParticleSystems = FindObjectOfType<LoopingParticleSystemsManager>();
                    oneshotParticleSystems = FindObjectOfType<OneshotParticleSystemsManager>();

                    loopingParticleSystems.Init();
                    oneshotParticleSystems.Init();
                }

                // ...

                void Start()
                {
                    // ...

                    cameraPositionStart = cameraTranslationTransform.localPosition;
                    cameraRotationStart = cameraRotationTransform.localEulerAngles;

                    ResetCameraTransformTargets();

                    // ...

                    switch (particleMode)
                    {
                        case ParticleMode.looping:
                            {
                                SetToLoopingParticleMode(true);

                                loopingParticleModeToggle.isOn = true;
                                oneshotParticleModeToggle.isOn = false;

                                break;
                            }
                        case ParticleMode.oneshot:
                            {
                                SetToOneshotParticleMode(true);

                                loopingParticleModeToggle.isOn = false;
                                oneshotParticleModeToggle.isOn = true;

                                break;
                            }
                        default:
                            {
                                print("Unknown case.");
                                break;
                            }
                    }

                    // ...

                    SetAdvancedRendering(advancedRendering);
                    advancedRenderingToggle.isOn = advancedRendering;

                    // ...

                    levelToggles =
                        levelTogglesContainer.GetComponentsInChildren<Toggle>(true);

                    for (int i = 0; i < levels.Length; i++)
                    {
                        // Toggle's OnValueChanged handles
                        // level state. No need to SetActive().

                        if (i == (int)currentLevel)
                        {
                            levels[i].SetActive(true);
                            levelToggles[i].isOn = true;
                        }
                        else
                        {
                            levels[i].SetActive(false);
                            levelToggles[i].isOn = false;
                        }
                    }

                    // ...

                    UpdateCurrentParticleSystemNameText();
                    timeScaleSlider.onValueChanged.AddListener(OnTimeScaleSliderValueChanged);

                    OnTimeScaleSliderValueChanged(timeScaleSlider.value);
                }

                // ...

                public void OnTimeScaleSliderValueChanged(float value)
                {
                    Time.timeScale = value;
                    timeScaleSliderValueText.text = value.ToString("0.00");
                }

                // ...

                public void SetToLoopingParticleMode(bool set)
                {
                    if (set)
                    {
                        oneshotParticleSystems.Clear();

                        loopingParticleSystems.gameObject.SetActive(true);
                        oneshotParticleSystems.gameObject.SetActive(false);

                        particleSpawnInstructionText.gameObject.SetActive(false);

                        particleMode = ParticleMode.looping;

                        UpdateCurrentParticleSystemNameText();
                    }
                }

                // ...

                public void SetToOneshotParticleMode(bool set)
                {
                    if (set)
                    {
                        loopingParticleSystems.gameObject.SetActive(false);
                        oneshotParticleSystems.gameObject.SetActive(true);

                        particleSpawnInstructionText.gameObject.SetActive(true);

                        particleMode = ParticleMode.oneshot;

                        UpdateCurrentParticleSystemNameText();
                    }
                }

                // ...

                public void setLevel(Level level)
                {
                    for (int i = 0; i < levels.Length; i++)
                    {
                        if (i == (int)level)
                        {
                            levels[i].SetActive(true);
                        }
                        else
                        {
                            levels[i].SetActive(false);
                        }
                    }

                    currentLevel = level;
                }

                // ...

                public void SetLevelFromToggle(Toggle toggle)
                {
                    if (toggle.isOn)
                    {
                        setLevel((Level)System.Array.IndexOf(levelToggles, toggle));
                    }
                }

                // ...

                public void SetAdvancedRendering(bool value)
                {
                    advancedRendering = value;
                    mainCamera.allowHDR = value;

                    if (value)
                    {
                        QualitySettings.SetQualityLevel(32, true);
                        mainCamera.renderingPath = RenderingPath.UsePlayerSettings;
                        mouse.gameObject.SetActive(true);
                    }
                    else
                    {
                        QualitySettings.SetQualityLevel(0, true);
                        mainCamera.renderingPath = RenderingPath.VertexLit;

                        mouse.gameObject.SetActive(false);
                    }

                    for (int i = 0; i < mainCameraPostEffects.Length; i++)
                    {
                        if (mainCameraPostEffects[i])
                        {
                            mainCameraPostEffects[i].enabled = value;
                        }
                    }
                }

                // ...

                public static Vector3 DampVector3(Vector3 from, Vector3 to, float speed, float dt)
                {
                    return Vector3.Lerp(from, to, 1.0f - Mathf.Exp(-speed * dt));
                }

                // ...

                Vector3 cameraPositionSmoothDampVelocity;
                Vector3 cameraRotationSmoothDampVelocity;

                void Update()
                {
                    // ...

                    input.x = Input.GetAxis("Horizontal");
                    input.y = Input.GetAxis("Vertical");

                    // Get targets.

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        targetCameraPosition.z += input.y * cameraMoveAmount;
                        targetCameraPosition.z = Mathf.Clamp(targetCameraPosition.z, -6.3f, -1.0f);
                    }
                    else
                    {

                        targetCameraRotation.y += input.x * cameraRotateAmount;
                        targetCameraRotation.x += input.y * cameraRotateAmount;

                        targetCameraRotation.x = Mathf.Clamp(targetCameraRotation.x, cameraAngleLimits.x, cameraAngleLimits.y);
                    }

                    // Camera position.

                    cameraTranslationTransform.localPosition = Vector3.SmoothDamp(
                        cameraTranslationTransform.localPosition, targetCameraPosition, ref cameraPositionSmoothDampVelocity, 1.0f / cameraMoveSpeed, Mathf.Infinity, Time.unscaledDeltaTime);

                    // Camera container rotation.

                    cameraRotation = Vector3.SmoothDamp(
                        cameraRotation, targetCameraRotation, ref cameraRotationSmoothDampVelocity, 1.0f / cameraRotationSpeed, Mathf.Infinity, Time.unscaledDeltaTime);

                    cameraRotationTransform.localEulerAngles = cameraRotation;

                    // Look at origin.

                    cameraTranslationTransform.LookAt(cameraLookAtPosition);

                    // Scroll through systems.

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        Next();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        Previous();
                    }

                    // Random prefab while holding key.

                    else if (Input.GetKey(KeyCode.R))
                    {
                        //if (particleMode == ParticleMode.oneshot)
                        //{
                        //    oneshotParticleSystems.randomize();
                        //    updateCurrentParticleSystemNameText();

                        //    // If also holding down, auto-spawn at random point.

                        //    if (Input.GetKey(KeyCode.T))
                        //    {
                        //        //oneshotParticleSystems.instantiateParticlePrefabRandom();
                        //    }
                        //}
                    }

                    // Left-click to spawn once.
                    // Right-click to continously spawn.

                    if (particleMode == ParticleMode.oneshot)
                    {
                        Vector3 mousePosition = Input.mousePosition;

                        if (Input.GetMouseButtonDown(0))
                        {
                            oneshotParticleSystems.InstantiateParticlePrefab(mousePosition, mouse.distanceFromCamera);
                        }
                        if (Input.GetMouseButton(1))
                        {
                            oneshotParticleSystems.InstantiateParticlePrefab(mousePosition, mouse.distanceFromCamera);
                        }
                    }

                    // Reset.

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        ResetCameraTransformTargets();
                    }
                }

                // ...

                void LateUpdate()
                {
                    // Update particle count display.

                    particleCountText.text = "PARTICLE COUNT: ";

                    if (particleMode == ParticleMode.looping)
                    {
                        particleCountText.text += loopingParticleSystems.GetParticleCount().ToString();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        particleCountText.text += oneshotParticleSystems.GetParticleCount().ToString();
                    }
                }

                // ...

                void ResetCameraTransformTargets()
                {
                    targetCameraPosition = cameraPositionStart;
                    targetCameraRotation = cameraRotationStart;
                }

                // ...

                void UpdateCurrentParticleSystemNameText()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        currentParticleSystemText.text = loopingParticleSystems.GetCurrentPrefabName(true);
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        currentParticleSystemText.text = oneshotParticleSystems.GetCurrentPrefabName(true);
                    }
                }

                // ...

                public void Next()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        loopingParticleSystems.Next();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        oneshotParticleSystems.Next();
                    }

                    UpdateCurrentParticleSystemNameText();
                }

                public void Previous()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        loopingParticleSystems.Previous();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        oneshotParticleSystems.Previous();
                    }

                    UpdateCurrentParticleSystemNameText();
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
