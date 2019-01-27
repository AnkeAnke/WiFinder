Shader "Unlit/WiFiShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//_ColorSchema("Color Scheme", Int) = 0 
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100
		Cull Off
		ZWrite On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			int _NumShadowLines = 0;
			float4 _ShadowLines[16];
			int _ColorSchema;

			static const int COL_GGJ = 0;
			static const int COL_RED = 1;
			static const int COL_GOLD = 2;

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

			struct frag2out
			{
				float4 color : SV_TARGET;
				float depth : SV_DEPTH;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float drawCircle(float pixelSize, float dist, float radius, float phase);
			float4 getColor(int ring);

			vert2frag vert (appdata v)
            {
				vert2frag output;
				output.vertex = UnityObjectToClipPos(v.vertex);
				output.uv = v.uv;
                return output;
            }

            frag2out frag (vert2frag input)
            {
                // Compute pixel size.
				float pixelSize = abs(ddx(input.uv.x));


				// Get polar coordinates for shadow check.
				float3 dir = float3(input.uv - float2(0.5f, 0.5f), 0);
				float angle = atan2(dir.y, dir.x);
				float radius = length(dir) * 2;

				bool grey = false;

				for (int s = 0; s < _NumShadowLines; s++)
				{
					float4 coords = _ShadowLines[s];
					float3 a = float3(coords.x, coords.y, 0);
					float3 b = float3(coords.z, coords.w, 0);

					float3 axc = cross(a, dir);
					float3 bxc = cross(b, dir);

					// Not within the piece.
					if (axc.z > 0 || bxc.z < 0)
						continue;

					// Behind line?
					if (cross(b - a, dir - a).z < 0)
						continue;

					// Grey.
					grey = true;
				}


				// Accumulate color.
				frag2out output;
				output.color = float4(0,0,0, 0);
				output.depth = -5;

				int c = grey ? 7 : 0;
				for (; c < 8; ++c)
				{
					float circleRadius = 0.9f / 8 * (c + 1);
					float phase = c * -0.2f;
					float circ = drawCircle(pixelSize, radius, circleRadius, phase);
					if (circ > 0)
					{
						output.depth = lerp(0.1f, 0, radius);
						if (output.color.a == 0)
							output.color += circ * getColor(c);
						else
							output.color += getColor(c) * (1.0f - output.color.a);
					}
				}
				if (output.color.a == 0) discard;

				if (grey)
				{
					output.color = float4(0.1f, 0.1f, 0.1f, 1.0f);
					output.depth = 0;
				}
				return output;
            }

			float4 getColor(int ring)
			{
				float grey = ((float)ring + 1.0f) / 8;
				switch (_ColorSchema)
				{
					case COL_GGJ:
						return CIRCLE_COLORS[ring];
					case COL_RED:
						return float4(grey, 0, 0, 1.0f);
					default:
						grey = lerp(0.2f, 0.02f, grey);
						return float4(grey, grey, grey, 1.0f);
				}
			}

			float drawCircle(float pixelSize, float dist, float radius, float phase)
			{
				float time = (_Time.w + phase) * 0.6f;
				float radiusTimed = 1.0f + 0.1f * cos(cos(time) + 1 + time);
				radiusTimed *= radius;

				return smoothstep(radiusTimed + pixelSize, radiusTimed - pixelSize, dist);
			}

            ENDCG
        }
    }
}
