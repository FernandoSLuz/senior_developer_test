Shader "Custom/FadeTransitionShader" {
    Properties{
        _MainTex("Starting Texture", 2D) = "white" {}
        _SecondTex("Final Texture", 2D) = "white" {}
        _GradientTex("Gradient Texture", 2D) = "white" {}
        _TransitionTime("Transition Time", Range(0, 1)) = 0.5
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert noforwardadd
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _SecondTex;
            sampler2D _GradientTex;
            float _TransitionTime;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                half4 c1 = tex2D(_MainTex, IN.uv_MainTex);
                half4 c2 = tex2D(_SecondTex, IN.uv_MainTex);
                half4 g = tex2D(_GradientTex, IN.uv_MainTex);
                half3 finalColor = lerp(c1.rgb, c2.rgb, g.rgb * _TransitionTime);
                o.Albedo = 0;
                o.Alpha = c1.a;
                o.Emission = finalColor;
            }
            ENDCG
        }
            FallBack "Diffuse"
}