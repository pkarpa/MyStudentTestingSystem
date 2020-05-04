namespace TestingSystemServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        Admin_Id = c.Int(),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.Administrators", t => t.Admin_Id)
                .Index(t => t.Admin_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SurName = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        Group_GroupId = c.Int(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId)
                .Index(t => t.Group_GroupId);
            
            CreateTable(
                "dbo.TestSessions",
                c => new
                    {
                        TestSessionId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Student_StudentId = c.Int(),
                        Test_TestId = c.Int(),
                    })
                .PrimaryKey(t => t.TestSessionId)
                .ForeignKey("dbo.Students", t => t.Student_StudentId)
                .ForeignKey("dbo.Tests", t => t.Test_TestId)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Test_TestId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        AnswerText = c.String(),
                        AnswerCorrects = c.Boolean(nullable: false),
                        Question_Id = c.Int(),
                        TestSession_TestSessionId = c.Int(),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .ForeignKey("dbo.TestSessions", t => t.TestSession_TestSessionId)
                .Index(t => t.Question_Id)
                .Index(t => t.TestSession_TestSessionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        QuestionText = c.String(),
                        AnswerOption = c.String(),
                        QuestionImage = c.Binary(),
                        QuestionCountScores = c.Double(nullable: false),
                        QuestionAnswer = c.String(),
                        Theme_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionTypes", t => t.Id)
                .ForeignKey("dbo.Themes", t => t.Theme_Id)
                .Index(t => t.Id)
                .Index(t => t.Theme_Id);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionTypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThemeName = c.String(),
                        Subject_SubjectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId)
                .Index(t => t.Subject_SubjectId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectName = c.String(),
                        Admin_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.Administrators", t => t.Admin_Id)
                .Index(t => t.Admin_Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestId = c.Int(nullable: false, identity: true),
                        TestName = c.String(),
                        TestTime = c.DateTime(nullable: false),
                        TestCountScores = c.Double(nullable: false),
                        MixQuestionsOrder = c.Boolean(nullable: false),
                        MixAnswersOrder = c.Boolean(nullable: false),
                        Theme_Id = c.Int(),
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.Themes", t => t.Theme_Id)
                .Index(t => t.Theme_Id);
            
            CreateTable(
                "dbo.TestGroups",
                c => new
                    {
                        Test_TestId = c.Int(nullable: false),
                        Group_GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Test_TestId, t.Group_GroupId })
                .ForeignKey("dbo.Tests", t => t.Test_TestId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .Index(t => t.Test_TestId)
                .Index(t => t.Group_GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tests", "Theme_Id", "dbo.Themes");
            DropForeignKey("dbo.TestSessions", "Test_TestId", "dbo.Tests");
            DropForeignKey("dbo.TestGroups", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.TestGroups", "Test_TestId", "dbo.Tests");
            DropForeignKey("dbo.TestSessions", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Answers", "TestSession_TestSessionId", "dbo.TestSessions");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Themes", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "Admin_Id", "dbo.Administrators");
            DropForeignKey("dbo.Questions", "Theme_Id", "dbo.Themes");
            DropForeignKey("dbo.Questions", "Id", "dbo.QuestionTypes");
            DropForeignKey("dbo.Students", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "Admin_Id", "dbo.Administrators");
            DropIndex("dbo.TestGroups", new[] { "Group_GroupId" });
            DropIndex("dbo.TestGroups", new[] { "Test_TestId" });
            DropIndex("dbo.Tests", new[] { "Theme_Id" });
            DropIndex("dbo.Subjects", new[] { "Admin_Id" });
            DropIndex("dbo.Themes", new[] { "Subject_SubjectId" });
            DropIndex("dbo.Questions", new[] { "Theme_Id" });
            DropIndex("dbo.Questions", new[] { "Id" });
            DropIndex("dbo.Answers", new[] { "TestSession_TestSessionId" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.TestSessions", new[] { "Test_TestId" });
            DropIndex("dbo.TestSessions", new[] { "Student_StudentId" });
            DropIndex("dbo.Students", new[] { "Group_GroupId" });
            DropIndex("dbo.Groups", new[] { "Admin_Id" });
            DropTable("dbo.TestGroups");
            DropTable("dbo.Tests");
            DropTable("dbo.Subjects");
            DropTable("dbo.Themes");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
            DropTable("dbo.TestSessions");
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
            DropTable("dbo.Administrators");
        }
    }
}
