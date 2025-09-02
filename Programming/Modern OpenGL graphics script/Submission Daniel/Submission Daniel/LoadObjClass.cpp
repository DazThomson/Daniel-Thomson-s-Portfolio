/////////////////////////////////////////////////////////////////////////////////          
// CreateSphere.cpp
//
// Forward-compatible core GL 4.3 version 
//
// Interaction:
// Press the up and down arrow keys to move the viewpoint over the field.
//
//
///////////////////////////////////////////////////////////////////////////////// 

#include <cmath>
#include <iostream>
#include <fstream>

#include <chrono>
#include <GL/glew.h>
#include <GL/freeglut.h>
#include <GLFW/glfw3.h>

#pragma comment(lib, "glew32.lib") 

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtc/matrix_inverse.hpp>


#include "shader.h"
#include "vertex.h"
#include "sphere.h"
#include "Model.h"
#include "Player.h"
#include "GameObject.h"

#include "soil/SOIL.h"

using namespace std;
using namespace glm;

static enum object {FIELD, SKY,SPHERE, HUMANOID, GROUND, SKYBOX, PLATFORM1, CYLINDER, PLATFORMSQUARE, COLLECTABLE1, PILLAR, PLATFORM2}; // VAO ids.
static enum buffer {FIELD_VERTICES, SKY_VERTICES,SPHERE_VERTICES, SPHERE_INDICES, HUMANOID_VERTICES, GROUND_VERTICES, SKYBOX_VERTICES, PLATFORM1_VERTICES, CYLINDER_VERTICES, PLATFORMSQUARE_VERTICES, COLLECTABLE1_VERTICES, PILLAR_VERTICES, PLATFORM2_VERTICES}; // VBO ids.



std::chrono::time_point<std::chrono::system_clock, std::chrono::milliseconds> programStartTime;

struct Material
{
	vec4 ambRefl;
	vec4 difRefl;
	vec4 specRefl;
	vec4 emitCols;
	float shininess;
};

struct Light
{
	vec4 ambCols;
	vec4 difCols;
	vec4 specCols;
	vec4 coords;
};

// Globals.




static mat4 projMat = mat4(1.0);//projection matrix for camera
static mat3 normalMat = mat3(1.0);  ///create normal matrix

static const vec4 globAmb = vec4(1.0, 0.2, 0.2, 0.0);
// Front and back material properties.
static const Material sphereFandB =
{
	vec4(1.0, 1.0, 0.0, 1.0),
	vec4(1.0, 1.0, 0.0, 1.0),
	vec4(1.0, 1.0, 0.0, 1.0),
	vec4(0.0, 0.0, 0.0, 1.0),
	40.0f
};
//values have been altered to fit the object and texture for the ground.
static const Light light0 =
{
	vec4(0.0, 0.0, 0.0, 1.0),
	vec4(0.0, 1.0, 1.0, 1.0),
	vec4(1.0, 1.0, 1.0, 1.0),
	vec4(1.0, 1.0, 1.0, 1.0)
};

static const Light light1 =
{
	vec4(1.0, 0.0, 0.0, 1.0),
	vec4(1.0, 1.0, 1.0, 1.0),
	vec4(1.0, 1.0, 1.0, 1.0),
	vec4(1.0, 1.0, 1.0, 1.0)
};

static unsigned int
   programId,
   vertexShaderId,
   fragmentShaderId,
   modelViewMatLoc,
   projMatLoc,
   objectLoc,
   TexYLoc, //To Move Texture.
   ObjTexLoc,
   grassTexLoc,  ///for grass texture 
   skyTexLoc,
   humanoidTexLoc,
   groundTexLoc,
   platform1TexLoc,
   cylinderTexLoc,
   platformSquareTexLoc,
   BallTexLoc,
   CollectableTexLoc,
   PillarTexLoc,   
   Platform2TexLoc,
   buffer[13], ///add one more object
   vao[12], ///add one more object
   texture[10]; 

static VertexWtihNormal* sphereVerticesNor = NULL;
static unsigned int* sphereIndices = NULL;
static Sphere Fireball;
static float zVal = 80.0; // Z Co-ordinates of the ball.
static float SphereY = 10.0f;//apply position of the sphere Y axis
static float SphereX = 55.0f;
static float width = 500;
static float height= 500;
float objYpos = 0;//set position of moving platform in scene
float yVal = 0;
float s = 0; //null value set for accessing updateModelMatrix function in Model.cpp
float camYaw = 90;
float camPitch = 0.0f;
float lastX = 800.0f / 2.0;
float LastY = 600.0 / 2.0;
float fov;
float newVal = 0;
float deltaTime = 0.0f;
float lastFrame = 0.0f;
vec3 pos;// apply posiiton to all objects in the game.
glm::vec3 cameraPosition = glm::vec3(0, 0, 0);
glm::vec3 cameraForward = glm::vec3(0.0, 0.0, 15.0);
glm::vec3 cameraUp = glm::vec3(0.0, 1.0, 0.0);




static float d = 0.0; //Camera position
static float y = 0.0;
static Player Humanoid("humanoid.obj");
static Model Ground("ground.obj");
static Model SkyBox("sky.obj");
static Model Platform1("platform1.obj");
static Model Cylinder("platform cylinder.obj");
static Model SquarePlatform("platform square 3.obj");
static Model Ditch("ditch.obj");
static Model Collectable1("Collectable.obj");
static Model Pillar("Pillars.obj");
static Model Platform2("platform2.obj");









// Initialization routine.
void setup(void) 
{
   glClearColor(1.0, 1.0, 1.0, 0.0); 
   glEnable(GL_DEPTH_TEST);

   
   glm::mat4 modelViewMat = glm::lookAt(cameraForward, glm::vec3(0), glm::vec3(0, 1, 0));
   glm::mat4 projectionMatrix = glm::perspective(glm::radians(60.0), (double)width / (double)height, 0.1, 1000.0);

   // Create shader program executable.
   vertexShaderId = setShader("vertex", "vertexShader.glsl");
   fragmentShaderId = setShader("fragment", "fragmentShader.glsl");
   programId = glCreateProgram(); 
   glAttachShader(programId, vertexShaderId); 
   glAttachShader(programId, fragmentShaderId); 
   glLinkProgram(programId); 
   glUseProgram(programId); 

   //codes for OpenGL lighting
   glUniform4fv(glGetUniformLocation(programId, "sphereFandB.ambRefl"), 1, &sphereFandB.ambRefl[0]);
   glUniform4fv(glGetUniformLocation(programId, "sphereFandB.difRefl"), 1, &sphereFandB.difRefl[0]);
   glUniform4fv(glGetUniformLocation(programId, "sphereFandB.specRefl"), 1, &sphereFandB.specRefl[0]);
   glUniform4fv(glGetUniformLocation(programId, "sphereFandB.emitCols"), 1, &sphereFandB.emitCols[0]);
   glUniform1f(glGetUniformLocation(programId, "sphereFandB.shininess"), sphereFandB.shininess);

   glUniform4fv(glGetUniformLocation(programId, "globAmb"), 1, &globAmb[0]);

   glUniform4fv(glGetUniformLocation(programId, "light0.ambCols"), 1, &light0.ambCols[0]);
   glUniform4fv(glGetUniformLocation(programId, "light0.difCols"), 1, &light0.difCols[0]);
   glUniform4fv(glGetUniformLocation(programId, "light0.specCols"), 1, &light0.specCols[0]);
   glUniform4fv(glGetUniformLocation(programId, "light0.coords"), 1, &light0.coords[0]);

   glUniform4fv(glGetUniformLocation(programId, "light1.ambCols"), 1, &light1.ambCols[0]);
   glUniform4fv(glGetUniformLocation(programId, "light1.difCols"), 1, &light1.difCols[0]);
   glUniform4fv(glGetUniformLocation(programId, "light1.specCols"), 1, &light1.specCols[0]);
   glUniform4fv(glGetUniformLocation(programId, "light1.coords"), 1, &light1.coords[0]);

   // Create VAOs and VBOs... 
   glGenVertexArrays(11, vao); ///add one more object
   glGenBuffers(11, buffer);  ///add one more object

  
  

   for (int x = 0; x < 16; x++)
   {
	  Fireball.PosX[x] = x * 12.0 - 18.0;
	  Fireball.PosY[x] = x * 12.0 - 18.0;
   }

   //Sphere vertex data here
   glGenVertexArrays(1, &vao[SPHERE]);
   glGenBuffers(1, &buffer[SPHERE_VERTICES]);

   Fireball.SetIDs(vao[SPHERE], buffer[SPHERE_VERTICES], 0);
   Fireball.Setup();

   

   //Binding VAO and VBO
  

   glGenVertexArrays(1, &vao[HUMANOID]);//setup humanoid
   glGenBuffers(1, &buffer[HUMANOID_VERTICES]);

   Humanoid.SetIDs(vao[HUMANOID], buffer[HUMANOID_VERTICES], 0);//set up game
   Humanoid.Setup();

   glGenVertexArrays(1, &vao[GROUND]);//setup up ground
   glGenBuffers(1, &buffer[GROUND_VERTICES]);

   Ground.SetIDs(vao[GROUND], buffer[GROUND_VERTICES], 0);
   Ground.Setup();

   glGenVertexArrays(1, &vao[SKYBOX]);
   glGenBuffers(1, &buffer[SKYBOX_VERTICES]);
   
  
   SkyBox.SetIDs(vao[SKYBOX], buffer[SKYBOX_VERTICES], 0);
   SkyBox.Setup();
  

   glGenVertexArrays(1, &vao[PLATFORM1]);
   glGenBuffers(1, &buffer[PLATFORM1_VERTICES]);

   Platform1.SetIDs(vao[PLATFORM1], buffer[PLATFORM1_VERTICES], 0);
   Platform1.Setup();

   glGenVertexArrays(1, &vao[CYLINDER]);
   glGenBuffers(1, &buffer[CYLINDER_VERTICES]);

   Cylinder.SetIDs(vao[CYLINDER], buffer[CYLINDER_VERTICES], 0);
   Cylinder.Setup();

   glGenVertexArrays(1, &vao[PLATFORMSQUARE]);
   glGenBuffers(1, &buffer[PLATFORMSQUARE_VERTICES]);

   SquarePlatform.SetIDs(vao[PLATFORMSQUARE], buffer[PLATFORMSQUARE_VERTICES], 0);
   SquarePlatform.Setup();

   glGenVertexArrays(1, &vao[FIELD]);
   glGenBuffers(1, &buffer[FIELD_VERTICES]);

   Ditch.SetIDs(vao[FIELD], buffer[FIELD_VERTICES], 0);
   Ditch.Setup();

   glGenVertexArrays(1, &vao[COLLECTABLE1]);
   glGenBuffers(1, &buffer[COLLECTABLE1_VERTICES]);

  Collectable1.SetIDs(vao[COLLECTABLE1], buffer[COLLECTABLE1_VERTICES], 0);
  Collectable1.Setup();

  glGenVertexArrays(1, &vao[PILLAR]);
  glGenBuffers(1, &buffer[PILLAR_VERTICES]);

  Pillar.SetIDs(vao[PILLAR], buffer[PILLAR_VERTICES], 0);
  Pillar.Setup();

  glGenVertexArrays(1, &vao[PLATFORM2]);
  glGenBuffers(1, &buffer[PLATFORM2]);

  Platform2.SetIDs(vao[PLATFORM2], buffer[PLATFORM2_VERTICES], 0);
  Platform2.Setup();



   // Obtain projection matrix uniform location and set value.
   projMatLoc = glGetUniformLocation(programId,"projMat");   //uniform mat4 projMat;
   projMat = frustum(-5.0, 5.0, -5.0, 5.0, 5.0, 1000.0); 
   glUniformMatrix4fv(projMatLoc, 1, GL_FALSE, value_ptr(projMat));
   
   // Obtain modelview matrix uniform and object uniform locations.
   modelViewMatLoc = glGetUniformLocation(programId,"modelViewMat");   //uniform mat4 modelViewMat;
   objectLoc = glGetUniformLocation(programId, "object");  //uniform uint object;

   // Load the images.
   std::string AllTextures[] = {//all texture files.
		"Textures/Ditch.png",
		"Textures/Sky.png",
		"Textures/humanoid_texture.png",
		"Textures/gorund final.png",
		"Textures/platform1 test_DefaultMaterial_BaseColor.png",
		"Textures/platform cylinder.png",
		"Textures/border.png",
		"Textures/FireBall.png",
		"Textures/collectable.png",
		"Textures/platform2.png",


   };

   // Create texture ids.
   glGenTextures(9, texture);

   int width, height;
   unsigned char* data;

   // Bind grass image.
   glActiveTexture(GL_TEXTURE0);
   glBindTexture(GL_TEXTURE_2D, texture[0]);


   // part of the script handles assigning textures to uniform sampler2D variables within the fragment shader script. this will allow me to apply lighting and 
   //textures to various objects in the scene
   //load image data using SOIL library
   data = SOIL_load_image(AllTextures[0].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);//texture is set to repeat for animation
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   grassTexLoc = glGetUniformLocation(programId, "grassTex");//the grass texture is sent to the shader.
   glUniform1i(grassTexLoc, 0); //send texture to shader

   glActiveTexture(GL_TEXTURE1);//assigned a different value so it moves along in the object
   glBindTexture(GL_TEXTURE_2D, texture[1]);

   data = SOIL_load_image(AllTextures[1].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);//texture also set to repeat to avoid black spaces in animation scroller
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
   glGenerateMipmap(GL_TEXTURE_2D);
   skyTexLoc = glGetUniformLocation(programId, "skyTex");
   glUniform1i(skyTexLoc, 1); //send texture to shader

   glActiveTexture(GL_TEXTURE2);
   glBindTexture(GL_TEXTURE_2D, texture[2]);
   
   data = SOIL_load_image(AllTextures[2].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);



   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   humanoidTexLoc = glGetUniformLocation(programId, "humanoidTex");
   glUniform1i(humanoidTexLoc, 2); //send texture to shader

  
   glActiveTexture(GL_TEXTURE3);
   glBindTexture(GL_TEXTURE_2D, texture[3]);

   data = SOIL_load_image(AllTextures[3].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);



   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);//the ground texture is set to repeat to prevent black borders around the edge of the mesh
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   groundTexLoc = glGetUniformLocation(programId, "groundTex");
   glUniform1i(groundTexLoc, 3); //send texture to shader

   glActiveTexture(GL_TEXTURE4);
   glBindTexture(GL_TEXTURE_2D, texture[4]);

   data = SOIL_load_image(AllTextures[4].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);



   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);//custom texture map which isn't a repeating texture like the ground or sky. this will prevent texture overlapping
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   platform1TexLoc = glGetUniformLocation(programId, "platform1Tex");
   glUniform1i(platform1TexLoc, 4); //send texture to shader

   glActiveTexture(GL_TEXTURE5);
   glBindTexture(GL_TEXTURE_2D, texture[5]);

   data = SOIL_load_image(AllTextures[5].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);//the texture is set to clamp to border the cylinder UV is aligned in a way in which that doesn't need to repeat
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   cylinderTexLoc = glGetUniformLocation(programId, "cylinderTex");
   glUniform1i(cylinderTexLoc, 5); //send texture to shader

   glActiveTexture(GL_TEXTURE6);
   glBindTexture(GL_TEXTURE_2D, texture[6]);

   data = SOIL_load_image(AllTextures[6].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   platformSquareTexLoc = glGetUniformLocation(programId, "squareTex");
   glUniform1i(platformSquareTexLoc, 6); //send texture to shader

   glActiveTexture(GL_TEXTURE7);
   glBindTexture(GL_TEXTURE_2D, texture[7]);

   data = SOIL_load_image(AllTextures[7].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   BallTexLoc = glGetUniformLocation(programId, "ballTex");
   glUniform1i(BallTexLoc, 7); //send texture to shader

   glActiveTexture(GL_TEXTURE8);
   glBindTexture(GL_TEXTURE_2D, texture[8]);

   data = SOIL_load_image(AllTextures[8].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   CollectableTexLoc = glGetUniformLocation(programId, "collectTex");
   glUniform1i(CollectableTexLoc, 8); //send texture to shader

   glActiveTexture(GL_TEXTURE9);
   glBindTexture(GL_TEXTURE_2D, texture[9]);

   data = SOIL_load_image(AllTextures[9].c_str(), &width, &height, 0, SOIL_LOAD_RGBA);
   glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
   SOIL_free_image_data(data);

   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
   glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
   glGenerateMipmap(GL_TEXTURE_2D);
   Platform2TexLoc = glGetUniformLocation(programId, "platform2Tex");
   glUniform1i(Platform2TexLoc, 9); //send texture to shader


}

// Drawing routine.
void drawScene(void)
{
   glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

   // Calculate and update modelview matrix.
   glm::mat4 modelViewMat = mat4(1.0);
   modelViewMat = lookAt(vec3(0.0, 600.0, 15.0), vec3(0.0 + d, 110.0, 600.0), vec3(0.0, 500.0, 0.0));
   glUniformMatrix4fv(modelViewMatLoc, 1, GL_FALSE, value_ptr(modelViewMat)); 
  

   //Sphere
   glUniform1fv(glGetUniformLocation(programId, "PosX"), 32, Fireball.PosX);  
   Fireball.updateModelMatrix(modelViewMatLoc, d, 0.15);
   glUniform1ui(objectLoc, SPHERE);  //if (object == SPHERE)
   Fireball.Draw();

   glUniform1fv(glGetUniformLocation(programId, "PosY"), 32, Fireball.PosY);
   Fireball.updateModelMatrix(modelViewMatLoc, d, 0.15);
   glUniform1ui(objectLoc, SPHERE);  //if (object == SPHERE)
   Fireball.Draw();

  //Draw Ditch
  

   pos = vec3(0.0, -100.0, 0.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Ditch.updateModelMatrix(modelViewMatLoc, d, y, 0.2, pos, modelViewMat);
   glUniform1ui(objectLoc, FIELD);  //if (object == MYMODEL)
   Ditch.Draw();//drawing 

  
   //Drawing humanoid
   pos = vec3(-80.0, -15.0, -5.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Humanoid.updateModelMatrix(modelViewMatLoc, d, 0.2f, pos);
   glUniform1ui(objectLoc, HUMANOID);  //if (object == MYMODEL)
   Humanoid.Draw();

   pos = vec3(0.0, -110.0, -280.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Ground.updateModelMatrix(modelViewMatLoc, d, y, 0.2, pos, modelViewMat);
   glUniform1ui(objectLoc, GROUND);  //if (object == MYMODEL)
   Ground.Draw();

   pos = vec3(0.0, -20.0, -200.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   SkyBox.updateModelMatrix(modelViewMatLoc, d, y, 0.2, pos, modelViewMat);//value has been changed to 0.15 to avoid issues with obj not rendering correctly.
   glUniform1ui(objectLoc, SKYBOX);  //if (object == MYMODEL)
   SkyBox.Draw();
   SkyBox.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);

   pos = vec3(-40.0, 10.0, 30.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();

   pos = vec3(-40.0, 10.0, 5.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();

   pos = vec3(-40.0, 10.0, -15.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();
  
   pos = vec3(5.0, 20.0, 28.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();

   pos = vec3(5.0, 30.0, 28.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();

   pos = vec3(5.0, 15.0, 40.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();
   
   pos = vec3(50.0, 10.0, 28.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);//updateModel matrix is called in modelh and passes in the cameraPos, cameraYpos (to allow camera movement properly), vec3 pos and model view mat
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();//draw the platform and it's textures

   pos = vec3(50.0, 10.0, 5.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);//updateModel matrix is called in modelh and passes in the cameraPos, cameraYpos (to allow camera movement properly), vec3 pos and model view mat
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();//draw the platform and it's textures

   pos = vec3(50.0, 10.0, -15.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);//updateModel matrix is called in modelh and passes in the cameraPos, cameraYpos (to allow camera movement properly), vec3 pos and model view mat
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();//draw the platform and it's textures
  

   
   pos = vec3(105.0, 45.0, 10.0);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);//updateModel matrix is called in modelh and passes in the cameraPos, cameraYpos (to allow camera movement properly), vec3 pos and model view mat
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();//draw the platform and it's textures

   pos = vec3(105.0, 45.0, -10.0);//this vec3 will allow me to set the object's position at the beginning of the program.
   Platform1.updateModelMatrix(modelViewMatLoc, d, y, 0.15, pos, modelViewMat);//updateModel matrix is called in modelh and passes in the cameraPos, cameraYpos (to allow camera movement properly), vec3 pos and model view mat
   glUniform1ui(objectLoc, PLATFORM1);  //if (object == MYMODEL)
   Platform1.Draw();//draw the platform and it's textures


   pos = vec3(180.0, -25.0, -55.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Cylinder.updateModelMatrix(modelViewMatLoc, d, y, 0.2, pos, modelViewMat);
   glUniform1ui(objectLoc, CYLINDER);  //if (object == MYMODEL)
   Cylinder.Draw();//draw the cylinder platform

   
   pos = vec3(0.0, -110.0, -280.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   SquarePlatform.updateModelMatrix(modelViewMatLoc, d, y, 0.2f, pos, modelViewMat);
   glUniform1ui(objectLoc, PLATFORMSQUARE);  //if (object == MYMODEL)
   SquarePlatform.Draw();//border around the ground (grass)

   pos = vec3(180.0, 40.0, -85.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Collectable1.updateModelMatrix(modelViewMatLoc, d, y, 0.2f, pos, modelViewMat);
   glUniform1ui(objectLoc, COLLECTABLE1);  //if (object == MYMODEL)
   Collectable1.Draw();// first collectable

   pos = vec3(8.5, 50.0, -50.0f);//this vec3 will allow me to set the object's position at the beginning of the program.
   Collectable1.updateModelMatrix(modelViewMatLoc, d, y, 0.2f, pos, modelViewMat);
   glUniform1ui(objectLoc, COLLECTABLE1);  //if (object == MYMODEL)
   Collectable1.Draw();// first collectable

   pos = vec3(8.0,20,-50);//this vec3 will allow me to set the object's position at the beginning of the program.
   Pillar.updateModelMatrix(modelViewMatLoc, d, y, 0.2f, pos, modelViewMat);
   glUniform1ui(objectLoc, PILLAR);  //if (object == MYMODEL)
   Pillar.Draw();// first collectable
  
   pos = vec3(8.0, 40, -50);//this vec3 will allow me to set the object's position at the beginning of the program.
   Pillar.updateModelMatrix(modelViewMatLoc, d, y, 0.2f, pos, modelViewMat);
   glUniform1ui(objectLoc, PILLAR);  //if (object == MYMODEL)
   Pillar.Draw();// first collectable

   
   glutSwapBuffers();
}

void animate() {;

//animate texture pngs
    yVal += 0.001;		
	TexYLoc = glGetUniformLocation(programId, "Time")/*float variable Time is located in the fragment shader glsl file. 													 this will send the value of yVal to this file and behave accordingly */;
	glUniform1f(TexYLoc, yVal);
	

	//newVal -= 0.1;
	//if (newVal < -80.0) newVal = 85.0;
	//TexYLoc = glGetUniformLocation(programId, "PositionX");
	//glUniform1f(TexYLoc, newVal);
	
//animate object position
	zVal = zVal - 0.2;
	if (zVal < -50.0) zVal = 40.0;
	Fireball.SetPosition(vec3(-15.0, SphereY, zVal)); //modify sphere's position
//animate cylinder platform pos*/
	objYpos = objYpos + 0.2;
	if (objYpos > 50.0) objYpos = -25.0f;
	Cylinder.SetPosition(vec3(0, objYpos, 0));

	glutPostRedisplay();//This will allow me update changes to the screen.


//animate player pos.

}



void UpdateCamera()
{
/*
	if (camPitch < -89)
	{
		camPitch = -89;
	}
	if (camPitch > 89)
	{
		camPitch = 89;
	}

	glm::vec3 CameraPoint = glm::vec3(0, 0, 0);
	
	CameraPoint.x = glm::cos(glm::radians(camPitch)) * -glm::cos(glm::radians(camYaw));
	CameraPoint.y = glm::sin(glm::radians(camPitch));
	CameraPoint.z = glm::cos(glm::radians(camPitch)) * glm::sin(glm::radians(camYaw));

	cameraForward = glm::normalize(CameraPoint);

	
	glm::mat4 modelViewMat = glm::lookAt(cameraForward, glm::vec3(0), glm::vec3(0, 1, 0));
	modelViewMat = glm::translate(modelViewMat, cameraPosition);
	



*/
}


// OpenGL window reshape routine.
void resize(int w, int h)
{
   glViewport(0, 0, w, h); 

  
   
   
}

// Keyboard input processing routine.
void KeyInputCallback(unsigned char key, int x, int y)
{
	switch (key)
	{
	case 'w':
	{
		cameraPosition += cameraForward;
		UpdateCamera();
	}break;
	case 's':
	{
		cameraPosition += -cameraForward;
		UpdateCamera();
	}break;
	case 'a':
	{
		cameraPosition += -glm::normalize(glm::cross(cameraForward, glm::vec3(0, 1, 0)));
		UpdateCamera();
	}break;
	case 'd':
	{
		cameraPosition += glm::normalize(cross(cameraForward, glm::vec3(0, 1, 0)));
		UpdateCamera();
	}break;
	case 'e':
	{
		cameraPosition += -glm::vec3(0, 1, 0);
		UpdateCamera();
	}break;
	case 'q':
	{
		cameraPosition += glm::vec3(0, 1, 0);
		UpdateCamera();
	}break;
	case 27:
	{
		exit(0);
	}break;
	}
	glutPostRedisplay();
}

void keySpecialDown(int key, int x, int y)
{
	
}

// Callback routine for non-ASCII key entry.
void specialKeyInput(int key, int x, int y)
{
	GameObject::specialKeys[key] = true;
   if (key == GLUT_KEY_LEFT) 
   {
	   //if (d > -50.0) 
	   d -= 1;
   }
   if (key == GLUT_KEY_RIGHT) 
   {
	   //if (d < 15.0) 
	   d += 1;
   }

   if (key == GLUT_KEY_DOWN)
   {
	   if (y > -50.0) y -= 0.1;
   }
   if (key == GLUT_KEY_UP)
   {
	   if (y < 15.0) y += 0.1;
   }
   glutPostRedisplay();
}

// Routine to output interaction instructions to the C++ window.
void printInteraction(void)
{
   cout << "Interaction:" << endl;
   cout << "Press the up and down arrow keys to move the viewpoint over the field." << endl; 
}

// Main routine.
int main(int argc, char **argv) 
{
   printInteraction();
   glutInit(&argc, argv);

   glutInitContextVersion(4, 3);
   glutInitContextProfile(GLUT_CORE_PROFILE);
   glutInitContextFlags(GLUT_FORWARD_COMPATIBLE);

   glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH); 
   glutInitWindowSize(600, 600);
   glutInitWindowPosition(50, 50); 
   glutCreateWindow("class demo");
   glutDisplayFunc(drawScene); 
   glutReshapeFunc(resize);  
   glutIdleFunc(animate); ///animation function
   glutKeyboardFunc(KeyInputCallback);
   glutSpecialFunc(specialKeyInput);

   glewExperimental = GL_TRUE;
   glewInit();

   setup(); 
   
   glutMainLoop(); 
}

