# DocumentUploadToBlob

This project is to Get, Upload, download & delete pdf file on Blob.

Azure storage Emulator is used to develop & test the functionality locally.

Steps to use:

1. launch the Azure Storage Emulator on local machine. if not exist download emulator and launch.
2. Download the code base and build the solution on Visual studio 2019
3. Click Run the project
4. Use below url to tset:

List Documents:
  Http Method: GET
  Url: https://localhost:44363/api/Documents

Upload Document:
  Http Method: POST
  Url: https://localhost:44363/api/Documents
  Parameter: fileName, Path
    
Download Document:
  Http Method: GET
  Url: https://localhost:44363/api/Documents/download
  Parameter: fileName, Path
  

delete Document:
  Http Method: DELETE
  Url: https://localhost:44363/api/Documents
  Parameter: fileName
  
