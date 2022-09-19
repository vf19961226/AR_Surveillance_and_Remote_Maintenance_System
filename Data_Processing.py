# -*- coding: utf-8 -*-
"""
Created on Sun May 29 19:56:37 2022

@author: vf199
"""

from multiprocessing import Process
from threading import Thread
from opcua import Server, ua
from PIL import Image
import matplotlib.pyplot as plt
import numpy as np
import os
import pyodbc
import time
import socket
import sys

class Data_Processing(Process):
    def __init__(self, queue, IP, OPC_PORT, SOC_PORT):
        super(Data_Processing, self).__init__() #需要繼承父類別的的東東
        
        self.queue = queue
        
        self.IP = IP
        self.OPC_PORT = OPC_PORT
        self.SOC_PORT = SOC_PORT

        self.server_init()
        
        self.x_axis_val = 0
        self.y_axis_val = 0
        self.z_axis_val = 0
        self.current_val = 0
        
        self.x_loc_val = 0
        self.y_loc_val = 0
        self.z_loc_val = 0
        
        self.x_velocity_val = 0
        self.y_velocity_val = 0
        self.z_velocity_val = 0
        
        self.x_torque_val = 0
        self.y_torque_val = 0
        self.z_torque_val = 0
        
        self.x_errcode_val = 0
        self.y_errcode_val = 0
        self.z_errcode_val = 0
        
        self.Alarm = "''"
        
        # 安全色的16進位色碼
        self.danger = '#BE0F1C'
        self.notice = '#FCBF65'
        self.safe = '#237459'
        
        self.s_background = self.safe
        self.x_background = self.safe
        self.y_background = self.safe
        self.z_background = self.safe
        
        self.demo_flag = False
        self.t_end = None
        
    def server_init(self):
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.s.bind((self.IP, self.SOC_PORT))
        self.s.listen(5)
        
    def loc2remote(self): #把CNC的位置資料傳到遠端平台
        while 1:
            conn, addr = self.s.accept()
            print('Connected by ' + str(addr))
            while 1:
                try:
                    loc = str(self.x_loc_val) + " " + str(self.y_loc_val) + " " + str(self.z_loc_val)
                    #print(loc)
                    conn.send(loc.encode("ascii"))
                except:
                    print("loc出錯")
                    conn.close()
                    break

    def Draw_Dashborad(self, title, data, background):
        fig, ax = plt.subplots(figsize = (6.4, 4.8), facecolor = background)
        
        if title == "Spindle":
            ax.plot(self.xlabel, data[0], label = "Vibration_X(mm/sec^2)", color = "red") #X軸資料,Y軸資料,標籤(圖例顯示),線的顏色
            ax.plot(self.xlabel, data[1], label = "Vibration_Y(mm/sec^2)", color = "green")
            ax.plot(self.xlabel, data[2], label = "Vibration_Z(mm/sec^2)", color = "blue")
            ax.plot(self.xlabel, data[3], label = "Current(A)", color = "yellow")
            ax.set_title(title) #設定標題
            
        else:
            ax.plot(self.xlabel, data[0], label = "Velocity(mm/min)", color = "red")
            ax.plot(self.xlabel, data[1], label = "Torque(N-m)", color = "green")
            ax.set_title(title + "\nLocation: " + str(data[2]) + " Error:" + str(data[3]))
        
        ax.legend(loc='upper right') #設定圖例位置
        ax.set_xlabel("Time")
        ax.set_ylabel("Value")
        ax.set_xticks(self.xlabel, self.time_fig, rotation = 45)
        
        fig.canvas.draw()
        dashborad = np.frombuffer(fig.canvas.tostring_rgb(), dtype=np.uint8)
        dashborad = dashborad.reshape(fig.canvas.get_width_height()[::-1] + (3,))
        '''
        plt.savefig('./Figure/' + title + '.png') #儲存圖像
        '''
        plt.close()

        return dashborad
        
    def Alarm_Logic(self):
        
        if self.x_errcode_val == 1 and self.y_errcode_val == 1 and self.z_errcode_val == 1:
            self.s_background = self.notice
            self.Alarm = "'Spindle Error Code(CNC Tool).'"
        elif self.x_errcode_val != 0:
            self.x_background = self.danger
            self.Alarm = "'X Drive ErrorCode.'"   
        elif self.y_errcode_val != 0:
            self.y_background = self.danger
            self.Alarm = "'Y Drive ErrorCode.'"
        elif self.z_errcode_val != 0:
            self.z_background = self.danger
            self.Alarm = "'Z Drive ErrorCode.'"
        else:
            self.s_background = self.safe
            self.x_background = self.safe
            self.y_background = self.safe
            self.z_background = self.safe
            
            self.Alarm = "Null"
        ''' 
        self.s_background = self.safe
        self.x_background = self.notice
        self.y_background = self.danger
        self.z_background = self.safe
        '''
    def Data_Analysis(self):
        self.Alarm_Logic()
    
    def convert2glimg(self, img):
        img = img[:, :, ::-1]
        glimg = Image.fromarray(img)
        glimg = glimg.tobytes("raw","BGRX", 0, -1)
        
        return glimg
    
    def data2sql_f(self, cursor):
        while 1:
            try:
                cursor.execute('''
                                USE Fog_Database;
                                INSERT INTO CNC_Sensing_Data (
                                    time, x_vibration, y_vibration, z_vibration, s_current, x_loc, y_loc, z_loc,
                                    x_velocity, y_velocity, z_velocity, x_torque, y_torque, z_torque, x_errcode, y_errcode, z_errcode, Alarm)
                                VALUES(
                                    GETDATE(),''' + str(self.x_axis_val) + ',' + str(self.y_axis_val) + ',' + str(self.z_axis_val) + ',' + str(self.current_val) + ',' + str(self.x_loc_val) + ',' + str(self.y_loc_val) + ',' + str(self.z_loc_val) + ',' + str(self.x_velocity_val) + ',' + str(self.y_velocity_val) + ',' + str(self.z_velocity_val) + ',' + str(self.x_torque_val) + ',' + str(self.y_torque_val) + ',' + str(self.z_torque_val) + ',' + str(self.x_errcode_val) + ',' + str(self.y_errcode_val) + ',' + str(self.z_errcode_val) + ',' + self.Alarm + ')'
                                )
                cursor.commit()
            except:
                pass
            time.sleep(1)
    
    def data2sql_c(self, cursor):
        while 1:
            try:
                cursor.execute('''
                                USE Cloud_Database;
                                INSERT INTO CNC_Sensing_Data (
                                    time, x_vibration, y_vibration, z_vibration, s_current, x_loc, y_loc, z_loc,
                                    x_velocity, y_velocity, z_velocity, x_torque, y_torque, z_torque, x_errcode, y_errcode, z_errcode, Alarm)
                                VALUES(
                                    GETDATE(),''' + str(self.x_axis_val) + ',' + str(self.y_axis_val) + ',' + str(self.z_axis_val) + ',' + str(self.current_val) + ',' + str(self.x_loc_val) + ',' + str(self.y_loc_val) + ',' + str(self.z_loc_val) + ',' + str(self.x_velocity_val) + ',' + str(self.y_velocity_val) + ',' + str(self.z_velocity_val) + ',' + str(self.x_torque_val) + ',' + str(self.y_torque_val) + ',' + str(self.z_torque_val) + ',' + str(self.x_errcode_val) + ',' + str(self.y_errcode_val) + ',' + str(self.z_errcode_val) + ',' + self.Alarm + ')'
                                )
                cursor.commit()
            except:
                pass
            time.sleep(60)
    
    def run(self):
        print("Data_Processing: {}".format(os.getpid()))
        
        #連線到SQL Server
        cnxn = pyodbc.connect(driver='{ODBC Driver 17 for SQL Server}', server=self.IP + ',1433', user='sa', password='password', autocommit=True)
        cursor = cnxn.cursor()
        
        var_double = ua.Variant(0, ua.VariantType.Double)
        var_int = ua.Variant(0, ua.VariantType.Int16)
        
        server = Server()
        url ="opc.tcp://" + self.IP + ":" + str(self.OPC_PORT)
        print("OpcUA server server start at: %s:%s" % (self.IP, self.OPC_PORT))
        server.set_endpoint(url)
        
        name ="MY_FIRST_OPCUA_SERVER"
        addspace =server.register_namespace(name)
        
        node=server.get_objects_node()
        
        Param=node.add_object(addspace,"CNC")
        
        #設定參數節點
        ##主軸震動感測器
        x_axis = Param.add_variable("ns=3;i=1", "x_axis", var_double)
        y_axis = Param.add_variable("ns=3;i=2", "y_axis", var_double)
        z_axis = Param.add_variable("ns=3;i=3", "z_axis", var_double)
        current = Param.add_variable("ns=3;i=4", "current", var_double)
        
        x_axis.set_writable()
        y_axis.set_writable()
        z_axis.set_writable()
        current.set_writable()
        
        ##三軸位置
        x_loc = Param.add_variable("ns=4;i=1", "x_loc", var_double)
        y_loc = Param.add_variable("ns=4;i=2", "y_loc", var_double)
        z_loc = Param.add_variable("ns=4;i=3", "z_loc", var_double)
        
        x_loc.set_writable()
        y_loc.set_writable()
        z_loc.set_writable()
        
        ##三軸速度
        x_velocity = Param.add_variable("ns=5;i=1", "x_velocity", var_int)
        y_velocity = Param.add_variable("ns=5;i=2", "y_velocity", var_int)
        z_velocity = Param.add_variable("ns=5;i=3", "z_velocity", var_int)
        
        x_velocity.set_writable()
        y_velocity.set_writable()
        z_velocity.set_writable()
        
        ##三軸扭矩
        x_torque = Param.add_variable("ns=6;i=1", "x_torque", var_int)
        y_torque = Param.add_variable("ns=6;i=2", "y_torque", var_int)
        z_torque = Param.add_variable("ns=6;i=3", "z_torque", var_int)
        
        x_torque.set_writable()
        y_torque.set_writable()
        z_torque.set_writable()
        
        ##三軸錯誤代碼
        x_errcode = Param.add_variable("ns=7;i=1", "x_errcode", var_double)
        y_errcode = Param.add_variable("ns=7;i=2", "y_errcode", var_double)
        z_errcode = Param.add_variable("ns=7;i=3", "z_errcode", var_double)
        
        x_errcode.set_writable()
        y_errcode.set_writable()
        z_errcode.set_writable()
        
        server.start() #開啟Server
        
        i = 1
        self.xlabel = []
        self.time_fig = []
        
        x_axis_list = []
        y_axis_list = []
        z_axis_list = []
        current_list = []
        
        x_velocity_list = []
        y_velocity_list = []
        z_velocity_list = []
        
        x_torque_list = []
        y_torque_list = []
        z_torque_list = []
        
        thread0 = Thread(target = self.data2sql_f, args = (cursor,)) #每1秒寫入1筆資料進SQL Server（多線程）
        thread1 = Thread(target = self.data2sql_c, args = (cursor,)) #每60秒寫入1筆資料進SQL Server（多線程）
        thread2 = Thread(target = self.loc2remote) #把目前機台三軸位置傳給遠端平台
        thread0.start()
        thread1.start()
        thread2.start()
        while 1:
            #OpcUA取資料
            self.x_axis_val = x_axis.get_value()
            self.y_axis_val = y_axis.get_value()
            self.z_axis_val = z_axis.get_value()
            self.current_val = current.get_value()
            
            self.x_loc_val = x_loc.get_value()
            self.y_loc_val = y_loc.get_value()
            self.z_loc_val = z_loc.get_value()
            
            self.x_velocity_val = x_velocity.get_value()
            self.y_velocity_val = y_velocity.get_value()
            self.z_velocity_val = z_velocity.get_value()
            
            self.x_torque_val = x_torque.get_value()
            self.y_torque_val = y_torque.get_value()
            self.z_torque_val = z_torque.get_value()
            
            self.x_errcode_val = x_errcode.get_value()
            self.y_errcode_val = y_errcode.get_value()
            self.z_errcode_val = z_errcode.get_value()
            
            x_axis_list.append(self.x_axis_val)
            y_axis_list.append(self.y_axis_val)
            z_axis_list.append(self.z_axis_val)
            current_list.append(self.current_val)
            
            x_velocity_list.append(self.x_velocity_val)
            y_velocity_list.append(self.y_velocity_val)
            z_velocity_list.append(self.z_velocity_val)
            
            x_torque_list.append(self.x_torque_val)
            y_torque_list.append(self.y_torque_val)
            z_torque_list.append(self.z_torque_val)
            
            localtime = time.localtime()
            result = time.strftime("%H:%M:%S", localtime)

            self.time_fig.append(result)

            if len(self.xlabel) <= 10:
                self.xlabel.append(i)
                i += 1
                
            else:
                x_axis_list.pop(0) #刪除陣列中的第一個元素
                y_axis_list.pop(0)
                z_axis_list.pop(0)
                current_list.pop(0)
                
                x_velocity_list.pop(0)
                y_velocity_list.pop(0)
                z_velocity_list.pop(0)
                
                x_torque_list.pop(0)
                y_torque_list.pop(0)
                z_torque_list.pop(0)
                
                self.time_fig.pop(0)
                
            self.Data_Analysis() #數據分析，並將結果寫入資料庫
                
            Spindle_Dashborad = self.Draw_Dashborad("Spindle", [x_axis_list, y_axis_list, z_axis_list, current_list], self.s_background)
            X_Dashborad = self.Draw_Dashborad("X-Axis", [x_velocity_list, x_torque_list, self.x_loc_val, self.x_errcode_val], self.x_background)
            Y_Dashborad = self.Draw_Dashborad("Y-Axis", [y_velocity_list, y_torque_list, self.y_loc_val, self.y_errcode_val], self.y_background)
            Z_Dashborad = self.Draw_Dashborad("Z-Axis", [z_velocity_list, z_torque_list, self.z_loc_val, self.z_errcode_val], self.z_background)
            
            try:
                self.queue.put({"S": self.convert2glimg(Spindle_Dashborad), "X": self.convert2glimg(X_Dashborad), "Y": self.convert2glimg(Y_Dashborad), "Z": self.convert2glimg(Z_Dashborad)}, timeout = 0.01)
            except: pass
        server.stop()
