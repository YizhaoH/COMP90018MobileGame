#if ENVIRO_LWRP
namespace UnityEngine.Rendering.LWRP
{
    /// <summary>
    /// Copy the given color buffer to the given destination color buffer.
    ///
    /// You can use this pass to copy a color buffer to the destination,
    /// so you can use it later in rendering. For example, you can copy
    /// the opaque texture to use it for distortion effects.
    /// </summary>
    internal class EnviroBlitPassShafts : UnityEngine.Rendering.Universal.ScriptableRenderPass
    {
        private Camera myCam;
        public Material blitMaterial = null;
        public Material clearMaterial = null;
        private RenderTargetIdentifier source { get; set; }
        private UnityEngine.Rendering.Universal.RenderTargetHandle destination { get; set; }
        private RenderTexture lrColorB;
        private RenderTexture lrDepthBuffer;

        #region LightShafts Var
        [HideInInspector]
        public enum ShaftsScreenBlendMode
        {
            Screen = 0,
            Add = 1,
        }
        [HideInInspector]public int radialBlurIterations = 2;

        private Transform lightSource;
        private Color treshold;
        private Color clr;
        #endregion

        public EnviroBlitPassShafts(UnityEngine.Rendering.Universal.RenderPassEvent renderPassEvent, Material blitMaterial, Material clearMaterial)
        {
            this.renderPassEvent = renderPassEvent;
            this.blitMaterial = blitMaterial;
            this.clearMaterial = clearMaterial;
        }

        public void Setup(Camera myCam, RenderTargetIdentifier source, UnityEngine.Rendering.Universal.RenderTargetHandle destination, Transform lightSource, Color treshold, Color clr)
        {
            this.myCam = myCam;
            this.source = source;
            this.destination = destination;
            this.lightSource = lightSource;
            this.treshold = treshold;
            this.clr = clr;
        }

        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref UnityEngine.Rendering.Universal.RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Light Shafts");

            int divider = 4;
            if (EnviroSkyMgr.instance.LightShaftsSettings.resolution == EnviroPostProcessing.SunShaftsResolution.Normal)
                divider = 2;
            else if (EnviroSkyMgr.instance.LightShaftsSettings.resolution == EnviroPostProcessing.SunShaftsResolution.High)
                divider = 1;

            Vector3 v = Vector3.one * 0.5f;
             
            if (lightSource)
                v = myCam.WorldToViewportPoint(lightSource.position);
            else
                v = new Vector3(0.5f, 0.5f, 0.0f);

            int rtW = renderingData.cameraData.cameraTargetDescriptor.width / divider;
            int rtH = renderingData.cameraData.cameraTargetDescriptor.height / divider;

            RenderTextureDescriptor textureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            textureDescriptor.width = rtW;
            textureDescriptor.height = rtH;
            textureDescriptor.depthBufferBits = 0;
            textureDescriptor.colorFormat = RenderTextureFormat.Default;

            if (lrDepthBuffer == null)
                lrDepthBuffer = new RenderTexture(textureDescriptor);

            if (lrColorB == null)
                lrColorB = new RenderTexture(textureDescriptor);

         //   lrDepthBuffer = RenderTexture.GetTemporary(textureDescriptor);

            cmd.SetGlobalVector("_BlurRadius4", new Vector4(1.0f, 1.0f, 0.0f, 0.0f) * EnviroSkyMgr.instance.LightShaftsSettings.blurRadius);
            cmd.SetGlobalVector("_SunPosition", new Vector4(v.x, v.y, v.z, EnviroSkyMgr.instance.LightShaftsSettings.maxRadius));
            cmd.SetGlobalVector("_SunThreshold", treshold);

            if(blitMaterial == null)
               blitMaterial = new Material(Shader.Find("Enviro/Effects/LightShafts"));

            blitMaterial.SetVector("_BlurRadius4", new Vector4(1.0f, 1.0f, 0.0f, 0.0f) * EnviroSkyMgr.instance.LightShaftsSettings.blurRadius);
            blitMaterial.SetVector("_SunPosition", new Vector4(v.x, v.y, v.z, EnviroSkyMgr.instance.LightShaftsSettings.maxRadius));
            blitMaterial.SetVector("_SunThreshold", treshold);

            RenderTextureDescriptor textureDescriptorSource = renderingData.cameraData.cameraTargetDescriptor;
            textureDescriptorSource.graphicsFormat = Experimental.Rendering.GraphicsFormat.R32G32B32A32_SFloat;
            textureDescriptorSource.depthBufferBits = 0;
            RenderTexture sourceRT = RenderTexture.GetTemporary(textureDescriptorSource);
            Blit(cmd, source, sourceRT);


            Graphics.Blit(sourceRT, lrDepthBuffer, blitMaterial, 2);
            //Blit(cmd, source, lrDepthBuffer, blitMaterial, 2);

            if(clearMaterial == null)
               clearMaterial = new Material(Shader.Find("Enviro/Effects/ClearLightShafts"));

            // paint a small black small border to get rid of clamping problems
            if (myCam.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono)
                DrawBorder(lrDepthBuffer, clearMaterial);

            // radial blur:
            radialBlurIterations = Mathf.Clamp(radialBlurIterations, 1, 4);
            float ofs = EnviroSkyMgr.instance.LightShaftsSettings.blurRadius * (1.0f / 768.0f);
            cmd.SetGlobalVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
            cmd.SetGlobalVector("_SunPosition", new Vector4(v.x, v.y, v.z, EnviroSkyMgr.instance.LightShaftsSettings.maxRadius));

            blitMaterial.SetVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
            blitMaterial.SetVector("_SunPosition", new Vector4(v.x, v.y, v.z, EnviroSkyMgr.instance.LightShaftsSettings.maxRadius));

            for (int it2 = 0; it2 < radialBlurIterations; it2++)
            {
                Graphics.Blit(lrDepthBuffer, lrColorB, blitMaterial, 1);
                //Blit(cmd, lrDepthBuffer, lrColorB, blitMaterial, 1);
                // RenderTexture.ReleaseTemporary(lrDepthBuffer);
                ofs = EnviroSkyMgr.instance.LightShaftsSettings.blurRadius * (((it2 * 2.0f + 1.0f) * 6.0f)) / 768.0f;
                cmd.SetGlobalVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
                blitMaterial.SetVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
                //lrDepthBuffer = RenderTexture.GetTemporary(textureDescriptor);

                Graphics.Blit(lrColorB, lrDepthBuffer, blitMaterial, 1);
                //Blit(cmd, lrColorB, lrDepthBuffer, blitMaterial, 1);
                //RenderTexture.ReleaseTemporary(lrColorB);
                ofs = EnviroSkyMgr.instance.LightShaftsSettings.blurRadius * (((it2 * 2.0f + 2.0f) * 6.0f)) / 768.0f;
                cmd.SetGlobalVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
                blitMaterial.SetVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
            }

            // put together:

            if (v.z >= 0.0f)
                cmd.SetGlobalVector("_SunColor", new Vector4(clr.r, clr.g, clr.b, clr.a) * EnviroSkyMgr.instance.LightShaftsSettings.intensity);
            else
                cmd.SetGlobalVector("_SunColor", Vector4.zero); // no backprojection !


            //FINAL
            // blitMaterial.SetTexture("_ColorBuffer", lrDepthBuffer);


            cmd.SetGlobalTexture("_ColorBuffer", lrDepthBuffer);
            Blit(cmd, sourceRT, source, blitMaterial, (EnviroSkyMgr.instance.LightShaftsSettings.screenBlendMode == EnviroPostProcessing.ShaftsScreenBlendMode.Screen) ? 0 : 4);
 
            //  RenderTexture.ReleaseTemporary(lrDepthBuffer);
            RenderTexture.ReleaseTemporary(sourceRT);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        void DrawBorder(RenderTexture dest, Material material)
        {
            float x1;
            float x2;
            float y1;
            float y2;

            RenderTexture.active = dest;
            bool invertY = true; // source.texelSize.y < 0.0ff;
                                 // Set up the simple Matrix
            GL.PushMatrix();
            GL.LoadOrtho();

            for (int i = 0; i < material.passCount; i++)
            {
                material.SetPass(i);

                float y1_; float y2_;
                if (invertY)
                {
                    y1_ = 1.0f; y2_ = 0.0f;
                }
                else
                {
                    y1_ = 0.0f; y2_ = 1.0f;
                }

                // left
                x1 = 0.0f;
                x2 = 0.0f + 1.0f / (dest.width * 1.0f);
                y1 = 0.0f;
                y2 = 1.0f;
                GL.Begin(GL.QUADS);

                GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
                GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
                GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
                GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

                // right
                x1 = 1.0f - 1.0f / (dest.width * 1.0f);
                x2 = 1.0f;
                y1 = 0.0f;
                y2 = 1.0f;

                GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
                GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
                GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
                GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

                // top
                x1 = 0.0f;
                x2 = 1.0f;
                y1 = 0.0f;
                y2 = 0.0f + 1.0f / (dest.height * 1.0f);

                GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
                GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
                GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
                GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

                // bottom
                x1 = 0.0f;
                x2 = 1.0f;
                y1 = 1.0f - 1.0f / (dest.height * 1.0f);
                y2 = 1.0f;

                GL.TexCoord2(0.0f, y1_); GL.Vertex3(x1, y1, 0.1f);
                GL.TexCoord2(1.0f, y1_); GL.Vertex3(x2, y1, 0.1f);
                GL.TexCoord2(1.0f, y2_); GL.Vertex3(x2, y2, 0.1f);
                GL.TexCoord2(0.0f, y2_); GL.Vertex3(x1, y2, 0.1f);

                GL.End();
            }

            GL.PopMatrix();
        }



        /// <inheritdoc/>
        public override void FrameCleanup(CommandBuffer cmd)
        {
           // if (destination == RenderTargetHandle.CameraTarget.Identifier())
           //     cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
        }
    }
}
#endif