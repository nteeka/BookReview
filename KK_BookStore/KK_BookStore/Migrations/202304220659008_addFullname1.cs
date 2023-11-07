namespace KK_BookStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFullname1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FullName");
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
    }
}
