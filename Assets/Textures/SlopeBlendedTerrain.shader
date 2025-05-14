Shader "Custom/SlopeBlendedTerrain"
{
    Properties
    {
        _GrassTex ("Grass Texture", 2D) = "white" {}
        _RockTex ("Rock Texture", 2D) = "white" {}
        _BlendSharpness ("Blend Sharpness", Range(0, 10)) = 4
        _SnowHeight ("Snow Height Threshold", Float) = 350
        _SnowColor ("Snow Color", Color) = (0,1,1,1)
        _SandTex ("Sand Texture", 2D) = "white" {}
        _SandHeight ("Sand Height Threshold", Float) = 80


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float slopeFactor : TEXCOORD1;
                float worldY : TEXCOORD2;
                float2 worldXZ : TEXCOORD3;
            };


            sampler2D _GrassTex;
            sampler2D _RockTex;
            float _BlendSharpness;
            float _SnowHeight;
            fixed4 _SnowColor;
            sampler2D _SandTex;
            float _SandHeight;



            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldY = worldPos.y;
                o.worldXZ = worldPos.xz;

                float slope = saturate(v.normal.y);
                o.slopeFactor = pow(slope, _BlendSharpness);

                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 grass = tex2D(_GrassTex, i.uv);
                fixed4 rock = tex2D(_RockTex, i.uv);
                fixed4 sand = tex2D(_SandTex, i.uv);

                // Base slope blend: rock for steep, grass for flat
                float slopeBlend = smoothstep(0.4, 0.8, i.slopeFactor);
                fixed4 baseTerrain = lerp(rock, grass, slopeBlend);

                // Natural variation using world XZ (worldXZ.xy)
                float variation = sin(i.worldXZ.x * 0.01) * cos(i.worldXZ.y * 0.01) * 10.0;

                // --- Snow ---
                float snowFactor = smoothstep(_SnowHeight - 10 + variation, _SnowHeight + 10 + variation, i.worldY);
                fixed4 snowBlended = lerp(baseTerrain, _SnowColor, snowFactor);

                // --- Sand blending ---
                // Fade in sand below a certain height, mixed with base terrain
                /*float sandBlendFactor = smoothstep(_SandHeight + 10 + variation, _SandHeight - 10 + variation, i.worldY);
                fixed4 sandBlended = lerp(snowBlended, sand, sandBlendFactor);*/

                return snowBlended;
            }




            ENDCG
        }
    }
}
