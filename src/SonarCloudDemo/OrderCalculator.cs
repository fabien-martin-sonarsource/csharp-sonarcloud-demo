namespace SonarCloudDemo;

public class OrderCalculator
{
    // Intentional: deeply nested conditionals and magic numbers to trigger
    // Cognitive Complexity (S3776) and "no magic numbers" (S109) rules.
    public double ComputeDiscount(double amount, string tier, bool isFirstOrder, bool isEmployee, int itemCount)
    {
        double discount = 0;

        if (tier == "GOLD")
        {
            if (amount > 100)
            {
                if (itemCount > 2)
                {
                    if (isFirstOrder)
                    {
                        discount = amount * 0.25;
                    }
                    else
                    {
                        if (isEmployee)
                        {
                            discount = amount * 0.3;
                        }
                        else
                        {
                            discount = amount * 0.2;
                        }
                    }
                }
                else
                {
                    discount = amount * 0.1;
                }
            }
            else
            {
                discount = amount * 0.05;
            }
        }
        else if (tier == "SILVER")
        {
            if (amount > 50)
            {
                if (isFirstOrder)
                {
                    discount = amount * 0.15;
                }
                else
                {
                    discount = amount * 0.1;
                }
            }
            else
            {
                discount = amount * 0.02;
            }
        }
        else
        {
            if (isEmployee)
            {
                discount = amount * 0.05;
            }
        }

        return amount - discount;
    }
}
