# -*- coding: utf-8 -*-
"""
Created on Fri May 27 19:41:38 2022

@author: vf199
"""

import Socket_Server
import Image_Analysis
import Image_Shading
import Data_Processing
import Image_Shading4_Demo

from multiprocessing import Queue, Process
import cv2
import os

import time

class test(Process):
    def __init__(self, queue1, queue2):
        super(test, self).__init__()
        
        self.queue1 = queue1
        self.queue2 = queue2
        
    def convert_img(self, img):
        img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        return img
    
    def run(self):
        print("test: {}".format(os.getpid()))
        while 1:
            img = self.queue1.get(True)
            img = self.convert_img(img)
            self.queue2.put(img)
            
class get_local_img(Process):
    def __init__(self, queue1, queue2, queue3, width, height, fps):
        super(get_local_img, self).__init__()
        
        self.queue1 = queue1
        self.queue2 = queue2
        self.queue3 = queue3
        
        self.width = width
        self.height = height
        self.fps = fps
        
    def run(self):
        print("get_img_local: {}".format(os.getpid()))
        
        cap = cv2.VideoCapture(0, cv2.CAP_DSHOW) #cv2.CAP_DSHOW據說為Windows特有，不知道Linux需不需要，主要是拿來消除影像黑邊（0代表第一台相機）
        cap.set(cv2.CAP_PROP_FRAME_WIDTH, self.width) #設定相機影像寬度
        cap.set(cv2.CAP_PROP_FRAME_HEIGHT, self.height) #設定相機影像高度
        cap.set(cv2.CAP_PROP_FPS, self.fps) #設定相機取像頻率
        cap.set(cv2.CAP_PROP_FOURCC, cv2.VideoWriter_fourcc(*"MJPG"))
        
        show_frame1 = None
        while 1:
            time1 = time.time()
            
            ret, frame = cap.read()
            if not ret:
                print("Can't receive frame (stream end?). Exiting ...")
                break
            
            self.queue1.put(frame)
            try:
                show_frame = self.queue2.get(True)
                show_frame1 = show_frame
            except:
                show_frame = show_frame1
                
            if show_frame is not None:
                cv2.imshow("Augmented Reality (Local Test)", show_frame)
                self.queue3.put(show_frame)
                if cv2.waitKey(1) == ord("q"):
                    break
                
                time2 = time.time()
                print(1/(time2 - time1))
            
        cap.release()
        cv2.destroyAllWindows()
            
if __name__ == "__main__":
    '''
    send_img1 = Queue()
    send_img2 = Queue()
    
    fog = Socket_Server.Socket_Server(send_img1, send_img2, "140.116.86.220", 7000)
    p2_test = test(send_img1, send_img2)

    fog.start()
    p2_test.start()
    
    fog.join()
    p2_test.join()
    '''
    
    SS2IA = Queue()
    IA2IS = Queue(1)
    IS2SS = Queue()
    DP2IS = Queue(1)
    SS2SS2 = Queue()
    #SS22IS = Queue()
    SS42IS = Queue()
    SS32IS = Queue()
    
    #p0 = get_local_img(SS2IA, IS2SS, SS2SS2, 1280, 720, 60) #測試用
    p0 = Socket_Server.Socket_Server(SS2IA, IS2SS, SS2SS2, "140.116.86.220", 7000) #Surface
    p1 = Socket_Server.Socket_Server2(SS2SS2, SS42IS, "140.116.86.220", 7001) #Remote Platform
    p2 = Socket_Server.Socket_Server3(SS32IS, "140.116.86.220", 7002) #Voice Chat
    p3 = Image_Analysis.Image_Analysis(SS2IA, IA2IS, IS2SS, "Camera_Calibration/Camera_Parameter_Surface720p_00255.npz")
    p4 = Image_Shading4_Demo.Image_Shading(IA2IS, IS2SS, DP2IS, SS42IS, SS32IS, 1280, 720) #Demo標註2（同動）
    p5 = Data_Processing.Data_Processing(DP2IS, "140.116.86.220", 4840, 7004)
    p6 = Socket_Server.Socket_Server4(SS42IS, "140.116.86.220", 7003) #Surface 選擇的XML檔名回傳
    
    p0.start()
    p1.start()
    p2.start()
    p3.start()
    p4.start()
    p5.start()
    p6.start()
    
    p0.join()
    p1.join()
    p2.join()
    p3.join()
    p4.join()
    p5.join()
    p6.join()
