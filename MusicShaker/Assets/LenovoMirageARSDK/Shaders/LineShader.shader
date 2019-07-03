Shader "Line/imaginary" {
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.1
		_Color("Color",Color) = (0,0,0,0)
	}
		SubShader
	{
		Pass
	{
		Material
	{
		Diffuse[_Color]
		Ambient[_Color]
	}
		Lighting Off
		AlphaTest GEqual[_Cutoff]

		SetTexture[_MainTex]{}
		SetTexture[_MainTex]{ combine texture + primary, previous }
	}
	}
		FallBack "Diffuse"
}