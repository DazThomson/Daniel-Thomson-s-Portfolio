#include "GameObject.h"

std::map<char, bool> GameObject::keys;
std::map<int, bool> GameObject::specialKeys;

GameObject::GameObject()
{
}

GameObject::GameObject(glm::vec3 pos)
{
	position = pos;
}

GameObject::~GameObject()
{
}

/*SphereCollider* GameObject::GetCollider()
{
	return collider;
}

void GameObject::AttachCollider(SphereCollider* attachingCollider)
{
	collider = attachingCollider;
}*/

void GameObject::Update(float deltaTime)
{
}



