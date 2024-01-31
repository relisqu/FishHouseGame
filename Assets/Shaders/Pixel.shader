// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/Pixel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize("Pixel Size", float)=64
        [ShowAsVector2]_Resolution("Resolution", Vector)= (1920,1080,0,0)
        _DepthThreshold ("DepthThreshold",float)=0.04
        _Scale ("Scale",float)=1


        _NormalThreshold ("NormalThreshold",float)=0.04
        _OutlineColor("OutlineColor", Color) = (0.2,0.2,0.9,1)
        [Toggle] _PerspectiveCorrection ("Use Perspective Correction", Float) = 1.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Edge"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            bool _PerspectiveCorrection;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);

                return o;
            }


            float _DepthThreshold;
            float _NormalThreshold;
            float _Scale;
            float4x4 _ViewMatrix;
            sampler2D _CameraDepthNormalsTexture;
            sampler2D _MainTex;
            float2 _MainTex_TexelSize;
            float4 _OutlineColor;

            vector _Resolution;
            float _PixelSize;
            float _PixelYSize;
            float _dx;
            float _dy;

            float getDepth(float2 uv, float2 offset)
            {
                float4 NormalDepth;
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv + offset), NormalDepth.w, NormalDepth.xyz);
                float depth = 1 - NormalDepth.w * 100;
                //depth = clamp(NormalDepth.w, 0, 1);
                return depth;
            }

            float3 getNormals(float2 uv, float2 offset)
            {
                float4 NormalDepth;
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv + offset), NormalDepth.w, NormalDepth.xyz);
                return NormalDepth.xyz;
            }

            float2 pixelateCoord(float2 coord)
            {
                float2 c = floor(coord / float(_PixelSize)) * float(_PixelSize) / _Resolution.xy;
                return c;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                int texelX = floor(i.uv.x * _Resolution.x);
                int texelY = floor(i.uv.y * _Resolution.y);
                float2 cord = pixelateCoord(float2(texelX, texelY));
                float depth = getDepth(cord, 0);
                float2 uvs[4];
                uvs[0] = cord + float2(0.0, _MainTex_TexelSize.y);
                uvs[1] = cord - float2(0.0, _MainTex_TexelSize.y);
                uvs[2] = cord + float2(_MainTex_TexelSize.x, 0);
                uvs[3] = cord - float2(_MainTex_TexelSize.x, 0);

                float depths[4];
                float depthDiff = 0.0;

                for (int i = 0; i < 4; ++i)
                {
                    depths[i] = getDepth(uvs[i], 0);
                    depthDiff += depth - depths[i];
                }

                float depthEdge = step(_DepthThreshold, depthDiff);

                float3 normal = getNormals(cord, 0);
                float3 normals[4];
                float dotsum = 0.0;
                for (int i = 0; i < 4; ++i)
                {
                    normals[i] = getNormals(uvs[i], 0);
                    float3 nDiff = normal - normals[i];
                    dotsum += dot(nDiff, nDiff);
                }
                float indic = sqrt(dotsum);
                float normalEdge = step(_NormalThreshold, indic);

                float outline =max(depthEdge,normalEdge);
                return outline;
            }
            ENDCG
        }
    }
}