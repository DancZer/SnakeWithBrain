using System;
using ConsoleTableExt;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SnakeWithBrain.Brain
{
    public class NetworkModel
    {
        public IList<NeuralLayer> Layers => _layers;

        private List<NeuralLayer> _layers = new List<NeuralLayer>();

        private readonly Random _random;

        public NetworkModel(Random random)
        {
            _random = random;
        }

        public void AddLayer(NeuralLayer layer)
        {
            _layers.Add(layer);
        }

        public void Build()
        {
            for (int i = 0; i < _layers.Count-1; i++)
            {
                CreateNetwork(_layers[i], _layers[i + 1]);
            }
        }

        private void CreateNetwork(NeuralLayer connectingFrom, NeuralLayer connectingTo)
        {
            foreach (var to in connectingTo.Neurons)
            {
                foreach (var from in connectingFrom.Neurons)
                {
                    to.AddDendrite(new Dendrite(from) {SynapticWeight = _random.NextDouble() });
                }
            }
        }

        public void Solve(NeuralData input, NeuralData output)
        {
            var inputLayer = _layers.First();

            for (int j = 0; j < input.Data[0].Length; j++)
            {
                inputLayer.Neurons[j].SetOutputValue(input.Data[0][j]);
            }
            ComputeOutput();

            var outputLayer = _layers.Last();
            for (int i = 0; i < outputLayer.Neurons.Count; i++)
            {
                var neuron = outputLayer.Neurons[i];
                output.Data[0][i] = neuron.OutputValue;
            }
        }

        double recentAverageSmoothingFactor = 100.0; // Number of training samples to average over

        public void Train(NeuralData input, NeuralData expected, int iterations, double eta = 0.15, double alpha = 0.5)
        {
            //Get the input layers
            var inputLayer = _layers.First();
            var outputLayer = _layers.Last();

            int epoch = 1;
            //Loop till the number of iterations
            while (iterations >= epoch)
            {
                double recentAverageError = 0;

                //Loop through the record
                for (int dIndex = 0; dIndex < input.Data.Length; dIndex++)
                {
                    //Set the input data into the first layer
                    for (int j = 0; j < inputLayer.Neurons.Count; j++)
                    {
                        inputLayer.Neurons[j].SetOutputValue(input.Data[dIndex][j]);
                    }

                    //Fire all the neurons and collect the output
                    ComputeOutput();
                    
                    // Calculate overal net error (RMS of output neuron errors)
                    var error = 0.0;

                    for (int j = 0; j < outputLayer.Neurons.Count; j++)
                    {
                        var output = outputLayer.Neurons[j].OutputValue;
                        var expectedOutput = expected.Data[dIndex][j];

                        var delta = output - expectedOutput;

                        error += delta * delta;
                    }

                    error /= outputLayer.Neurons.Count; // get average error squared
                    error = Math.Sqrt(error); // RMS

                    recentAverageError = (recentAverageError * recentAverageSmoothingFactor + error) / (recentAverageSmoothingFactor + 1.0);

                    // Calculate output layer gradients
                    for (int j = 0; j < outputLayer.Neurons.Count - 1; ++j)
                    {
                        var expectedOutput = expected.Data[dIndex][j];
                        var neuron = outputLayer.Neurons[j];

                        neuron.CalcOutputGradients(expectedOutput);
                    }

                    if(_layers.Count >= 2) { 
                        // Calculate gradients on hidden layers
                        for (int i = _layers.Count-2; i > 1; i--)
                        {
                            var layer = _layers[i];

                            foreach (var neuron in layer.Neurons)
                            {
                                neuron.CalcHiddenGradients();
                            }
                        }
                    }

                    // Calculate weights
                    for (int i = _layers.Count - 1; i > 0; i--)
                    {
                        var layer = _layers[i];

                        foreach (var neuron in layer.Neurons)
                        {
                            neuron.UpdateInputWeights(eta, alpha);
                        }
                    }
                }
               
                LogWindow.Console.WriteLine("Epoch: {0}, AverageError: {1} %", epoch, recentAverageError);
                epoch++;
            }
        }
        
        public void Print()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Neurons");

            foreach (var element in _layers)
            {
                DataRow row = dt.NewRow();
                row[0] = element.Name;
                row[1] = element.Neurons.Count;

                dt.Rows.Add(row);
            }

            ConsoleTableBuilder builder = ConsoleTableBuilder.From(dt);

            LogWindow.Console.WriteLine(builder.Export().ToString());
        }

        private void ComputeOutput()
        {
            for (int i = 1; i < _layers.Count; i++)
            {
                _layers[i].Forward();
            }
        }

    }
}