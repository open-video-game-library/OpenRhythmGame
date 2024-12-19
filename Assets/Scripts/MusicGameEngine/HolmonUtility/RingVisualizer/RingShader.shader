Shader "Custom/RingShader" {
    Properties {
        // Main texture and color properties
        _MainTex ("Texture", 2D) = "white" { }
        _Color ("Color", Color) = (1, 0, 0, 1) // Default color is red
        _InnerRadius ("Inner Radius", Range (0.0, 1.0)) = 0.3 // Inner radius
        _OuterRadius ("Outer Radius", Range (0.0, 1.0)) = 0.5 // Outer radius
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha // Set blending for transparency
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform float _InnerRadius;
            uniform float _OuterRadius;
            uniform float4 _Color; // Color from Image or SpriteRenderer

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainTex_ST;

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag(v2f i) : COLOR {
                half4 texcol = _Color; // Apply _Color for dynamic color/alpha
                float dist = distance(i.uv, float2(0.5, 0.5));

                // Calculate ring visibility based on distance from center
                if (dist < _InnerRadius || dist > _OuterRadius) {
                    clip(-1.0); // Discard pixel if outside the ring
                }

                return texcol;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
