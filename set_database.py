# -*- coding: utf-8 -*-
"""
Created on Tue Jun 14 23:02:17 2022

@author: vf199
"""

import pyodbc

server = '127.0.0.1' 
port = '1433'
username = 'sa' 
password = 'password' 
cnxn = pyodbc.connect(driver='{ODBC Driver 17 for SQL Server}', server=server + ',' + port, user=username, password=password, autocommit=True)
cursor = cnxn.cursor()

#創建霧節點資料庫
cursor.execute("CREATE DATABASE Fog_Database;") #創建一個資料庫
cursor.commit()

cursor.execute('''
               USE Fog_Database;
               CREATE TABLE Vertex_Specifiation (
                   number INT IDENTITY(1,1),
                   type VARCHAR(10),
                   file_name VARCHAR(20),
                   PRIMARY KEY(number)
                   );
               ''') #創建Table Vertex_Specifiation
cursor.commit()

cursor.execute('''
               USE Fog_Database;
               CREATE TABLE CNC_Sensing_Data (
                   number INT IDENTITY(1,1),
                   time DATETIME,
                   x_vibration DECIMAL(6,4),
                   y_vibration DECIMAL(6,4),
                   z_vibration DECIMAL(6,4),
                   s_current DECIMAL(6,4),
                   x_loc DECIMAL(5,2),
                   y_loc DECIMAL(5,2),
                   z_loc DECIMAL(5,2),
                   x_velocity INT,
                   y_velocity INT,
                   z_velocity INT,
                   x_torque DECIMAL(5,3),
                   y_torque DECIMAL(5,3),
                   z_torque DECIMAL(5,3),
                   x_errcode INT,
                   y_errcode INT,
                   z_errcode INT,
                   Alarm VARCHAR(100),
                   PRIMARY KEY(number)
                   );
               ''') #創建Table CNC_Sensing_Data
cursor.commit()

cursor.execute('''
               USE Fog_Database;
               CREATE TABLE Operate_Log (
                   number INT IDENTITY(1,1),
                   time DATETIME,
                   file_name VARCHAR(20),
                   tag VARCHAR(100),
                   PRIMARY KEY(number)
                   );
               ''') #創建Table Operate_Log
cursor.commit()

#創建雲端資料庫
cursor.execute("CREATE DATABASE Cloud_Database;") #創建一個資料庫
cursor.commit()

cursor.execute('''
               USE Cloud_Database;
               CREATE TABLE Vertex_Specifiation (
                   number INT IDENTITY(1,1),
                   type VARCHAR(10),
                   file_name VARCHAR(20),
                   PRIMARY KEY(number)
                   );
               ''') #創建Table Vertex_Specifiation
cursor.commit()

cursor.execute('''
               USE Cloud_Database;
               CREATE TABLE CNC_Sensing_Data (
                   number INT IDENTITY(1,1),
                   time DATETIME,
                   x_vibration DECIMAL(6,4),
                   y_vibration DECIMAL(6,4),
                   z_vibration DECIMAL(6,4),
                   s_current DECIMAL(6,4),
                   x_loc DECIMAL(5,2),
                   y_loc DECIMAL(5,2),
                   z_loc DECIMAL(5,2),
                   x_velocity INT,
                   y_velocity INT,
                   z_velocity INT,
                   x_torque DECIMAL(5,3),
                   y_torque DECIMAL(5,3),
                   z_torque DECIMAL(5,3),
                   x_errcode INT,
                   y_errcode INT,
                   z_errcode INT,
                   Alarm VARCHAR(100),
                   PRIMARY KEY(number)
                   );
               ''') #創建Table CNC_Sensing_Data
cursor.commit()

cursor.execute('''
               USE Cloud_Database;
               CREATE TABLE Operate_Log (
                   number INT IDENTITY(1,1),
                   time DATETIME,
                   file_name VARCHAR(20),
                   tag VARCHAR(100),
                   PRIMARY KEY(number)
                   );
               ''') #創建Table Operate_Log
cursor.commit()

#斷開連線、釋放資源
cursor.close()
cnxn.close()
