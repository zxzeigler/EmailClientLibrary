# EmailClient

To Use Demo Applications:

After download create new Solution.
In solution add each existing project.

(NOTE: EmailClient must be added to solution and compiled before Console and API applications can add reference for EmailClient DLL)
After all projects have been added right-click Console and API projects: 
    Select Add -> Project Reference -> Check EmailClient -> Click "OK"
    
Before running Console and/or API update each project's respective appsettings.json with the appropriate information of your smtp email and machine environment
      Recommended update:
               -"SMTP_Host_Address",
               -"Port",
               -"Use_TLS",
               -"Username",
               -"Password",
               -"Sender_Name",
               -"Send_Attempt_Count",
               -"Resend_Delay_Milliseconds",
               -"Log_File_Path",
               -"Log_File_Prefix",
               -"Log_File_Suffix"
               
To use the API for demo recomended to build requests using swagger
Copy request URL from swagger and change https to http and update listening port

NOTE: When sending request in postman "Enable SSL certificate verification" setting must be disabled.


Credit to Knowledge Sources:
    https://www.youtube.com/watch?v=J0EVd5HbtUY
    https://www.youtube.com/watch?v=PvO_1T0FS_A
    https://www.youtube.com/watch?v=9v6RENPk5iM
    https://www.youtube.com/watch?v=KB3FVgGpZbk
    https://www.youtube.com/watch?v=5tYSO5mAjXs
    https://www.youtube.com/watch?v=eRJFNGIsJEo
