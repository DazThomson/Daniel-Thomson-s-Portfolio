#pragma once
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtc/matrix_inverse.hpp>
#include "vertex.h"
#include "GameObject.h"

using namespace glm;

class Sphere : public GameObject
{
private:
	VertexWtihNormal* sphereVerticesNor;  //Sphere vertices data with normals
	unsigned int* sphereIndices;          //Sphere triangle indices    

	int stacks; // nunber of segments
	int slices; // number of segments
	float radius;

	unsigned int VAO;
	unsigned int VBO;
	unsigned int IBO; //for triangle indices buffer

	mat4 ModelMatrix;

	void CreateSpherewithNormal();
public:

	float PosX[16];
	float PosY[16];
	Sphere();
	~Sphere();

	void SetPosition(vec3 newPos);
	vec3 GetPosition(void);

	void SetIDs(unsigned int, unsigned int, unsigned int);
	void updateModelMatrix(unsigned int, float, float);

	void Setup();
	void Draw();
	void Update(float, glm::vec3 offset);
};

