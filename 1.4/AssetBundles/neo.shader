Shader "Custom/neo" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Vector) = (1, 1, 1, 1)
	}

	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

            struct vertex_data {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD;
            };

			struct fragment_data {
				float4 vertex : SV_POSITION;
                float4 color : COLOR;
				float2 texcoord : TEXCOORD;
                float4 object_in_world : TEXCOORD1;
			};
			
            float4 _PlanetSunLightDirection;
			float _PlanetSunLightEnabled;
			float4 _Color;
			sampler2D _MainTex;
			 
			fragment_data vert(vertex_data v) {
                fragment_data f;
                f.object_in_world = mul(unity_ObjectToWorld, v.vertex);
                f.color = _Color;
                f.texcoord.xy = v.texcoord.xy;
                f.vertex = mul(unity_MatrixVP, f.object_in_world);
                return f;
			}
			
			fixed4 frag(fragment_data f) : SV_Target {
                float4 pixel;
                pixel = tex2D(_MainTex, f.texcoord.xy);
                if (pixel.w <= 0.5) {
                    discard;
                }
                float4 light;
                light.x = dot(f.object_in_world.xyz, f.object_in_world.xyz);
                light.x = rsqrt(light.x);
                light.xyz = light.xxx * f.object_in_world.xyz;
                light.x = dot(light.xyz, _PlanetSunLightDirection.xyz);
                light.x = light.x * 0.5 + 0.5;
                light.x = -_PlanetSunLightEnabled * light.x + 0.55;
                light.x = saturate(light.x * 10.0);
                light.xyz = light.xxx * float3(0.518, 0.397, 0.318) + float3(0.482, 0.603, 0.682);
                pixel.xyz = pixel.xyz * light.xyz * f.color;
                return pixel;
			}
			ENDCG
		}
	}
	Fallback "Unlit/Texture"
}
