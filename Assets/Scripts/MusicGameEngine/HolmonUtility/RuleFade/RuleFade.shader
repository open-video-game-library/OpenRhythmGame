Shader "Unlit/RuleFade"  // �V�F�[�_����K�؂ɕύX
{
    Properties
    {
        [PerRendererData] _MainTex("Main Texture", 2D) = "white" {}  // ���C���e�N�X�`��
        _RuleTex("Rule Texture (Mask)", 2D) = "white" {}  // �t�F�[�h�Ɏg�p���郋�[���e�N�X�`��
        _Color("Tint", Color) = (1,1,1,1)  // �e�B���g�J���[
        _Alpha ("Fade Amount", Range(0, 1)) = 0  // �t�F�[�h��
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
            sampler2D _RuleTex;  // �ǉ�: ���[���e�N�X�`��

            // ���_�V�F�[�_�[�̊�{
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

            // �t���O�����g�V�F�[�_�[
            fixed4 frag(v2f IN) : SV_Target
            {
                // ���C���e�N�X�`���̃J���[���擾
                fixed4 mainColor = tex2D(_MainTex, IN.texcoord);

                // ���[���e�N�X�`���̓����x���擾
                fixed4 ruleColor = tex2D(_RuleTex, IN.texcoord);

                // ���[���e�N�X�`���Ɋ�Â��ăt�F�[�h����
                half fadeFactor = saturate(ruleColor.a + (_Alpha * 2 - 1));

                // �t�F�[�h���ꂽ�����x��K�p
                mainColor.a *= fadeFactor;

                return mainColor * _Color;
            }
            ENDCG
        }
    }

    FallBack "UI/Default"
}
