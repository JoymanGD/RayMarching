#define PS_SHADERMODEL ps_5_0

#define MAX_STEPS 100
#define MAX_DISTANCE 100.
#define SURFACE_DISTANCE .01

float3 playerPos = float3(0,0,0);

float Sphere(float4 spherePosition, float3 position){
    float sphereDistance = length(position-spherePosition.xyz)-spherePosition.w;
    return sphereDistance;
}

float Plane(float3 position){
    return position.y;
}

float SmoothBlending(float distA, float distB, float k){
    float h = clamp(0.5 + 0.5*(distA-distB)/k, 0., 1.);
    return lerp(distA, distB, h) - k*h*(1.-h);
}

float GetDistance(float3 position){
    
    float plane = Plane(position);
    float sphere = Sphere(float4(.5 + playerPos.x, 1 + playerPos.y, 6 + playerPos.z, 1), position);
    float sphere2 = Sphere(float4(1, 1, 6, 1), position);
    
    float spheres = SmoothBlending(sphere, sphere2, .4);
    float d = SmoothBlending(spheres, plane, .7);
    
    return d;
}

float3 GetNormal(float3 p){
    float d = GetDistance(p);
    float2 correction = float2(.01, 0);
    float3 Normal = d-float3(
        GetDistance(p-correction.xyy),
        GetDistance(p-correction.yxy),
        GetDistance(p-correction.yyx)
    );
    
    return normalize(Normal);
}

float RayMarch(float3 rayOrigin, float3 rayDirection){
    float currentDistance = 0.;
    
    for(int i=0; i < MAX_STEPS; i++){
        float3 position = rayOrigin + rayDirection*currentDistance;
        float distanceStep = GetDistance(position);
        currentDistance += distanceStep;
        
        if(currentDistance > MAX_DISTANCE || distanceStep < SURFACE_DISTANCE) break;
    }
    
    return currentDistance;
}

float GetLight(float3 p){
    float3 LightPos = float3(0, 5, 1);
    float3 LightVector = normalize(LightPos - p);
    float3 NormalVector = GetNormal(p);
    float Light = clamp(dot(NormalVector, LightVector), 0., 1.);
    float d = RayMarch(p+NormalVector*SURFACE_DISTANCE*2., LightVector);
    if(d < length(LightPos-p)) Light *= .1;
    return Light;
}


float4 MainPS(float4 POS : SV_POSITION, float4 COL : COLOR0, float2 TEXCOORD : TEXCOORD0) : SV_TARGET0
{
    float2 uv = -(TEXCOORD - .5);
    
    float3 ro = float3(0, 1, 0);
    float3 rd = normalize(float3(uv.x, uv.y, 1));
    
    float d = RayMarch(ro, rd);
    
    float3 p = ro + rd * d;
    
    float dif = GetLight(p);
    float3 col = float3(dif, dif, dif);

    // Output to screen
    return float4(col,1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};