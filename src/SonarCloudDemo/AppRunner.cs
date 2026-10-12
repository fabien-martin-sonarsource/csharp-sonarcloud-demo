using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SonarCloudDemo.Invoicing;

namespace SonarCloudDemo;

public class AppRunner(ILogger<AppRunner> logger)
{
    public string Run()
    {
        var repository = new UserRepository();
        repository.CreateUser("alice", "P@ssw0rd123");

        var orderCalculator = new OrderCalculator();
        var finalPrice = orderCalculator.ComputeDiscount(120, "GOLD", true, false, 3);
        logger.LogInformation("Final price after discount: {FinalPrice}", finalPrice);

        var report = ReportBuilder.BuildSummary("Q1", 1000, 50);
        var report2 = ReportBuilder.BuildDetailedSummary("Q2", 2000, 80);

        var invoices = RunInvoicingDemo();

        // Intentional: serializing with an outdated, vulnerable Newtonsoft.Json version
        // (see SonarCloudDemo.csproj) for dependency-risk experimentation.
        return JsonConvert.SerializeObject(new { report, report2, invoices });
    }

    // Demo: exercises every "_VX" copy of the invoicing API (see src/SonarCloudDemo/Invoicing),
    // which is the SonarQube Cloud branch-based versioning demo described in the plan. Each
    // copy is a near-identical duplicate, on purpose.
    private static string[] RunInvoicingDemo()
    {
        var v1 = new InvoiceService();
        var v2 = new InvoiceService_V2();
        var v3 = new InvoiceService_V3();
        var v4 = new InvoiceService_V4();

        var total1 = v1.ComputeInvoiceTotal(620, "GOLD", true, false, 6);
        var total2 = v2.ComputeInvoiceTotal(620, "GOLD", true, false, 6);
        var total3 = v3.ComputeInvoiceTotal(620, "GOLD", true, false, 6);
        var total4 = v4.ComputeInvoiceTotal(620, "GOLD", true, false, 6);

        return
        [
            v1.BuildInvoiceRecord("INV-0001", total1, null),
            v2.BuildInvoiceRecord("INV-0002", total2, "APPROVED"),
            v3.BuildInvoiceRecord("INV-0003", total3 - v3.ApplyLatePaymentFee(total3, 45), "REJECTED"),
            v4.BuildInvoiceRecord("INV-0004", v4.ConvertToCurrency(total4, "USD"), null)
        ];
    }
}
