Shader "UI/GradientRevealText"
{
    Properties
    {
        [PerRendererData] _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Text Color", Color) = (1,1,1,1)
        _RevealAmount ("Reveal Amount", Range(0, 1)) = 0
        _GradientWidth ("Gradient Width", Range(0, 0.5)) = 0.15
        _NoiseStrength ("Noise Strength", Range(0, 0.2)) = 0.05
        _NoiseScale ("Noise Scale", Range(1, 20)) = 10
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _RevealAmount;
            float _GradientWidth;
            float _NoiseStrength;
            float _NoiseScale;

            // 简单的噪声函数
            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 采样字体纹理
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                
                // 计算从上到下的渐变位置（UV.y: 0=顶部, 1=底部）
                float revealPos = _RevealAmount;
                float currentPos = i.texcoord.y;
                
                // 添加噪声，使边缘参差不齐
                float noise = random(i.texcoord * _NoiseScale) * _NoiseStrength;
                float edgePos = revealPos + noise;
                
                // 计算透明度（渐变过渡）
                float alpha = 0;
                if (currentPos <= edgePos - _GradientWidth)
                {
                    alpha = 1;  // 完全显示区域
                }
                else if (currentPos >= edgePos)
                {
                    alpha = 0;  // 未显示区域
                }
                else
                {
                    // 渐变过渡区域
                    float t = (currentPos - (edgePos - _GradientWidth)) / _GradientWidth;
                    alpha = 1 - t;
                }
                
                // 可选：添加边缘光晕效果
                if (currentPos > edgePos - _GradientWidth * 0.5f && currentPos < edgePos)
                {
                    float glow = (currentPos - (edgePos - _GradientWidth * 0.5f)) / (_GradientWidth * 0.5f);
                    col.rgb += fixed3(0.5f, 0.5f, 0.5f) * (1 - glow) * 0.3f;
                }
                
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}