#include "Model.h"
#include "OBJloader.h"
#include <fstream>
#include <GL/glew.h>
#include <GL/freeglut.h>

Model::Model(const char * name)
{
	position = vec3(0);
	VAO = VBO = 0;
	VerticesData = NULL; 
	TotalVertices = 0; //empty value which will be filled once we create an object in the scene.
	CreateObject(name);
}

Model::~Model()
{
	free(VerticesData);
}

void Model::CreateObject(const char * name)
{
	std::vector<VertexWithAll> mesh = loadOBJ(name);
	TotalVertices = mesh.size();

	this->VerticesData = new VertexWithAll[TotalVertices];

	for (size_t i = 0; i < TotalVertices; i++)
	{
		this->VerticesData[i] = mesh[i];
	}

}

void Model::SetPosition(vec3 newPos)
{
	position = newPos;
}

vec3 Model::GetPosition(void)
{
	return position;
}

void Model::SetIDs(unsigned int vao, unsigned int vbo, unsigned int ibo)
{
	VAO = vao;
	VBO = vbo;
	IBO = ibo;
}

void Model::Setup()
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

void Model::updateModelMatrix(unsigned int modelViewMatLoc,float d, float y, float scale, vec3 offsetPos, glm::mat4 viewMatrix)
{
	viewMatrix = lookAt(vec3(0.0, 10.0, 15.0), vec3(0.0 + d, 10.0, 0.0 + y), vec3(0.0, 1.0, 0.0)); //camera matrix, apply first
	viewMatrix = glm::scale(viewMatrix, vec3(scale, scale, scale));  //scale down the model
	viewMatrix = glm::translate(viewMatrix, offsetPos);
	viewMatrix = glm::translate(viewMatrix, GetPosition());
	glUniformMatrix4fv(modelViewMatLoc, 1, GL_FALSE, value_ptr(viewMatrix));  //send modelview matrix to the shader
}

void Model::Draw()
{
	/*int triCount = 660;
	glBindVertexArray(VAO);
	glDrawElements(GL_TRIANGLE_STRIP, triCount, GL_UNSIGNED_INT, sphereIndices);*/

	glBindVertexArray(VAO);
	glDrawArrays(GL_TRIANGLES, 0, TotalVertices);
}

void Model::Update(float deltaTime, glm::vec3 offset)
{
	//collider->Update(deltaTime, position, offset);
}