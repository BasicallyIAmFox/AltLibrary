sampler uImage : register(s0);
float frame;

static const float2 uImageSize = float2(60, 58);

float4 FilterMyShader(float2 coords : TEXCOORD0) : COLOR0
{
    float empty = 0.0;
    float2 ab = float2(0, 0);
    float2 cur = coords * uImageSize;

    if (frame == 1 && cur.x >= 30 && cur.y >= 16 && cur.y <= 28) {
        ab.x = -1;
        if (cur.x <= 34) {
            empty = 1;
        }
    }
    else if (frame == 3) {
        if (cur.x >= 14 && cur.x <= 40 && cur.y >= 10 && cur.y <= 14) {
            ab.x = -3;
        }
        if (cur.x >= 28 && cur.y >= 16 && cur.y <= 22) {
            ab.x = -1;
            if (cur.x <= 30) {
                empty = 1;
            }
        }
    }
    else if (frame == 4 || frame == 5) {
        if (cur.y >= 14 && cur.y <= 26) {
            if (cur.x >= 30) {
                ab.x = -8;
            }
            else {
                ab.x = 7;
            }

            if (cur.x <= 14 || cur.x >= 44) {
                empty = 1;
            }
        }

        if (frame == 5 && cur.x >= 24 && cur.y >= 42 && cur.y <= 48) {
            ab.x = -3;
            if (cur.x <= 26) {
                empty = 1;
            }
        }
    }
    else if (frame == 6) {
        if (cur.x >= 26 && cur.x <= 34 && cur.y >= 48 && cur.y <= 54) {
            empty = 1;
        }
        else if (cur.y >= 14 && cur.y <= 26) {
            ab.x = -8;
        }
        else if (cur.x >= 24 && cur.y >= 42 && cur.y <= 48) {
            ab.x = -3;
            if (cur.x <= 26) {
                empty = 1;
            }
        }
        else if (cur.x >= 14 && cur.x <= 26 && cur.y >= 30) {
            if (cur.y >= 42 && cur.y <= 56) {
                ab.y = -6;
                if (cur.x >= 24 && cur.y >= 54) {
                    empty = 1;
                }
            }
            else {
                empty = 1;
            }
            if (cur.y >= 32 && cur.y <= 40) {
                ab = float2(4, 7);
                empty = cur.y <= 34 && cur.x <= 24;
            }
        }
    }
    else if (frame == 7 || frame == 8 || frame == 9) {
        if (frame != 9) {
            if (cur.y >= 14 && cur.y <= 26) {
                ab.x = -2;
                if (frame == 8) {
                    ab.x += 4;
                }
            }
        }
        else {
            if (cur.y >= 14 && cur.y <= 26) {
                if (cur.x >= 30) {
                    ab.x = -8;
                }
                else {
                    ab.x = 7;
                }

                if (cur.x <= 14 || cur.x >= 44) {
                    empty = 1;
                }
            }
        }

        if (cur.x >= 16 && cur.x <= 34 && cur.y >= 46) {
            ab.x = 3;
            empty = cur.y <= 48 && cur.x <= 26 || cur.x >= 28 && cur.y >= 46;
        }

        if (cur.x >= 24 && cur.y >= 42 && cur.y <= 48) {
            if (cur.y <= 46) {
                ab.x = -3;
            }
            else if (cur.x >= 34 && cur.x <= 40) {
                ab.x = -3;
            }
        }

        if (cur.x >= 12 && cur.y >= 30 && cur.x <= 26 && cur.y <= 44) {
            empty = 1;
            if (cur.y <= 40 && cur.x <= 24) {
                empty = 0;
                ab = float2(1, 1);
                if (cur.x >= 20 && cur.y >= 30 && cur.x <= 22 && cur.y <= 32 || cur.x >= 22 && cur.y >= 34 && cur.x <= 24 && cur.y <= 38) {
                    empty = 1;
                    if (cur.y >= 36) {
                        empty = 0;
                        ab.x -= 1;
                    }
                }
                if (cur.x >= 16 && cur.y >= 32 && cur.x <= 22 && cur.y <= 34) {
                    ab.y -= 1;
                }
                if (cur.x >= 18 && cur.y >= 38 && cur.x <= 22 && cur.y <= 40) {
                    ab.x -= 1;
                }
            }
        }
        if (cur.x >= 18 && cur.y >= 54 && cur.x <= 20 && cur.y <= 56) {
            ab.y = -14;
        }

        if (frame == 8 && (cur.x >= 6 && cur.y >= 8 && cur.x <= 10 && cur.y <= 16 || cur.x >= 14 && cur.y >= 8 && cur.x <= 18 && cur.y <= 14 || cur.x >= 30 && cur.y >= 26 && cur.x <= 34 && cur.y <= 32 || cur.x >= 20 && cur.y >= 38 && cur.x <= 24 && cur.y <= 42)
            || frame == 9 && (cur.x >= 28 && cur.y >= 10 && cur.x <= 32 && cur.y <= 18 || cur.x >= 42 && cur.y >= 10 && cur.x <= 46 && cur.y <= 14 || cur.x >= 18 && cur.y >= 22 && cur.x <= 22 && cur.y <= 26 || cur.x >= 38 && cur.y >= 26 && cur.x <= 42 && cur.y <= 32)) {
            empty = 2;
        }
    }
    else if (frame == 11 && cur.x >= 30 && cur.y >= 16 && cur.y <= 28) {
        ab.x = -2;
        empty = cur.x <= 34;
    }
    else if (frame == 13) {
        if (cur.x >= 12 && cur.x <= 48) {
            if (cur.y >= 44 && cur.y <= 54) {
                ab.y = -14;
                empty = cur.x >= 30 && cur.x <= 34;
                if (cur.x >= 34) {
                    ab.x = -2;
                }
            }
            else if (cur.y >= 26 && cur.y <= 28 && cur.x >= 30 && cur.y <= 38) {
                empty = cur.x <= 34;
                ab.x = -2;
            }
        }
        if (cur.x >= 12 && cur.y >= 16 && cur.x <= 44 && cur.y <= 26) {
            ab.y = 14;

            if (cur.y >= 20 && cur.x <= 16 && cur.y <= 24 || !(cur.x <= 20 && cur.y <= 22) && cur.x >= 18 && cur.y >= 20 && cur.x <= 24 && cur.y <= 26) {
                return tex2Dlod(uImage, float4(33 / uImageSize.x, 33 / uImageSize.y, 0, 0));
            }
        }
    }
    else if (frame == 14) {
        if (cur.x >= 14 && cur.y >= 10 && cur.y <= 14 && cur.x <= 40) {
            ab.x = -3;
            empty = cur.x <= 20;
        }
        else if (cur.x >= 28 && cur.y >= 16 && cur.y <= 22) {
            ab.x = -1;
            empty = cur.x <= 30;
        }
    }

    ab = ab * 2 / uImageSize;
    if (empty == 1)
        return float4(0, 0, 0, 0);
    else if (empty == 2)
        return float4(196, 213, 196, 255) / 255;
    return tex2D(uImage, coords + ab);
}

technique Technique1
{
    pass FilterMyShader
    {
        PixelShader = compile ps_3_0 FilterMyShader();
    }
}