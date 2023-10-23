Shader "Unlit/ToonShader"
{

    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset]
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _ShadowStrength ("Shadow Strength", Range(0, 1)) = 0.5
        [HDR]
        _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32
    }

    SubShader
    {

        Pass
        {
            Tags
            {
                "RenderType"="Opaque" 
                "LightMode" = "ForwardBase"
                "Queue" = "Geometry" 
                "PassFlags" = "OnlyDirectional"
            }
            Cull Off 

            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            #include "UnityCG.cginc"
            // Files below include macros and functions to assist
            // with lighting and shadows.
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
                float3 normal : NORMAL;
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float3 viewDir : TEXCOORD1;
                float4 vertex : SV_POSITION; // clip space position
                float3 worldNormal : NORMAL;
                SHADOW_COORDS(2)
            };

            // vertex shader
            v2f vert(appdata v)
            {
                v2f o;
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                TRANSFER_SHADOW(o)
                return o;
            }

            // texture we will sample
            sampler2D _MainTex;
            float4 _Color;
            float _ShadowStrength;
            float4 _AmbientColor;
            float _Glossiness;
            float4 _SpecularColor;

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag(v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float NdotL = dot(_WorldSpaceLightPos0, normal);

                float lightIntensity = NdotL > 0 ? 1 : _ShadowStrength;
                float4 light = lightIntensity * _LightColor0;
                // Calculate specular reflection.
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);

                float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
                float specularIntensitySmooth = smoothstep(0, 0.001, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor;
                fixed4 col = tex2D(_MainTex, i.uv);

                return (light + _AmbientColor + specular) * _Color * col;
            }
            ENDCG
        }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}