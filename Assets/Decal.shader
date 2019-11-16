Shader "Custom/Decal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,1)
	}
	SubShader
	{      
        GrabPass
        {
            "_BackgroundTexture"
        }

		Pass
		{
            Tags {"RenderType"="Opaque"}
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest LEqual Cull Off
            ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                float3 worldPos    : TEXCOORD1;
                float4 scrPos : TEXCOORD2;
                float3 objPos : TEXCOORD3; 
                float4 grabPos : TEXCOORD4;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            sampler2D _CameraDepthTexture;
            float4 _CameraDepthTexture_TexelSize;
            sampler2D _BackgroundTexture;
            float4 _Tint;
            
			v2f vert (appdata_full v)
			{
                float3 worldScale = float3(
                length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)), // scale x axis
                length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)), // scale y axis
                length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))  // scale z axis
                );
				v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.uv = v.texcoord;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.scrPos=ComputeScreenPos(o.vertex);
                o.objPos = mul(unity_WorldToObject,mul(unity_ObjectToWorld, float4(0,0,0,1) ).xyz);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
            float greater_than(float x , float y)
            {
                return step(y, x);
            }
            float less_than(float x, float y)
            {
                return step(x, y);
            }
            float3 Multiply(float3 a, float3 b)
            {
                return a * b;
            }
            float3 Overlay(float3 a, float3 b)
            {
                float luminance = dot(a, float3(0.2126729, 0.7151522, 0.0721750));
                return lerp(a + b, Multiply(a,b), luminance * 0.6);
            }
            
			fixed4 frag (v2f i) : SV_Target
			{
                UNITY_SETUP_INSTANCE_ID(i); 
                float depthSample  = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, i.scrPos)).r;
                float3 direction = normalize(i.worldPos - _WorldSpaceCameraPos);
                float newDepth = depthSample - i.scrPos.w;
                float3 projDecalPos = mul(unity_WorldToObject,i.worldPos + (direction * newDepth));
                half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
                fixed4 col = 1;
                float alpha = less_than(projDecalPos.x,i.objPos.x + 0.5) * greater_than(projDecalPos.x,i.objPos.x - 0.5) * 
                                less_than(projDecalPos.z,i.objPos.z + 0.5) * greater_than(projDecalPos.z,i.objPos.z - 0.5);
                col.rgb = Overlay(bgcolor.rgb, tex2D(_MainTex, (((projDecalPos.xz - i.objPos.xz)) + 0.5))) * _Tint.rgb;
                alpha *= tex2D(_MainTex, (((projDecalPos.xz - i.objPos.xz) + 0.5) * _MainTex_ST.xy) + _MainTex_ST.zw).a;
                col.a = alpha * _Tint.a;

				return col;
			}
			ENDCG
		}
        
	}
}