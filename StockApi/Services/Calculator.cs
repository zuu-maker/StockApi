using StockApi.Modals;

namespace StockApi.Services
{
    public class Calculator
    {
        public static StockResult Calculate(int unitSizeMl, double tareWeightG, double fullGrossG, double currentGrossG, int shotSizeMl, double lossAllowance)
        {
            // 1. Calculate liquid weight (Gross - Tare)
            double fullContentWeight = Math.Max(0, fullGrossG - tareWeightG);
            double currentContentWeight = Math.Max(0, currentGrossG - tareWeightG);

            // 2. Calculate Percentage (Clamped between 0 and 1)
            double percentRemaining = 0;
            if (fullContentWeight > 0)
            {
                percentRemaining = Math.Clamp(currentContentWeight / fullContentWeight, 0.0, 1.0);
            }

            // 3. Convert to Volume
            double remainingVolumeMl = unitSizeMl * percentRemaining;

            // 4. Calculate Sellable Servings (accounting for "Loss Allowance" / Spillage)
            double usableVolume = remainingVolumeMl * (1.0 - lossAllowance);

            // 5. Calculate Shots
            double exactShots = usableVolume / shotSizeMl;
            int wholeShots = (int)Math.Round(exactShots); // Round to nearest whole number

            return new StockResult
            {
                RemainingMl = remainingVolumeMl,
                WholeShots = wholeShots,
                ExactShots = exactShots
            };
        }
    }
}
