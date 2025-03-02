Shader "Unlit/RuleFade"  // シェーダ名を適切に変更
{
    Properties
    {
        [PerRendererData] _MainTex("Main Texture", 2D) = "white" {}  // メインテクスチャ
        _RuleTex("Rule Texture (Mask)", 2D) = "white" {}  // フェードに使用するルールテクスチャ
        _Color("Tint", Color) = (1,1,1,1)  // ティントカラー
        _Alpha ("Fade Amount", Range(0, 1)) = 0  // フェード量
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest[unity_GUIZTestMode]
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed _Alpha;
            sampler2D _MainTex;
            sampler2D _RuleTex;  // 追加: ルールテクスチャ

            // 頂点シェーダーの基本
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET
                OUT.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1,1);
#endif
                return OUT;
            }

            // フラグメントシェーダー
            fixed4 frag(v2f IN) : SV_Target
            {
                // メインテクスチャのカラーを取得
                fixed4 mainColor = tex2D(_MainTex, IN.texcoord);

                // ルールテクスチャの透明度を取得
                fixed4 ruleColor = tex2D(_RuleTex, IN.texcoord);

                // ルールテクスチャに基づいてフェード処理
                half fadeFactor = saturate(ruleColor.a + (_Alpha * 2 - 1));

                // フェードされた透明度を適用
                mainColor.a *= fadeFactor;

                return mainColor * _Color;
            }
            ENDCG
        }
    }

    FallBack "UI/Default"
}
