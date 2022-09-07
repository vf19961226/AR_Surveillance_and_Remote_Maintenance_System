# AR_Surveillance_and_Remote_Maintenance_System
將AR系統中的影像處理運算部署於霧節點中，以5G網路確保AR設備與霧節點間的影像傳輸延遲，藉此解決AR硬體運算效能不足問題，並提升AR系統的可擴展性。並以此為基礎發展AR監控與遠端維修系統，以霧節點為中繼站整合與分析來自CNC機台、遠端維修平台等數據於AR系統中。使現場人員可透過AR設備監控CNC狀態與接收遠端專家之維修指示，使現場人員可快速發現CNC異常並即時排除，以降低臨時停機所造成之額外成本。本研究以5G進行影像傳輸，以確保延遲低於20毫秒，且相較於AR設備本地端之影像處理運算效能至少提升28％，以此為基礎開發之監控系統以209毫秒之頻率更新AR可視化數據，以本系統進行維修輔助測試後發現對於不熟悉CNC維護任務之使用者達成 30％之維修效率提升，上述成果證明本研究之AR遠端維修系統架構於工業場域之效益與可行性，並為AR導入工業應用提供一種解決方案。

## 系統架構
本研究提出結合5G與霧運算之AR監控與遠端維修系統，並以CNC三軸加工機為例進行系統開發。本系統使用霧運算技術運行數據分析、AR演算法等延遲敏感應用，並透過5G通訊技術進行邊緣與霧層的資料傳遞，系統架構如下圖所示。系統程式可依據部署位置分為邊緣層（Edge Layer）、霧層（Fog Layer）、雲層（Cloud Layer），其中邊緣層依據設備分為AR設備（AR Device）與CNC，並將程式以部署位置之不同儲存於本Github專案之不同分支（Branches），以下將對各分支進行詳細說明。

![系統架構](https://user-images.githubusercontent.com/77768660/188812957-9fcc322c-7992-4b1e-9bc2-31a26ecb8265.png)

### 紀錄文件（[Recode_Document](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System)）
本分支為本專案之預設分支，主要儲存系統實作記錄文檔，並講解系統之程式架構與使用方法。

### CNC三軸加工機（[CNC](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/tree/CNC)）
儲存CNC三軸加工機之控制程式，包含了維修測試功能，以及Opc UA Client與感測資料擷取之功能，詳細可參考[2022/05/14 CNC感測資料擷取與可視化（1） - OpcUA Client資料傳輸](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_14%20CNC%E6%84%9F%E6%B8%AC%E8%B3%87%E6%96%99%E6%93%B7%E5%8F%96%E8%88%87%E5%8F%AF%E8%A6%96%E5%8C%96%EF%BC%881%EF%BC%89.md#opcua-client%E8%B3%87%E6%96%99%E5%82%B3%E8%BC%B8)。

### AR設備（[AR Device](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/tree/AR_Device)）
儲存部署於AR設備上之AR檢視器（AR Viewer）程式，該程式以C＃ .NET架構以Winform應用程式進行開發，用於擷取現場影像、與霧節點進行影像交換、顯示霧節點渲染完成之AR影像以及與遠端專家進行通話等功能。詳細可參考[2022/05/19 CNC感測資料擷取與可視化（2） - AR系統人機介面](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_19%20CNC%E6%84%9F%E6%B8%AC%E8%B3%87%E6%96%99%E6%93%B7%E5%8F%96%E8%88%87%E5%8F%AF%E8%A6%96%E5%8C%96%EF%BC%882%EF%BC%89.md#ar%E7%B3%BB%E7%B5%B1%E4%BA%BA%E6%A9%9F%E4%BB%8B%E9%9D%A2)、[2022/06/18 霧節點伺服器建置（2） - 資料庫查看介面](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_18%20%E9%9C%A7%E7%AF%80%E9%BB%9E%E4%BC%BA%E6%9C%8D%E5%99%A8%E5%BB%BA%E7%BD%AE%EF%BC%882%EF%BC%89.md#%E8%B3%87%E6%96%99%E5%BA%AB%E6%9F%A5%E7%9C%8B%E4%BB%8B%E9%9D%A2)與[2022/06/14 遠端維修平台建置（2） - AR設備](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_14%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%882%EF%BC%89.md#ar%E8%A8%AD%E5%82%99)（語音通話）。

### 霧層（[Fog_Layer](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/tree/Fog_Layer)）
儲存霧層各功能模組之程式，主要以Python程式語言進行開發，包含了數據處理（Data Processing）、影像分析（Image Analysis）、影像渲染（Image Shading）、Socket伺服器（Socket Server）等功能模組，詳細可參考以下文檔。

#### 影像分析
1. [2022/04/26 單機實現AR演算法（1）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_04_26%20%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEAR%E6%BC%94%E7%AE%97%E6%B3%95%EF%BC%881%EF%BC%89.md#20220426-%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEar%E6%BC%94%E7%AE%97%E6%B3%951)
2. [2022/04/27 單機實現AR演算法（2）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_04_27%20%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEAR%E6%BC%94%E7%AE%97%E6%B3%95%EF%BC%882%EF%BC%89.md#20220427-%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEar%E6%BC%94%E7%AE%97%E6%B3%952)
3. [2022/05/31 霧節點伺服器建置（1） - 影像分析](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_31%20%E9%9C%A7%E7%AF%80%E9%BB%9E%E4%BC%BA%E6%9C%8D%E5%99%A8%E5%BB%BA%E7%BD%AE%EF%BC%881%EF%BC%89.md#%E5%BD%B1%E5%83%8F%E5%88%86%E6%9E%90image-analysis)

#### 影像渲染
1. [2022/04/29 單機實現AR演算法（3）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_04_29%20%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEAR%E6%BC%94%E7%AE%97%E6%B3%95%EF%BC%883%EF%BC%89.md#20220429-%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEar%E6%BC%94%E7%AE%97%E6%B3%953)
2. [2022/05/05 單機實現AR演算法（4）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_05%20%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEAR%E6%BC%94%E7%AE%97%E6%B3%95%EF%BC%884%EF%BC%89.md#20220505-%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEar%E6%BC%94%E7%AE%97%E6%B3%954)
3. [2022/05/08 單機實現AR演算法（5）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_08%20%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEAR%E6%BC%94%E7%AE%97%E6%B3%95%EF%BC%885%EF%BC%89.md#20220508-%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEar%E6%BC%94%E7%AE%97%E6%B3%955)
4. [2022/05/25 單機實現AR演算法（6）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_25%20%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEAR%E6%BC%94%E7%AE%97%E6%B3%95%EF%BC%886%EF%BC%89.md#20220525-%E5%96%AE%E6%A9%9F%E5%AF%A6%E7%8F%BEar%E6%BC%94%E7%AE%97%E6%B3%956)
5. [2022/05/31 霧節點伺服器建置（1） - 影像渲染](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_31%20%E9%9C%A7%E7%AF%80%E9%BB%9E%E4%BC%BA%E6%9C%8D%E5%99%A8%E5%BB%BA%E7%BD%AE%EF%BC%881%EF%BC%89.md#%E5%BD%B1%E5%83%8F%E6%B8%B2%E6%9F%93image-shading)
6. [2022/06/22 遠端維修平台建置（3）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_22%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%883%EF%BC%89.md)（遠端標註_渲染）

#### 數據處理
1. [2022/05/14 CNC感測資料擷取與可視化（1） - OpcUA Server資料蒐集與可視化](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_14%20CNC%E6%84%9F%E6%B8%AC%E8%B3%87%E6%96%99%E6%93%B7%E5%8F%96%E8%88%87%E5%8F%AF%E8%A6%96%E5%8C%96%EF%BC%881%EF%BC%89.md#opcua-server%E8%B3%87%E6%96%99%E8%92%90%E9%9B%86%E8%88%87%E5%8F%AF%E8%A6%96%E5%8C%96)
2. [2022/05/31 霧節點伺服器建置（1） - 數據處理](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_31%20%E9%9C%A7%E7%AF%80%E9%BB%9E%E4%BC%BA%E6%9C%8D%E5%99%A8%E5%BB%BA%E7%BD%AE%EF%BC%881%EF%BC%89.md#%E6%95%B8%E6%93%9A%E8%99%95%E7%90%86data-processing)
3. [2022/06/18 霧節點伺服器建置（2） - CNC感測資料分析](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_18%20%E9%9C%A7%E7%AF%80%E9%BB%9E%E4%BC%BA%E6%9C%8D%E5%99%A8%E5%BB%BA%E7%BD%AE%EF%BC%882%EF%BC%89.md#cnc%E6%84%9F%E6%B8%AC%E8%B3%87%E6%96%99%E5%88%86%E6%9E%90)

#### Socket伺服器
1. [2022/05/19 CNC感測資料擷取與可視化（2） - 霧節點影像處理](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_19%20CNC%E6%84%9F%E6%B8%AC%E8%B3%87%E6%96%99%E6%93%B7%E5%8F%96%E8%88%87%E5%8F%AF%E8%A6%96%E5%8C%96%EF%BC%882%EF%BC%89.md#%E9%9C%A7%E7%AF%80%E9%BB%9E%E5%BD%B1%E5%83%8F%E8%99%95%E7%90%86)
2. [2022/05/31 霧節點伺服器建置（1） - Socket Server](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_05_31%20%E9%9C%A7%E7%AF%80%E9%BB%9E%E4%BC%BA%E6%9C%8D%E5%99%A8%E5%BB%BA%E7%BD%AE%EF%BC%881%EF%BC%89.md#socket-server)
3. [2022/06/14 遠端維修平台建置（2） - 霧節點](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_14%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%882%EF%BC%89.md#%E9%9C%A7%E7%AF%80%E9%BB%9E)（語音通話）
4. [2022/06/22 遠端維修平台建置（3）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_22%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%883%EF%BC%89.md)（遠端標註_接收XML）

### 遠端平台（[Remote_Platform](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/tree/Remote_Platform)）
儲存部署於遠端電腦之遠端平台專案程式，該專案以Unity遊戲引擎搭配C＃ .NET架構進行開發，並於其中實現標註模型編輯模式、傳送標註結果、現場人員AR影像、遠端通話模組、查看歷史數據、CNC姿態模擬模型、新增標註模型等功能，詳細可參考[2022/06/10 遠端維修平台建置（1）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_10%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%881%EF%BC%89.md#20220610-%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE1)、[2022/06/14 遠端維修平台建置（2） - 遠端平台](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_14%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%882%EF%BC%89.md#%E9%81%A0%E7%AB%AF%E5%B9%B3%E5%8F%B0)（語音通話）與[2022/06/22 遠端維修平台建置（3）](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Recode_Document/2022_06_22%20%E9%81%A0%E7%AB%AF%E7%B6%AD%E4%BF%AE%E5%B9%B3%E5%8F%B0%E5%BB%BA%E7%BD%AE%EF%BC%883%EF%BC%89.md)（遠端標註）。

## 使用說明
1. 依據各分支之安裝說明將功能模組部署於相對應之裝置上。
2. 執行部署於霧節點之程式，可直接使用Anaconda啟動相對應環境之終端機（Terminal），並將路徑指向霧節點程式資料夾，接著執行`python main.py`指令，如執行成功將會出現**Image of Augmented Reality**視窗，如下圖所示，但因AR設備尚未連線傳送影像，故該視窗顯示空白畫面。
![Fog_Layer執行畫面](https://user-images.githubusercontent.com/77768660/188840502-a5e55221-cabc-47bd-8e41-17c2a829326d.png)

3. 啟動部署於CNC三軸加工機之CNC控制程式（**EMP-CNC**），並將CNC先進行Servo On與原點復歸，完成後按下介面中的**Connect**按鈕，將CNC之感測資料以Opc UA通訊協議傳送至霧節點，如下圖所示。
![CNC操作介面](https://user-images.githubusercontent.com/77768660/188845369-ad50356e-29d2-494b-ae4a-231787d0cfdb.PNG)

4. 啟動部署於AR設備之AR檢視器，並按下介面中的**Connect**按鈕，即可開始擷取與傳送AR設備之相機影像，並接收與顯示處理完成之AR影像，如下圖所示，若成功進行這時位於霧節點的**Image of Augmented Reality**視窗將顯示渲染影像。
![AR檢視器](https://user-images.githubusercontent.com/77768660/188843393-d5550769-f91c-4f5b-9d6d-9699e73cc97f.png)

5. 啟動部署於遠端電腦之遠端維修介面，並可看到平台中之右上角將同步顯示AR檢視器畫面，如下圖所示。
![遠端維修介面](https://user-images.githubusercontent.com/77768660/188847815-58a2f00b-c199-425d-bc26-9100da643350.jpg)

6. 將AR設備之相機朝向貼在CNC設備上之AR Uco標籤，AR檢視器將顯示CNC感測數據可視化數據，並可使用金屬物品觸碰極限感測器，用以查看數據處理模組是否正常運作，顯示紅色警告於AR檢視器中，如下圖所示。成功後以按下CNC警急停止鈕並解開，用以解除觸碰極限所產生的控制器Error Code。
![AR檢視器之CNC可視化數據警告](https://user-images.githubusercontent.com/77768660/188849689-ca53890e-ed2a-4836-8c9e-5bdaef33e1ef.png)

7. 
