using UnityEngine;
using UnityEngine.Rendering;

namespace FrameplaySDK.Public
{
    /// <summary>
    /// Hooks into Unity's render pipeline for gathering Ad Space metrics.
    /// Note experimental SRP in Unity 2018 is not supported
    /// </summary>
    public sealed class FrameplayRenderPipelineManager
    {
        private static FrameplayRenderPipelineManager s_instance;
        private bool _registeredWithRPManager;
#if !UNITY_2018
        /// <summary>
        /// The current tracked camera
        /// </summary>
        private Camera _camera;
#endif

        /// <summary>
        /// Initialize singleton, listen for change camera and shutdown events.
        /// </summary>
        public static void Init()
        {
            if (s_instance != null)
            {
                //already initialized
                return;
            }

            s_instance = new FrameplayRenderPipelineManager();
            FrameplayInternal.OnCameraSet += s_instance.OnCameraSet;
            FrameplayInternal.OnSessionEnd += s_instance.OnSessionEnd;
        }

        /// <summary>
        /// Cleanup previous functionality and notify internal system of the current render pipeline type.
        /// </summary>
        /// <param name="camera"></param>
        private void OnCameraSet(Camera camera)
        {
            CleanUp();
            
            if (camera == null)
            {
                return;
            }

            if (GraphicsSettings.renderPipelineAsset == null)
            {
                FrameplayInternal.SetupBuiltInRP(camera);
            }
            else
            {
#if !UNITY_2018
                FrameplayInternal.SetupSRP(camera);
                _registeredWithRPManager = true;
                _camera = camera;
                RenderPipelineManager.endCameraRendering += RenderPipelineManagerOnEndCameraRendering;
#else
                UnityEngine.Debug.LogError("Scriptable Render Pipeline for Unity 2018 is currently not supported, please send request of this feature to our support team. Shutting down.");
                Frameplay.Shutdown();
#endif
            }
        }

#if !UNITY_2018
        /// <summary>
        /// Execute command buffer and gather Ad Space metrics
        /// </summary>
        /// <param name="context"></param>
        /// <param name="camera"></param>
        private void RenderPipelineManagerOnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera != _camera)
            {
                return;
            }
            var cmd = FrameplayInternal.GetSRPCommand();
            if (cmd != null)
            {
                context.ExecuteCommandBuffer(cmd);
                context.Submit();
            }
        }
#endif

        private void OnSessionEnd()
        {
            CleanUp();
            FrameplayInternal.OnCameraSet -= OnCameraSet;
            FrameplayInternal.OnSessionEnd -= OnSessionEnd;
            s_instance = null;
        }

        private void CleanUp()
        {
            FrameplayInternal.CleanupRP();
#if !UNITY_2018
            _camera = null;
            if (_registeredWithRPManager)
            {
                _registeredWithRPManager = false;
                RenderPipelineManager.endCameraRendering -= RenderPipelineManagerOnEndCameraRendering;
            }
#endif
        }
    }
}
