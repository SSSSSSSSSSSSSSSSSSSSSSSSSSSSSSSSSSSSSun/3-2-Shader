#pragma once

enum SelectMode {
	TEST,
	SOLID_RECT,
	PARTICLE,
	GRID_MESH,
	PRACTICE = 0xff
};

#include <string>
#include <cstdlib>
#include <fstream>
#include <iostream>

#include "Dependencies\glew.h"

class Timer {
private:
	std::chrono::steady_clock::time_point m_fPrevTime{ std::chrono::high_resolution_clock::now() };
	float m_fAllTime{};
public:
	Timer() {};
	~Timer() {};

	void GetDeltaTime();
	float& GetAllTime() { return m_fAllTime; }
};

class Renderer
{
public:
	Renderer(int windowSizeX, int windowSizeY);
	~Renderer();

	bool IsInitialized();
	void ReloadAllShaderPrograms();		// 09/23
	void DrawSolidRect(float x, float y, float z, float size, float r, float g, float b, float a);
	void DrawTest();
	void DrawParticle();
	void DrawGridMesh();
	void DrawFullScreenColor(float r, float g, float b, float a);
	Timer& GetTimer() { return m_Timer; }

	//Draw Mode
	SelectMode m_NowMode{ GRID_MESH };

private:
	void Initialize(int windowSizeX, int windowSizeY);
	void CompileAllShaderPrograms();	// 09.23
	void DeleteAllShaderPrograms();		// 09.23
	bool ReadFile(char* filename, std::string* target);
	void AddShader(GLuint ShaderProgram, const char* pShaderText, GLenum ShaderType);
	GLuint CompileShaders(char* filenameVS, char* filenameFS);
	void CreateVertexBufferObjects();
	void GetGLPosition(float x, float y, float* newX, float* newY);
	void GenerateParticles(int NumParticle);
	void CreateGridMesh(int x, int y);

	bool m_Initialized = false;

	unsigned int m_WindowSizeX = 0;
	unsigned int m_WindowSizeY = 0;

	GLuint m_VBORect = 0;
	GLuint m_SolidRectShader = 0;

	//lecture02
	GLuint m_VBOTestPos{};
	GLuint m_VBOTestColor{};
	GLuint m_TestShader{};

	//Time
	Timer m_Timer{};

	//Particle
	GLuint m_ParticleShader{};
	GLuint m_VBOParticle{};
	GLuint m_VBOParticleVertexCount{};

	//Grid mesh
	GLuint m_GridMeshShader{};
	GLuint m_GridMeshVertexCount{};
	GLuint m_GridMeshVBO{};

	//Full screen
	GLuint m_VBOFullScreen{};
	GLuint m_FullScreenShader{};

	//For raindrop effect
	float m_Points[400];

	// Practice
	GLuint m_PracticeShader{};
	GLuint m_PracticeVBO{};
};

