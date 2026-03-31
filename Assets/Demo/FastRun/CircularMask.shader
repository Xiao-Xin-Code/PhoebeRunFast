Shader "UI/CircularMask"
{
	Properties
	{
		 [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Range(0, 1)) = 0.5
        _MaskColor ("Mask Color", Color) = (0,0,0,1)
	}

	 SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha  // 使用透明混合

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
            float4 _MainTex_ST;
            fixed4 _Color;
            float2 _Center;
            float _Radius;
            float4 _MaskColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 获取原始图片颜色（遮罩图本身）
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                
                // 修正宽高比：将UV坐标转换为等比例空间
                float2 uv = i.texcoord - _Center;

                float2 corner = float2(0, 0);
                if(_Center.x < 0.5) corner.x = 1;
                if(_Center.y < 0.5) corner.y = 1;
                float2 toCorner = corner - _Center;
                float maxRadius = length(toCorner);

                // 将半径映射到实际距离
                float actualRadius = _Radius * maxRadius;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                uv.x *= aspect;
                float dist = length(uv);

                // 计算到圆心的距离
                //float dist = distance(i.texcoord, _Center);
                
                // 圆形内：完全透明，显示下层内容
                if (dist <= actualRadius)
                {
                    return fixed4(0, 0, 0, 0);  // 完全透明
                }
                // 圆形外：显示遮罩颜色
                else
                {
                    return _MaskColor;
                }
            }
            ENDCG
        }
    }





}
