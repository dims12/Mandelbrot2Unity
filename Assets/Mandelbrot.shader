Shader "Mandelbrot/Mandelbrot"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Top("Bottom", Float) = 2
		_Bottom( "Bottom", Float) = -2
        _Left("Left", Float) = -2
        _Right("Right", Float) = 2
        _Angle("Angle", range(-180, 180)) = 0
        _Iterations("Iterations", Int) = 1000
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _Top;
			float _Bottom;
            float _Left;
			float _Right;
            float _Angle;
			sampler2D _MainTex;
            int _Iterations;

			float2 rot(float2 p, float2 pivot, float a) {
				float s = sin(a);
				float c = cos(a);

				p -= pivot;

				p = float2(p.x*c-p.y*s, p.x*s+p.y*c);

				p += pivot;

				return p;
			}


			fixed4 frag (v2f i) : SV_Target
			{
                

				float2 c = float2(_Left, _Top) + i.uv * (float2(_Right, _Bottom) - float2(_Left, _Top));
				c = rot(c, float2((_Left + _Right)/2., (_Top + _Bottom)/2.), _Angle/180*3.1415);

				float2 z;

				float iter;
				for(iter=0; iter<255; ++iter) {
					z = float2(z.x*z.x -z.y*z.y, 2*z.x*z.y) + c;
					if(length(z)>2) break;
				}

                if (iter < _Iterations - 1) {
                    return fixed4(iter * iter * 3 / _Iterations / _Iterations, iter * iter * 2 / _Iterations / _Iterations, iter * iter / _Iterations / _Iterations, 1);
                }
                else {
                    return 0;
                }

			}
            ENDCG
        }
    }
}
