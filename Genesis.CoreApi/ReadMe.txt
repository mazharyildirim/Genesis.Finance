Migration işlemi için

Enable-Migrations -StartUpProjectName Genesis.CoreApi -ContextTypeName Genesis.CoreApi.Repository.CoreDBContext
Bu işlemden önce
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools
bu paketlerin yüklü olması gerekli.

Add-Migration 
Update-Database 

.net7 connection scriptte kullanılmalı TrustServerCertificate=True;
  "MsSQLConnection": "Server=localhost;Database=GenesisCore;User ID=sa;Password=Aa12345_;TrustServerCertificate=True;"