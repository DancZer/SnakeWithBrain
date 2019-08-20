
using System.Windows;
using static SnakeWithBrain.LogWindow;

namespace SnakeWithBrain.Brain
{
    public class SimpleNeuralNet
    {
        public static void Run(BrainWindow brainWindow)
        {
            var random = new System.Random(1234);
            NetworkModel model = new NetworkModel(random);
            model.AddLayer(NeuralLayer.Create(2, random, "INPUT"));
            model.AddLayer(NeuralLayer.Create(1, random, "OUTPUT"));

            brainWindow.Content = new NetworkModelView(model);

            model.Build();
            Console.WriteLine("----Before Training------------");
            model.Print();

            Console.WriteLine();

            NeuralData input = new NeuralData(4);
            input.Add(0, 0); //1
            input.Add(0, 1); //2
            input.Add(1, 0); //3
            input.Add(1, 1); //4
           

            NeuralData expectedOutput = new NeuralData(4);
            expectedOutput.Add(0); //1
            expectedOutput.Add(0); //2
            expectedOutput.Add(0); //3
            expectedOutput.Add(1); //4


            // overall net learning rate
            var eta = 0.15;

            //alpha  momentum, multiplier of last deltaWeight, [0.0..n]
            var alpha = 0.5;

            model.Train(input, expectedOutput, 100, eta, alpha);
            Console.WriteLine();
            Console.WriteLine("----After Training------------");
            model.Print();

        }
    }
}