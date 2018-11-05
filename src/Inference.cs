using static NewFAHP.Numbers;

namespace NewFAHP
{
    /*
        public static int TS_SE = 0;
        public static int TS_MF = 1;
        public static int TS_SA = 2;
        public static int TS_LA = 3;
        public static int TS_AA = 4;
     */

    public static class Inference
    {

        public static (double, double, double)[,] ComparisonMatrix(int[] values, int ConfLevel)
        {
            if (values.Length < 5)
                throw new System.ArgumentException("At least 5 integers are expected.");

            for (int i = 0; i < 4; i++)
                if (values[i] < -3 || values[i] > 3)
                    throw new System.ArgumentException($"Error at {i} = {values[i]}: Fuzzy input must be between -3 and +3.");

            if (ConfLevel < 0 || ConfLevel > 4)
                throw new System.ArgumentException("Confidence level must be between 1 and 5.");

            (double, double, double)[,] CompMat = new (double, double, double)[6, 6];

            (double, double, double)[,] TFNs =
            {
                { (1, 1, 3), (1, 3, 5), (3, 5, 7), (5, 7, 9), (7, 9, 9) },
                { (1, 1, 2.5), (1.5, 3, 4.5), (3.5, 5, 6.5), (5.5, 7, 8.5), (8.5, 9, 9) },
                { (1, 1, 2), (2, 3, 4), (4, 5, 6), (6, 7, 8), (8, 9, 9) },
                { (1, 1, 1), (2.5, 3, 3.5), (4.5, 5, 5.5), (6.5, 7, 7.5), (9, 9, 9) },
                { (1, 1, 1), (3, 3, 3), (5, 5, 5), (7, 7, 7), (9, 9, 9) }
            };

            for (int j = 0; j < 5; j++)
                CompMat[j, j] = TFNs[ConfLevel, 0]; // 0-th => Equality
            
            for (int k = 0, l, m; k < 5; k++)
            {
                l = k + 1;
                m = values[k];
                if (m >= 0)
                {
                    CompMat[0, l] = TFNs[ConfLevel, m];
                    CompMat[l, 0] = TFNs[ConfLevel, m].Inverse();
                }
                else
                {
                    CompMat[l, 0] = TFNs[ConfLevel, - m];
                    CompMat[0, l] = TFNs[ConfLevel, - m].Inverse();
                }
            }

             for (int r = 1; r < 5; r++)
             for (int c = r + 1; c < 5; c++)
             {
                 CompMat[r, c] = CompMat[c - 1, r - 1].Multiply(CompMat[r - 1, c]);
                 CompMat[c, r] = CompMat[r, c].Inverse();
             }

            return CompMat;
        }

        public static (double, double, double) Multiply(this (double, double, double) a, (double, double, double) b)
        {
            double[] Items = { a.Item1 * b.Item1, a.Item1 * b.Item3, a.Item3 * b.Item1, a.Item3 * b.Item3 };
            
            double Left = Items[0], Middle = a.Item2 * b.Item2, Right = Items[0];

            for (int i = 0; i < 4; i++)
            {
                if (Items[i] < Left)
                    Left = Items[i];
                if (Items[i] > Right)
                    Right = Items[i];
            }

            return (Left, Middle, Right);
        }

        public static (double, double, double) Inverse(this (double, double, double) a)
            => (1.0 / a.Item3, 1.0 / a.Item2, 1.0 / a.Item1);

        
        
    }
}