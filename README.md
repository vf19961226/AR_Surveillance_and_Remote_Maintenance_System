# AR_Surveillance_and_Remote_Maintenance_System_(Fog_Layer)
本分支儲存了系統部署於霧節點之所有模組程式碼，模組根據功能不同可分為Socket伺服器、影像分析、影像渲染、數據處理等4個部分，並搭配Microsoft SQL Server 2019資料庫進行運作，其中模組主要以Python程式語言搭配各項套件進行開發。以下將詳述如何安裝本程式。

## 程式開發環境
1. 電腦系統實作環境

|**項目**|**版本**|
|:---:|:---:|
|**Windows 11 Education**|21H2|
|**Anaconda**|4.10.3|
|**pip**|21.2.2|
|**Python**|3.8.12|
|**Microsoft SQL Server 2019**|15.0.2000.5(X64)|

2. 安裝的Python套件包

|**項目**|**版本**|**安裝指令**
|:---:|:---:|:---:
|**OpenCV**|4.5.5|conda install -c conda-forge opencv
|**OpenGL**|3.1.1a1|conda install -c anaconda pyopengl
|**NumPy**|1.22.3|（安裝OpenCV時已附帶安裝）
|**Pygame**|2.1.2|pip install pygame
|**Pillow**|9.0.1|conda install -c anaconda pillow
|**Pyodbc**|4.0.32|conda install -c anaconda pyodbc|

## 程式安裝
本程式將安裝於霧節點伺服器中，其中霧節點伺服器建議選用安裝有GPU之伺服器或電腦，以利圖形運算更加迅速。本程式之安裝步驟如下所述。

1. 從Github下載程式，步驟如下圖所示。
![下載程式碼](https://user-images.githubusercontent.com/77768660/189566714-50e75006-e8e9-45b6-838e-0de4245237ae.png)

2. 將其進行解壓縮後，開啟Anaconda建立新環境，可於Base環境的終端機（Terminal）中依序輸入以下指令，即可完成本程式之Pyhon運行環境建立。
    1. 創建新環境：`conda create -name Fog_Layer python=3.8`
    2. 啟動剛剛創建的新環境：`activate Fog_Layer`
    3. 將路徑指向解壓縮完畢的程式資料夾：`cd C:\Users\vf199\Download\AR_Surveillance_and_Remote_Maintenance_System-Fog_Layer`，**路徑請自行依電腦設定進行修改**。
    4. 安裝所需套件：`pip install -r requirements.txt`

3. 安裝**Microsoft SQL Server 2019**資料庫，詳細安裝步驟可參考《[Windows Server 如何安裝 SQL Server 2019 免費開發版](https://blog.hungwin.com.tw/windows-server-sql-server-2019-install/)》，安裝完成後可使用[**set_database.py**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/set_database.py)對資料庫進行設定，但在使用前須先更改[第10行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/set_database.py#L10)至[第13行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/set_database.py#L13)之參數，如下所示，將其更改為新安裝之資料庫的相應參數，最終可直接於上步驟新建立之Anaconda環境中以終端機輸入`python set_database.py`指令進行設定。

    ```python
    server = '127.0.0.1'  #安裝有Microsoft SQL Server 2019之電腦的IP
    port = '1433' #安裝有Microsoft SQL Server 2019之電腦對應至SQL Server之連接阜（1433為SQL Server預設）
    username = 'sa' #Microsoft SQL Server 2019之資料庫管理員使用者名稱（sa為SQL Server預設）
    password = 'password' #Microsoft SQL Server 2019之資料庫管理員使用者密碼
    ```

4. 修改程式中的IP以及資料庫密碼，需修改檔案與行數如下所述。
    * [**main.py**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/main.py)
        1. [112行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/main.py#L112)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7000`更改為霧節點相對應之連接阜。
        2. [113行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/main.py#L113)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7001`更改為霧節點相對應之連接阜。
        3. [114行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/main.py#L114)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7002`更改為霧節點相對應之連接阜。
        4. [117行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/main.py#L117)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`4840`更改為霧節點相對應之OPC UA連接阜，`7004`更改為霧節點相對應之Socket Server連接阜。
        5. [118行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/main.py#L118)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7003`更改為霧節點相對應之連接阜。
  
    * [**Socket_Server.py**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/Socket_Server.py)
        1. [159行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/Socket_Server.py#L159)，將`password`更改為SQL Server之密碼，其他如連接阜（1433，[157行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/Socket_Server.py#L157)）或使用者名稱（sa，[158行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/Socket_Server.py#L158)）如有變動再自行修改。
  
    * [**Data_Processing.py**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/Data_Processing.py)
        1. [191行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Fog_Layer/Data_Processing.py#L191)，將其中之`password`更改為SQL Server之密碼，其他如連接阜（1433）或使用者名稱（sa）如有變動再自行修改。

5. 使用`python main.py`指令輸入新建立之Anaconda環境的終端機，用以確認程式是否正常運行，若正常運行將可看到如下圖之運行畫面。
![Fog_Layer執行畫面](https://user-images.githubusercontent.com/77768660/188840502-a5e55221-cabc-47bd-8e41-17c2a829326d.png)
