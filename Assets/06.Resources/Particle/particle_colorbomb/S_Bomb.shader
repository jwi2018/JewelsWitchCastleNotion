// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Bomb Smoke"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TintColor("Tint Color", Color) = (0,0,0,0)
		_MainTexture("Main Texture", 2D) = "white" {}
		_Dissolve("Dissolve", 2D) = "white" {}
		_DissolveVal("Dissolve Val", Float) = -0.36
		_SoftParticleFactor("Soft Particle Factor", Range( 0 , 0.999)) = 0.5
		[Toggle(_USEPARTICLE_ON)] _UseParticle("Use Particle", Float) = 0
		_Ins("Ins", Range( 0 , 1)) = 0.4906557
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _USEPARTICLE_ON
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
			float4 screenPos;
		};

		uniform float4 _TintColor;
		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform sampler2D _Dissolve;
		uniform float _DissolveVal;
		uniform float _SoftParticleFactor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Ins;
		uniform float _Cutoff = 0.5;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			float4 tex2DNode45 = tex2D( _MainTexture, uv_MainTexture );
			o.Emission = ( _TintColor * ( i.vertexColor * tex2DNode45 ) ).rgb;
			o.Alpha = 1;
			float cos27 = cos( 1.0 * _Time.y );
			float sin27 = sin( 1.0 * _Time.y );
			float2 rotator27 = mul( i.uv_texcoord - float2( 0.5,0.5 ) , float2x2( cos27 , -sin27 , sin27 , cos27 )) + float2( 0.5,0.5 );
			#ifdef _USEPARTICLE_ON
				float staticSwitch35 = i.uv_tex4coord.z;
			#else
				float staticSwitch35 = _DissolveVal;
			#endif
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth34 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			#ifdef _USEPARTICLE_ON
				float staticSwitch52 = i.uv_tex4coord.w;
			#else
				float staticSwitch52 = _Ins;
			#endif
			clip( ( saturate( ( tex2DNode45.a * ( step( ( 1.0 - ( ( tex2D( _Dissolve, rotator27 ).r * 6.57 ) + staticSwitch35 ) ) , 0.96 ) * i.vertexColor.r * saturate( ( ( 1.0 - _SoftParticleFactor ) * ( eyeDepth34 - ase_screenPos.w ) * 3.0 ) ) ) ) ) * staticSwitch52 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
1920;0;1920;1019;3032.324;1366.059;2.505058;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-3157.454,-50.398;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;25;-3078.319,474.5816;Inherit;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;27;-2694.583,253.8624;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;28;-2236.657,805.8142;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-2185.628,722.8124;Inherit;False;Property;_DissolveVal;Dissolve Val;4;0;Create;True;0;0;0;False;0;False;-0.36;-0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-2360.947,280.2162;Inherit;True;Property;_Dissolve;Dissolve;3;0;Create;True;0;0;0;False;0;False;-1;None;77418e813cee38b4e8da9049e3675b03;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;34;-1826.78,971.9047;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;33;-1817.778,1044.019;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;-1909.997,899.7545;Inherit;False;Property;_SoftParticleFactor;Soft Particle Factor;5;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0.999;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2033.551,349.8077;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;6.57;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;35;-1978.657,744.8143;Inherit;False;Property;_UseParticle;Use Particle;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;38;-1634.65,904.7189;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1720.978,417.3931;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-1626.645,1016.829;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1623.151,1131.047;Inherit;False;Constant;_3;3;4;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;41;-1499.025,390.386;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1463.291,990.3992;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;43;-1353.303,801.7218;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;42;-1339.66,990.8298;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;44;-1332.024,406.386;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.96;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1119.948,782.25;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;45;-1671.317,-35.99852;Inherit;True;Property;_MainTexture;Main Texture;2;0;Create;True;0;0;0;False;0;False;-1;None;00d301b9065a4764c904e5137cdbfa03;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1055.461,189.3905;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-914.2386,349.172;Inherit;False;Property;_Ins;Ins;7;0;Create;True;0;0;0;False;0;False;0.4906557;0.507;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;48;-925.3206,432.1393;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;50;-1510.46,-263.6096;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;52;-606.5618,432.2331;Inherit;False;Property;_UseParticle;Use Particle;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;-1087.597,-390.7458;Inherit;False;Property;_TintColor;Tint Color;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.01027946,0.1981132,0.02071467,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;51;-695.4607,78.39038;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1015.079,-150.1851;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-336.9829,217.8124;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-788.1947,-262.0433;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;54.53851,34.40556;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;FX/Bomb Smoke;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;26;0
WireConnection;27;1;25;0
WireConnection;29;1;27;0
WireConnection;31;0;29;1
WireConnection;35;1;30;0
WireConnection;35;0;28;3
WireConnection;38;0;32;0
WireConnection;37;0;31;0
WireConnection;37;1;35;0
WireConnection;36;0;34;0
WireConnection;36;1;33;4
WireConnection;41;0;37;0
WireConnection;40;0;38;0
WireConnection;40;1;36;0
WireConnection;40;2;39;0
WireConnection;42;0;40;0
WireConnection;44;0;41;0
WireConnection;46;0;44;0
WireConnection;46;1;43;1
WireConnection;46;2;42;0
WireConnection;49;0;45;4
WireConnection;49;1;46;0
WireConnection;52;1;47;0
WireConnection;52;0;48;4
WireConnection;51;0;49;0
WireConnection;54;0;50;0
WireConnection;54;1;45;0
WireConnection;53;0;51;0
WireConnection;53;1;52;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;0;2;57;0
WireConnection;0;10;53;0
ASEEND*/
//CHKSM=485B4953BC9C3DA2402A299F9266C20A249716C2