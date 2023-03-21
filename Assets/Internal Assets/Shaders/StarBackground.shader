Shader "Unlit/StarBackground_Shader"
{
	Properties
	{
		_StarCount("StarCount", Float) = 300
		_StarSizeMultiplier("StarSizeMultiplier", Float) = 1
		_Move("Move", Vector) = (0, 0, 0, 0)
		_Seed("Seed", Vector) = (32.9898, 78.233, 0, 0)
		_StarColorMin("StarColorMin", Color) = (155., 176., 255., 255.)
		_StarColorMax("StarColorMax", Color) = (255., 204., 111., 255.)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			LOD 100

			ZWrite Off
			Blend SrcAlpha One

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

				float2 _Move;
				float _StarCount;
				float _StarSizeMultiplier;
				float2 _Seed;
				float4 _StarColorMin;
				float4 _StarColorMax;


				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;// TRANSFORM_TEX(v.uv, _MainTex);

					return o;
				}

				//https://www.shadertoy.com/view/ldKGDd

				float rand(float i) {
					//float2 seed = float2(32.9898, 78.233);
					return frac(sin(dot(float2(i, i), _Seed)) * 43758.5453);
				}

				fixed4 frag(v2f input) : SV_Target
				{
					float4 f4one = float4(1.0, 1.0, 1.0, 1.0);

					fixed4 fragColor = fixed4(0.0, 0.0, 0.0, 0);

					float3 col1 = _StarColorMin.rgb / 256.;//float3(155., 176., 255.) / 256.; // Coolest star color
					float3 col2 = _StarColorMax.rgb / 256.;//float3(255., 204., 111.) / 256.; // Hottest star color

					float2 uv = input.uv;

					// static far stars
					float uvRand = rand(uv.x * uv.y);
					float4 sStar = uvRand * f4one;
					sStar *= pow(uvRand, 200.);
					sStar.xyz *= lerp(col1, col2, rand(uv.x + uv.y));
					fragColor += sStar;

					// Dynamic Stars    
					for (float n = 0; n < _StarCount; n += 1.0) {
						//position of the star
						float3 pos = float3(rand(n), rand(n + 1.), rand(n + 2.)) * 2;

						pos.xy += _Move.xy * 0.001f * pos.z;

						// parralax effect
						pos.x = frac(pos.x * pos.z);
						pos.y = frac(pos.y * pos.z);

						//drawing the star
						float4 col = (pow(length(pos.xy - uv), -1.25) * 0.0001 * _StarSizeMultiplier * pos.z * rand(n + 3.)) * f4one;

						//coloring the star
						col.xyz *= lerp(col1, col2, rand(n + 4.));

						//star flickering
						//col.xyz *= lerp(rand(n + 5.), 1.0, abs(cos(fmod(_Time.y, 1) * rand(n + 6.) * 5.)));

						fragColor += fixed4(col);
					}

					return fragColor;
				}


				ENDCG
			}
		}
}
