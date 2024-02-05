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
        _InnerOutlineColor("InnerOutlineColor", Color) = (0.8,0.8,0.8,1)
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
            float4 _InnerOutlineColor;

            vector _Resolution;
            float _PixelSize;
            float _PixelYSize;
            float _dx;
            float _dy;

            float getDepth(float2 uv, float2 offset)
            {
                float4 NormalDepth;
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv + offset), NormalDepth.w, NormalDepth.xyz);
                float depth = NormalDepth.w;
                depth = clamp(NormalDepth.w * 100, 0, 1);
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
                uvs[0] = cord + pixelateCoord(float2(0.0, _PixelSize));
                uvs[1] = cord - pixelateCoord(float2(0.0, _PixelSize));
                uvs[2] = cord + pixelateCoord(float2(_PixelSize, 0));
                uvs[3] = cord - pixelateCoord(float2(_PixelSize, 0));

                float depths[4];
                float depthDiff = 0.0;

                for (int i = 0; i < 4; ++i)
                {
                    depths[i] = getDepth(uvs[i], 0);
                    depthDiff += depth - depths[i];
                }

                float depthEdge = step(_DepthThreshold, depthDiff);

                float3 normal = getNormals(cord, 0);
                float3 bottomLeftNormal = getNormals(cord, pixelateCoord(-float2(_PixelSize, _PixelSize)));
                float3 topRightNormal = getNormals(cord, pixelateCoord(float2(_PixelSize, _PixelSize)));
                float3 bottomRightNormal = getNormals(cord, pixelateCoord(float2(_PixelSize, -_PixelSize)));
                float3 topLeftNormal = getNormals(cord, pixelateCoord(float2(-_PixelSize, _PixelSize)));

                float3 normalFiniteDifference0 = topRightNormal - bottomLeftNormal;
                float3 normalFiniteDifference1 = topLeftNormal - bottomRightNormal;
                float3 normals[4];
                float3 normalsDiff = 0.0;

                for (int i = 0; i < 4; ++i)
                {
                    normals[i] = getNormals(uvs[i], 0);
                    normalsDiff += normal - normals[i];
                }
                float normalsEdge = step(_NormalThreshold, normalsDiff);


                half4 color = 0;
                color.rgb = normal;
                float outline = max(depthEdge, normalsEdge);

                if (depthEdge > 0)
                {
                    return depthEdge * _OutlineColor;
                }
                if (normalsEdge > 0)
                {
                    return (tex2D(_MainTex, cord)*0.5f)+_InnerOutlineColor;
                }
                else
                {
                    return tex2D(_MainTex, cord);
                }
            }
            ENDCG
        }
    }
}