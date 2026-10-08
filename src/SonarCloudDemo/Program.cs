using SonarCloudDemo;

var repository = new UserRepository();
repository.CreateUser("alice", "P@ssw0rd123");

var orderCalculator = new OrderCalculator();
Console.WriteLine(orderCalculator.ComputeDiscount(120, "GOLD", true, false, 3));

var report = ReportBuilder.BuildSummary("Q1", 1000, 50);
Console.WriteLine(report);

var report2 = ReportBuilder.BuildDetailedSummary("Q2", 2000, 80);
Console.WriteLine(report2);
