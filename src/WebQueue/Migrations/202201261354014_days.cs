namespace WebQueue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class days : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WorkStartTime = c.DateTime(),
                        WorkEndTime = c.DateTime(),
                        ExactDate = c.DateTime(),
                        DayOfWeek = c.Int(),
                        IsWorkTime = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Days");
        }
    }
}
