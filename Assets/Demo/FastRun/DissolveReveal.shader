// DissolveReveal.shader
Shader "Custom/DissolveReveal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
        _DissolveSpeed ("Dissolve Speed", Range(0, 2)) = 1
        _EdgeWidth ("Edge Width", Range(0, 0.5)) = 0.05
        _EdgeColor ("Edge Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 2)) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _Progress;
            float _DissolveSpeed;
            float _EdgeWidth;
            float4 _EdgeColor;
            float _GlowIntensity;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 获取主纹理颜色
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 获取噪点纹理值 (0-1)
                float noise = tex2D(_NoiseTex, i.uv).r;
                
                // 溶解阈值（从0到1逐渐增加，实现从空到完整的复原效果）
                float threshold = _Progress;
                
                // 计算溶解区域
                float dissolve = noise - threshold;
                
                // 边缘发光区域
                float edge = step(abs(dissolve), _EdgeWidth);
                
                // 完全溶解的部分透明
                if (dissolve < 0)
                {
                    col.a = 0;
                }
                else
                {
                    col.a = 1;
                    // 边缘添加发光效果
                    if (edge > 0)
                    {
                        col.rgb += _EdgeColor.rgb * _GlowIntensity;
                    }
                }
                
                return col;
            }
            ENDCG
        }
    }
}