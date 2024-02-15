using NewtonInterpolationSolver.View;

namespace NewtonInterpolationSolver.Logic
{
    // Клас для реалізації інтерполяційних методів
    public class Interpolator
    {
        // Перша інтерполяційна формула Ньютона для равностоячих узлів
        public static double NewtonFirstInterpolation(double[] x, double[] y, double value)
        {
            // Кількість вузлів
            int n = x.Length;

            TextViewer.ChangeColor("\nІніціалізація\n", "blue");
            // Ініціалізація результату значенням у першому вузлі
            double result = y[0];
            Console.WriteLine("Результат після ініціалізації: " + result);

            // Масив для збереження різницевих коефіцієнтів
            double[] diffCoeff = new double[n];

            // Ініціалізація масиву різницевих коефіцієнтів початковими значеннями
            for (int i = 0; i < n; i++)
            {
                diffCoeff[i] = y[i];
            }

            TextViewer.ChangeColor("\nОбчислення різнецевих коефіцієнтів\n", "blue");
            // Обчислення різницевих коефіцієнтів
            for (int i = 1; i < n; i++)
            {
                TextViewer.ChangeColor($"Крок {i}\n", "blue");
                for (int j = n - 1; j >= i; j--)
                {
                    // Розрахунок різницевого коефіцієнта за формулою (f(x_j) - f(x_{j-1})) / (x_j - x_{j-i})
                    diffCoeff[j] = (diffCoeff[j] - diffCoeff[j - 1]) / (x[j] - x[j - i]);
                    TextViewer.ChangeColor($"\t({y[j]} - {y[j - 1]}) / ({x[j]} - {x[j - i]}) = {diffCoeff[j]}\n", "magenta");
                }
                Console.WriteLine($"Різницеві коефіцієнти після кроку {i}:\n[{string.Join(", ", diffCoeff)}]\n");
            }

            TextViewer.ChangeColor("Обчислення значення інтерполяції за формулою Ньютона\n", "blue");
            // Обчислення значення інтерполяції за формулою Ньютона
            for (int i = 1; i < n; i++)
            {
                TextViewer.ChangeColor($"\nДоданки для кроку {i}:\n", "blue");
                double term = diffCoeff[i];

                // Обчислення кожного доданку за формулою term *= (value - x[j])
                for (int j = 0; j < i; j++)
                {
                    TextViewer.ChangeColor($"\tДоданок {j + 1}: {term} * ({value} - {x[j]}) = ", "magenta");
                    term *= (value - x[j]);
                    TextViewer.ChangeColor($"\t = {term}\n", "magenta");
                }

                // Додавання доданку до результату
                result += term;
            }

            TextViewer.ChangeColor("Сума усіх отриманих додатків\n", "blue");
            // Повернення результату інтерполяції
            return result;
        }

        // Друга інтерполяційна формула Ньютона для равностоячих узлів
        public static double NewtonSecondInterpolation(double[] x, double[] y, double value)
        {
            // Отримання кількості точок для інтерполяції
            int n = x.Length;

            // Ініціалізація результату і коефіцієнтів для методу інтерполяції
            double result = 0;
            double[] originalY = new double[n]; // Копія вихідного масиву y
            Array.Copy(y, originalY, n); // Створення копії вихідного масиву y
            double[] coeffs = new double[n]; // Масив для зберігання коефіцієнтів

            // Обчислення першого коефіцієнта
            coeffs[0] = originalY[0]; // Використання копії масиву для обчислень

            // Обчислення коефіцієнтів за методом Ньютона
            for (int i = 1; i < n; i++)
            {
                for (int j = n - 1; j >= i; j--)
                {
                    originalY[j] = (originalY[j] - originalY[j - 1]) / (x[j] - x[j - i]);
                }
                coeffs[i] = originalY[i];
            }

            // Обчислення значення інтерполяції
            result = coeffs[0];
            string formula = $"{coeffs[0]}";
            for (int i = 1; i < n; i++)
            {
                double term = coeffs[i];
                for (int j = 0; j < i; j++)
                {
                    term *= value - x[j];
                    formula += $" * ({value} - {x[j]})";
                }
                result += term;

                // Виведення проміжного результату для кожної ітерації
                Console.WriteLine($"\nПроміжний результат для ітерації {i}:");
                TextViewer.ChangeColor($"\t\n{formula} = {result}", "magenta");
            }

            return result;
        }
    }
}
