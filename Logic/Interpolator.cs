using NewtonInterpolationSolver.View;

namespace NewtonInterpolationSolver.Logic
{
    // Клас для реалізації інтерполяційних методів
    public class Interpolator
    {
        // Перша інтерполяційна формула Ньютона для равностоящих узлів
        public static double NewtonFirstInterpolation(double[] x, double[] y, double value)
        {
            int n = x.Length;
            double result = 0;

            // Ітерація по узлам
            for (int i = 0; i < n; i++)
            {
                double term = y[i];
                string formula = $"{y[i]}";

                // Обчислення члену формули для даного узла
                for (int j = 0; j < i; j++)
                {
                    term *= (value - x[j]) / (x[i] - x[j]);
                    formula += $" * ({value} - {x[j]}) / ({x[i]} - {x[j]})";
                }

                // Додавання до результату
                result += term;

                // Вивід проміжного результату
                Console.WriteLine($"\nПроміжний результат для ітерації {i + 1}:");
                TextViewer.ChangeColor($"\t\n{formula} = {result}", "magenta");
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

            // Обчислення коефіцієнтів для інтерполяційного полінома
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

            // Обчислення інтерполяційного полінома та проміжних результатів
            for (int i = 1; i < n; i++)
            {
                double term = coeffs[i];
                for (int j = 0; j < i; j++)
                {
                    term *= value - x[j];
                    formula += $" * ({value} - {x[j]})";
                }
                result += term;

                // Вивід проміжного результату
                Console.WriteLine($"\nПроміжний результат для ітерації {i}:");
                TextViewer.ChangeColor($"\t\n{formula} = {result}", "magenta");
            }

            return result;
        }
    }
}
