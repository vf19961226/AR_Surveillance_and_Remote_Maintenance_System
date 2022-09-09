# AR_Surveillance_and_Remote_Maintenance_System_(AR_Device)
本分支儲存了部署於AR設備之AR檢視器程式，在系統中我們以Microsoft Surface Pro 7平板電腦作為AR設備，並於其中以C＃ .NET架構開發Winform應用程式，實現影像收發與語音通話功能，以下將詳述如何安裝本程式以及AR檢視器介面之基本操作。

## 程式開發環境
1. 電腦系統開發環境（[Surface Pro 7 平板電腦](https://www.microsoft.com/zh-tw/surface/devices/surface-pro-7#techspecs)）

|**項目**|**版本**|
|:---:|:---:|
|**Windows 10 家用版**|20H2|
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

2. 解壓縮後將其部署於CNC控制電腦中，控制介面可透過開啟[Surface_AR_Viewer.sln](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer.sln)專案檔以Visual Studio進行編譯並開啟，或是透過[Surface_AR_Viewer.exe](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/AR_Device/Surface_AR_Viewer/bin/Debug/Surface_AR_Viewer.exe)執行檔開啟，其路徑為`Surface_AR_Viewer/bin/Debug/Surface_AR_Viewer.exe`。

## AR檢視器介面基本操作
本部分將介紹AR檢視器介面之基本操作，介面右上方有四個按鈕，分別代表連線至霧節點、與霧節點斷開連線、語音通話、檢視歷史數據，如下圖所示，以下將逐一介紹個按鈕功能。

![AR檢視器介面](https://user-images.githubusercontent.com/77768660/189081715-39b23b0e-86c7-49a1-a31d-c998769473cf.png)

* **連線至霧節點：** 按下此按鈕後AR設備將與霧節點建立連線，開始擷取相機影像並傳送至霧節點，同時接收霧節點回傳渲染完成的AR影像。
* **與霧節點斷開連線：** 按下此按鈕後AR設備將與霧節點斷開連線，並關閉程式。按下此按鈕前須先注意AR設備是否與霧節點為連線狀態，否則將會報錯。
* **語音通話：** 當需要請求遠端專家協助時，可按下此按鈕，將可與遠端專家進行語音通話，並且顯示介面將由CNC數據監控模式變為遠端標註模式，遠端專家將以語音通話輔以遠端標註傳達協作指令。當協作任務結束後，再按一下此按鈕即可退出遠端標註模式，回復到CNC數據監控模式繼續監控CNC狀態。
* **檢視歷史數據：** 按下此按鈕後將顯示歷史數據檢視介面，可查看CNC歷史感測數據以及遠端標註之歷史數據，並可透過介面右上方切換CNC歷史數據與遠端標註數據頁面，如下圖所示。

![AR檢視器介面 - 歷史數據](https://user-images.githubusercontent.com/77768660/189082435-27b218a1-186b-40b9-896c-abbc8e71b7bc.png)
