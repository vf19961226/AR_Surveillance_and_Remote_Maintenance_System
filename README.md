# AR_Surveillance_and_Remote_Maintenance_System_(AR_Device)
本分支儲存了部署於AR設備之AR檢視器程式，在系統中我們以Microsoft Surface Pro 7平板電腦作為AR設備，並於其中以C＃ .NET架構開發Winform應用程式，實現影像收發與語音通話功能，以下將詳述如何安裝本程式以及AR檢視器介面之基本操作。

## 程式開發環境
1. AR設備系統開發環境（[Surface Pro 7 平板電腦](https://www.microsoft.com/zh-tw/surface/devices/surface-pro-7#techspecs)）

    |**項目**|**版本**|
    |:---:|:---:|
    |**Windows 10 Home**|20H2|
    |**Visual Studio 2017**|15.9.23|
    |**.NET**|4.7.2|

2. 安裝的NuGet套件

    |**項目**|**版本**|**安裝指令**
    |:---:|:---:|:---:
    |**OpenCvSharp4**|4.5.5.20211231|（直接由NuGet套件管理頁面安裝）
    |**OpenCvSharp4.Extensions**|4.5.5.20211231|（直接由NuGet套件管理頁面安裝）
    |**OpenCvSharp4.runtime.win**|4.5.5.20211231|（直接由NuGet套件管理頁面安裝）

## 程式安裝
本程式將安裝於AR設備（Microsoft Surface Pro 7平板電腦）上，須注意設備上是否有鏡頭與麥克風等感測裝置，用以擷取現場影像與音訊。安裝步驟如下所述。

1. 從Github下載程式，步驟如下圖所示。
![image](https://user-images.githubusercontent.com/77768660/189074875-4b47ccd4-b389-40bd-afaa-330b314ae958.png)

2. 解壓縮後修改程式中的IP以及資料庫密碼，需修改檔案與位置如下所述。
    * [**Form1.cs**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/Form1.cs)
        1. [67行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/Form1.cs#L67)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7000`更改為霧節點相對應之AR設備影像傳輸連接埠。
        2. [108行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/Form1.cs#L108)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7002`更改為霧節點相對應之語音通話連接埠。
        
    * [**Form2.cs**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/Form2.cs)
        1. [29行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/Form2.cs#L29)，將其中**data source**之`140.116.86.220`更改為SQL伺服器之IP，**user id**更改為SQL伺服器之使用者名稱（sa為預設），**password**更改為SQL伺服器之使用者密碼。
        2. [33行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/Form2.cs#L33)，將其中之`140.116.86.220`更改為霧節點伺服器之IP，`7003`更改為霧節點相對應之接收AR設備選擇XML檔案連接埠。

3. 若有使用5G企業專網需求，則需先將設備接上5G無線網卡，設定方法可參考[5G無線網卡設定（企業專有網路）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/5G%E7%84%A1%E7%B7%9A%E7%B6%B2%E5%8D%A1%E8%A8%AD%E5%AE%9A%EF%BC%88%E4%BC%81%E6%A5%AD%E5%B0%88%E6%9C%89%E7%B6%B2%E8%B7%AF%EF%BC%89.md)。

4. 將其部署於CNC控制電腦中，控制介面可透過開啟[Surface_AR_Viewer.sln](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer.sln)專案檔以Visual Studio進行編譯並開啟，或是透過[Surface_AR_Viewer.exe](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/bin/Debug/Surface_AR_Viewer.exe)執行檔開啟，其路徑為`Surface_AR_Viewer/bin/Debug/Surface_AR_Viewer.exe`。

## AR檢視器介面基本操作
本部分將介紹AR檢視器介面之基本操作，介面右上方有四個按鈕，分別代表連線至霧節點、與霧節點斷開連線、語音通話、檢視歷史數據，如下圖所示，以下將逐一介紹個按鈕功能。

![AR檢視器介面](https://user-images.githubusercontent.com/77768660/189081715-39b23b0e-86c7-49a1-a31d-c998769473cf.png)

* **連線至霧節點：** 按下此按鈕後AR設備將與霧節點建立連線，開始擷取相機影像並傳送至霧節點，同時接收霧節點回傳渲染完成的AR影像。
* **與霧節點斷開連線：** 按下此按鈕後AR設備將與霧節點斷開連線，並關閉程式。按下此按鈕前須先注意AR設備是否與霧節點為連線狀態，否則將會報錯。
* **語音通話：** 當需要請求遠端專家協助時，可按下此按鈕，將可與遠端專家進行語音通話，並且顯示介面將由CNC數據監控模式變為遠端標註模式，遠端專家將以語音通話輔以遠端標註傳達協作指令。當協作任務結束後，再按一下此按鈕即可退出遠端標註模式，回復到CNC數據監控模式繼續監控CNC狀態。
* **檢視歷史數據：** 按下此按鈕後將顯示歷史數據檢視介面，可查看CNC歷史感測數據以及遠端標註之歷史數據，並可透過介面右上方切換CNC歷史數據與遠端標註數據頁面，如下圖所示。

![AR檢視器介面 - 歷史數據](https://user-images.githubusercontent.com/77768660/189082435-27b218a1-186b-40b9-896c-abbc8e71b7bc.png)
