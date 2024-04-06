#ifndef RADIALBLUR_INCLUDED
#define RADIALBLUR_INCLUDED



// Returns % between start and stop
float InverseLerp(float start, float stop, float value)
{
	return (value - start) / (stop - start);
}

float Remap(float inStart, float inStop, float outStart, float outStop, float v)
{
	float t = InverseLerp(inStart, inStop, v); 
	return lerp(outStart, outStop, saturate(t));
}

void DoRadialBlur_float(Texture2D MainTex, float2 UV, float2 Center, float Intensity, int SampleCount, float Delay, out float3 Color)
{
	Color = float3(0, 0, 0);
	
	#ifndef SHADERGRAPH_PREVIEW
	const float MAX_DISTANCE = 2.828;
	Intensity *= Intensity;
	Intensity *= 0.2;
	float2 screenPos = 1.0 - (UV * 2.0);
	float2 uvNormalizedAround0 = screenPos;
	screenPos -= Center;
	
	float dist = distance(Center, uvNormalizedAround0);
	
	
	float3 srcColor = _MainTex.SampleLevel(sampler_MainTex, UV, 0).rgb;
	Color = srcColor;
	
	float invSampleCount = rcp((float)SampleCount);
	float intensityPerSample = Intensity * invSampleCount;
	float2 offsetDirection = normalize(screenPos);
	
	float offsetAmount = Remap(Delay, MAX_DISTANCE, 0.0, 1.0, dist) * intensityPerSample;
	
	if(offsetAmount > 0)
	{
		for (int i = 1; i < SampleCount; i++)
		{	
			float2 offset = offsetDirection * offsetAmount * i;
			float2 targetCoord = UV + offset;
		
			Color += _MainTex.SampleLevel(sampler_MainTex, targetCoord, 0).rgb;
		}
	
		Color *= invSampleCount;
	}
	
	#endif
}

#endif