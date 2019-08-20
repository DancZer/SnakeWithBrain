namespace SnakeWithBrain.Brain
{
    public class Dendrite
    {
        public Neuron Input { get; }

        public double DeltaWeight { get; set; }

        public double SynapticWeight { get; set; }

        public Dendrite(Neuron neuron)
        {
            Input = neuron;
        }
    }
}