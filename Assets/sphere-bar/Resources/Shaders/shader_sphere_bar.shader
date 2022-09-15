Shader "ShardShaders/SphereBar"
{
    Properties
    {
        _MainTex ("Mask", 2D) = "white" {}
        _FillFrontTex ("Fill Front Texture", 2D) = "white" {}
        _FillBackTex ("Fill Back Texture", 2D) = "white" {}

        _FrontWaveSpeed("Front Wave Speed", Range(-1,1)) = 0
        
        _BackWaveSpeed("Back Wave Speed", Range(-1,1)) = 0
        
		_Progress("Progress", Range(0,1)) = 0.5
		
    }
    SubShader
    {
        Tags
		{ 
			"Queue"="Overlay" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

        Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {

            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            
            sampler2D _FillFrontTex;
            float4 _FillFrontTex_ST;
            float4 _FillFrontTex_TexelSize;
            
             sampler2D _FillBackTex;
            float4 _FillBackTex_ST;
            float4 _FillBackTex_TexelSize;
            

            float _FrontWaveSpeed;
            float _BackWaveSpeed;
            
            float _Progress;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            			
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mask = tex2D(_MainTex, i.uv);
				
				fixed2 frontWaveUV = fixed2(i.uv.x - _FrontWaveSpeed*round(_Time.y/_FillFrontTex_TexelSize.x)*_FillFrontTex_TexelSize.x, i.uv.y-round(_Progress/_FillFrontTex_TexelSize.x)*_FillFrontTex_TexelSize.x);
				fixed2 backWaveUV = fixed2(i.uv.x - _BackWaveSpeed*round(_Time.y/_FillBackTex_TexelSize.x)*_FillBackTex_TexelSize.x, i.uv.y-round(_Progress/_FillBackTex_TexelSize.x)*_FillBackTex_TexelSize.x);
				
				fixed4 fillMoving = tex2D(_FillFrontTex, frontWaveUV);
				fixed4 fillMoving2 = tex2D(_FillBackTex, backWaveUV);

                fixed4 fillFront = mask.a*fillMoving;
                fixed4 fillBack = mask.a*fillMoving2;
                
                return lerp(fillBack , fillFront,fillFront.a);
            }
            ENDCG
        }
    }
}
