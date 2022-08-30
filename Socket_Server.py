# -*- coding: utf-8 -*-
"""
Created on Fri May 27 17:27:04 2022

@author: vf199
"""

import os
import io
import socket
import cv2
import numpy as np
import pyodbc
from PIL import Image
from multiprocessing import Process
from xml.etree import ElementTree as ET
from xml.dom import minidom
from threading import Thread

import time

class Socket_Server(Process): #給平版用的
    def __init__(self, queue1, queue2, queue3, IP, PORT):
        super(Socket_Server, self).__init__() #需要繼承父類別的的東東
        
        self.queue1 = queue1
        self.queue2 = queue2
        self.queue3 = queue3
        
        self.IP = IP
        self.PORT = PORT
        
        self.server_init()
        self.open_cv_image2 = None
        
        #print("Socket Init")
        
    def server_init(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.s.bind((self.IP, self.PORT))
        self.s.listen(5)
        
    def bytes2img(self, indata):
        data = np.frombuffer(indata, dtype='uint8')
        try:
            image = Image.open(io.BytesIO(data))
            open_cv_image = np.array(image) 
            # Convert RGB to BGR 
            open_cv_image = open_cv_image[:, :, ::-1].copy() 
            self.open_cv_image2 = open_cv_image
        except:
            open_cv_image = self.open_cv_image2
        
        if open_cv_image is not None:
            self.queue1.put(open_cv_image)
            #print("影像傳出去了!")
            
    def img2bytes(self):
        #print("等待影像回傳!")
        img = self.queue2.get(True)
        #print("影像傳回來了!")
        
        img_encode = cv2.imencode('.jpg', img)[1]
        data_encode = np.array(img_encode)
        str_encode = data_encode.tobytes()
        
        return str_encode

    def run(self):
        #print("準備連線")
        print("Socket_Server: {}".format(os.getpid()))
        while 1:
            #print("等待連線...")
            conn, addr = self.s.accept()
            print('Connected by ' + str(addr))
            
            while 1:
                #time1 = time.time()
                
                indata = conn.recv(2000000)
                #print("我收到影像了!")
                self.bytes2img(indata)            
                #print("佇列2有值")
                outdata = self.img2bytes()
                self.queue3.put(outdata)
                conn.send(outdata)
                
                #time2 = time.time()
                #print("Socket接收到傳送共花了 " + str(time2 - time1) + "秒")
                
class Socket_Server2(Process): #給遠端平台用的
    def __init__(self, queue1, queue2, IP, PORT):
        super(Socket_Server2, self).__init__() #需要繼承父類別的的東東
        
        self.queue1 = queue1
        self.queue2 = queue2
        
        self.IP = IP
        self.PORT = PORT
        
        self.server_init()
        
        #print("Socket Init")
        
    def server_init(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.s.bind((self.IP, self.PORT))
        self.s.listen(5)
        
    def string_split(self, text):
        text = text.strip('(')
        text = text.strip(')')
        arr = text.split(', ')
        arr = [float(i) for i in arr]
        
        return arr
        
    def read_xml(self, data):
        model_info = []
        
        root = data.getroot()
        obj = root.findall('Model_Data/Number')
        for i in obj:
            Type = int(i[0].text)
            Loc = self.string_split(i[1].text)
            Size = self.string_split(i[2].text)
            Rota = self.string_split(i[3].text)
            
            info = {'Type': Type, 'Loc': Loc, 'Size': Size, 'Rota': Rota}
            model_info.append(info)
            
        self.queue2.put(model_info) #把XML資料傳到Image_Shading4
        
    def bytes2xml(self, indata):
        data = indata.decode("utf-8")
        
        xml = minidom.parseString(data)
        xml_pretty_str = xml.toprettyxml()
        
        data = ET.ElementTree(ET.fromstring(xml_pretty_str))
        #self.queue2.put(data) #把XML資料傳到Image_Shading4
        self.read_xml(data)
        
        localtime = time.localtime()
        result = time.strftime("%Y%m%d%H%M%S", localtime)
        file_name = "log_" + result
        data.write(file_name + ".xml")
        self.xml2sql(file_name)
            
    def xml2sql(self, file_name): #把收到的XML存到資料庫
        file_name = "'" + file_name + "'"
        tag = "'This is a test.'"
    
        server = self.IP
        port = '1433'
        username = 'sa' 
        password = 'asD0540317' 
        cnxn = pyodbc.connect(driver='{ODBC Driver 17 for SQL Server}', server=server + ',' + port, user=username, password=password, autocommit=True)
        cursor = cnxn.cursor()

        cursor.execute('''
                        USE Fog_Database;
                        INSERT INTO Operate_Log (
                            time, file_name, tag)
                        VALUES(
                            GETDATE(),''' + file_name + "," + tag + ")"
                        )
        cursor.commit()
        
        # 斷開連線、釋放資源
        cursor.close()
        cnxn.close()
        
        print("有收到XML資料並儲存了")
            
    def img2bytes(self):
        img = self.queue1.get(True)
        '''
        img = cv2.imread("FR_dUysVkAASlsZ.jpg")
        img = cv2.resize(img, (640, 360), interpolation=cv2.INTER_AREA)
        '''
        img_encode = cv2.imencode('.jpg', img)[1]
        data_encode = np.array(img_encode)
        str_encode = data_encode.tobytes()

        return str_encode

    def get_xml(self, conn):
        while 1:
            indata = conn.recv(2000000) #接收Remote Platform傳來的資料
            self.bytes2xml(indata)

    def run(self):
        #print("準備連線")
        print("Socket_Server2: {}".format(os.getpid()))
        while 1:
            #print("等待連線...")
            conn, addr = self.s.accept()
            print('Connected by ' + str(addr))
            
            thread0 = Thread(target = self.get_xml, args = (conn,)) #創建接收XML資料的線程，不然沒有傳XML資料的時候會卡住
            thread0.start()
            while 1:
                #time1 = time.time()
                
                #indata = conn.recv(2000000) #接收Remote Platform傳來的資料
                #self.bytes2xml(indata)
                outdata = self.queue1.get(True)
                conn.send(outdata)
                
                #time2 = time.time()
                #print("Socket接收到傳送共花了 " + str(time2 - time1) + "秒")
                
class Socket_Server3(Process): #Surface跟遠端平台的語音通話
    def __init__(self, queue, IP, PORT):
        super(Socket_Server3, self).__init__() #需要繼承父類別的的東東
        
        self.IP = IP
        self.PORT = PORT
        self.queue = queue
        
        self.server_init()
        
        #print("Socket Init")
        
    def server_init(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.s.bind((self.IP, self.PORT))
        self.s.listen(5)
        
    def send_data(self, conn0, conn1, addr):
        print(addr)
        while 1:
            try:
                data = conn0.recv(3200) #接收音訊資料
                if data != None:
                    conn1.send(data) #傳送音訊資料
            except:
                self.queue.put(False) #只要其中一邊斷開，就離開遠端協作模式
                break
        
    def run(self):
        #print("準備連線")
        print("Socket_Server3: {}".format(os.getpid()))

        while 1:
            #print("等待連線...")
            conn, addr = self.s.accept()
            print('1. Connected by ' + str(addr))
            conn2, addr2 = self.s.accept()
            print('2. Connected by ' + str(addr2))
            
            self.queue.put(True) #兩邊都連線，就進入遠端協作模式
            
            thread0 = Thread(target = self.send_data, args = (conn, conn2, addr))
            thread1 = Thread(target = self.send_data, args = (conn2, conn, addr2))
            #thread0.setDaemon(True)
            #thread1.setDaemon(True)
            thread0.start()
            thread1.start()
            thread0.join()
            thread1.join()
        '''
        while 1:
            time1 = time.time()
            data = conn.recv(3200) #接收音訊資料
            if data != None:
                conn2.send(data) #傳送音訊資料
            time2 = time.time()
            print("1. Socket接收到傳送共花了 " + str(time2 - time1) + "秒")
            
            time1 = time.time()
            data = conn2.recv(3200) #接收音訊資料
            if data != None:
                conn.send(data) #傳送音訊資料
            time2 = time.time()
            print("2. Socket接收到傳送共花了 " + str(time2 - time1) + "秒")
        '''
        
class Socket_Server4(Process): #Surface 選擇的XML檔名回傳
    def __init__(self, queue, IP, PORT):
        super(Socket_Server4, self).__init__() #需要繼承父類別的的東東
        
        self.queue = queue
        
        self.IP = IP
        self.PORT = PORT
        
        self.server_init()
        
        #print("Socket Init")
        
    def server_init(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.s.bind((self.IP, self.PORT))
        self.s.listen(5)
        
    def string_split(self, text):
        text = text.strip('(')
        text = text.strip(')')
        arr = text.split(', ')
        arr = [float(i) for i in arr]
        
        return arr
        
    def read_xml(self, file):
        model_info = []
        
        xml = ET.parse(file)
        root = xml.getroot()
        obj = root.findall('Model_Data/Number')
        for i in obj:
            Type = int(i[0].text)
            Loc = self.string_split(i[1].text)
            Size = self.string_split(i[2].text)
            Rota = self.string_split(i[3].text)
            
            info = {'Type': Type, 'Loc': Loc, 'Size': Size, 'Rota': Rota}
            model_info.append(info)
        
        self.queue.put(model_info)
        
    def run(self):
        print("準備連線")
        print("Socket_Server4: {}".format(os.getpid()))
        
        while 1:
            conn, addr = self.s.accept()
            print('Connected by ' + str(addr))
            while 1:
                indata = conn.recv(20) #接收Remote Platform傳來的資料
                file_path = "./XML_File/" + indata.decode("utf-8") + ".xml"
                #self.queue.put(file_path) #把XML資料傳到Image_Shading4（需要先讀取XML檔案的內容）
                self.read_xml(file_path)
                
                conn.close()
                break