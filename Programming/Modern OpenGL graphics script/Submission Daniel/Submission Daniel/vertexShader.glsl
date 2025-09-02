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




layout(location=0) in vec4 Coords;
layout(location=1) in vec2 TexCoords;
layout(location=2) in vec4 sphereCoords;
layout(location=3) in vec3 sphereNormals;
layout(location=4) in vec3 objCoords;
layout(location=5) in vec3 objNormals;
layout(location=6) in vec2 objTexCoords;
layout(location=7) in vec3 aColour;


uniform mat4 modelViewMat;
uniform mat4 projMat;


uniform uint object;
uniform float PositionX;
uniform float PosX[16];
uniform float PositionY[1];

flat out int InstanceID;
out vec2 texCoordsExport;
out vec3 normalExport;
out vec3 ourColour;
vec4 coords;


void main(void)
{   
   if (object == FIELD)
   {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      texCoordsExport = objTexCoords;
   }
   if (object == SKY)
   {
      coords = Coords;
      texCoordsExport = TexCoords;
   }
   if (object == SPHERE)
   {
      InstanceID = gl_InstanceID; 
      coords = vec4(sphereCoords.x+PosX[gl_InstanceID * 17 - 12],sphereCoords.y,sphereCoords.z,sphereCoords.w);     
      normalExport = sphereNormals;
      texCoordsExport = TexCoords;
      
   }

   if (object == HUMANOID)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      texCoordsExport = objTexCoords;
    }

     if (object == GROUND)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }
     if (object == SKYBOX)
    {
      coords = vec4(objCoords, 1.0f);    
      normalExport = objNormals;    
      texCoordsExport = objTexCoords;
    
      
    }

     if (object == PLATFORM1)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }
      if (object == CYLINDER)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }

      if (object == PLATFORMSQUARE)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }

      if (object == COLLECTABLE1)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }

     if (object == PILLAR)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }

      if (object == PLATFORM2)
    {
      coords = vec4(objCoords, 1.0f);
      normalExport = objNormals;
      ourColour = aColour;
      texCoordsExport = objTexCoords;
      
    }
   
   gl_Position = projMat * modelViewMat * coords;
   
}
