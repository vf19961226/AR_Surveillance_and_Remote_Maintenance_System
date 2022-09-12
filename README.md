# AR_Surveillance_and_Remote_Maintenance_System_(Remote_Platform)
本分支儲存了遠端專家用於遠端協作的遠端平台，該平台以Unity遊戲引擎進行開發，並於其中以C＃ .NET腳本建立各項功能，以實現如遠端標註、語音通話、CNC姿態模擬、查看歷史數據等功能，以下將詳述如何安裝本程式以及遠端平台之基本操作。

## 程式開發環境
|**項目**|**版本**|
|:---:|:---:|
|**Windows 11 Education**|21H2|
|**Visual Studio Professional 2019**|16.11.8|
|**Unity**|2019.4.39f1|
|**NuGetForUnity**|3.0.5|
|**OpenCvSharp**|4.6.0.20220608|

## 程式安裝
本程式需使用Unity平台進行開啟，Unity運行環境如程式開發環境所述，並且須注意設備是否有安裝麥克風等感測裝置，用以與現場人員進行遠端通話。其安裝步驟如下所述。

1. 從Github下載程式，並解壓縮至資料夾中，步驟如下圖所示。
![下載程式碼](https://user-images.githubusercontent.com/77768660/189569302-89d52e82-2675-41cd-a2fb-e58ba5ece845.png)

2. 開啟Unity Hub檢查授權，若授權已到期，則需手動更新授權，可直接點選**同意並取得個人版授權**，如下圖所示。
![Unity授權更新](https://user-images.githubusercontent.com/77768660/189570891-73c1c76b-d103-4b7a-b61e-159cbcaa76ea.png)

3. 開啟**專案**頁面，並選擇右上方**開啟**，選擇第一步驟解壓縮完畢的資料夾，最後選擇欲使用的Unity版本，即可開啟本程式，步驟如下圖所示。
![開啟專案](https://user-images.githubusercontent.com/77768660/189570068-c0cfc4c1-1b7f-4498-bf15-0243f06ccd16.png)

4. 開啟程式後，切換至**Game**頁面，並點選中間上方的**三角形播放鍵**進入運行模式，如下圖所示，即可使用程式中的各項功能。
4. 修改程式中的IP以及資料庫密碼，需修改物件、檔案與位置如下所述。
    * **Main Camera（Unity物件）**
        1. **Socket_TCP_Client**腳本中的**Server_IP**與**Server_Port**，需修改為對應霧節點中的遠端平台AR畫面監控傳輸設定（IP與連接埠）。
        2. **CNC_Loc_Sync**腳本中的**Server_IP**與**Server_Port**，需修改為對應霧節點中的CNC姿態模擬模型位置傳輸設定（IP與連接埠）。
        
    * **Canvas→Data_btn（Unity物件）**
        1. **DB_Panel_Show**腳本中的**SQL_Server_IP**、**User_ID**與**Password**，需修改為對應資料庫的IP位置及其使用者名稱與密碼。
        
    * [**Form1.cs（Voice_Chat2）**](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Remote_Platform/Voice_Chat2/Voice_Chat2/Form1.cs)
        1. [101行](https://github.com/vf19961226/AR_Surveillance_and_Remote_Maintenance_System/blob/Remote_Platform/Voice_Chat2/Voice_Chat2/Form1.cs#L101)，將其中的IP`140.116.86.220`與連接埠`7002`更改為相對應霧節點中的語音通話功能設定（IP與連接埠），並重新編譯執行一次。

5. 開啟程式後，切換至**Game**頁面，並點選中間上方的**三角形播放鍵**進入運行模式，如下圖所示，即可使用程式中的各項功能。
![進入運行模式](https://user-images.githubusercontent.com/77768660/189570393-c40906a3-3c83-4b7c-bc0c-cadb54b2cbe9.png)

## 遠端平台基本操作
本部分將介紹遠端平台之基本操作，介面可劃分為標註模型編輯模式、傳送標註結果、CNC姿態模擬模型、現場人員AR影像、新增標註模型、遠端通話模組、查看歷史數據等7個部分，如下圖所示，以下將逐一進行敘述。
![遠端平台](https://user-images.githubusercontent.com/77768660/189480329-20eec532-a492-472d-957a-ca66b33aa2a4.png)

* **標註模型編輯模式：** 在編輯模式中由左至右分為移動、旋轉、縮放、刪除等4種模式，其中使用移動、旋轉、縮放等3種模式時，會顯示如下圖之把手進行輔助，點選相對軸向之把手可讓使用者透過滑鼠拖曳的方式對標註模型進行編輯。
![模型編輯模式輔助把手](https://user-images.githubusercontent.com/77768660/189488704-d0bc4a1a-9258-4f2a-aabe-0a579d7a7984.png)

* **傳送標註結果：** 將讀取目前已生成的標註模型位置、旋轉量、縮放比例等資訊，並以XML格式傳送至霧節點進行渲染，使現場人員可透過AR設備觀看遠端專家的標註結果，同時將XML檔案儲存於資料庫中，以方便後續重複調用。

* **CNC姿態模擬模型：** 接收來自霧節點轉發的CNC三軸位置，將其經過座標轉換之後，將CNC姿態模擬模型中的各軸位置進行相對應之移動，使遠端專家可透過本模型確認CNC目前姿態，並可將標註模型正確的標註於相應位置上。

* **現場人員AR影像：** 使用Raw Image物件將顯示霧節點回傳的影像，使遠端專家可同步查看現場人員AR設備所顯示的影像，一方面可確認現場人員機台操作是否正確，另一方面可確認標註物件是否正確渲染。

* **新增標註模型：** 當現場人員提出協助請求的時候，遠端專家可根據欲傳達的資訊選擇相對應之模型進行輔助，使現場人員在資訊理解上更加容易，並降低資訊理解的歧異。模型種類由上至下分別為箭頭、手勢、注意、白板，標註模型詳細如下表所示。

  |**編號**|**名稱**|**圖示**|**使用方法**
  |:---:|:---:|:---:|:---
  |1|箭頭|![箭頭](https://user-images.githubusercontent.com/77768660/189489141-797a1940-354d-48f0-a73e-4d35e978da5e.png)|表示實體物件所在位置。
  |2|手勢|![手勢](https://user-images.githubusercontent.com/77768660/189489177-ecb13514-9681-4752-97e5-0b16fb1e04ea.png)|表示現場人員進行操作時特定的手部姿勢。
  |3|注意|![注意](https://user-images.githubusercontent.com/77768660/189489199-b0da8d8d-f887-4674-865d-d295b2964c08.png)|提醒現場人員須提高警覺。
  |4|白板|![白板](https://user-images.githubusercontent.com/77768660/189489224-d1da2703-cc76-4e04-8558-faa36cc4de36.png)|顯示文字訊息與簡圖，以彌補標註之空間限制。

* **遠端通話模組：** 本模組使用C＃程式語言開發的WinForm介面，採用呼叫外部執行檔的方式執行，如下圖所示，其中按下**Call**按鈕可與現場人員建立通話連線，按下**Cancel**可斷開通話。使現場人員可與遠端專家進行語音通話，以降低對標註或是操作理解的歧異。    
![遠端通話模組](https://user-images.githubusercontent.com/77768660/189489310-009e4c98-af72-446e-b172-2b5206d16b59.png)

* **查看歷史數據：** 透過讀取雲端資料庫中的CNC感測數據與遠端標註結果，並將其顯示於平面（Panel）物件上，方便遠端專家查看CNC感測數據與過去遠端標註結果，並可透過該介面上之鍵號切換CNC感測數據與遠端標註資料頁面，如下圖所示。其中在操作紀錄頁面可選擇過去遠端標註結果並匯入，以節省重複標註的時間。
![歷史數據介面](https://user-images.githubusercontent.com/77768660/189489508-c089923e-ac7b-41c2-b300-cb7368fbcb82.png)
