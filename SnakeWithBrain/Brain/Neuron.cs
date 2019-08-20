using System;
using System.Collections.Generic;

namespace SnakeWithBrain.Brain
{
    public class Neuron
    {
        public IReadOnlyList<Dendrite> Dendrites => _dendrites;

        public double OutputValue { get; private set; }

        public double Gradient { get; private set; }

        private List<Dendrite> _dendrites = new List<Dendrite>();

        public Neuron(double gradient)
        {
            Gradient = gradient;
        }

        public void SetOutputValue(double v)
        {
            OutputValue = v;
        }

        public void AddDendrite(Dendrite dendrite)
        {
            _dendrites.Add(dendrite);
        }

        public void Fire()
        {
            var sumValue = Sum();

            OutputValue = TransferFunction(sumValue);
        }

        private double Sum()
        {
            double computeValue = 0.0f;
            foreach (var d in _dendrites)
            {
                computeValue += d.Input.OutputValue * d.SynapticWeight;
            }
            
            return computeValue;
        }

        public void UpdateInputWeights(double eta, double alpha)
        {
            // The weights to be updated are in the Connection container
            // in the nuerons in the preceding layer

            foreach (var d in _dendrites)
            {
                double oldDeltaWeight = d.DeltaWeight;

                double newDeltaWeight =
                    // Individual input, magnified by the gradient and train rate:
                    eta
                    * d.Input.OutputValue
                    * Gradient
                    // Also add momentum = a fraction of the previous delta weight
                    + alpha
                    * oldDeltaWeight;

                d.DeltaWeight = newDeltaWeight;
                d.SynapticWeight += newDeltaWeight;
            }
        }

        public void CalcHiddenGradients()
        {
            double dow = SumDOW();
            Gradient = dow * TransferFunctionDerivative(OutputValue);
        }

        private double SumDOW()
        {
            double sum = 0.0;

            // Sum our contributions of the errors at the nodes we feed

            foreach (var d in _dendrites)
            {
                sum += d.SynapticWeight * d.Input.Gradient;
            }

            return sum;
        }


        public void CalcOutputGradients(double targetVals)
        {
            double delta = targetVals - OutputValue;
            Gradient = delta * TransferFunctionDerivative(OutputValue);
        }

        public double TransferFunction(double x)
        {
            // tanh - output range [-1.0..1.0]
            return Math.Tanh(x);
        }

        public double TransferFunctionDerivative(double x)
        {
            // tanh derivative
            return 1.0 - x * x;
        }
    }
}