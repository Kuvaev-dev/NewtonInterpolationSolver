using System;
using System.Globalization;
using System.IO;
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
                // Зчитуємо усі рядки з файлу
                string[] lines = File.ReadAllLines(_filePath);

                // Перевіряємо, чи є принаймні три рядки (для значень x, y та інтерполяції)
                if (lines.Length < 3)
                {
                    TextViewer.ChangeColor($"\nПОМИЛКА: Файл '{_filePath}' не містить достатньої кількості рядків.", "red");
                    return null;
                }

                // Розділяємо перший та другий рядки для отримання значень x та y
                double[] xValues = Array.ConvertAll(lines[0].Split(' '), s => double.Parse(s, CultureInfo.InvariantCulture));
                double[] yValues = Array.ConvertAll(lines[1].Split(' '), s => double.Parse(s, CultureInfo.InvariantCulture));

                // Отримуємо значення інтерполяції з третього рядка
                double interpolationValue = double.Parse(lines[2], CultureInfo.InvariantCulture);

                // Повертаємо об'єкт DataArrays з отриманими значеннями
                return new DataArrays { XValues = xValues, YValues = yValues, InterpolationValue = interpolationValue };
            }
            catch (FileNotFoundException)
            {
                // Обробляємо виняток, якщо файл не знайдено
                TextViewer.ChangeColor($"\nПОМИЛКА: Файл '{_filePath}' не знайдено. Повторіть спробу, будь-ласка.", "red");
                return null;
            }
            catch (IOException)
            {
                // Обробляємо виняток, якщо сталася помилка читання файлу
                TextViewer.ChangeColor($"\nПОМИЛКА: Помилка читання файлу '{_filePath}'. Повторіть спробу, будь-ласка.", "red");
                return null;
            }
            catch (Exception ex)
            {
                // Обробляємо загальний виняток та виводимо повідомлення про помилку
                TextViewer.ChangeColor($"\nПОМИЛКА: {ex.Message}", "red");
                return null;
            }
        }
    }
}
