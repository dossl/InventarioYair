namespace InventarioOtro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                  INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'31c1c25a-cf1e-4d37-8cdb-cfc4e555d270', NULL, 0, N'ACoEQuJL+fKDteMpAxnA3/R1A5bowSPUhOj48xgwsrbIuIm+plBG6aNcePCvZ6FNMw==', N'd9a8ccdd-91f8-4c18-9c51-50d834dae667', NULL, 0, 0, NULL, 1, 0, N'admin')
                  INSERT INTO[dbo].[AspNetUsers]([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES(N'3f659a95-f0d7-4eb5-9757-ac90a06a87c2', NULL, 0, N'AHGZ1/2hK3Q8QCGu6dwmwLxC1EOyVoZiLq+Kywr4U+rKihUEkkkOmm1Q7nk2PgcGAg==', N'f7d0e86c-36a7-4ee9-b891-9139d7bc43ce', NULL, 0, 0, NULL, 1, 0, N'guest')
                  INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'99182a4e-7fdf-4dd7-af7c-4e751f2b6ae0', N'Admin')                
                  INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'31c1c25a-cf1e-4d37-8cdb-cfc4e555d270', N'99182a4e-7fdf-4dd7-af7c-4e751f2b6ae0')
                ");
        }
        
        public override void Down()
        {
        }
    }
}
