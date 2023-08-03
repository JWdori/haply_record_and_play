using Haply.HardwareAPI.Unity;
using UnityEngine;


    public class PlaneForce : MonoBehaviour
    {
        [Range(0, 800)]
        public float stiffness = 2f;
        [Range(0, 3)]
        public float damping = 1;

        public Transform ground;

        private HapticThread m_hapticThread;

        private float m_groundHeight;
        private float m_cursorRadius;

        // Workspace
        private float m_workspaceScale = 1;
        private float m_workspaceHeight;


        [Range(-2, 2)]
        public float forceX;
        [Range(-2, 2)]
        public float forceY;
        [Range(-2, 2)]
        public float forceZ;


        private void Start()
        {
            // find the HapticThread object
            m_hapticThread = GetComponent<HapticThread>();

            StoreTransformInfos();

            // run the haptic loop with given function
            m_hapticThread.onInitialized.AddListener(() => m_hapticThread.Run(ForceCalculation));
        }

        /// <summary>
        /// store all transform information which cannot be acceded in haptic tread loop
        ///
        /// <remarks>Do not use this method for dynamic objects in Update() or FixedUpdate() except for debug in editor
        /// (prefer <see cref="HapticThread.GetAdditionalData{T}"/>)</remarks>
        /// </summary>
        private void StoreTransformInfos()
        {
            m_groundHeight = ground.transform.position.x;
            m_cursorRadius = m_hapticThread.avatar.lossyScale.x / 2;

            //var workspace = m_hapticThread.avatar.parent;
            //m_workspaceScale = workspace.lossyScale.x;
            //m_workspaceHeight = workspace.position.x;
        }

#if UNITY_EDITOR
        private void Update () => StoreTransformInfos();
#endif

        /// <summary>
        /// Calculate force to apply when the cursor hit the ground.
        /// <para>This method is called once per haptic frame (~1000Hz) and needs to be efficient</para>
        /// </summary>
        /// <param name="position">cursor position</param>
        /// <param name="velocity">cursor velocity (optional)</param>
        /// <returns>Force to apply</returns>
        private Vector3 ForceCalculation(in Vector3 position, in Vector3 velocity)
        {
            var force = Vector3.zero;

            // Contact point scaled by parent offset
            var contactPoint = (position.x * m_workspaceScale) + m_workspaceHeight - m_cursorRadius;

            var penetration = m_groundHeight - contactPoint;
            if (penetration > 0)
            {
                force.x = penetration * stiffness - velocity.x * damping;

                // invert the offset scale to avoid stiffness relative to it
                force.x /= m_workspaceScale;
                Debug.Log(force);
            }
            else
            {
                force = new Vector3(forceX, forceY, forceZ);
            }
            return force;
        }
    }
