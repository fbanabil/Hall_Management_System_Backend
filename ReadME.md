**Use Appsettings:**


{

  "ConnectionStrings": {

    "DefaultConnection": "Your Connection String"

  },

  "Logging": {

    "LogLevel": {

    "Default": "Information",

    "Microsoft.AspNetCore": "Warning"

    }

  },

  "AllowedHosts": "*",

  "AppSettings": {

    "PasswordKey": "Use Random String As passwordKey",

    "TokenKey": "Use Random String As TokenKey"

  },

  "SmtpSettings": {

    "Server": "smtp.gmail.com",

    "Port": 587,

    "SenderName": "Set it",

    "SenderEmail": "Email to be used",

    "Username": "Any user name,

    "Password": "App specific password"

  }

}


Be sure cors policy and database is set up accurately.

Use "dotnet run" cmd to run.
