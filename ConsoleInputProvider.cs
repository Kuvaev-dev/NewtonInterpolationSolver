namespace NewtonInterpolationSolver
{
    // Клас для введення даних з консолі
    public class ConsoleInputProvider : IInputProvider
    {
        public double[] GetData()
        {
            Console.WriteLine("Введіть дані через пробіл:");
            string input = Console.ReadLine();
            string[] values = input.Split(' ');

            double[] data = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!double.TryParse(values[i], out data[i]))
                {
                    TextViewer.ChangeColor($"\nПОМИЛКА: Неправильний формат введення '{values[i]}'. Будь ласка, введіть числа.\n", "red");
                    return null;
                }
            }

            return data;
        }
    }
}
