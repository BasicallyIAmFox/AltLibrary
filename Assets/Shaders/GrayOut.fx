sampler uImage : register(s0);

float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 current = tex2D(uImage, coords);
    current.rgb = float3(0, 0, 0);
    current.a *= 0.2;
    return current;
}

technique Technique1
{
    pass FilterMyShader
    {
        PixelShader = compile ps_3_0 FilterMyShader();
    }
}