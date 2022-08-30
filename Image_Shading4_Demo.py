# -*- coding: utf-8 -*-
"""
Created on Sat Jul  2 15:28:22 2022

@author: vf199
"""

import os
import numpy as np
from ctypes import *
from OpenGL.GLUT import *
from OpenGL.GLU import *
from OpenGL.GL import *
from multiprocessing import Process

import objloader

class Image_Shading(Process):
    def __init__(self, queue1, queue2, queue3, queue4, queue5, img_width, img_height, test = False):
        super(Image_Shading, self).__init__() #需要繼承父類別的的東東
        
        self.queue1 = queue1
        self.queue2 = queue2
        self.queue3 = queue3
        self.queue4 = queue4 #SS42IS
        self.queue5 = queue5 #SS32IS
        
        self.xml_data = [] #Help模式的XML_DATA一開始是甚麼都沒有
        self.dashborads_before = None
        
        self.W = img_width
        self.H = img_height
        
        self.test = test #是否為本機測試模式
        
        self.help = False #是否為遠端協作模式
        
        #背景用的
        self.BG_VERTEX_SHADER = """   
                                #version 410
                                
                                layout(location = 0) in vec2 position;
                                layout(location = 1) in vec2 texcoord;
                                
                                out vec2 coord;
                                
                                void main()
                                {
                                	gl_Position = vec4(position, 0.0, 1.0);
                                	coord = texcoord;
                                }
                                """
                            
        self.BG_FRAGMENT_SHADER = """
                                #version 410
                                
                                in vec2 coord;
                                out vec4 color;
                                uniform sampler2D tex;
                                
                                void main(void)
                                {
                                	color = texture(tex, coord);
                                }
                                """
        
        #儀表板用的
        self.DB_VERTEX_SHADER = """
                                #version 410
                                
                                layout(location = 0) in vec2 position; //模型座標（x, y）
                                layout(location = 1) in vec2 texcoord; //貼圖座標（u, v）
                                
                                uniform vec3 translate;
                                uniform mat4 move;
                                uniform mat4 modelview;
                                uniform mat4 projection;
                                
                                out vec2 coord;
                                
                                void main()
                                {
                                    vec4 translate_model = vec4(position.x + translate.x, position.y + translate.y, 0.0 + translate.z, 1.0);
                                	gl_Position = projection * modelview * move * translate_model;
                                	coord = texcoord;
                                }
                                """
                            
        self.DB_FRAGMENT_SHADER = """
                                #version 410
                                
                                in vec2 coord;
                                out vec4 color;
                                uniform sampler2D tex; //貼圖（圖像）
                                
                                void main(void)
                                {
                                	color = texture(tex, coord);
                                }
                                """
                                
    def background_init(self):
        glClearColor(1.0, 1.0, 1.0, 1.0)
        glEnable(GL_DEPTH_TEST)
        glDepthFunc(GL_LEQUAL)
        
        # Initialize shaders
        ########################
        self.bg_program = glCreateProgram()
         
        bg_vs = glCreateShader(GL_VERTEX_SHADER)
        bg_fs = glCreateShader(GL_FRAGMENT_SHADER)
        
        glShaderSource(bg_vs, self.BG_VERTEX_SHADER)
        glShaderSource(bg_fs, self.BG_FRAGMENT_SHADER)
        
        glCompileShader(bg_vs)
        glCompileShader(bg_fs)
        
        glAttachShader(self.bg_program, bg_vs)
        glAttachShader(self.bg_program, bg_fs)
        
        glLinkProgram(self.bg_program)
         
        #glUseProgram(program)
        
        # Define vertex
        ########################       
        data = np.array([[1.0, -1.0, 1.0, 0.0],
                         [-1.0, -1.0, 0.0, 0.0],
                         [-1.0, 1.0, 0.0, 1.0],
                         [1.0, 1.0, 1.0, 1.0]], dtype = "float32") #[世界座標X軸, 世界座標Y軸, 視窗座標X軸, 視窗座標Y軸]
        
        data = data.flatten()
        
        # Create buffer
        ########################
        bg_buffer = glGenBuffers(1)
        
        glBindBuffer(GL_ARRAY_BUFFER, bg_buffer)
        
        glBufferData(GL_ARRAY_BUFFER, data.nbytes, data, GL_STATIC_DRAW)
        
        self.bg_vao = glGenVertexArrays(1)
        glBindVertexArray(self.bg_vao)
        
        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 16, None)
        glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 16, c_void_p(8)) #位移一個元素4bit，共為一9個元素36bit，型別要用ctypes的c_void_p
        
        glEnableVertexAttribArray(0)
        glEnableVertexAttribArray(1)
        
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
    
        self.bg_texture = glGenTextures(1)
        glBindTexture(GL_TEXTURE_2D, self.bg_texture)
        
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR)
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR)
        
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, self.W, self.H, 0, GL_RGBA, GL_UNSIGNED_BYTE, None) #######這裡貼圖的長寬須注意（儀錶板）
    
        glBindTexture(GL_TEXTURE_2D, 0)
        glBindVertexArray(0)
            
    def draw_background(self, img): #渲染背景
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
        
        glUseProgram(self.bg_program)
        glBindVertexArray(self.bg_vao)
        
        glBindTexture(GL_TEXTURE_2D, self.bg_texture) #1是背景的貼圖渲染器
        glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, self.W, self.H, GL_RGBA, GL_UNSIGNED_BYTE, img)
        glDrawArrays(GL_TRIANGLE_FAN, 0, 4)
        glClear(GL_DEPTH_BUFFER_BIT)
        glBindTexture(GL_TEXTURE_2D, 0) #0是內建的，用來取消綁定貼圖
        glBindVertexArray(0) #0是內建的，用來取消綁定VAO
        
        glUseProgram(0)
        
    def dashborad_init(self): #儀表板緩衝器初始化
        glClearColor(1.0, 1.0, 1.0, 1.0)
        glEnable(GL_DEPTH_TEST)
        glDepthFunc(GL_LEQUAL)
        
        # Initialize shaders
        ########################
        self.db_program = glCreateProgram()
         
        db_vs = glCreateShader(GL_VERTEX_SHADER)
        db_fs = glCreateShader(GL_FRAGMENT_SHADER)
        
        glShaderSource(db_vs, self.DB_VERTEX_SHADER)
        glShaderSource(db_fs, self.DB_FRAGMENT_SHADER)
        
        glCompileShader(db_vs)
        glCompileShader(db_fs)
        
        glAttachShader(self.db_program, db_vs)
        glAttachShader(self.db_program, db_fs)
        
        glLinkProgram(self.db_program)
        
        glUseProgram(self.db_program)
        
        self.translate = glGetUniformLocation(self.db_program, "translate")
        self.move = glGetUniformLocation(self.db_program, "move")
        self.model_view = glGetUniformLocation(self.db_program, "modelview")
        self.projection = glGetUniformLocation(self.db_program, "projection")
        
        glUseProgram(0)
        
        # Define vertex
        ######################## 之後需要更換頂點座標位置（？
        data = np.array([[0.5, -0.5, 1.0, 0.0],
                         [-0.5, -0.5, 0.0, 0.0],
                         [-0.5, 0.5, 0.0, 1.0],
                         [0.5, 0.5, 1.0, 1.0]], dtype = "float32") #[世界座標X軸, 世界座標Y軸, 視窗座標X軸, 視窗座標Y軸] [頂點座標X, 頂點座標Y, 貼圖座標X, 貼圖座標Y]
        
        data = data.flatten()
        
        # Create buffer
        ########################
        db_buffer = glGenBuffers(1)
        
        glBindBuffer(GL_ARRAY_BUFFER, db_buffer)
        
        glBufferData(GL_ARRAY_BUFFER, data.nbytes, data, GL_STATIC_DRAW)
        
        self.db_vao = glGenVertexArrays(1)
        glBindVertexArray(self.db_vao)
        
        glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 16, None)
        glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 16, c_void_p(8)) #位移一個元素4bit，共為一9個元素36bit，型別要用ctypes的c_void_p
        
        glEnableVertexAttribArray(0)
        glEnableVertexAttribArray(1)
        
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
    
        self.db_texture = glGenTextures(1)
        glBindTexture(GL_TEXTURE_2D, self.db_texture)
        
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR)
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR)
        
        glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, 640, 480, 0, GL_RGBA, GL_UNSIGNED_BYTE, None) ###這裡需先知道儀表板的大小（長跟寬）
    
        glBindTexture(GL_TEXTURE_2D, 0)
        glBindVertexArray(0)
        
    def model_rotate_scaling(self, theta, axis, a = 1, b = 1, c = 1): #調整位置、大小、旋轉
        opt_matrix = np.eye(4) #opt_matrix初始化
        
        scaling = np.array([[a, 0, 0, 0],
                                 [0, b, 0, 0],
                                 [0, 0, c, 0],
                                 [0, 0, 0, 1]], dtype = "float32")
        
        for t, a in zip(theta, axis):
            t = t * np.pi / 180 #轉成弧度
            if a == 0:
                rotate = np.array([[1, 0, 0, 0],
                                   [0, np.cos(t), -np.sin(t), 0],
                                   [0, np.sin(t), np.cos(t), 0],
                                   [0, 0, 0, 1]], dtype = "float32")
                
            elif a == 1:
                rotate = np.array([[np.cos(t), 0, np.sin(t), 0],
                                   [0, 1, 0, 0],
                                   [-np.sin(t), 0, np.cos(t), 0],
                                   [0, 0, 0, 1]], dtype = "float32")
                
            elif a == 2:
                rotate = np.array([[np.cos(t), -np.sin(t), 0, 0],
                                   [np.sin(t), np.cos(t), 0, 0],
                                   [0, 0, 1, 0],
                                   [0, 0, 0, 1]], dtype = "float32")
                
            opt_matrix = np.dot(rotate, opt_matrix)
            
        opt_matrix = np.dot(scaling, opt_matrix)
        
        opt_matrix = np.linalg.inv(opt_matrix)
        
        return opt_matrix.flatten()
        
    def draw_dashborad(self, modelView, Projection): #渲染儀表板
        dashborads = None
    
        if self.queue3.empty() != True:
            dashborads = self.queue3.get(True)
            self.dashborads_before = dashborads
        else:
            dashborads = self.dashborads_before
        
        dashborads_list = ["S", "X", "Y", "Z"]
        
        for i in dashborads_list:
            dashborad = dashborads[i]
            
            glUseProgram(self.db_program)
            glBindVertexArray(self.db_vao)
            
            glBindTexture(GL_TEXTURE_2D, self.db_texture) #1是背景的貼圖渲染器
            glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, 640, 480, GL_RGBA, GL_UNSIGNED_BYTE, dashborad)

            if i == "S":
                glUniform3fv(self.translate, 1, np.array([0, 0, 0], dtype = "float32").flatten()) #平移[x, y, z]
            elif i == "X":
                glUniform3fv(self.translate, 1, np.array([0, -1.5, 0], dtype = "float32").flatten()) #平移[x, y, z]
            elif i == "Y":
                glUniform3fv(self.translate, 1, np.array([-1.5, 0, 0], dtype = "float32").flatten()) #平移[x, y, z]
            elif i == "Z":
                glUniform3fv(self.translate, 1, np.array([-1.5, -1.5, 0], dtype = "float32").flatten()) #平移[x, y, z]
                
            glUniformMatrix4fv(self.move, 1, GL_FALSE, self.model_rotate_scaling([0], [0], a = 0.5, b = 0.5))#調整大小
            glUniformMatrix4fv(self.model_view, 1, GL_FALSE, modelView)
            glUniformMatrix4fv(self.projection, 1, GL_FALSE, Projection)
            
            glDrawArrays(GL_TRIANGLE_FAN, 0, 4)
            
            glBindTexture(GL_TEXTURE_2D, 0) #0是內建的，用來取消綁定貼圖
            
            glBindVertexArray(0)
            glUseProgram(0)
            
    def draw_obj(self, modelView, projection): #渲染虛擬物件
        # 畫箭頭
        glMatrixMode(GL_PROJECTION)
        glLoadIdentity()
        glMultMatrixf(projection)
    
        glMatrixMode(GL_MODELVIEW)
        glLoadIdentity()
        glLoadMatrixf(modelView)
        
        glTranslatef(6.8, 1, 0) #（這個函式是移動模型位置的）
        glRotatef(90, 0, 0, 1);
                
        glEnable(GL_LIGHT0)
        glCallList(1)
        glDisable(GL_LIGHT0)
        
        #畫手
        glMatrixMode(GL_PROJECTION)
        glLoadIdentity()
        glMultMatrixf(projection)
    
        glMatrixMode(GL_MODELVIEW)
        glLoadIdentity()
        glLoadMatrixf(modelView)
        
        glTranslatef(3, -3, 1.5) #（這個函式是移動模型位置的）
        glRotatef(180, 1, 0, 0);
                
        glEnable(GL_LIGHT0)
        glCallList(2)
        glDisable(GL_LIGHT0)
        
        #畫注意
        glMatrixMode(GL_PROJECTION)
        glLoadIdentity()
        glMultMatrixf(projection)
    
        glMatrixMode(GL_MODELVIEW)
        glLoadIdentity()
        glLoadMatrixf(modelView)
        
        glTranslatef(3, 0, 3) #（這個函式是移動模型位置的）
        glRotatef(180, 0, 0, 1);
                
        glEnable(GL_LIGHT0)
        glCallList(3)
        glDisable(GL_LIGHT0)
        '''
        #畫白板
        glMatrixMode(GL_PROJECTION)
        glLoadIdentity()
        glMultMatrixf(projection)
    
        glMatrixMode(GL_MODELVIEW)
        glLoadIdentity()
        glLoadMatrixf(modelView)
        
        glTranslatef(-7, 0, 0) #（這個函式是移動模型位置的）
        glRotatef(30, 0, 1, 0);
                
        glEnable(GL_LIGHT0)
        glCallList(4)
        glDisable(GL_LIGHT0)
        '''
    def load_xml_data(self, data, modelView, projection): #渲染來自XML檔案的標註資訊
        for i in data:
            Type = i['Type']
            Loc = i['Loc']
            Size = i['Size']
            Rota = i['Rota']
            
            glMatrixMode(GL_PROJECTION)
            glLoadIdentity()
            glMultMatrixf(projection)
        
            glMatrixMode(GL_MODELVIEW)
            glLoadIdentity()
            glLoadMatrixf(modelView)
            
            glTranslatef(Loc[0] + 0.65, Loc[1] - 0.775, Loc[2] - 1.4) #（這個函式是移動模型位置的）#須注意與Unity空間座標的換算 X = (X+0.65)*12 Y = (Y-0.775)*17 Z = (Z-1.4)(-4)
            glRotatef(180, 1, 0, 0);
            glRotatef(Rota[0], 1, 0, 0);
            glRotatef(Rota[1], 0, 1, 0);
            glRotatef(Rota[2], 0, 0, 1);
            
            glEnable(GL_LIGHT0)
            glCallList(Type + 1)
            glDisable(GL_LIGHT0)
            #glCallList(0)
            
    def optimg(self): #取得渲染完成影像
        img_data = glReadPixels(0, 0, self.W, self.H, GL_BGRA, GL_FLOAT)
        img_data = np.frombuffer(img_data, np.float32)
        img_data.shape = self.H, self.W, 4
        img_data = img_data[::-1, :]
        
        if self.test:
            self.queue2.put(img_data)
        else:
            self.queue2.put(img_data * 255)
            
    def light_init(self): #加燈光
        glLightfv(GL_LIGHT0, GL_AMBIENT, (0.5, 0.5, 0.5, 1.0)) #設置環境光的屬性，0號
        glLightfv(GL_LIGHT0, GL_DIFFUSE, (0.6, 0.6, 0.6, 1.0)) #設置環境光的散射屬性，0號
        glLightfv(GL_LIGHT0, GL_POSITION, (0.0, 0.0, 1.0, 1.0)) #設置環境光的位置，0號
        glEnable(GL_LIGHTING)
        glEnable(GL_LIGHT0)
        
    def Display(self): #渲染影像迴圈
        data = self.queue1.get(True) #接收渲染影像跟相機矩陣
        
        if self.queue5.empty() != True: 
            flag = self.queue5.get(True) #接收現在模式（有沒有按下Help）（渲染儀表板或是OBJ）
            self.help = flag
        
        #time1 = time.time()
        
        img = data["img"]
        modelView = data["mv"]
        projection = data["pj"]
        
        self.draw_background(img)
        #self.draw_obj(modelView, projection)
        
        if self.help: #如果按下Help
            #self.draw_obj(modelView, projection)
            if self.queue4.empty() != True:
                self.xml_data = self.queue4.get(True)

            if len(self.xml_data) == 3:
                self.draw_obj(modelView, projection)
            else:
                self.load_xml_data(self.xml_data, modelView, projection) #須注意與Unity空間座標的換算 X+0.65 Y-0.775 Z-1.4

        else:
            self.draw_dashborad(modelView, projection)
        
        self.optimg()
        
        #time2 = time.time()
        #print("影像渲染共花了 " + str(time2 - time1) + "秒")
        
        glutSwapBuffers()
        
    def run(self):
        print("Image_Shading: {}".format(os.getpid()))
        
        # Initialize GLUT and GLEW, then create a window.
        ############################
        glutInit()
        glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_DEPTH)
        
        glutInitWindowPosition(100, 100)
        glutInitWindowSize(self.W, self.H)
        glutCreateWindow(b"Image of Augmented Reality") # You cannot use OpenGL functions before this line; The OpenGL context must be created first by glutCreateWindow()!
        
        glEnable(GL_COLOR_MATERIAL) #開啟上色功能
        
        self.light_init() #燈光初始化
        self.background_init() #畫背景的buffer初始化，包含一些頂點甚麼之類的
        self.dashborad_init() #畫儀表板的buffer初始化，包含一些頂點甚麼之類的
        
        # 事先匯入標註模型
        objloader.OBJ("./OBJ_File/Arrow2.obj", swapyz = True) #讀取OBJ檔案，並且把它弄成一個渲染程序輸出（glCallList編號為1）
        objloader.OBJ("./OBJ_File/hand2.obj", swapyz = True) #讀取OBJ檔案，並且把它弄成一個渲染程序輸出（glCallList編號為2）
        objloader.OBJ("./OBJ_File/Notice2.obj", swapyz = True) #讀取OBJ檔案，並且把它弄成一個渲染程序輸出（glCallList編號為3）
        objloader.OBJ("./OBJ_File/Whiteboard_Word.obj", swapyz = True) #讀取OBJ檔案，並且把它弄成一個渲染程序輸出（glCallList編號為4）
         
        #Register GLUT callback functions
        ############################
        glutDisplayFunc(self.Display)
        glutIdleFunc(self.Display)
        ############################
        
        # Enter main event loop.
        glutMainLoop()