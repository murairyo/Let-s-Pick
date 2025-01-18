Shader "Custom/Hologram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmissionColor ("Emission Color", Color) = (0.0, 1.0, 1.0, 1.0)
        _ScanSpeed ("Scanline Speed", Float) = 1.0
        _Transparency ("Transparency", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _EmissionColor;
            float _ScanSpeed;
            float _Transparency;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 基本テクスチャ
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // スキャンライン効果
                float scanline = sin(i.uv.y * 100.0 + _Time.y * _ScanSpeed) * 0.5 + 0.5;

                // 透明感 (Fresnel Effect)
                float fresnel = pow(1.0 - abs(i.uv.y - 0.5), 3.0);

                // 合成
                fixed4 color = texColor * scanline;
                color.rgb += _EmissionColor.rgb * fresnel;

                // 透明度の設定
                color.a = _Transparency * fresnel;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
