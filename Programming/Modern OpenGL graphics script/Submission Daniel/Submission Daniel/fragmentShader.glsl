#version 430 core

#define FIELD 0
#define SKY 1
#define SPHERE 2
#define HUMANOID 3
#define GROUND 4
#define SKYBOX 5
#define PLATFORM1 6
#define CYLINDER 7
#define PLATFORMSQUARE 8
#define COLLECTABLE1 9
#define PILLAR 10
#define PLATFORM2 11

in vec2 texCoordsExport;
in vec3 normalExport;
in vec3 ourColour;

flat in int InstanceID;

struct Light
{
   vec4 ambCols;
   vec4 difCols;
   vec4 specCols;
   vec4 coords;
};

struct Material
{
   vec4 ambRefl;
   vec4 difRefl;
   vec4 specRefl;
   vec4 emitCols;
   float shininess;
};

uniform Light light0;
uniform vec4 globAmb;
uniform Material sphereFandB;

uniform sampler2D grassTex;
uniform sampler2D skyTex;
uniform sampler2D humanoidTex;
uniform sampler2D groundTex;
uniform sampler2D platform1Tex;
uniform sampler2D cylinderTex;
uniform sampler2D squareTex;
uniform sampler2D ballTex;
uniform sampler2D collectTex;
uniform sampler2D platform2Tex;

uniform uint object;
uniform float Time;

out vec4 colorsOut;

vec4 fieldTexColor, skyTexColor, humanoidTexColour, groundTexColour, platform1TexColour, cylinderTexColour, squareTexColour, ballTexColour, collectableTexColour, platform2TexColour;
vec3 normal, lightDirection;
vec4 fAndBDif;

void main(void)
{  
   fieldTexColor = texture(grassTex, vec2(-texCoordsExport.y + Time, texCoordsExport.x));//this approach for animating the shaders will ensure that the object won't be moving aswell
   skyTexColor = texture(skyTex, vec2(texCoordsExport.x + Time, texCoordsExport.y));//Before, i had problems with this and foudn out that the way i was doing it was both moving the skybox which looked strange.
   //Link to forum which helped me achieve this : https://stackoverflow.com/questions/10847985/glsl-shader-that-scroll-texture
   humanoidTexColour = texture(humanoidTex, vec2(texCoordsExport.x + Time, texCoordsExport.y));
   groundTexColour = texture(groundTex, texCoordsExport);
   platform1TexColour = texture(platform1Tex, texCoordsExport);
   cylinderTexColour = texture(cylinderTex, texCoordsExport);
   squareTexColour = texture(squareTex, texCoordsExport);
   ballTexColour = texture(ballTex, vec2(-texCoordsExport.x + Time, texCoordsExport.y));
   collectableTexColour = texture(collectTex, vec2(-texCoordsExport.x + Time, texCoordsExport.y));
   platform2TexColour = texture(platform2Tex, texCoordsExport);

   
   
   if (object == FIELD)
   {
    normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 1.0f) * vec4(1.0, 1.0, 1.0, 1.0); 
	colorsOut = vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0) * fieldTexColor;
    
   }
   if (object == SKY) colorsOut = skyTexColor;
   if (object == SPHERE) 
   {
    normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));

     if(InstanceID==0)
	    {
		  fAndBDif = max(dot(normal, lightDirection), 0.0f) * vec4(1.0,1.0,0.0,1.0); //red color
		  colorsOut = vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0) * ballTexColour;
		}
		if(InstanceID==1)
	    {
		  fAndBDif = max(dot(normal, lightDirection), 0.0f) * vec4(1.0,1.0,0.0,1.0); //the spheres are supposed to resemble fire balls with moving textures
		  //this is set to red to bring out the firery colour in the ball while keeping it's texture
		  colorsOut =  colorsOut = vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0) * ballTexColour;
		}
		
	
   }
   if (object == HUMANOID)
   {
     colorsOut = humanoidTexColour;//Humanoid is set to it's original texture to prevent 
   }

   if (object == GROUND)
   {
    normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 1.0f) * (light0.difCols * sphereFandB.difRefl); 
    colorsOut = vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0) * groundTexColour;//the default lighting is applied to make this object more vibrant.
   }

   if (object == SKYBOX)
   {
    normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 1.0f) * vec4(0.0,1.0,1.0,1.0); 
    colorsOut = skyTexColor;
   }
    if (object == PLATFORM1)
   {  
    normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 0.0f) * vec4(1.0, 1.0, 1.0, 1.0); 
    colorsOut =  vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0) * platform1TexColour;
   }

     if (object == CYLINDER)
   {   
    normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 0.0f) * (light0.difCols * sphereFandB.difRefl); 
    colorsOut = cylinderTexColour;
   }

      if (object == PLATFORMSQUARE)
   {
     normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 0.0f) * (light0.difCols * sphereFandB.difRefl);  
    colorsOut = squareTexColour;
   }

        if (object == COLLECTABLE1)
   {
     normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 0.0f) * vec4(0.0, 1.0, 1.0, 1.0);//colour set to blue to add vibrance to the texture's initial light.  
    colorsOut = vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0) * collectableTexColour;
   }
   if (object == PILLAR)
   {
	normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 0.0f) *  vec4(1.0, 1.0, 1.0, 1.0);//colour set to blue to add vibrance to the texture's initial light.  
	colorsOut = vec4(vec3(min(fAndBDif, vec4(1.0))), 1.0);

   }
    if (object == PLATFORM2)
   {
	normal = normalize(normalExport);
	lightDirection = normalize(vec3(light0.coords));
	fAndBDif = max(dot(normal, lightDirection), 0.0f) * (light0.difCols * sphereFandB.difRefl);//colour set to blue to add vibrance to the texture's initial light.  
	colorsOut = platform2TexColour;

   }

}