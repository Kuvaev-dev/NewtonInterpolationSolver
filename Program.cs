using System.Globalization;
using System.Text;
using NewtonInterpolationSolver.Data;
using NewtonInterpolationSolver.Input;
using NewtonInterpolationSolver.Logic;
using NewtonInterpolationSolver.View;

namespace NewtonInterpolationSolver
{
    // Приклад введення з файлу та вручну:
    // 1. Вручну
    // x: 0 1 2 3 4
    // y: 1.3 2.4 3.5 4.6 5.7
    // in: 1.5
    // 2. З файлу
    // 0 1 2 3 4
    // 1.3 2.4 3.5 4.6 5.7
    // 1.5

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = Encoding.Unicode;

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Оберіть спосіб введення даних:");
                TextViewer.ChangeColor("\t1. Вручну", "blue");
                TextViewer.ChangeColor("\t2. З файлу", "yellow");
                TextViewer.ChangeColor("\t3. Вийти", "red");
                Console.WriteLine("Ваш вибір: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Некоректний ввід. Введіть число від 1 до 3", "red");
                    Console.ReadLine();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        ProcessInputManually(); // Обробка введення даних вручну
                        break;
                    case 2:
                        Console.Clear();
                        TextViewer.ChangeColor("Шлях до файлу", "blue");
                        Console.WriteLine("Введіть шлях до файлу:");
                        string filePath = Console.ReadLine();
                        Console.WriteLine();
                        ProcessInput(new FileInputProvider(filePath)); // Обробка введення даних з файлу
                        break;
                    case 3:
                        return; // Вихід з програми
                    default:
                        TextViewer.ChangeColor("\nПОМИЛКА: Опції не існує. Повторіть спробу, будь-ласка", "red");
                        break;
                }

                Console.ReadLine();
            }
        }

        static void ProcessInputManually()
        {
            // Введення даних вручну
            TextViewer.ChangeColor("Значення X", "blue");
            Console.WriteLine("Введіть значення X через пробіл:");
            string inputX = Console.ReadLine();

            // Перевірка наявності введених даних
            if (string.IsNullOrWhiteSpace(inputX))
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Введіть значення для X.", "red");
                return;
            }

            // Розділення введеного рядка на масив чисел
            string[] xTokens = inputX.Split(' ');
            double[] xValues;

            // Перевірка правильності формату введених даних для X
            if (!xTokens.All(token => double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out _)))
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Некоректний формат даних для X. Будь ласка, введіть числа.", "red");
                return;
            }

            // Конвертація рядка у масив чисел
            xValues = Array.ConvertAll(xTokens, token => double.Parse(token, CultureInfo.InvariantCulture));

            TextViewer.ChangeColor("\nЗначення Y", "blue");
            Console.WriteLine("Введіть значення Y через пробіл:");
            string inputY = Console.ReadLine();

            // Перевірка наявності введених даних
            if (string.IsNullOrWhiteSpace(inputY))
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Введіть значення для Y.", "red");
                return;
            }

            // Розділення введеного рядка на масив чисел
            string[] yTokens = inputY.Split(' ');
            double[] yValues;

            // Перевірка правильності формату введених даних для Y
            if (!yTokens.All(token => double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out _)))
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Некоректний формат даних для Y. Будь ласка, введіть числа.", "red");
                return;
            }

            // Конвертація рядка у масив чисел
            yValues = Array.ConvertAll(yTokens, token => double.Parse(token, CultureInfo.InvariantCulture));

            TextViewer.ChangeColor("\nЗначення інтерполяції", "blue");
            Console.WriteLine("Введіть значення інтерполяції:");

            // Перевірка правильності введеного значення інтерполяції
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double interpolationValue))
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Некоректне значення для інтерполяції.", "red");
                return;
            }

            // Перевірка наявності введених даних
            if (xValues.Length != yValues.Length)
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Кількість значень Х не відповідає кількості значень Y.", "red");
                return;
            }

            double result;
            int choice;
            do
            {
                // Вибір методу інтерполяції
                TextViewer.ChangeColor("\nМетод інтерполяції", "blue");
                Console.WriteLine("Виберіть метод інтерполяції:");
                TextViewer.ChangeColor("\t1. Перша формула Ньютона", "blue");
                TextViewer.ChangeColor("\t2. Друга формула Ньютона", "yellow");
                Console.WriteLine("Ваш вибір: ");
            } while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2));

            TextViewer.ChangeColor("\nРозв'язання", "blue");
            if (choice == 1)
            {
                result = Interpolator.NewtonFirstInterpolation(xValues, yValues, interpolationValue); // Виклик методу першої формули Ньютона
            }
            else
            {
                result = Interpolator.NewtonSecondInterpolation(xValues, yValues, interpolationValue); // Виклик методу другої формули Ньютона
            }

            TextViewer.ChangeColor($"\nРезультат інтерполяції: {result}", "yellow");
            SaveResultsToFile(xValues, yValues, choice, result);
        }


        static void ProcessInput(IInputProvider inputProvider)
        {
            // Обробка введення даних з файлу
            double[] xValues, yValues;
            double interpolationValue;

            TextViewer.ChangeColor("Значення X", "blue");
            DataArrays dataX = inputProvider.GetData();
            if (dataX != null && dataX.XValues != null && dataX.XValues.Length > 0)
            {
                xValues = dataX.XValues;
                Console.WriteLine("Зчитані значення X:");
                Console.WriteLine(string.Join(" ", dataX.XValues));
            }
            else
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Некоректні дані для X.", "red");
                return;
            }

            TextViewer.ChangeColor("\nЗначення Y", "blue");
            DataArrays dataY = inputProvider.GetData();
            if (dataY != null && dataY.YValues != null && dataY.YValues.Length > 0)
            {
                yValues = dataY.YValues;
                Console.WriteLine("Зчитані значення Y:");
                Console.WriteLine(string.Join(" ", dataY.YValues));
            }
            else
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Некоректні дані для Y.", "red");
                return;
            }

            TextViewer.ChangeColor("\nЗначення інтерполяції", "blue");
            DataArrays dataInterpolation = inputProvider.GetData();
            if (dataInterpolation != null && !double.IsNaN(dataInterpolation.InterpolationValue))
            {
                interpolationValue = dataInterpolation.InterpolationValue;
                Console.WriteLine($"Зчитане значення інтерполяції: {interpolationValue}");
            }
            else
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Некоректне значення для інтерполяції.", "red");
                return;
            }

            if (xValues.Length != yValues.Length)
            {
                TextViewer.ChangeColor("\nПОМИЛКА: Кількість значень Х не відповідає кількості значень Y.", "red");
                return;
            }

            int choice;
            do
            {
                // Вибір методу інтерполяції
                TextViewer.ChangeColor("\nМетод інтерполяції", "blue");
                Console.WriteLine("Виберіть метод інтерполяції:");
                TextViewer.ChangeColor("\t1. Перша формула Ньютона", "blue");
                TextViewer.ChangeColor("\t2. Друга формула Ньютона", "yellow");
                Console.WriteLine("Ваш вибір: ");
                if (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Некоректне значення. Будь ласка, введіть 1 або 2", "red");
                }
            }
            while (choice != 1 && choice != 2);

            TextViewer.ChangeColor("\nРозв'язання", "blue");
            double result;
            if (choice == 1)
            {
                result = Interpolator.NewtonFirstInterpolation(xValues, yValues, interpolationValue); // Виклик методу першої формули Ньютона
            }
            else
            {
                result = Interpolator.NewtonSecondInterpolation(xValues, yValues, interpolationValue); // Виклик методу другої формули Ньютона
            }

            TextViewer.ChangeColor($"\n\tРезультат інтерполяції: {result}", "yellow");
            SaveResultsToFile(xValues, yValues, choice, result);
        }

        static void SaveResultsToFile(double[] xValues, double[] yValues, int choice, double result)
        {
            // Генерація ім'я файлу на основі поточної дати та часу
            string fileName = $"Interpolation_Result_{DateTime.Now:yyyyMMddHHmmss}.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    // Запис значень X у файл
                    writer.WriteLine("Значення Х:");
                    writer.WriteLine(string.Join(" ", xValues));

                    // Запис значень Y у файл
                    writer.WriteLine("Значення Y:");
                    writer.WriteLine(string.Join(" ", yValues));

                    // Запис вибраного методу інтерполяції у файл
                    writer.WriteLine("Метод Ньютона:");
                    writer.WriteLine(choice == 1 ? "Перша формула Ньютона" : "Друга формула Ньютона");

                    // Запис результату інтерполяції у файл
                    writer.WriteLine("Результат інтерполяції:");
                    writer.WriteLine(result);
                }

                TextViewer.ChangeColor($"\nРезультати збережено у файл: {fileName}", "green");
            }
            catch (Exception ex)
            {
                TextViewer.ChangeColor($"\nПомилка при збереженні результатів у файл: {ex.Message}", "red");
            }
        }
    }
}
