# AR_Surveillance_and_Remote_Maintenance_System_(CNC)
本分支儲存了CNC三軸加工機之控制程式，主要用以控制CNC以及擷取並發送CNC感測數據至霧節點，以下將詳述如何安裝本程式以及CNC基礎控制。如需開發其他功能請參閱[工研院EPCIO使用手冊](https://www.epcio.com.tw/support/UserManual.aspx)。

## 程式安裝
本程式將安裝於**CNC三軸加工機的控制電腦**中，並搭配安裝於CNC控制電腦中的RTX運行，因一般電腦並無安裝RTX，所以無法運行本程式。安裝步驟如下所述。

1. 從Github下載程式，步驟如下圖所示。
![Github下載程式](https://user-images.githubusercontent.com/77768660/189030424-672c1110-4a40-4c50-9e85-15ed9c471914.png)

2. 解壓縮後將其部署於CNC控制電腦中，即可運行。


## CNC基礎控制
本部分將利用本程式所開發之介面對CNC三軸加工機進行控制，介面功能如下圖所示，並於以下逐一敘述個功能之使用方法。
![CNC控制介面](https://user-images.githubusercontent.com/77768660/189040822-2e389ff9-ddcc-4ee1-904d-2fb3a37287c6.png)

* **系統啟動：** 在開啟介面之後，按下System Start後才可以開始控制CNC，此按鈕同時綁定Sover On功能，所以按下去之後會聽到「瘩」一聲，代表啟動成功，就可以開始控制CNC了。
* **原點復歸：** 在CNC成功啟動後，建議先進行原點復歸功能，使CNC位置歸零，在開始進行各項功能控制。
* **移至換刀點：** 當CNC需要更換刀具時，可按下此按鈕，讓X軸與Z軸移動至CNC最左側，以方便進行換刀作業。
* **各軸吋動：** 先透過Axis更換移動的軸向，接著透過Speed設定移動速度，最後可透過JOG+與JOG-控制各軸往正方向與負方向移動。
* **數據監控：** 顯示CNC各項數值，包括各軸位置、錯誤代碼、速度、轉矩、震動等感測數據，其中**Connect**按鈕可啟動OPC UA功能，開始將數據傳送到霧節點。
* **其他功能：** 主要為測試時所使用的功能，如test為模擬加工，STOP為暫停。
