Shader "Unlit/WiFiShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			static fixed4 CIRCLE_COLORS[8] = 
			{
				fixed4(37, 33, 98, 256) / 256,
				fixed4(61, 45, 100, 256) / 256,
				fixed4(87, 55, 105, 256) / 256,
				fixed4(112, 80, 135, 256) / 256,
				fixed4(138, 145, 166, 256) / 256,
				fixed4(134, 187, 160, 256) / 256,
				fixed4(202, 220, 106, 256) / 256,
				fixed4(244, 236, 88, 256) / 256
			};

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vert2frag
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float drawCircle(float pixelSize, float dist, float radius, float phase);

			vert2frag vert (appdata v)
            {
				vert2frag output;
				output.vertex = UnityObjectToClipPos(v.vertex);
				output.uv = v.uv;
                return output;
            }

            fixed4 frag (vert2frag input) : SV_Target
            {
                // sample the texture
				float pixelSize = abs(ddx(input.uv.x));
				float radius = length(input.uv - float2(0.5f, 0.5f)) * 2;

				float4 color = float4(0, 0, 0, 0);

				for (int c = 0; c < 8; ++c)
				{
					float circleRadius = 0.9f / 8 * (c + 1);
					float phase = c * -0.2f;
					float circ = drawCircle(pixelSize, radius, circleRadius, phase);
					if (circ > 0)
					{
						if (color.a == 0)
							color += circ * CIRCLE_COLORS[c];
						else
							color += CIRCLE_COLORS[c] * (1.0f - color.a);
					}
				}
				if (color.a == 0) discard;
				return color;
            }

			float drawCircle(float pixelSize, float dist, float radius, float phase)
			{
				float time = (_Time.w + phase) * 0.6f;
				float radiusTimed = 1.0f + 0.1f * cos(cos(time) + 1 + time);
				radiusTimed *= radius;

				return smoothstep(radiusTimed + pixelSize, radiusTimed - pixelSize, dist);
				//if (dist < radiusTimed) return 1;
				//return 0;
			}

            ENDCG
        }
    }
}
