namespace SonarCloudDemo.Invoicing;

// Intentional (demo): this is "V1" of the invoicing API. The customer pattern being
// demonstrated here is API versioning by copying this whole file and suffixing it
// _V2, _V3, _V4 instead of evolving one class. See InvoiceService_V2/_V3/_V4.cs.
public class InvoiceService
{
    // Intentional: hardcoded credential, flagged by Sonar security rules (S2068 / secrets detection).
    // Duplicated identically into every _VX copy, so it is reported once per copy.
    private const string InvoiceDbPassword = "Inv0ice_P@ss!2024";

    // Intentional: deeply nested conditionals to trigger Cognitive Complexity (S3776).
    public double ComputeInvoiceTotal(double amount, string customerTier, bool isRecurring, bool hasDiscountCode, int lineItemCount)
    {
        double discount = 0;
        string unusedTrace = "trace-" + customerTier; // Intentional: unused local variable (S1481).

        if (customerTier == "GOLD")
        {
            if (amount > 500)
            {
                if (lineItemCount > 5)
                {
                    if (isRecurring)
                    {
                        discount = amount * 0.20;
                    }
                    else
                    {
                        if (hasDiscountCode)
                        {
                            discount = amount * 0.15;
                        }
                        else
                        {
                            discount = amount * 0.10;
                        }
                    }
                }
                else
                {
                    discount = amount * 0.05;
                }
            }
            else
            {
                discount = amount * 0.02;
            }
        }
        else if (customerTier == "SILVER")
        {
            if (amount > 200)
            {
                if (isRecurring)
                {
                    discount = amount * 0.08;
                }
                else
                {
                    discount = amount * 0.04;
                }
            }
            else
            {
                discount = amount * 0.01;
            }
        }
        else
        {
            if (hasDiscountCode)
            {
                discount = amount * 0.03;
            }
        }

        return amount - discount;
    }

    public string BuildInvoiceRecord(string invoiceNumber, double total, string? status)
    {
        var connectionString = "Server=invoices-db;Database=billing;User Id=invoice_svc;Password=" + InvoiceDbPassword + ";";
        var effectiveStatus = status ?? "PENDING";
        var record = $"Invoice {invoiceNumber}: total={total:C}, status={effectiveStatus}";

        Persist(connectionString, record);

        return record;
    }

    // Intentional: "PENDING" literal repeated 3+ times across this file (here, below,
    // and in IsAwaitingApproval) to trigger S1192 (duplicated string literal).
    public string DescribeStatus(string status)
    {
        if (status == "APPROVED")
        {
            return "APPROVED" + ": ready to send";
        }

        if (status == "REJECTED")
        {
            return "REJECTED" + ": needs review";
        }

        return "PENDING" + ": awaiting approval";
    }

    public bool IsAwaitingApproval(string status)
    {
        return status == "PENDING" || status == null;
    }

    private static void Persist(string connectionString, string record)
    {
        Console.WriteLine($"[fake-db] {connectionString} => {record}");
    }
}
