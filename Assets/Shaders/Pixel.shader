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
                return NormalDepth.w;
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

            float GetDepthBorder(float2 uv, float depthThreshold)
            {
                float2 bottomLeftDepth = getDepth(
                    uv, -float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float2 topRightDepth = getDepth(
                    uv, float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float2 bottomRightDepth = getDepth(
                    uv, float2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y));
                float2 topLeftDepth = getDepth(uv, float2(-_MainTex_TexelSize.x,
                                                          _MainTex_TexelSize.y));

                float depthFiniteDifference0 = topRightDepth - bottomLeftDepth;
                float depthFiniteDifference1 = topLeftDepth - bottomRightDepth;
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;
                edgeDepth = edgeDepth > depthThreshold ? 1 : 0;

                return edgeDepth;
            }

            float GetNormalBorder(float2 uv, float normalThreshold)
            {
                float3 bottomLeftDepth = getNormals(
                    uv, -float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float3 topRightDepth = getNormals(
                    uv, float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float3 bottomRightDepth = getNormals(
                    uv, float2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y));
                float3 topLeftDepth = getNormals(uv, float2(-_MainTex_TexelSize.x,
                                                            _MainTex_TexelSize.y));

                float depthFiniteDifference0 = topRightDepth - bottomLeftDepth;
                float depthFiniteDifference1 = topLeftDepth - bottomRightDepth;
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2));
                edgeDepth = edgeDepth > normalThreshold ? 1 : 0;
                return edgeDepth;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                int texelX = floor(i.uv.x * _Resolution.x);
                int texelY = floor(i.uv.y * _Resolution.y);
                float2 cord = pixelateCoord(float2(texelX, texelY));

                float edgeDepth = GetDepthBorder(cord, _DepthThreshold);
                float edgeNormal = GetNormalBorder(cord, _NormalThreshold);
                return edgeDepth * _OutlineColor + edgeNormal;
                float edge = max(edgeDepth, edgeNormal);
                if (edgeDepth > 0)
                {
                    return edge;
                }
                else
                {
                    return tex2D(_MainTex, i.uv);
                }
            }
            ENDCG
        }
    }
}