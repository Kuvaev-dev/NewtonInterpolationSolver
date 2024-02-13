using System.Globalization;
using NewtonInterpolationSolver.Data;
using NewtonInterpolationSolver.View;

namespace NewtonInterpolationSolver.Input
{
    // Клас для введення даних з файлу
    public class FileInputProvider : IInputProvider
    {
        // Шлях до файлу, з якого будуть читатися дані
        private readonly string _filePath;

        // Конструктор класу, який приймає шлях до файлу
        public FileInputProvider(string filePath)
        {
            _filePath = filePath;
        }

        // Метод для отримання даних з файлу
        public DataArrays GetData()
        {
            try
            {
                // Спроба зчитати всі рядки з файлу
                string[] lines = File.ReadAllLines(_filePath);

                // Перевірка, чи є у файлі достатня кількість рядків
                if (lines.Length < 3)
                {
                    // Якщо рядків недостатньо, виводиться помилка
                    TextViewer.ChangeColor($"\nПОМИЛКА: Файл '{_filePath}' не містить достатньої кількості рядків.", "red");
                    return null;
                }

                // Розділення першого рядка на числа X
                double[] xValues = Array.ConvertAll(lines[0].Split(' '), s => double.Parse(s, CultureInfo.InvariantCulture));

                // Розділення другого рядка на числа Y
                double[] yValues = Array.ConvertAll(lines[1].Split(' '), s => double.Parse(s, CultureInfo.InvariantCulture));

                // Отримання значення для інтерполяції з третього рядка
                double interpolationValue = double.Parse(lines[2], CultureInfo.InvariantCulture);

                // Перевірка, чи кількість значень X відповідає кількості значень Y
                if (xValues.Length != yValues.Length)
                {
                    // Якщо кількості не відповідають, виводиться помилка
                    TextViewer.ChangeColor("\nПОМИЛКА: Кількість значень X не відповідає кількості значень Y.", "red");
                    return null;
                }

                // Повертаємо об'єкт DataArrays з отриманими даними
                return new DataArrays { XValues = xValues, YValues = yValues, InterpolationValue = interpolationValue };
            }
            catch (FileNotFoundException)
            {
                // Обробка помилки, якщо файл не знайдено
                TextViewer.ChangeColor($"\nПОМИЛКА: Файл '{_filePath}' не знайдено. Повторіть спробу, будь-ласка.", "red");
                return null;
            }
            catch (IOException)
            {
                // Обробка помилки вводу-виводу
                TextViewer.ChangeColor($"\nПОМИЛКА: Помилка читання файлу '{_filePath}'. Повторіть спробу, будь-ласка.", "red");
                return null;
            }
            catch (Exception ex)
            {
                // Обробка інших винятків
                TextViewer.ChangeColor($"\nПОМИЛКА: {ex.Message}", "red");
                return null;
            }
        }

    }
}
