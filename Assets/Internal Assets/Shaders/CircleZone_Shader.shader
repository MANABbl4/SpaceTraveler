Shader "Unlit/CircleZone_Shader"
{
    Properties
    {
		_InZoneColor("In Color", Color) = (1, 1, 1, 1)
		_OutZoneColor("Out Color", Color) = (0, 0, 0, 0)
		_PlaneSize("Size", Float) = 10
		_MinRadius("Min Radius", Float) = 1
		_MaxRadius("Max Radius", Float) = 100
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }
        LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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

			float4 _InZoneColor;
			float4 _OutZoneColor;
			float _MinRadius;
			float _MaxRadius;
			float _PlaneSize;


			v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;// TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col;

				float minRadius = _MinRadius / _PlaneSize;
				float maxRadius = _MaxRadius / _PlaneSize;

				float x = i.uv.x;
				float y = i.uv.y;
				float dist = sqrt((0.5 - x)* (0.5 - x) + (0.5 - y)* (0.5 - y));

				if (dist >= minRadius && dist <= maxRadius) {
					col = _InZoneColor;
				}
				else {
					col = _OutZoneColor;
				}
				return col;
            }
            ENDCG
        }
    }
}
