#pragma once

#include <GL/glew.h>
#include <GL/freeglut.h>
#include <glm/glm.hpp>
#include <map>
//#include "SphereCollider.h"

class GameObject
{
protected:
	
public:
	GameObject();
	GameObject(glm::vec3 pos);
	~GameObject();
	static std::map<char, bool> keys; 
	static std::map<int, bool> specialKeys; 

	glm::vec3 position;
	
	//SphereCollider* collider;
	//SphereCollider* GetCollider();
	//void AttachCollider(SphereCollider* attachingCollider);

	virtual void Setup() = 0;
	virtual void Draw() = 0;
	virtual void Update(float);
};

