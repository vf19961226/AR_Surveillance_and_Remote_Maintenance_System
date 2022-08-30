# -*- coding: utf-8 -*-
"""
Created on Sat May 28 19:52:28 2022

@author: vf199
"""

import os
import cv2
import numpy as np
from PIL import Image
from multiprocessing import Process

import time

class Image_Analysis(Process):
    def __init__(self, queue1, queue2, queue3, CamMatrix_path):
        super(Image_Analysis, self).__init__() #需要繼承父類別的的東東
        
        self.queue1 = queue1
        self.queue2 = queue2
        self.queue3 = queue3
        
        self.get_CamMatrix(CamMatrix_path)
        
        #避免模型抖動
        self.pre_trans_x = None
        self.pre_trans_y = None
        self.pre_trans_z = None
        
    def get_CamMatrix(self, file_path):
        parameter = np.load(file_path)
        self.cameraMatrix = parameter['mtx']
        self.distCoeffs = parameter['dist']
        
    def extrinsic2ModelView(self, RVEC, TVEC, R_vector = True):
        R, _ = cv2.Rodrigues(RVEC)
    
        Rx = np.array([
            [1, 0, 0],
            [0, -1, 0],
            [0, 0, -1]
        ])
    
        TVEC = TVEC.flatten().reshape((3, 1))
    
        #transform_matrix = Rx @ np.hstack((R, TVEC))
        transform_matrix = np.dot(Rx, np.hstack((R, TVEC)))
        M = np.eye(4)
        
        M[:3, :] = transform_matrix
        return M.T.flatten()
    
    def intrinsic2Project(self, MTX, width, height, near_plane = 0.01, far_plane = 100.0):
        P = np.zeros(shape = (4, 4), dtype = np.float32)
    
        fx, fy = MTX[0, 0], MTX[1, 1]
        cx, cy = MTX[0, 2], MTX[1, 2]
    
        P[0, 0] = 2 * fx / width
        P[1, 1] = 2 * fy / height
        P[2, 0] = 1 - 2 * cx / width
        P[2, 1] = 2 * cy / height - 1
        P[2, 2] = -(far_plane + near_plane) / (far_plane - near_plane)
        P[2, 3] = -1.0
        P[3, 2] = - (2 * far_plane * near_plane) / (far_plane - near_plane)
    
        return P.flatten()
    
    def convert2bgimg(self, img):
        bgimg = Image.fromarray(img)
        bgimg = bgimg.tobytes("raw","BGRX", 0, -1)
        
        return bgimg
    
    def tag_update(self, tvecs):
        trans_x, trans_y, trans_z = tvecs[0][0], tvecs[0][1], tvecs[0][2]
        is_mark_move = False
        if self.pre_trans_x is not None:
            if abs(self.pre_trans_x - trans_x) > 0.001 or abs(self.pre_trans_y - trans_y) > 0.002 or abs(self.pre_trans_z - trans_z) > 0.015:
                is_mark_move = True
                
        self.pre_trans_x, self.pre_trans_y, self.pre_trans_z = trans_x, trans_y, trans_z
        return is_mark_move
    
    def run(self):
        print("Image_Processing: {}".format(os.getpid()))
        
        dictionary = cv2.aruco.Dictionary_get(10)
        parameters = cv2.aruco.DetectorParameters_create()
        while 1:
            img = self.queue1.get(True)
            
            #time1 = time.time()
            
            gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
            corners, ids, rejected_corners = cv2.aruco.detectMarkers(gray, dictionary, parameters = parameters)
            if ids is not None:
                rvecs, tvecs, _ = cv2.aruco.estimatePoseSingleMarkers(corners, 1, self.cameraMatrix, self.distCoeffs)
                for rvec, tvec in zip(rvecs, tvecs):
                    modelView = self.extrinsic2ModelView(rvec, tvec)
                    projection = self.intrinsic2Project(self.cameraMatrix, img.shape[1], img.shape[0])
                    
                data = {"img" : self.convert2bgimg(img), "mv" : modelView, "pj" : projection}
                '''
                if self.queue2.empty(): #############################TEST
                    self.queue2.put(data)
                '''
                self.queue2.put(data)
                
                #time2 = time.time()
                #print("有TAG的影像分析共花了 " + str(time2 - time1) + "秒")
            else:
                self.queue3.put(img)
                
                #time2 = time.time()
                #print("沒TAG的影像分析共花了 " + str(time2 - time1) + "秒")