#pragma once

enum SelectMode {
	TEST,
	SOLID_RECT,
	PARTICLE,
	GRID_MESH,
	FS,
	TEXTURE,
	PRACTICE = 0xff
};

#include <string>
#include <cstdlib>
#include <fstream>
#include <iostream>
#include <cassert>

#include "Dependencies\glew.h"
#include "LoadPng.h"

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
	void DrawFS();
	void DrawTexture(float x, float y, float sizeX, float sizeY, GLuint TexID);
	void DrawDebugTexture();
	void DrawFBOs();
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
	void CreateFS(float r, float g, float b, float a);
	void CreateTexture();
	GLuint CreatePngTexture(char* filePath, GLuint samplingMethod);
	void CreateFBOs();

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

	// For Fragment Shader factory;
	GLuint m_VBOFS{};
	GLuint m_FSShader{};

	GLuint m_RGBTexture{};
	GLuint m_Texture{};

	std::array<GLuint, 11> m_NumTexture;

	//Texture
	GLuint m_TexVBO{};
	GLuint m_TexShader{};

	//FBO Color Buffers
	GLuint m_RT0_0{};
	GLuint m_RT0_1{};
	GLuint m_RT1_0{};
	GLuint m_RT1_1{};
	GLuint m_RT2{};
	GLuint m_RT3{};
	GLuint m_RT4{};

	//FBOs
	GLuint m_FBO0{};
	GLuint m_FBO1{};
	GLuint m_FBO2{};
	GLuint m_FBO3{};
	GLuint m_FBO4{};
};

