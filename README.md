Install Project
===============================
[ ] git clone http://cptsvs551/016623/MasterCore8_V2022.git

OR

[ ] git clone http://{user}@cptsvs551/016623/MasterCore8_V2022.git 

[ ] cd MasterCore8_V2022

[ ] dotnet restore 

[ ] dotnet watch run

Copy & New Project ( Example Change Project Name To ICD_PRO2022 )
===============================
[ ] Create Git Repository ICD_PRO2022

[ ] CMD: git clone http://cptsvs551/016623/MasterCore8_V2022.git ICD_PRO2022

[ ] CMD: cd ICD_PRO2022

[ ] git remote set-url origin http://{GITUSERNAME}@cptsvs551/016623/ICD_PRO2022.git

[ ] CMD: code .

[ ] Open VSCode Search (Match Case)  *MasterCore8* Replace *ICD_PRO* All File

[ ] Rename *MasterCore8.csproj* to *ICD_PRO.csproj*

[ ] VSCode (Ctrl + Shift + P) and execute the command >Reload Window

[ ] dotnet restore -f

[ ] Create Database & Edit Connection Database in appsettings.json, appsettings.Production.json

[ ] dotnet ef database update

[ ] dotnet watch run

[ ] git add .

[ ] git commit -m "change project name to ICD_PRO2022"

[ ] git push -u origin main


