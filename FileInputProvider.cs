using System.IO;

namespace NewtonInterpolationSolver
{
    public class FileInputProvider : IInputProvider
    {
        private readonly string _filePath;

        public FileInputProvider(string filePath)
        {
            _filePath = filePath;
        }

        public double[] GetData()
        {
            try
            {
                string[] lines = File.ReadAllLines(_filePath);

                // Отримуємо кількість точок з файлу (уявіть, що вона знаходиться в першому рядку)
                int numberOfPoints = int.Parse(lines[0]);

                // Перевіряємо, чи в файлі достатньо даних для обробки
                if (lines.Length < numberOfPoints + 2) // +2, бо перший рядок містить кількість точок, а останній - значення для інтерполяції
                {
                    Console.WriteLine("\nПОМИЛКА: Недостатньо рядків у файлі для отримання даних.");
                    return null;
                }

                double[] xValues = new double[numberOfPoints];
                double[] yValues = new double[numberOfPoints];

                for (int i = 0; i < numberOfPoints; i++)
                {
                    string[] values = lines[i + 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length != 2)
                    {
                        Console.WriteLine("\nПОМИЛКА: Некоректний формат даних у файлі. Будь ласка, переконайтеся, що всі значення є числами та розділені пробілами.");
                        return null;
                    }

                    if (!double.TryParse(values[0], out xValues[i]) || !double.TryParse(values[1], out yValues[i]))
                    {
                        Console.WriteLine("\nПОМИЛКА: Некоректні дані у файлі.");
                        return null;
                    }
                }

                double interpolationValue;
                if (!double.TryParse(lines[numberOfPoints + 1].Replace(',', '.'), out interpolationValue))
                {
                    Console.WriteLine("\nПОМИЛКА: Некоректне значення для інтерполяції.");
                    return null;
                }

                return new double[] { interpolationValue }; // Повертаємо тільки значення для інтерполяції
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\nПОМИЛКА: Файл '{_filePath}' не знайдено. Повторіть спробу, будь-ласка.");
                return null;
            }
            catch (IOException)
            {
                Console.WriteLine($"\nПОМИЛКА: Помилка читання файлу '{_filePath}'. Повторіть спробу, будь-ласка.");
                return null;
            }
            catch (FormatException)
            {
                Console.WriteLine("\nПОМИЛКА: Некоректний формат даних у файлі. Будь ласка, переконайтеся, що всі значення є числами та розділені пробілами.");
                return null;
            }
        }


    }
}
