namespace SonarCloudDemo.Invoicing;

// Intentional (demo): "V4" of the invoicing API — yet another full copy of
// InvoiceService.cs, this time shipped to add multi-currency support. Everything below
// ComputeInvoiceTotal...Persist is byte-identical to InvoiceService.cs,
// InvoiceService_V2.cs and InvoiceService_V3.cs, which is exactly why it is flagged as
// duplicated code against all three.
public class InvoiceService_V4
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

    // V4-only addition: multi-currency conversion. The reason this shipped as a new _V4
    // file instead of a change to InvoiceService.cs, InvoiceService_V2.cs or
    // InvoiceService_V3.cs.
    public double ConvertToCurrency(double amountInEur, string targetCurrency)
    {
        if (targetCurrency == "USD")
        {
            return amountInEur * 1.08;
        }

        if (targetCurrency == "GBP")
        {
            return amountInEur * 0.86;
        }

        return amountInEur;
    }
}
