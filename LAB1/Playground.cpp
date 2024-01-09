#include <GL/glut.h>
#include <cmath>
#include <iostream>
#define _USE_MATH_DEFINES
#include <vector>
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>


const aiScene* scene = nullptr;

struct Position {
    float x, y, z;

    void addX(float dx) {
        x += dx;
    }

    void addY(float dy) {
        y += dy;
    }

    void addZ(float dz) {
        z += dz;
    }
};

std::vector<Position> positions; //lista tocaka krivuljr
std::vector<Position> tangentPositions; //lista tocaka tangente
size_t currentPositionIndex = 0;  //trenutni indeks za tocke krivulje (pomicanje objekta)
size_t currentPositionIndex2 = 0;  //trenutni indeks za tangenete

Position startLine;
Position endLine;

Position currentOrientation = { 0.0f, 1.0f, 0.0f };
Position targetOrientation;
Position os;

void calculateBSplinePoints() {
    std::vector<std::vector<float>> matrix = {
        { -1, 3, -3, 1 },
        { 3, -6, 3, 0 },
        { -3, 0, 3, 0 },
        { 1, 4, 1, 0 }
    };

    for (int ind = 0; ind <= 8; ind++)
    {
        for (float t = 0; t <= 1.01; t += 0.05f)
        {
            std::vector<float> vector = { std::pow(t, 3), std::pow(t, 2), t, 1 };
            std::vector<float> result(4);

            float scaleFactor = 1.0 / 6;
            std::vector<std::vector<float>> scaledMatrix(4, std::vector<float>(4));

            for (int i = 0; i < matrix.size(); i++)
            {
                scaledMatrix[i] = std::vector<float>(4);
                for (int j = 0; j < matrix[i].size(); j++)
                {
                    scaledMatrix[i][j] = scaleFactor * matrix[i][j];
                }
            }
            for (int i = 0; i < scaledMatrix[0].size(); i++)
            {
                for (int j = 0; j < vector.size(); j++)
                {
                    result[i] += vector[j] * scaledMatrix[j][i];
                }
            }

            std::vector<std::vector<double>> matrix2(4);

            if (ind == 0) {
                //segment 1, tocke 1,2,3,4
                matrix2[0] = { 0, 0, 0 };
                matrix2[1] = { 0, 10, 5 };
                matrix2[2] = { 10, 10, 10 };
                matrix2[3] = { 10, 0, 15 };
            }
            if (ind == 1) {
                //segment 2, tocke 2,3,4,5
                matrix2[0] = { 0, 10, 5 };
                matrix2[1] = { 10, 10, 10 };
                matrix2[2] = { 10, 0, 15 };
                matrix2[3] = { 0, 0, 20 };
            }
            if (ind == 2) {
                //segment 3, tocke 3,4,5,6
                matrix2[0] = { 10, 10, 10 };
                matrix2[1] = { 10, 0, 15 };
                matrix2[2] = { 0, 0, 20 };
                matrix2[3] = { 0, 10, 25 };
            }
            if (ind == 3) {
                //segment 4, tocke 4,5,6,7
                matrix2[0] = { 10, 0, 15 };
                matrix2[1] = { 0, 0, 20 };
                matrix2[2] = { 0, 10, 25 };
                matrix2[3] = { 10, 10, 30 };
            }
            if (ind == 4) {
                //segment 5, tocke 5,6,7,8
                matrix2[0] = { 0, 0, 20 };
                matrix2[1] = { 0, 10, 25 };
                matrix2[2] = { 10, 10, 30 };
                matrix2[3] = { 10, 0, 35 };
            }
            if (ind == 5)
            {
                //segment 6, tocke 6 7 8 9
                matrix2[0] = { 0, 10, 25 };
                matrix2[1] = { 10, 10, 30 };
                matrix2[2] = { 10, 0, 35 };
                matrix2[3] = { 0, 0, 40 };
            }
            if (ind == 6)
            {
                //segment 7, tocke 7 8 9 10
                matrix2[0] = { 10, 10, 30 };
                matrix2[1] = { 10, 0, 35 };
                matrix2[2] = { 0, 0, 40 };
                matrix2[3] = { 0, 10, 45 };
            }
            if (ind == 7)
            {
                //segment 8, tocke 8 9 10 11
                matrix2[0] = { 10, 0, 35 };
                matrix2[1] = { 0, 0, 40 };
                matrix2[2] = { 0, 10, 45 };
                matrix2[3] = { 10, 10, 50 };
            }
            if (ind == 8)
            {
                //segment 8, tocke 8 9 10 11
                matrix2[0] = { 0, 0, 40 };
                matrix2[1] = { 0, 10, 45 };
                matrix2[2] = { 10, 10, 50 };
                matrix2[3] = { 10, 0, 55 };
            }

            std::vector<double> result2(3, 0.0);
            Position pos = { 0,0,0 };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result2[j] += result[i] * matrix2[i][j];
                    if (j == 0) {
                        pos.x = result2[j];
                    }
                    if (j == 1) {
                        pos.y = result2[j];
                    }
                    if (j == 2) {
                        pos.z = result2[j];
                    }
                }
            }
            positions.push_back(pos);
            glColor3f(1.0, 0.0, 0.0); //crvena boja
            for (size_t i = 0; i < result2.size(); i += 3) {
                glVertex3f(result2[i], result2[i + 1], result2[i + 2]);
            }

            std::cout << "Result2 for t=" << t << " : ";
            for (size_t i = 0; i < result2.size(); i++) {
                std::cout << result2[i];
                if (i < result2.size() - 1) {
                    std::cout << ", ";
                }
            }
            std::cout << std::endl;
        }
        std::cout << std::endl;
    }
}

void calculateTangentVectors() {
    std::vector<std::vector<float>> matrix = {
        { -1, 3, -3, 1 },
        { 2, -4, 2, 0 },
        { -1, 0, 1, 0 }
    };

    for (int ind = 0; ind <= 8; ind++)
    {
        for (float t = 0; t <= 1.01; t += 0.05f)
        {
            std::vector<float> vector = {std::pow(t, 2), t, 1 };
            std::vector<float> result(4);

            float scaleFactor = 1.0 / 2;
            std::vector<std::vector<float>> scaledMatrix(3, std::vector<float>(4));

            for (int i = 0; i < matrix.size(); i++)
            {
                scaledMatrix[i] = std::vector<float>(4);
                for (int j = 0; j < matrix[i].size(); j++)
                {
                    scaledMatrix[i][j] = scaleFactor * matrix[i][j];
                }
            }
            for (int i = 0; i < scaledMatrix[0].size(); i++)
            {
                for (int j = 0; j < vector.size(); j++)
                {
                    result[i] += vector[j] * scaledMatrix[j][i];
                }
            }

            std::vector<std::vector<double>> matrix2(4);

            if (ind == 0) {
                //segment 1, tocke 1,2,3,4
                matrix2[0] = { 0, 0, 0 };
                matrix2[1] = { 0, 10, 5 };
                matrix2[2] = { 10, 10, 10 };
                matrix2[3] = { 10, 0, 15 };
            }
            if (ind == 1) {
                //segment 2, tocke 2,3,4,5
                matrix2[0] = { 0, 10, 5 };
                matrix2[1] = { 10, 10, 10 };
                matrix2[2] = { 10, 0, 15 };
                matrix2[3] = { 0, 0, 20 };
            }
            if (ind == 2) {
                //segment 3, tocke 3,4,5,6
                matrix2[0] = { 10, 10, 10 };
                matrix2[1] = { 10, 0, 15 };
                matrix2[2] = { 0, 0, 20 };
                matrix2[3] = { 0, 10, 25 };
            }
            if (ind == 3) {
                //segment 4, tocke 4,5,6,7
                matrix2[0] = { 10, 0, 15 };
                matrix2[1] = { 0, 0, 20 };
                matrix2[2] = { 0, 10, 25 };
                matrix2[3] = { 10, 10, 30 };
            }
            if (ind == 4) {
                //segment 5, tocke 5,6,7,8
                matrix2[0] = { 0, 0, 20 };
                matrix2[1] = { 0, 10, 25 };
                matrix2[2] = { 10, 10, 30 };
                matrix2[3] = { 10, 0, 35 };
            }
            if (ind == 5)
            {
                //segment 6, tocke 6 7 8 9
                matrix2[0] = { 0, 10, 25 };
                matrix2[1] = { 10, 10, 30 };
                matrix2[2] = { 10, 0, 35 };
                matrix2[3] = { 0, 0, 40 };
            }
            if (ind == 6)
            {
                //segment 7, tocke 7 8 9 10
                matrix2[0] = { 10, 10, 30 };
                matrix2[1] = { 10, 0, 35 };
                matrix2[2] = { 0, 0, 40 };
                matrix2[3] = { 0, 10, 45 };
            }
            if (ind == 7)
            {
                //segment 8, tocke 8 9 10 11
                matrix2[0] = { 10, 0, 35 };
                matrix2[1] = { 0, 0, 40 };
                matrix2[2] = { 0, 10, 45 };
                matrix2[3] = { 10, 10, 50 };
            }
            if (ind == 8)
            {
                //segment 9, tocke 8 9 10 11
                matrix2[0] = { 0, 0, 40 };
                matrix2[1] = { 0, 10, 45 };
                matrix2[2] = { 10, 10, 50 };
                matrix2[3] = { 10, 0, 55 };
            }

            std::vector<double> result2(3, 0.0);
            Position pos = { 0,0,0 };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result2[j] += result[i] * matrix2[i][j];
                    if (j == 0) {
                        pos.x = result2[j];
                    }
                    if (j == 1) {
                        pos.y = result2[j];
                    }
                    if (j == 2) {
                        pos.z = result2[j];
                    }
                }
            }
            tangentPositions.push_back(pos);
            glColor3f(1.0, 0.0, 0.0); //crvena boja

            std::cout << "tangent Result2 IND = " << ind <<" and for t=" << t << " : ";
            for (size_t i = 0; i < result2.size(); i++) {
                std::cout << result2[i];
                if (i < result2.size() - 1) {
                    std::cout << ", ";
                }
            }
            std::cout << std::endl;
        }
        std::cout << std::endl;
    }
}

void renderCube() {
    glBegin(GL_QUADS);

    glColor3f(0.0f, 1.0f, 0.0f);
    glVertex3f(-1.0f, -1.0f, 1.0f);
    glVertex3f(1.0f, -1.0f, 1.0f);
    glVertex3f(1.0f, 1.0f, 1.0f);
    glVertex3f(-1.0f, 1.0f, 1.0f);

    glColor3f(0.0f, 0.0f, 1.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, 1.0f, -1.0f);
    glVertex3f(-1.0f, 1.0f, -1.0f);

    glColor3f(1.0f, 0.0f, 0.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);
    glVertex3f(-1.0f, 1.0f, -1.0f);
    glVertex3f(-1.0f, 1.0f, 1.0f);
    glVertex3f(-1.0f, -1.0f, 1.0f);

    glColor3f(1.0f, 1.0f, 0.0f);
    glVertex3f(1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, 1.0f, -1.0f);
    glVertex3f(1.0f, 1.0f, 1.0f);
    glVertex3f(1.0f, -1.0f, 1.0f);

    glColor3f(1.0f, 0.0f, 1.0f);
    glVertex3f(-1.0f, 1.0f, -1.0f);
    glVertex3f(1.0f, 1.0f, -1.0f);
    glVertex3f(1.0f, 1.0f, 1.0f);
    glVertex3f(-1.0f, 1.0f, 1.0f);

    glColor3f(0.0f, 1.0f, 1.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, -1.0f, 1.0f);
    glVertex3f(-1.0f, -1.0f, 1.0f);

    glEnd();
}

void renderPyramid() {
    glBegin(GL_TRIANGLES);

    glColor3f(0.0f, 1.0f, 1.0f);
    glVertex3f(0.0f, 1.0f, 0.0f);
    glVertex3f(-1.0f, -1.0f, 1.0f);
    glVertex3f(1.0f, -1.0f, 1.0f);

    glColor3f(1.0f, 1.0f, 0.0f);
    glVertex3f(0.0f, 1.0f, 0.0f);
    glVertex3f(1.0f, -1.0f, 1.0f);
    glVertex3f(1.0f, -1.0f, -1.0f);

    glColor3f(0.0f, 0.0f, 1.0f);
    glVertex3f(0.0f, 1.0f, 0.0f);
    glVertex3f(1.0f, -1.0f, -1.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);

    glColor3f(1.0f, 0.0f, 0.0f);
    glVertex3f(0.0f, 1.0f, 0.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);
    glVertex3f(-1.0f, -1.0f, 1.0f);

    glColor3f(0.0f, 1.0f, 0.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, -1.0f, -1.0f);
    glVertex3f(1.0f, -1.0f, 1.0f);

    glVertex3f(1.0f, -1.0f, 1.0f);
    glVertex3f(-1.0f, -1.0f, 1.0f);
    glVertex3f(-1.0f, -1.0f, -1.0f);

    glEnd();
}

void drawSpline() {
    glBegin(GL_LINES);
    glColor3f(1.0, 0.0, 0.0);
    for (size_t i = 0; i < positions.size(); i += 3) {
        glVertex3f(positions[i].x, positions[i].y, positions[i].z);
    }
    glEnd();
}

void drawTangent() {
    glBegin(GL_LINES);
    glColor3f(0.0, 0.0, 1.0);
    glVertex3f(startLine.x, startLine.y, startLine.z);
    glVertex3f(startLine.x + endLine.x, startLine.y + endLine.y, startLine.z + endLine.z);
    glEnd();

}

void updateRotation() {
    Position s = currentOrientation;
    Position e = targetOrientation;
    os.x = currentOrientation.y * targetOrientation.z - targetOrientation.y * currentOrientation.z;
    os.y = -(currentOrientation.x * targetOrientation.z - targetOrientation.x * currentOrientation.z);
    os.z = currentOrientation.x * targetOrientation.y - currentOrientation.y * targetOrientation.x;

    float se = s.x * e.x + s.y * e.y + s.z * e.z;
    float sNorm = sqrt(std::pow(s.x, 2) + std::pow(s.y, 2) + std::pow(s.z, 2));
    float eNorm = sqrt(std::pow(e.x, 2) + std::pow(e.y, 2) + std::pow(e.z, 2));

    double result = std::acos(se/(sNorm*eNorm));
    float resultDeg = result * (180.0 / M_PI);

    glRotatef(resultDeg, os.x, os.y, os.z);
}

void updateParameters() {
    GLfloat modelviewMatrix[16];
    glGetFloatv(GL_MODELVIEW_MATRIX, modelviewMatrix);
    GLfloat forwardVector[3] = { modelviewMatrix[2], modelviewMatrix[6], modelviewMatrix[10] };

    currentOrientation.x = forwardVector[0];
    currentOrientation.y = forwardVector[1];
    currentOrientation.z = forwardVector[2];

    targetOrientation.x = tangentPositions[currentPositionIndex].x;
    targetOrientation.y = tangentPositions[currentPositionIndex].y;
    targetOrientation.z = tangentPositions[currentPositionIndex].z;
}

void initialRotation() {
    //rotiraj u smjeru -z
    float angleX = atan2(0.0f, -1.0f) * 180.0f / M_PI;
    float angleY = atan2(-1.0f, 0.0f) * 180.0f / M_PI;
    float angleZ = 0.0f;

    glRotatef(angleX, 1.0, 0.0, 0.0);
    glRotatef(angleY, 0.0, 1.0, 0.0);
    glRotatef(angleZ, 0.0, 0.0, 1.0);

    //rotiraj u smjeru krivulje (smjer tangente u prvoj tocki)
    GLfloat modelviewMatrix[16];
    glGetFloatv(GL_MODELVIEW_MATRIX, modelviewMatrix);
    GLfloat forwardVector[3] = { modelviewMatrix[2], modelviewMatrix[6], modelviewMatrix[10] };

    //azuriraj podatke za izracun rotacije
    currentOrientation.x = forwardVector[0];
    currentOrientation.y = forwardVector[1];
    currentOrientation.z = forwardVector[2];

    targetOrientation.x = tangentPositions[1].x;
    targetOrientation.y = tangentPositions[1].y;
    targetOrientation.z = tangentPositions[1].z;
}

void renderScene() {
    //krivulja
    glPushMatrix();
    drawSpline();
    glPopMatrix();

    //tangente
    glPushMatrix();
    if (currentPositionIndex2 < tangentPositions.size()) {
        drawTangent();
    }
    glPopMatrix();

    //objekt
    glPushMatrix();
    if (currentPositionIndex < positions.size()) {
        glTranslatef(positions[currentPositionIndex].x, positions[currentPositionIndex].y, positions[currentPositionIndex].z);
        if (currentPositionIndex == 1) {
            initialRotation();
        }
        else {
            updateParameters();
        }
        updateRotation();
    }
    glScalef(2, 2, 2);

    //renderCube();
    renderPyramid();
    glPopMatrix();

}

bool loadModel(const std::string& filePath) {
    Assimp::Importer importer;
    scene = importer.ReadFile(filePath, aiProcess_Triangulate);

    if (!scene) {
        std::cerr << "Model ne postoji " << importer.GetErrorString() << std::endl;
        return false;
    }

    return true;
}

void display() {
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glPointSize(5.0);
    glEnable(GL_POINT_SMOOTH);
    glMatrixMode(GL_MODELVIEW);
    glLoadIdentity();
    gluLookAt(0, 40, 30, 0, -40, 30, 0, 0, 1);
    
    renderScene();

    glutSwapBuffers();
}

void reshape(int w, int h) {
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    gluPerspective(90.0, 1.0, 1.0, 100.0);
    glMatrixMode(GL_MODELVIEW);

}

void timer(int value) {
    glutPostRedisplay();
    glutTimerFunc(16, timer, 0); // 60 FPS

    // azuriraj indeks za listu tocaka krivulje
    if (currentPositionIndex < positions.size() - 1) {
        currentPositionIndex++;
    }
    else {
        currentPositionIndex = 0;
    }

    //azuriraj indeks za listu tocaka tangente
    if (currentPositionIndex2 < tangentPositions.size() - 1) {
        currentPositionIndex2++;
    }
    else {
        currentPositionIndex2 = 0;
    }

    //azuriraj pocetnu i krajnju tocku tangente
    startLine = positions[currentPositionIndex];
    endLine = tangentPositions[currentPositionIndex];
}

int main(int argc, char** argv) {
    glutInit(&argc, argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
    glutInitWindowSize(800, 600);
    glutCreateWindow("RACANI Lab1");
    glEnable(GL_DEPTH_TEST);

    calculateBSplinePoints();
    calculateTangentVectors();

    glutDisplayFunc(display);
    glutReshapeFunc(reshape);

    glutTimerFunc(0, timer, 0);

    glClearColor(1.0, 1.0, 1.0, 1.0); //bijela pozadina
    glutMainLoop();

    return 0;
}
