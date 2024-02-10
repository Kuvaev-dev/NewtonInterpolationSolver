namespace NewtonInterpolationSolver
{
    // Клас для реалізації інтерполяційних методів
    public class Interpolator
    {
        // Перша інтерполяційна формула Ньютона для равностоящих узлів
        public static double NewtonFirstInterpolation(double[] x, double[] y, double value)
        {
            int n = x.Length;
            double result = 0;

            for (int i = 0; i < n; i++)
            {
                double term = y[i];
                string formula = $"{y[i]}";
                for (int j = 0; j < i; j++)
                {
                    term *= (value - x[j]) / (x[i] - x[j]);
                    formula += $" * ({value} - {x[j]}) / ({x[i]} - {x[j]})";
                }
                result += term;
                TextViewer.ChangeColor($"L[{i + 1}]: {formula} = {result}", "magenta");
            }

            return result;
        }

        // Друга інтерполяційна формула Ньютона для равностоящих узлів
        public static double NewtonSecondInterpolation(double[] x, double[] y, double value)
        {
            int n = x.Length;
            double result = 0;
            double[] coeffs = new double[n];
            coeffs[0] = y[0];

            for (int i = 1; i < n; i++)
            {
                for (int j = n - 1; j >= i; j--)
                {
                    y[j] = (y[j] - y[j - 1]) / (x[j] - x[j - i]);
                }
                coeffs[i] = y[i];
            }

            result = coeffs[0];
            string formula = $"{coeffs[0]}";
            for (int i = 1; i < n; i++)
            {
                double term = coeffs[i];
                for (int j = 0; j < i; j++)
                {
                    term *= (value - x[j]);
                    formula += $" * ({value} - {x[j]})";
                }
                result += term;
                TextViewer.ChangeColor($"\nL[{i}]: {formula} = {result}", "magenta");
            }

            return result;
        }
    }
}
