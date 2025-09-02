#include "Player.h"
#

#include "OBJloader.h"
#include <fstream>
#include <GL/glew.h>
#include <GL/freeglut.h>

Player::Player(const char* name) : GameObject()
{
	position = vec3(0);
	VAO = VBO = 0;
	VerticesData = NULL;
	TotalVertices = 0; //empty value which will be filled once we create an object in the scene.
	Speed = 1.0f;
	CreateObject(name);
}

Player::~Player()
{
	free(VerticesData);
}

void Player::CreateObject(const char* name)
{
	std::vector<VertexWithAll> mesh = loadOBJ(name);
	TotalVertices = mesh.size();

	this->VerticesData = new VertexWithAll[TotalVertices];

	for (size_t i = 0; i < TotalVertices; i++)
	{
		this->VerticesData[i] = mesh[i];
	}

}

void Player::SetPosition(vec3 newPos)
{
	position = newPos;
}

vec3 Player::GetPosition(void)
{
	return position;
}

void Player::SetIDs(unsigned int vao, unsigned int vbo, unsigned int ibo)
{
	VAO = vao;
	VBO = vbo;
	IBO = ibo;
}

void Player::Setup()
{
	glBindVertexArray(VAO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(VertexWithAll) * TotalVertices, VerticesData, GL_STATIC_DRAW);

	glVertexAttribPointer(4, 3, GL_FLOAT, GL_FALSE, sizeof(VertexWithAll), (GLvoid*)offsetof(VertexWithAll, position));
	glEnableVertexAttribArray(4);
	glVertexAttribPointer(5, 3, GL_FLOAT, GL_FALSE, sizeof(VertexWithAll), (GLvoid*)offsetof(VertexWithAll, normal));
	glEnableVertexAttribArray(5);

	glVertexAttribPointer(6, 2, GL_FLOAT, GL_FALSE, sizeof(VertexWithAll), (GLvoid*)offsetof(VertexWithAll, textcoord));
	glEnableVertexAttribArray(6);

}

void Player::updateModelMatrix(unsigned int modelViewMatLoc, float d, float scale, vec3 offsetPos)
{
	ModelMatrix = mat4(1.0);
	ModelMatrix = lookAt(vec3(0.0, 10.0, 15.0), vec3(0.0 + d, 10.0, 0.0), vec3(0.0, 1.0, 0.0)); //camera matrix, apply first
	ModelMatrix = glm::scale(ModelMatrix, vec3(scale, scale, scale));  //scale down the model
	ModelMatrix = glm::translate(ModelMatrix, offsetPos);
	ModelMatrix = glm::translate(ModelMatrix, GetPosition());
	glUniformMatrix4fv(modelViewMatLoc, 1, GL_FALSE, value_ptr(ModelMatrix));  //send modelview matrix to the shader
}

void Player::Draw()
{
	/*int triCount = 660;
	glBindVertexArray(VAO);
	glDrawElements(GL_TRIANGLE_STRIP, triCount, GL_UNSIGNED_INT, sphereIndices);*/

	glBindVertexArray(VAO);
	glDrawArrays(GL_TRIANGLES, 0, TotalVertices);
}

void Player::Update(float deltaTime, glm::vec3 offset)//a custom movement script which allows the player to move the player model around the scene.
{
	if (GameObject::specialKeys['w'] == true)//if the player presses w, the player model moves on the z access.
	{
		position.z -= Speed * deltaTime;

	}
	if (GameObject::specialKeys['a'] == true)
	{
		position.x -= Speed * deltaTime;

	}
	if (GameObject::specialKeys['s'] == true)
	{
		position.z += Speed * deltaTime;
	}
	if (GameObject::specialKeys['d'] == true)
	{
		position.x += Speed * deltaTime;
	}




	//collider->Update(deltaTime, position, offset);
}