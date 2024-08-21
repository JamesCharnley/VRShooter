// WarFX Shader
// (c) 2015 Jean Moreno

Shader "WFX/Scroll/Alpha Blended"
{
	Properties
	{
		_MainTex ("Looped Texture + Alpha Mask", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		
		_ScrollSpeed ("Scroll Speed", Float) = 2.0
	}
	
	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off
		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
	
		// ---- Fragment program cards
		SubShader
		{
			Pass 
			{
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_particles
	
				#include "UnityCG.cginc"
	
				sampler2D _MainTex;
				
				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};
	
				struct v2f
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD1;
					#endif
					UNITY_VERTEX_OUTPUT_STEREO
				};
	
				float4 _MainTex_ST;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v); //Insert
					UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos (o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
	
				sampler2D _CameraDepthTexture;
				float _InvFade;
				float _ScrollSpeed;
				
				fixed4 frag (v2f i) : COLOR
				{
					#ifdef SOFTPARTICLES_ON
					float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
					float partZ = i.projPos.z;
					float fade = saturate (_InvFade * (sceneZ-partZ));
					i.color.a *= fade;
					#endif
					
					fixed4 prev = i.color.a * tex2D(_MainTex, i.texcoord).a;
					i.texcoord.y -= fmod(_Time*_ScrollSpeed,1);
					prev.rgb = i.color.rgb * tex2D(_MainTex, i.texcoord).rgb;
//					prev.rgb = 1-prev.rgb;
					return prev;
				}
				ENDCG 
			}
		}
	}
}