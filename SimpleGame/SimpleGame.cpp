/*
Copyright 2022 Lee Taek Hee (Tech University of Korea)

This program is free software: you can redistribute it and/or modify
it under the terms of the What The Hell License. Do it plz.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY.
*/

#include "stdafx.h"
#include <iostream>
#include "Dependencies\glew.h"
#include "Dependencies\freeglut.h"

#include "Renderer.h"

Renderer *g_Renderer = NULL;
bool g_bNeedReloadShaderPrograms{ false };

void RenderScene(void)
{
	g_Renderer->GetTimer().GetDeltaTime();

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
	//g_Renderer->DrawFullScreenColor(0, 0, 0, 0.5f);

	if (g_bNeedReloadShaderPrograms) {
		g_Renderer->ReloadAllShaderPrograms();
		g_bNeedReloadShaderPrograms = false;
	}


	// Renderer Test
	switch (g_Renderer->m_NowMode) {
	case TEST:
		g_Renderer->DrawTest();
		break;
	case SOLID_RECT:
		g_Renderer->DrawSolidRect(0, 0, 0, 100, 1, 1, 1, 0.5f);
		break;
	case PARTICLE:
		g_Renderer->DrawParticle();
		break;
	case GRID_MESH:
		g_Renderer->DrawGridMesh();
		break;
	case FS:
		g_Renderer->DrawFS();
		break;

	case PRACTICE:
		break;
	}

	glutSwapBuffers();
}

void Idle(void)
{
	RenderScene();
}

void MouseInput(int button, int state, int x, int y)
{

}

void KeyInput(unsigned char key, int x, int y)
{
	switch (key) {
	case '1':
		g_bNeedReloadShaderPrograms = true;
		break;
	case 'Q':
	case 'q':
		g_Renderer->m_NowMode = TEST;
		break;
	case 'W':
	case 'w':
		g_Renderer->m_NowMode = SOLID_RECT;
		break;
	case 'E':
	case 'e':
		g_Renderer->m_NowMode = PARTICLE;
		break;
	case 'R':
	case 'r':
		g_Renderer->m_NowMode = GRID_MESH;
		break;
	case 'T':
	case 't':
		g_Renderer->m_NowMode = FS;
		break;
	case 'Z':
	case 'z':
		g_Renderer->m_NowMode = PRACTICE;
		break;
	default:
		break;
	}
}

void SpecialKeyInput(int key, int x, int y)
{
	
}

int main(int argc, char **argv)
{
	// Initialize GL things
	int winX{ 500 }, winY{ 500 };

	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DEPTH | GLUT_DOUBLE | GLUT_RGBA);
	glutInitWindowPosition(0, 0);
	glutInitWindowSize(winX, winY);
	glutCreateWindow("Game Software Engineering KPU");

	glewInit();
	if (glewIsSupported("GL_VERSION_3_0"))
	{
		std::cout << " GLEW Version is 3.0\n ";
	}
	else
	{
		std::cout << "GLEW 3.0 not supported\n ";
	}

	// Initialize Renderer
	g_Renderer = new Renderer(winX, winY);
	if (!g_Renderer->IsInitialized())
	{
		std::cout << "Renderer could not be initialized.. \n";
	}

	glutDisplayFunc(RenderScene);
	glutIdleFunc(Idle);
	glutKeyboardFunc(KeyInput);
	glutMouseFunc(MouseInput);
	glutSpecialFunc(SpecialKeyInput);

	glutMainLoop();

	delete g_Renderer;

    return 0;
}

