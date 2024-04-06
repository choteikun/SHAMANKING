using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.RadialBlur.Runtime
{
    public class RadialBlurFeature : ScriptableRendererFeature
    {
        class RadialBlurPass : ScriptableRenderPass
        {
            private const string bufferName = "Radial Blur Pass";
            private const string radialBlurTargetName = "Radial Blur Target";
            private const string radialBlurShaderId = "Shader Graphs/RadialBlurShader";
            private Material radialBlurMaterial = null;

            private RenderTargetHandle target;

            private RadialBlurPostProcess radialBlur = null;

            public RadialBlurPass()
            {
                target.Init(radialBlurTargetName);
            }

            internal bool RegisterStackComponent()
            {
                radialBlur = VolumeManager.instance.stack.GetComponent<RadialBlurPostProcess>();

                if (radialBlur == null)
                    return false;

                return radialBlur.IsActive();
            }

            internal bool SetupMaterial()
            {
                if (radialBlurMaterial == null)
                {
                    Shader shader = Shader.Find(radialBlurShaderId);
                    if (shader != null)
                    {
                        radialBlurMaterial = CoreUtils.CreateEngineMaterial(radialBlurShaderId);
                    }
                }

                return radialBlurMaterial != null;
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                cmd.GetTemporaryRT(target.id, renderingData.cameraData.cameraTargetDescriptor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
                CommandBuffer cmd = CommandBufferPool.Get(bufferName);

                SetShaderParams();
                Blit(cmd, source, target.Identifier(), radialBlurMaterial);
                Blit(cmd, target.Identifier(), source);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(target.id);
            }

            private void SetShaderParams()
            {
                radialBlurMaterial.SetVector(RadialBlurShaderParams.center, radialBlur.GetCenter());
                radialBlurMaterial.SetFloat(RadialBlurShaderParams.intensity, radialBlur.GetIntensity());
                radialBlurMaterial.SetFloat(RadialBlurShaderParams.delay, radialBlur.GetDelay());
                radialBlurMaterial.SetInt(RadialBlurShaderParams.sampleCount, radialBlur.GetSampleCount());
            }
        }

        RadialBlurPass radialBlurPass;

        public override void Create()
        {
            radialBlurPass = new RadialBlurPass();
            radialBlurPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.camera.cameraType == CameraType.Reflection)
                return;

            if (renderingData.cameraData.camera.cameraType == CameraType.Preview)
                return;

            if (!radialBlurPass.RegisterStackComponent())
                return;

            if (!radialBlurPass.SetupMaterial())
                return;

            renderer.EnqueuePass(radialBlurPass);
        }

        private static class RadialBlurShaderParams
        {
            public static int center = Shader.PropertyToID("_Center");
            public static int intensity = Shader.PropertyToID("_Intensity");
            public static int delay = Shader.PropertyToID("_Delay");
            public static int sampleCount = Shader.PropertyToID("_SampleCount");
        }
    }
}
