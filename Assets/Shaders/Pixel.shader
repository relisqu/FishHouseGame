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

            float getNormals(float2 uv, float2 offset)
            {
                float4 NormalDepth;
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv + offset), NormalDepth.w, NormalDepth.xyz);
                return NormalDepth.xyz;
            }

            float2 pixelateCoord(float2 coord)
            {
                float2 c = floor(coord / float(_PixelSize)) * float(_PixelSize) / _Resolution.xy;

                //c.y = 1.0 - c.y;
                return c;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                int texelX = floor(i.uv.x * _Resolution.x);
                int texelY = floor(i.uv.y * _Resolution.y);
                float2 cord = pixelateCoord(float2(texelX, texelY));
                return tex2D(_MainTex,cord);


                float4 NormalDepth;
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv), NormalDepth.w, NormalDepth.xyz);
                float2 texelSize = _MainTex_TexelSize;

                float2 curDepth = NormalDepth.w;
                float2 bottomLeftDepth = getDepth(
                    i.uv, -float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float2 topRightDepth = getDepth(
                    i.uv, float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float2 bottomRightDepth = getDepth(
                    i.uv, float2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y));
                float2 topLeftDepth = getDepth(i.uv, float2(-_MainTex_TexelSize.x,
                                                            _MainTex_TexelSize.y));

                float depthFiniteDifference0 = topRightDepth - bottomLeftDepth;
                float depthFiniteDifference1 = topLeftDepth - bottomRightDepth;
                float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;
                edgeDepth = edgeDepth > _DepthThreshold ? 1 : 0;

                float3 bottomLeftNormal = getNormals(i.uv, -float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float3 topRightNormal = getNormals(i.uv, float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y));
                float3 bottomRightNormal = getNormals(i.uv, float2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y));
                float3 topLeftNormal = getNormals(i.uv, float2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y));

                float3 normalFiniteDifference0 = topRightNormal - bottomLeftNormal;
                float3 normalFiniteDifference1 = topLeftNormal - bottomRightNormal;

                float edgeNormal = sqrt(
                    dot(normalFiniteDifference0, normalFiniteDifference0) + dot(
                        normalFiniteDifference1, normalFiniteDifference1));
                float dNFD0 = dot(normalFiniteDifference0, normalFiniteDifference0);
                float dNFD1 = dot(normalFiniteDifference1, normalFiniteDifference1);

                //edgeNormal = edgeNormal > _NormalThreshold ? 1 : 0;
                dNFD0 = dNFD0;
                dNFD1 = dNFD1;
                //edgeNormal = sqrt(dNFD0 + dNFD1);
                edgeNormal = edgeNormal > _NormalThreshold ? 1 : 0;


                float3 viewPos = UnityObjectToViewPos(i.vertex);
                float3 viewDir = normalize(viewPos);

                float edge = max(edgeDepth, edgeNormal);

                half4 color = 0;
                color.rgb = NormalDepth.xyz;
                if (edgeDepth > 0) { return tex2D(_MainTex, i.uv) * 0.4 + _OutlineColor; }
                else
                {
                    return tex2D(_MainTex, i.uv) + edgeDepth * _OutlineColor;
                }
            }
            ENDCG
        }
    }
}