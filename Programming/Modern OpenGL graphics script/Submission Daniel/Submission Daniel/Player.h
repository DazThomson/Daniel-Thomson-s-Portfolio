#pragma once
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtc/matrix_inverse.hpp>
#include "vertex.h"
#include "GameObject.h"


using namespace glm;


class Player : public GameObject
{

private:
	VertexWithAll* VerticesData;  //Data with vertices, normal, texCoords

	unsigned int VAO;
	unsigned int VBO;
	unsigned int IBO; //for triangle indices buffer

	mat4 ModelMatrix;
	int TotalVertices;

	void CreateObject(const char*);
public:
	float Speed;
	Player(const char*);
	~Player();

	void SetPosition(vec3 newPos);//function which receive a vec3 called newPos. This is here in the event of applying an animation to the object or custom controls.
	vec3 GetPosition(void);//

	void SetIDs(unsigned int, unsigned int, unsigned int);
	void updateModelMatrix(unsigned int, float, float, vec3);

	void Setup();
	void Draw();
	void Update(float, glm::vec3 offset);
};

