using System;

namespace ESPN
{
    public class Toolkit
    {
        public static float MoneyLineToImpliedProbability(int money_line)
        {
            if (money_line < 0) //negative
            {
                float ml = Convert.ToSingle(money_line);
                return (-1f * ml) / ((-1 * ml) + 100f);
            }
            else
            {
                float ml = Convert.ToSingle(money_line);
                return 100f / (ml + 100f);
            }
        }
    }
}