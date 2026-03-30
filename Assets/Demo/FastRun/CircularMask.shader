Shader "Hidden/CircularMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Center ("Center", Vector) = (0,0,0,0)
        _Radius ("Radius", Float) = 500
        _MaskColor ("Mask Color", Color) = (0,0,0,1)
        _ScreenSize ("Screen Size", Vector) = (1920,1080,0,0)
        _Feather ("Feather", Range(0, 0.1)) = 0  // 可选：边缘羽化效果
    }
    
    SubShader
    {
        // 不需要深度写入，不需要剔除，始终渲染
        Cull Off 
        ZWrite Off 
        ZTest Always
        
        Pass
        {
            Name "CircularMaskPass"
            
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
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
            float2 _Center;
            float _Radius;
            float4 _MaskColor;
            float2 _ScreenSize;
            float _Feather;
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 获取原始颜色
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 计算当前像素的屏幕坐标
                float2 screenPos = i.uv * _ScreenSize;
                float dist = distance(screenPos, _Center);
                
                // 圆形内显示原始画面
                if (dist <= _Radius)
                {
                    return col;
                }
                // 圆形外显示遮罩颜色
                else
                {
                    // 可选：边缘羽化效果
                    if (_Feather > 0)
                    {
                        float alpha = smoothstep(_Radius, _Radius + _Feather, dist);
                        return lerp(col, _MaskColor, alpha);
                    }
                    
                    return _MaskColor;
                }
            }
            ENDCG
        }
    }
    
    // 如果设备不支持上述SubShader，使用这个Fallback
    Fallback "Diffuse"
}