using System;
using System.Collections.Generic;

namespace SnakeWithBrain.Brain
{
    public class NeuralLayer
    {

        public string Name { get; }
        
        public IReadOnlyList<Neuron> Neurons => _neurons;
        
        private readonly List<Neuron> _neurons;

        public static NeuralLayer Create(int count, Random random, string name = "")
        {
            List<Neuron> neurons = new List<Neuron>();

            for (int i = 0; i < count; i++)
            {
                neurons.Add(new Neuron(random.NextDouble()));
            }

            return new NeuralLayer(neurons, name);
        }

        protected NeuralLayer(List<Neuron> neurons, string name = "")
        {
            Name = name;
            _neurons = neurons;
        }

        public void Forward()
        {
            foreach (var neuron in _neurons)
            {
                neuron.Fire();
            }
        }
        
        public void Log()
        {
            Console.WriteLine("{0}, Weight: {1}", Name);
        }
    }
}