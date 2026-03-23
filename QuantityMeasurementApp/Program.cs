using QuantityMeasurementApp.Controller;

namespace QuantityMeasurementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var controller = new QuantityMeasurementController();
            controller.ShowMainMenu();
        }
    }
}
