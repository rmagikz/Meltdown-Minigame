Shader "Test/Toxic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _RippleColor ("Ripple Color", Color) = (1,1,1,1)
        _TextureOpacity ("Texture Opacity", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float4 _RippleColor;
            float _TextureOpacity;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        struct VertexInput
        {
            float4 position : POSITION;
            float2 uv : TEXCOORD0;

        };

        struct VertexOutput
        {
            float4 position : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float2 N22 (float2 p)
            {
                float3 a = frac(p.xyx*float3(212.21, 456.12, 101.41));
                a += dot(a, a+21.56);
                return frac(float2(a.x*a.y, a.y*a.z));
            }

            VertexOutput vert(VertexInput i)
            {
                VertexOutput o;

                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                return o;
            }

            float4 frag(VertexOutput i) : SV_Target
            {
                float4 baseTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                //float2 uv = (2.0 * i.uv - _ScreenParams.xy)/_ScreenParams.y;
                float2 uv = (i.uv - 0.5) * 2;
                
                float m = 0.;
                float t = _Time.y * 1.2;

                float minDist = 100;

                for (float i = 0.0; i < 50.0; i++)
                {
                    float2 n = N22(float2(i,i));
                    float2 p = sin(n*t*0.2);

                    float d = length(uv-p);
                    m += smoothstep(0.05, 0.03, d);

                    if (d < minDist)
                        minDist = d;
                }
                
                return baseTex * 1-_TextureOpacity * (_BaseColor + _RippleColor * minDist);
            }

            ENDHLSL
        }
    }
}
