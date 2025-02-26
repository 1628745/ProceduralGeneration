Shader "Custom/SlopeBlendedTerrain"
{
    Properties
    {
        _GrassTex ("Grass Texture", 2D) = "white" {}
        _RockTex ("Rock Texture", 2D) = "white" {}
        _BlendSharpness ("Blend Sharpness", Range(0, 10)) = 4
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
            };

            sampler2D _GrassTex;
            sampler2D _RockTex;
            float _BlendSharpness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Calculate slope factor based on normal's Y component
                float slope = saturate(v.normal.y); // y = 1 for flat, y = 0 for vertical
                o.slopeFactor = pow(slope, _BlendSharpness); // Sharpen transition


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 grass = tex2D(_GrassTex, i.uv);
                fixed4 rock = tex2D(_RockTex, i.uv);

                // Use step function such that slopeFactor < 0.25 -> grass, slopeFactor > 0.75 -> rock
                fixed4 result = lerp(rock, grass, step(0.5, i.slopeFactor));
                return result;
            }
            ENDCG
        }
    }
}
